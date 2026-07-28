param(
    [switch]$All,
    [switch]$Smoke,
    [string]$Version,
    [string]$ManifestPath,
    [string]$OutputDirectory
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
. (Join-Path $PSScriptRoot "nuget-package.ps1")
if (-not $ManifestPath) {
    $ManifestPath = Join-Path $repoRoot "website/humanizer-versions.json"
}

if (-not (Test-Path $ManifestPath -PathType Leaf)) {
    throw "Version manifest not found: $ManifestPath"
}

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
$versions = @($manifest.versions)
if ($Version) {
    $versions = @($versions | Where-Object version -eq $Version)
} elseif (-not $All) {
    $versions = @($versions | Where-Object { $_.latestStable -or $_.version -eq "current" })
}

if ($versions.Count -eq 0) {
    throw "No API versions matched the requested verification."
}
if ($OutputDirectory -and $versions.Count -ne 1) {
    throw "API output requires exactly one selected version."
}

function Get-DirectoryDigest {
    param([Parameter(Mandatory = $true)][string]$Path)

    $lines = Get-ChildItem $Path -File -Recurse |
        Sort-Object FullName |
        ForEach-Object {
            $relativePath = [System.IO.Path]::GetRelativePath($Path, $_.FullName).Replace('\', '/')
            "$relativePath $((Get-FileHash $_.FullName -Algorithm SHA256).Hash)"
        }
    $bytes = [System.Text.Encoding]::UTF8.GetBytes(($lines -join "`n"))
    return [Convert]::ToHexString([System.Security.Cryptography.SHA256]::HashData($bytes))
}

function Get-NuGetApiInput {
    param([Parameter(Mandatory = $true)]$Entry)

    $package = Get-DocsNuGetPackage -Entry $Entry -RepoRoot $repoRoot
    $apiDirectory = Join-Path $package.ExtractPath "lib/$($Entry.referenceTfm)"
    return [PSCustomObject]@{
        Dll = Join-Path $apiDirectory "Humanizer.dll"
        Xml = Join-Path $apiDirectory "Humanizer.xml"
    }
}

function Get-CheckoutApiInput {
    param([Parameter(Mandatory = $true)]$Entry)

    $projectPath = Join-Path $repoRoot "src/Humanizer/Humanizer.csproj"
    Invoke-DocsCheckedCommand `
        -FilePath "dotnet" `
        -ArgumentList @(
            "build", $projectPath,
            "--configuration", "Release",
            "--framework", $Entry.referenceTfm,
            "--nologo"
        ) `
        -WorkingDirectory $repoRoot

    $targetPathLog = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-target-$([guid]::NewGuid().ToString('N')).txt"
    & dotnet msbuild $projectPath `
        "-getProperty:TargetPath" `
        "-p:Configuration=Release" `
        "-p:TargetFramework=$($Entry.referenceTfm)" `
        "-nologo" > $targetPathLog
    if ($LASTEXITCODE -ne 0) {
        throw "Could not resolve the preview Humanizer build output."
    }

    $dllPath = (Get-Content $targetPathLog | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | Select-Object -Last 1).Trim()
    Remove-Item $targetPathLog -Force
    if (-not [System.IO.Path]::IsPathRooted($dllPath)) {
        $dllPath = Join-Path $repoRoot $dllPath
    }

    return [PSCustomObject]@{
        Dll = $dllPath
        Xml = [System.IO.Path]::ChangeExtension($dllPath, ".xml")
    }
}

function Assert-GeneratedApi {
    param(
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$VersionLabel
    )

    $requiredFiles = @(
        "Humanizer.CasingExtensions.md",
        "Humanizer.CollectionHumanizeExtensions.md",
        "Humanizer.LetterCasing.md",
        "Humanizer.StringHumanizeExtensions.md"
    )
    foreach ($file in $requiredFiles) {
        $path = Join-Path $OutputPath $file
        if (-not (Test-Path $path -PathType Leaf)) {
            throw "$VersionLabel did not generate the required API proof file $file."
        }
    }

    $stringApi = Get-Content -Raw (Join-Path $OutputPath "Humanizer.StringHumanizeExtensions.md")
    $collectionApi = Get-Content -Raw (Join-Path $OutputPath "Humanizer.CollectionHumanizeExtensions.md")
    if ($stringApi -notmatch "## StringHumanizeExtensions Class") {
        throw "$VersionLabel did not emit the representative API type."
    }
    if ($collectionApi -notmatch "Humanize\\<T\\>") {
        throw "$VersionLabel did not preserve generic member names."
    }

    $localPathPattern = "file://|$([regex]::Escape($repoRoot))"
    $leakedFile = Get-ChildItem $OutputPath -Filter "*.md" -File -Recurse |
        Where-Object { (Get-Content -Raw $_.FullName) -match $localPathPattern } |
        Select-Object -First 1
    if ($leakedFile) {
        throw "$VersionLabel leaked a local source path in $($leakedFile.Name)."
    }

    if ($VersionLabel -in @("3.0.10", "current")) {
        if ($stringApi -notmatch "StringHumanizeExtensions\\\.Humanize\\\(this string, LetterCasing\\\) Method") {
            throw "$VersionLabel did not preserve overload headings."
        }
        if ($stringApi -notmatch "\[ApplyCase\\\(this string, LetterCasing\\\)\]\(Humanizer\.CasingExtensions\.md#") {
            throw "$VersionLabel did not preserve XML cross-references as relative Markdown links."
        }
        if ($collectionApi -notmatch "\[collection\]\(Humanizer\.CollectionHumanizeExtensions\.md#") {
            throw "$VersionLabel did not preserve XML parameter references."
        }
        if ($stringApi -notmatch "<a name='Humanizer\.StringHumanizeExtensions\.Humanize") {
            throw "$VersionLabel did not emit member anchors."
        }
        if ($stringApi -match "<xref") {
            throw "$VersionLabel left unresolved XML cross-references."
        }
    }
}

function Assert-CommittedApiProof {
    param(
        [Parameter(Mandatory = $true)][string]$GeneratedPath,
        [Parameter(Mandatory = $true)][string]$VersionLabel
    )

    $committedPath = if ($VersionLabel -eq "current") {
        Join-Path $repoRoot "website/docs/api"
    } elseif ($VersionLabel -eq "3.0.10") {
        Join-Path $repoRoot "website/versioned_docs/version-3.0.10/api"
    } else {
        return
    }

    $requiredFiles = @(
        "Humanizer.CasingExtensions.md",
        "Humanizer.CollectionHumanizeExtensions.md",
        "Humanizer.LetterCasing.md",
        "Humanizer.StringHumanizeExtensions.md"
    )
    foreach ($file in $requiredFiles) {
        $generatedFile = Join-Path $GeneratedPath $file
        $committedFile = Join-Path $committedPath $file
        if (-not (Test-Path $committedFile -PathType Leaf) -or
            (Get-FileHash $generatedFile -Algorithm SHA256).Hash -ne
            (Get-FileHash $committedFile -Algorithm SHA256).Hash) {
            throw "Committed API proof file is stale or modified: $committedFile"
        }
    }
}

$deterministicVersions = @("3.0.10", "current")
foreach ($entry in $versions) {
    if (-not $entry.apiPackage -or -not $entry.referenceTfm) {
        throw "Version $($entry.version) is missing API package or reference TFM metadata."
    }

    $input = if ($entry.source.kind -eq "nuget") {
        Get-NuGetApiInput -Entry $entry
    } elseif ($entry.source.kind -eq "checkout") {
        Get-CheckoutApiInput -Entry $entry
    } else {
        throw "Unsupported API source kind '$($entry.source.kind)' for $($entry.version)."
    }

    if (-not (Test-Path $input.Dll -PathType Leaf) -or -not (Test-Path $input.Xml -PathType Leaf)) {
        throw "Version $($entry.version) is missing its declared DLL/XML pair for $($entry.referenceTfm)."
    }

    $preserveOutput = -not [string]::IsNullOrWhiteSpace($OutputDirectory)
    $firstOutput = if ($preserveOutput) {
        [System.IO.Path]::GetFullPath($OutputDirectory)
    } else {
        Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-api-$($entry.version)-$([guid]::NewGuid().ToString('N'))"
    }
    if ($preserveOutput -and (Test-Path $firstOutput)) {
        throw "API output directory already exists: $firstOutput"
    }
    New-Item -ItemType Directory -Path $firstOutput | Out-Null
    try {
        Invoke-DocsCheckedCommand `
            -FilePath "dotnet" `
            -ArgumentList @(
                "tool", "run", "defaultdocumentation", "--",
                "--AssemblyFilePath", $input.Dll,
                "--DocumentationFilePath", $input.Xml,
                "--OutputDirectoryPath", $firstOutput,
                "--AssemblyPageName", "assembly",
                "--GeneratedAccessModifiers", "Public",
                "--GeneratedPages", "Namespaces,Types",
                "--Sections", "Default"
            ) `
            -WorkingDirectory $repoRoot
        Assert-GeneratedApi -OutputPath $firstOutput -VersionLabel $entry.version
        Assert-CommittedApiProof -GeneratedPath $firstOutput -VersionLabel $entry.version

        if (($All -or $Smoke) -and $deterministicVersions -contains $entry.version) {
            $secondOutput = "$firstOutput-second"
            New-Item -ItemType Directory -Path $secondOutput | Out-Null
            try {
                Invoke-DocsCheckedCommand `
                    -FilePath "dotnet" `
                    -ArgumentList @(
                        "tool", "run", "defaultdocumentation", "--",
                        "--AssemblyFilePath", $input.Dll,
                        "--DocumentationFilePath", $input.Xml,
                        "--OutputDirectoryPath", $secondOutput,
                        "--AssemblyPageName", "assembly",
                        "--GeneratedAccessModifiers", "Public",
                        "--GeneratedPages", "Namespaces,Types",
                        "--Sections", "Default"
                    ) `
                    -WorkingDirectory $repoRoot
                if ((Get-DirectoryDigest $firstOutput) -ne (Get-DirectoryDigest $secondOutput)) {
                    throw "API generation for $($entry.version) is not deterministic."
                }
            } finally {
                Remove-Item $secondOutput -Recurse -Force -ErrorAction SilentlyContinue
            }
        }

        Write-Host "API proof passed: $($entry.version) ($($entry.apiPackage), $($entry.referenceTfm))"
    } finally {
        if (-not $preserveOutput) {
            Remove-Item $firstOutput -Recurse -Force -ErrorAction SilentlyContinue
        }
    }
}
