param(
    [string[]]$Version,
    [string]$ManifestPath,
    [switch]$RequireNativeAot
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
if (-not $ManifestPath) {
    $ManifestPath = Join-Path $repoRoot "website/humanizer-versions.json"
}

& (Join-Path $PSScriptRoot "verify-manifest.ps1") `
    -ManifestPath $ManifestPath

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
$latestStableVersion = [string](
    $manifest.versions |
        Where-Object latestStable |
        Select-Object -ExpandProperty version
)
$requestedVersions = @(
    if ($Version.Count -gt 0) {
        $Version
    } else {
        $latestStableVersion
        "current"
    }
)
$entries = @(
    $manifest.versions |
        Where-Object { $_.version -in $requestedVersions }
)
if ($entries.Count -ne $requestedVersions.Count) {
    throw "One or more requested AOT verification versions are not in the manifest."
}

$runtimeIdentifier = [System.Runtime.InteropServices.RuntimeInformation]::RuntimeIdentifier
$supportedRuntimeIdentifiers = @(
    "win-x64",
    "win-arm64",
    "linux-x64",
    "linux-arm64",
    "linux-musl-x64",
    "linux-musl-arm64",
    "osx-x64",
    "osx-arm64"
)
if ($runtimeIdentifier -notin $supportedRuntimeIdentifiers) {
    $message = (
        "Native AOT documentation verification does not support host RID " +
        "'$runtimeIdentifier'."
    )
    if ($RequireNativeAot) {
        throw $message
    }
    Write-Warning "$message Skipping the local AOT gate."
    return
}

$project = Join-Path (
    $repoRoot
) "website/docs/_examples/scenarios-aot/Aot.csproj"
$checkoutPackageVersion = $null
$tempRoot = Join-Path (
    [System.IO.Path]::GetTempPath()
) "humanizer-docs-aot-$([guid]::NewGuid().ToString('N'))"
$packageRoot = Join-Path $tempRoot "packages"
$restorePackagesRoot = Join-Path $tempRoot "restore-packages"
$checkoutPackage = $null

function Invoke-PublishedExample {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)][string]$Mode,
        [Parameter(Mandatory = $true)][bool]$PublishAot
    )

    $modeRoot = Join-Path $tempRoot "$($Entry.version)/$Mode"
    $publishRoot = Join-Path $modeRoot "publish"
    $arguments = @(
        "publish",
        $project,
        "--configuration", "Release",
        "--runtime", $runtimeIdentifier,
        "--self-contained", "true",
        "--artifacts-path", (Join-Path $modeRoot "artifacts"),
        "--nologo",
        "-p:RestorePackagesPath=$restorePackagesRoot",
        "-p:PublishDir=$publishRoot/",
        "-p:PublishTrimmed=true",
        "-p:TreatWarningsAsErrors=true"
    )
    if ($PublishAot) {
        $arguments += "-p:PublishAot=true"
    }
    if ($Entry.source.kind -eq "checkout") {
        $arguments += "-p:HumanizerPackageVersion=$checkoutPackageVersion"
        $arguments += @(
            "--source", $packageRoot,
            "--source", "https://api.nuget.org/v3/index.json"
        )
    } else {
        $arguments += "-p:HumanizerPackageVersion=$($Entry.source.packageVersion)"
    }
    $exampleExcludedAssetsProperty = $Entry.PSObject.Properties[
        "exampleExcludedAssets"
    ]
    if ($null -ne $exampleExcludedAssetsProperty -and
        @($exampleExcludedAssetsProperty.Value).Count -gt 0) {
        $arguments += "-p:HumanizerExampleExcludeAssets=$(
            @($exampleExcludedAssetsProperty.Value) -join '%3B'
        )"
    }

    & dotnet @arguments
    if ($LASTEXITCODE -ne 0) {
        throw "$Mode publish failed for Humanizer $($Entry.version)."
    }

    $assetFiles = @(
        Get-ChildItem $modeRoot `
            -Filter "project.assets.json" `
            -File `
            -Recurse
    )
    if ($assetFiles.Count -ne 1) {
        throw "$Mode publish did not produce exactly one restore graph for $($Entry.version)."
    }
    $assets = Get-Content -Raw $assetFiles[0].FullName |
        ConvertFrom-Json -Depth 100
    $expectedPackageRoot = [System.IO.Path]::GetFullPath(
        $restorePackagesRoot
    ).TrimEnd([char[]]@("/", "\"))
    $resolvedPackageRoots = @(
        $assets.packageFolders.PSObject.Properties.Name |
            ForEach-Object {
                [System.IO.Path]::GetFullPath($_).TrimEnd(
                    [char[]]@("/", "\")
                )
            }
    )
    if ($resolvedPackageRoots.Count -ne 1 -or
        $resolvedPackageRoots[0] -ne $expectedPackageRoot) {
        throw "$Mode publish did not use the isolated package root for $($Entry.version)."
    }
    $resolvedVersion = if ($Entry.source.kind -eq "checkout") {
        $checkoutPackageVersion
    } else {
        $Entry.source.packageVersion
    }
    $libraryKey = "$($Entry.installPackage)/$resolvedVersion"
    $library = $assets.libraries.PSObject.Properties[$libraryKey]
    if ($null -eq $library -or $library.Value.type -ne "package") {
        throw "$Mode publish did not resolve $libraryKey."
    }
    $resolvedPackageDirectory = [System.IO.Path]::GetFullPath(
        (Join-Path $restorePackagesRoot ([string]$library.Value.path))
    )
    if (-not $resolvedPackageDirectory.StartsWith(
        "$expectedPackageRoot$([System.IO.Path]::DirectorySeparatorChar)",
        [System.StringComparison]::Ordinal
    )) {
        throw "$mode publish resolved $libraryKey outside the isolated package root."
    }
    if ($Entry.source.kind -eq "checkout") {
        $resolvedHashFiles = @(
            Get-ChildItem $resolvedPackageDirectory `
                -Filter "*.nupkg.sha512" `
                -File
        )
        if ($resolvedHashFiles.Count -ne 1) {
            throw "$Mode publish has no package hash proof for $libraryKey."
        }
        $expectedHash = [Convert]::ToBase64String(
            [System.Security.Cryptography.SHA512]::HashData(
                [System.IO.File]::ReadAllBytes($checkoutPackage.FullName)
            )
        )
        if ((Get-Content -Raw $resolvedHashFiles[0].FullName).Trim() -ne
            $expectedHash) {
            throw "$Mode publish did not resolve the freshly packed $libraryKey archive."
        }
    }

    $executableName = if ($IsWindows) { "Aot.exe" } else { "Aot" }
    $executable = Join-Path $publishRoot $executableName
    if (-not (Test-Path $executable -PathType Leaf)) {
        throw "$Mode publish did not produce $executableName for $($Entry.version)."
    }
    & $executable
    if ($LASTEXITCODE -ne 0) {
        throw "$Mode executable failed for Humanizer $($Entry.version)."
    }
    Write-Host "$Mode publish passed: $($Entry.version) ($runtimeIdentifier)"
}

try {
    if (@($entries | Where-Object { $_.source.kind -eq "checkout" }).Count -gt 0) {
        New-Item -ItemType Directory -Path $packageRoot -Force | Out-Null
        & dotnet pack `
            (Join-Path $repoRoot "src/Humanizer/Humanizer.csproj") `
            --configuration Release `
            --output $packageRoot `
            --nologo
        if ($LASTEXITCODE -ne 0) {
            throw "Could not pack the current Humanizer checkout for AOT verification."
        }
        $checkoutPackages = @(
            Get-ChildItem $packageRoot -Filter "Humanizer.*.nupkg" -File |
                Where-Object { $_.Name -notlike "*.snupkg" }
        )
        if ($checkoutPackages.Count -ne 1) {
            throw "Current checkout did not produce exactly one Humanizer package."
        }
        $checkoutPackage = $checkoutPackages[0]
        $archive = [System.IO.Compression.ZipFile]::OpenRead(
            $checkoutPackage.FullName
        )
        try {
            $nuspec = $archive.GetEntry("Humanizer.nuspec")
            if ($null -eq $nuspec) {
                throw "Packed Humanizer archive has no Humanizer.nuspec."
            }
            $reader = [System.IO.StreamReader]::new($nuspec.Open())
            try {
                [xml]$nuspecXml = $reader.ReadToEnd()
            } finally {
                $reader.Dispose()
            }
            $checkoutPackageVersion = [string]$nuspecXml.package.metadata.version
        } finally {
            $archive.Dispose()
        }
        if ([string]::IsNullOrWhiteSpace($checkoutPackageVersion)) {
            throw "Packed Humanizer archive has no package version."
        }
    }
    foreach ($entry in $entries) {
        Invoke-PublishedExample `
            -Entry $entry `
            -Mode "trimmed" `
            -PublishAot $false
        Invoke-PublishedExample `
            -Entry $entry `
            -Mode "native-aot" `
            -PublishAot $true
    }
} finally {
    Remove-Item $tempRoot -Recurse -Force -ErrorAction SilentlyContinue
}
