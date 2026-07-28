param(
    [switch]$All,
    [switch]$Smoke,
    [string]$Version,
    [string]$ManifestPath,
    [string]$OutputDirectory
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
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

function Invoke-Checked {
    param(
        [Parameter(Mandatory = $true)][string]$FilePath,
        [Parameter(Mandatory = $true)][string[]]$ArgumentList,
        [string]$WorkingDirectory = $repoRoot
    )

    $logPath = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-command-$([guid]::NewGuid().ToString('N')).log"
    Push-Location $WorkingDirectory
    try {
        & $FilePath @ArgumentList *> $logPath
        if ($LASTEXITCODE -ne 0) {
            $details = Get-Content -Raw $logPath
            throw "$FilePath failed with exit code $LASTEXITCODE.`n$details"
        }
    } finally {
        Pop-Location
        Remove-Item $logPath -Force -ErrorAction SilentlyContinue
    }
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

function Assert-NuGetPackage {
    param(
        [Parameter(Mandatory = $true)][string]$PackageId,
        [Parameter(Mandatory = $true)][string]$PackageVersion,
        [Parameter(Mandatory = $true)][string]$PackagePath
    )

    $registrationUri = "https://api.nuget.org/v3/registration5-semver1/$PackageId/$PackageVersion.json"
    $registration = Invoke-RestMethod `
        -Uri $registrationUri `
        -MaximumRetryCount 6 `
        -RetryIntervalSec 10 `
        -TimeoutSec 30
    $catalog = Invoke-RestMethod `
        -Uri $registration.catalogEntry `
        -MaximumRetryCount 6 `
        -RetryIntervalSec 10 `
        -TimeoutSec 30
    if ($catalog.packageHashAlgorithm -ne "SHA512" -or -not $catalog.packageHash) {
        throw "NuGet.org did not publish a SHA-512 package hash for $PackageId $PackageVersion."
    }

    $actualHash = [Convert]::ToBase64String(
        [System.Security.Cryptography.SHA512]::HashData(
            [System.IO.File]::ReadAllBytes($PackagePath)
        )
    )
    if ($actualHash -ne $catalog.packageHash) {
        throw "NuGet.org package hash validation failed for $PackageId $PackageVersion."
    }

    $archive = [System.IO.Compression.ZipFile]::OpenRead($PackagePath)
    try {
        if (-not ($archive.Entries.FullName -contains ".signature.p7s")) {
            throw "NuGet package is unsigned: $PackageId $PackageVersion."
        }
    } finally {
        $archive.Dispose()
    }

    try {
        Invoke-Checked -FilePath "dotnet" -ArgumentList @(
            "nuget", "verify", $PackagePath,
            "--all",
            "--configfile", (Join-Path $repoRoot "nuget.config")
        )
        Write-Host "NuGet signature passed: $PackageId $PackageVersion"
    } catch {
        $allowedHistoricalCodes = @("NU3028", "NU3037")
        $reportedCodes = @(
            [regex]::Matches($_.Exception.Message, "NU\d{4}") |
                ForEach-Object Value |
                Sort-Object -Unique
        )
        $unexpectedCodes = @(
            $reportedCodes |
                Where-Object { $_ -notin $allowedHistoricalCodes }
        )
        if ($reportedCodes.Count -eq 0 -or $unexpectedCodes.Count -gt 0) {
            throw
        }

        Write-Host "NuGet signature is historical; signed package and NuGet.org SHA-512 passed: $PackageId $PackageVersion"
    }
}

function Get-NuGetApiInput {
    param([Parameter(Mandatory = $true)]$Entry)

    $packageVersion = $Entry.source.packageVersion
    if (-not $packageVersion) {
        throw "Version $($Entry.version) has no NuGet package version."
    }

    $packageId = $Entry.apiPackage.ToLowerInvariant()
    $cacheRoot = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-nuget"
    $packageRoot = Join-Path $cacheRoot "$packageId/$packageVersion"
    $packagePath = Join-Path $packageRoot "$packageId.$packageVersion.nupkg"
    $extractPath = Join-Path $packageRoot "package"

    New-Item -ItemType Directory -Path $packageRoot -Force | Out-Null
    $downloadPackage = {
        $uri = "https://api.nuget.org/v3-flatcontainer/$packageId/$packageVersion/$packageId.$packageVersion.nupkg"
        $downloadPath = "$packagePath.$([guid]::NewGuid().ToString('N')).tmp"
        try {
            Invoke-WebRequest `
                -Uri $uri `
                -OutFile $downloadPath `
                -MaximumRetryCount 6 `
                -RetryIntervalSec 10 `
                -TimeoutSec 30
            Assert-NuGetPackage `
                -PackageId $packageId `
                -PackageVersion $packageVersion `
                -PackagePath $downloadPath
            Move-Item $downloadPath $packagePath -Force
        } finally {
            Remove-Item $downloadPath -Force -ErrorAction SilentlyContinue
        }
    }

    if (Test-Path $packagePath -PathType Leaf) {
        try {
            Assert-NuGetPackage `
                -PackageId $packageId `
                -PackageVersion $packageVersion `
                -PackagePath $packagePath
        } catch {
            Remove-Item $packagePath -Force
            & $downloadPackage
        }
    } else {
        & $downloadPackage
    }

    Remove-Item $extractPath -Recurse -Force -ErrorAction SilentlyContinue
    [System.IO.Compression.ZipFile]::ExtractToDirectory($packagePath, $extractPath)

    $apiDirectory = Join-Path $extractPath "lib/$($Entry.referenceTfm)"
    return [PSCustomObject]@{
        Dll = Join-Path $apiDirectory "Humanizer.dll"
        Xml = Join-Path $apiDirectory "Humanizer.xml"
    }
}

function Get-CheckoutApiInput {
    param([Parameter(Mandatory = $true)]$Entry)

    $projectPath = Join-Path $repoRoot "src/Humanizer/Humanizer.csproj"
    Invoke-Checked -FilePath "dotnet" -ArgumentList @(
        "build", $projectPath,
        "--configuration", "Release",
        "--framework", $Entry.referenceTfm,
        "--nologo"
    )

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
        Invoke-Checked -FilePath "dotnet" -ArgumentList @(
            "tool", "run", "defaultdocumentation", "--",
            "--AssemblyFilePath", $input.Dll,
            "--DocumentationFilePath", $input.Xml,
            "--OutputDirectoryPath", $firstOutput,
            "--AssemblyPageName", "assembly",
            "--GeneratedAccessModifiers", "Public",
            "--GeneratedPages", "Namespaces,Types",
            "--Sections", "Default"
        )
        Assert-GeneratedApi -OutputPath $firstOutput -VersionLabel $entry.version
        Assert-CommittedApiProof -GeneratedPath $firstOutput -VersionLabel $entry.version

        if (($All -or $Smoke) -and $deterministicVersions -contains $entry.version) {
            $secondOutput = "$firstOutput-second"
            New-Item -ItemType Directory -Path $secondOutput | Out-Null
            try {
                Invoke-Checked -FilePath "dotnet" -ArgumentList @(
                    "tool", "run", "defaultdocumentation", "--",
                    "--AssemblyFilePath", $input.Dll,
                    "--DocumentationFilePath", $input.Xml,
                    "--OutputDirectoryPath", $secondOutput,
                    "--AssemblyPageName", "assembly",
                    "--GeneratedAccessModifiers", "Public",
                    "--GeneratedPages", "Namespaces,Types",
                    "--Sections", "Default"
                )
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
