param(
    [switch]$All,
    [switch]$Smoke,
    [string]$Version,
    [string]$ManifestPath,
    [string]$OutputDirectory,
    [string]$WebsiteRoot
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
. (Join-Path $PSScriptRoot "nuget-package.ps1")
. (Join-Path $PSScriptRoot "snapshot-state.ps1")
. (Join-Path $PSScriptRoot "api-approval.ps1")
if (-not $ManifestPath) {
    $ManifestPath = Join-Path $repoRoot "website/humanizer-versions.json"
}
if (-not $WebsiteRoot) {
    $WebsiteRoot = Join-Path $repoRoot "website"
}
$WebsiteRoot = [System.IO.Path]::GetFullPath($WebsiteRoot)

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

function Get-ApiDigest {
    param([Parameter(Mandatory = $true)][string]$Path)

    $lines = Get-ChildItem $Path -File -Recurse |
        Where-Object {
            [System.IO.Path]::GetRelativePath(
                $Path,
                $_.FullName
            ).Replace("\", "/") -ne "index.md"
        } |
        Sort-Object FullName |
        ForEach-Object {
            $relativePath = [System.IO.Path]::GetRelativePath(
                $Path,
                $_.FullName
            ).Replace("\", "/")
            "$relativePath $((Get-FileHash $_.FullName -Algorithm SHA256).Hash)"
        }
    $bytes = [System.Text.Encoding]::UTF8.GetBytes(($lines -join "`n"))
    return [Convert]::ToHexString(
        [System.Security.Cryptography.SHA256]::HashData($bytes)
    )
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

function Use-ApiInput {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)][scriptblock]$Action
    )

    if ($Entry.source.kind -eq "nuget") {
        $consumeApiInput = $Action
        return Use-DocsNuGetPackage `
            -Entry $Entry `
            -RepoRoot $repoRoot `
            -Action {
            param($package)

            $apiDirectory = Join-Path $package.ExtractPath "lib/$($Entry.referenceTfm)"
            & $consumeApiInput ([PSCustomObject]@{
                Dll = Join-Path $apiDirectory "Humanizer.dll"
                Xml = Join-Path $apiDirectory "Humanizer.xml"
            })
        }
    }
    if ($Entry.source.kind -eq "checkout") {
        return & $Action (Get-CheckoutApiInput -Entry $Entry)
    }

    throw "Unsupported API source kind '$($Entry.source.kind)' for $($Entry.version)."
}

function Invoke-ApiGenerator {
    param(
        [Parameter(Mandatory = $true)]$ApiInput,
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [string]$AccessModifiers = "Api"
    )

    Invoke-DocsCheckedCommand `
        -FilePath "dotnet" `
        -ArgumentList @(
            "tool", "run", "defaultdocumentation", "--",
            "--AssemblyFilePath", $ApiInput.Dll,
            "--DocumentationFilePath", $ApiInput.Xml,
            "--OutputDirectoryPath", $OutputPath,
            "--AssemblyPageName", "assembly",
            "--GeneratedAccessModifiers", $AccessModifiers,
            "--IncludeUndocumentedItems", "true",
            "--GeneratedPages", "Namespaces,Types",
            "--Sections", "Default",
            "--LinksOutputFilePath", $LinksPath,
            "--LinksBaseUrl", "./"
        ) `
        -WorkingDirectory $repoRoot
}

function Assert-GeneratedApi {
    param(
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$VersionLabel,
        [Parameter(Mandatory = $true)][string]$ApprovalPath,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [string[]]$ExpectedExtraTypes = @()
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

    $approvalTypes = [System.Collections.Generic.List[string]]::new()
    $namespace = ""
    $parents = @{}
    foreach ($line in Get-Content $ApprovalPath) {
        if ($line -match "^namespace (?<name>\S+)") {
            $namespace = $Matches["name"]
            $parents = @{}
            continue
        }
        if ($line -notmatch "^(?<indent> +)public (?:(?:static|sealed|abstract|readonly|ref|partial) )*(?:class|struct|interface|enum|delegate)(?: static)? (?<name>[A-Za-z0-9_]+(?:<[^>]+>)?)") {
            continue
        }

        $depth = [int]($Matches["indent"].Length / 4)
        $parents[$depth] = $Matches["name"]
        foreach ($key in @($parents.Keys)) {
            if ($key -gt $depth) {
                $parents.Remove($key)
            }
        }
        $qualifiedName = "$namespace.$(
            ((1..$depth | ForEach-Object { $parents[$_] }) -join ".")
        )"
        $approvalTypes.Add(
            $qualifiedName.
                Replace("<", "_").
                Replace(">", "_").
                Replace(",", "_")
        )
    }
    $approvalTypes = @($approvalTypes | Sort-Object -Unique)
    $generatedTypes = @(
        Get-ChildItem $OutputPath -Filter "*.md" -File |
            Where-Object {
                (Get-Content -Raw $_.FullName) -match (
                    "(?m)^## .+ (?:Class|Struct|Interface|Enum|Delegate)$"
                )
            } |
            ForEach-Object BaseName |
            Sort-Object -Unique
    )
    $missingTypes = @($approvalTypes | Where-Object { $_ -notin $generatedTypes })
    $extraTypes = @($generatedTypes | Where-Object { $_ -notin $approvalTypes })
    if ($approvalTypes.Count -eq 0 -or $missingTypes.Count -gt 0) {
        throw "$VersionLabel API type coverage differs from its PublicAPI approval. Missing: $($missingTypes -join ', '); extra: $($extraTypes -join ', ')."
    }
    Assert-GeneratedExtraTypes `
        -VersionLabel $VersionLabel `
        -Actual $extraTypes `
        -Expected $ExpectedExtraTypes
    Assert-GeneratedApiLinks `
        -OutputPath $OutputPath `
        -LinksPath $LinksPath `
        -VersionLabel $VersionLabel

    $localPathPattern = "file://|$([regex]::Escape($repoRoot))"
    foreach ($generatedFile in Get-ChildItem $OutputPath -Filter "*.md" -File) {
        $content = Get-Content -Raw $generatedFile.FullName
        if ($content -match $localPathPattern) {
            throw "$VersionLabel leaked a local source path in $($generatedFile.Name)."
        }
        if ($content -match "<xref") {
            throw "$VersionLabel left unresolved XML cross-references in $($generatedFile.Name)."
        }
        foreach ($link in [regex]::Matches(
            $content,
            "\]\((?<target>[^)\s]+\.md)(?:#[^)\s']+)?"
        )) {
            $targetPath = Join-Path $OutputPath $link.Groups["target"].Value
            if (-not (Test-Path $targetPath -PathType Leaf)) {
                throw "$VersionLabel generated a broken API link in $($generatedFile.Name): $($link.Value)"
            }
        }
    }

    if ($stringApi -notmatch "<a name='Humanizer\.StringHumanizeExtensions\.Humanize") {
        throw "$VersionLabel did not emit member anchors."
    }
}

function Assert-CommittedApi {
    param(
        [Parameter(Mandatory = $true)][string]$GeneratedPath,
        [Parameter(Mandatory = $true)][string]$VersionLabel
    )

    $committedPath = if ($VersionLabel -eq "current") {
        Join-Path $WebsiteRoot "docs/api"
    } else {
        Join-Path $WebsiteRoot "versioned_docs/version-$VersionLabel/api"
    }
    if (-not (Test-Path $committedPath -PathType Container) -or
        (Get-ApiDigest $GeneratedPath) -ne
        (Get-ApiDigest $committedPath)) {
        throw "Committed API tree is stale or modified: $committedPath"
    }
}

function Get-PublicApiApproval {
    param([Parameter(Mandatory = $true)]$Entry)

    if ($Entry.version -eq "current") {
        return Join-Path $repoRoot $Entry.publicApiApproval.path
    }

    $approvalRoot = Join-Path (
        [System.IO.Path]::GetTempPath()
    ) "humanizer-docs-public-api/$($Entry.version)"
    $approvalPath = Join-Path $approvalRoot "approval.txt"
    New-Item -ItemType Directory -Path $approvalRoot -Force | Out-Null
    $uri = "https://raw.githubusercontent.com/Humanizr/Humanizer/$($Entry.publicApiApproval.tag)/$($Entry.publicApiApproval.path)"
    Invoke-WebRequest `
        -Uri $uri `
        -OutFile $approvalPath `
        -MaximumRetryCount 6 `
        -RetryIntervalSec 10 `
        -TimeoutSec 30
    return $approvalPath
}

function Test-ApiEntry {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)]$ApiInput
    )

    if (-not (Test-Path $ApiInput.Dll -PathType Leaf) -or
        -not (Test-Path $ApiInput.Xml -PathType Leaf)) {
        throw "Version $($Entry.version) is missing its declared DLL/XML pair for $($Entry.referenceTfm)."
    }
    if ((Split-Path $ApiInput.Dll -Leaf) -ne "Humanizer.dll" -or
        (Split-Path $ApiInput.Xml -Leaf) -ne "Humanizer.xml") {
        throw "Version $($Entry.version) selected a supporting assembly instead of Humanizer."
    }
    $approvalPath = Get-PublicApiApproval -Entry $Entry
    if (-not (Test-Path $approvalPath -PathType Leaf)) {
        throw "Version $($Entry.version) is missing PublicAPI approval evidence."
    }

    $preserveOutput = -not [string]::IsNullOrWhiteSpace($OutputDirectory)
    $firstOutput = if ($preserveOutput) {
        [System.IO.Path]::GetFullPath($OutputDirectory)
    } else {
        Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-api-$($Entry.version)-$([guid]::NewGuid().ToString('N'))"
    }
    if ($preserveOutput -and (Test-Path $firstOutput)) {
        throw "API output directory already exists: $firstOutput"
    }
    $firstLinks = "$firstOutput-links.txt"
    $publicOutput = "$firstOutput-public"
    $publicLinks = "$publicOutput-links.txt"
    New-Item -ItemType Directory -Path $firstOutput | Out-Null
    try {
        Invoke-ApiGenerator `
            -ApiInput $ApiInput `
            -OutputPath $firstOutput `
            -LinksPath $firstLinks
        Assert-GeneratedApi `
            -OutputPath $firstOutput `
            -VersionLabel $Entry.version `
            -ApprovalPath $approvalPath `
            -LinksPath $firstLinks `
            -ExpectedExtraTypes @(
                $Entry.publicApiApproval.generatedExtraTypes
            )
        New-Item -ItemType Directory -Path $publicOutput | Out-Null
        Invoke-ApiGenerator `
            -ApiInput $ApiInput `
            -OutputPath $publicOutput `
            -LinksPath $publicLinks `
            -AccessModifiers "Public"
        Assert-GeneratedApiLinks `
            -OutputPath $publicOutput `
            -LinksPath $publicLinks `
            -VersionLabel "$($Entry.version) public"
        Assert-ApiAccessCoverage `
            -ApiLinksPath $firstLinks `
            -PublicLinksPath $publicLinks `
            -ApprovalPath $approvalPath `
            -VersionLabel $Entry.version
        if (-not $preserveOutput) {
            Assert-CommittedApi `
                -GeneratedPath $firstOutput `
                -VersionLabel $Entry.version
        }

        if ($All -or $Smoke) {
            $secondOutput = "$firstOutput-second"
            $secondLinks = "$secondOutput-links.txt"
            New-Item -ItemType Directory -Path $secondOutput | Out-Null
            try {
                Invoke-ApiGenerator `
                    -ApiInput $ApiInput `
                    -OutputPath $secondOutput `
                    -LinksPath $secondLinks
                if ((Get-SnapshotDirectoryDigest $firstOutput) -ne
                    (Get-SnapshotDirectoryDigest $secondOutput) -or
                    (Get-FileHash $firstLinks -Algorithm SHA256).Hash -ne
                    (Get-FileHash $secondLinks -Algorithm SHA256).Hash) {
                    throw "API generation for $($Entry.version) is not deterministic."
                }
            } finally {
                Remove-Item $secondOutput -Recurse -Force -ErrorAction SilentlyContinue
                Remove-Item $secondLinks -Force -ErrorAction SilentlyContinue
            }
        }

        Write-Host "API reference passed: $($Entry.version) ($($Entry.apiPackage), $($Entry.referenceTfm))"
    } finally {
        if (-not $preserveOutput) {
            Remove-Item $firstOutput -Recurse -Force -ErrorAction SilentlyContinue
        }
        Remove-Item $firstLinks -Force -ErrorAction SilentlyContinue
        Remove-Item $publicOutput -Recurse -Force -ErrorAction SilentlyContinue
        Remove-Item $publicLinks -Force -ErrorAction SilentlyContinue
    }
}

foreach ($entry in $versions) {
    if (-not $entry.apiPackage -or -not $entry.referenceTfm) {
        throw "Version $($entry.version) is missing API package or reference TFM metadata."
    }

    Use-ApiInput -Entry $entry -Action {
        param($apiInput)
        Test-ApiEntry -Entry $entry -ApiInput $apiInput
    }
}
