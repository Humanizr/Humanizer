param(
    [string]$ManifestPath,
    [string]$WebsiteRoot,
    [switch]$SkipNativeState
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
. (Join-Path $PSScriptRoot "snapshot-state.ps1")
if (-not $WebsiteRoot) {
    $WebsiteRoot = Join-Path $repoRoot "website"
}
$WebsiteRoot = [System.IO.Path]::GetFullPath($WebsiteRoot)
if (-not $ManifestPath) {
    $ManifestPath = Join-Path $WebsiteRoot "humanizer-versions.json"
}
if (-not (Test-Path $ManifestPath -PathType Leaf)) {
    throw "Version manifest not found: $ManifestPath"
}

function Resolve-SafePath {
    param(
        [Parameter(Mandatory = $true)][string]$Root,
        [Parameter(Mandatory = $true)][string]$RelativePath,
        [Parameter(Mandatory = $true)][string]$Description
    )

    if ([System.IO.Path]::IsPathRooted($RelativePath) -or
        $RelativePath.Contains("\") -or
        $RelativePath -match "(^|/)\.\.(/|$)") {
        throw "$Description must be a safe forward-slash relative path: $RelativePath"
    }

    $resolvedRoot = [System.IO.Path]::GetFullPath($Root)
    $resolvedPath = [System.IO.Path]::GetFullPath((Join-Path $resolvedRoot $RelativePath))
    $rootPrefix = $resolvedRoot.TrimEnd(
        [System.IO.Path]::DirectorySeparatorChar
    ) + [System.IO.Path]::DirectorySeparatorChar
    if (-not $resolvedPath.StartsWith(
        $rootPrefix,
        [System.StringComparison]::Ordinal
    )) {
        throw "$Description escapes its root: $RelativePath"
    }

    return $resolvedPath
}

function Assert-OverlayPath {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][string]$Description
    )

    if ([string]::IsNullOrWhiteSpace($Path) -or
        [System.IO.Path]::IsPathRooted($Path) -or
        $Path.Contains("\") -or
        $Path -match "(^|/)\.\.(/|$)" -or
        $Path -match "[*?\[\]]" -or
        $Path -match "^api(/|$)" -or
        ($Path -notmatch "\.mdx?$" -and
            $Path -notmatch "^_examples/.+\.(?:cs|csproj)$")) {
        throw "$Description is not an authored page or runnable example: $Path"
    }
}

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
if ($manifest.schemaVersion -ne 1) {
    throw "Unsupported version manifest schema."
}

$versions = @($manifest.versions)
if ($versions.Count -eq 0) {
    throw "The version manifest is empty."
}

$requiredProperties = @(
    "version",
    "label",
    "source",
    "installPackage",
    "apiPackage",
    "referenceTfm",
    "publicApiApproval",
    "compatibilityOverlay",
    "route",
    "published",
    "latestStable"
)
foreach ($entry in $versions) {
    foreach ($property in $requiredProperties) {
        if ($null -eq $entry.$property) {
            throw "Version $($entry.version) is missing '$property'."
        }
    }
    if ([string]::IsNullOrWhiteSpace($entry.version) -or
        [string]::IsNullOrWhiteSpace($entry.label) -or
        [string]::IsNullOrWhiteSpace($entry.installPackage) -or
        [string]::IsNullOrWhiteSpace($entry.apiPackage) -or
        $entry.published -isnot [bool] -or
        $entry.latestStable -isnot [bool]) {
        throw "Version $($entry.version) contains an invalid required value."
    }
    if ($entry.installPackage -ne "Humanizer" -or
        $entry.apiPackage -notin @("Humanizer", "Humanizer.Core")) {
        throw "Version $($entry.version) declares an unsupported package identity."
    }
    if ($entry.referenceTfm -notmatch "^net(?:standard)?\d+(?:\.\d+)?$") {
        throw "Version $($entry.version) declares an invalid reference TFM."
    }
    if ($null -eq $entry.publicApiApproval.path -or
        [string]::IsNullOrWhiteSpace($entry.publicApiApproval.path) -or
        [System.IO.Path]::IsPathRooted($entry.publicApiApproval.path) -or
        $entry.publicApiApproval.path.Contains("\") -or
        $entry.publicApiApproval.path -match "(^|/)\.\.(/|$)" -or
        $entry.publicApiApproval.path -notmatch "\.(?:approved|verified)\.txt$") {
        throw "Version $($entry.version) declares invalid PublicAPI approval evidence."
    }
    $expectedGeneratedExtras = if ($entry.version -eq "2.10.1") {
        @(
            "Humanizer.ICulturedStringTransformer",
            "Humanizer.Localisation.DataUnit",
            "Humanizer.MetricNumeralFormats"
        )
    } else {
        @()
    }
    $generatedExtrasProperty = $entry.publicApiApproval.PSObject.Properties[
        "generatedExtraTypes"
    ]
    $rawDeclaredGeneratedExtras = if ($null -eq $generatedExtrasProperty) {
        @()
    } else {
        @($generatedExtrasProperty.Value)
    }
    $declaredGeneratedExtras = @(
        $rawDeclaredGeneratedExtras |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
            Sort-Object -Unique
    )
    if ($rawDeclaredGeneratedExtras.Count -ne $declaredGeneratedExtras.Count -or
        ($declaredGeneratedExtras -join "`n") -ne
        ($expectedGeneratedExtras -join "`n")) {
        throw "Version $($entry.version) declares invalid generated PublicAPI exceptions."
    }
    $exampleExcludedAssetsProperty = $entry.PSObject.Properties[
        "exampleExcludedAssets"
    ]
    $exampleExcludedAssets = if ($null -eq $exampleExcludedAssetsProperty) {
        @()
    } else {
        @($exampleExcludedAssetsProperty.Value)
    }
    $supportedExcludedAssets = @(
        "analyzers",
        "build",
        "buildTransitive"
    )
    if ($exampleExcludedAssets.Count -ne
        @($exampleExcludedAssets | Sort-Object -Unique).Count -or
        @(
            $exampleExcludedAssets |
                Where-Object { $_ -notin $supportedExcludedAssets }
        ).Count -gt 0) {
        throw "Version $($entry.version) declares invalid example asset exclusions."
    }
    if ($entry.route -ne "" -and
        ($entry.route -notmatch "^[a-z0-9][a-z0-9.-]*$" -or
            $entry.route.EndsWith("."))) {
        throw "Version $($entry.version) declares an invalid route."
    }

    $expectedOverlay = "version-overrides/$($entry.version)"
    if ($entry.compatibilityOverlay -ne $expectedOverlay) {
        throw "Version $($entry.version) must own overlay '$expectedOverlay'."
    }
    $overlayRoot = Resolve-SafePath `
        -Root $WebsiteRoot `
        -RelativePath $entry.compatibilityOverlay `
        -Description "Compatibility overlay"
    if (-not (Test-Path $overlayRoot -PathType Container)) {
        throw "Compatibility overlay is missing: $overlayRoot"
    }
    $overlayManifestPath = Join-Path $overlayRoot "overlay.json"
    if (-not (Test-Path $overlayManifestPath -PathType Leaf)) {
        throw "Compatibility overlay manifest is missing: $overlayManifestPath"
    }

    $overlay = Get-Content -Raw $overlayManifestPath |
        ConvertFrom-Json -Depth 20
    if ($overlay.schemaVersion -ne 1 -or
        $null -eq $overlay.PSObject.Properties["replacements"] -or
        $null -eq $overlay.PSObject.Properties["exclusions"]) {
        throw "Compatibility overlay for $($entry.version) has an invalid schema."
    }
    $replacements = @($overlay.replacements)
    $exclusions = @($overlay.exclusions)
    $duplicateOverlayPaths = @(
        @($replacements + $exclusions) |
            Group-Object |
            Where-Object Count -gt 1
    )
    if ($duplicateOverlayPaths.Count -gt 0) {
        throw "Compatibility overlay for $($entry.version) repeats a page."
    }

    foreach ($path in $replacements) {
        Assert-OverlayPath `
            -Path $path `
            -Description "Compatibility replacement"
        $canonicalPath = Resolve-SafePath `
            -Root (Join-Path $WebsiteRoot "docs") `
            -RelativePath $path `
            -Description "Canonical replacement target"
        $replacementPath = Resolve-SafePath `
            -Root $overlayRoot `
            -RelativePath $path `
            -Description "Compatibility replacement"
        if (-not (Test-Path $canonicalPath -PathType Leaf) -or
            -not (Test-Path $replacementPath -PathType Leaf)) {
            throw "Compatibility replacement '$path' is incomplete."
        }
        if ((Get-FileHash $canonicalPath -Algorithm SHA256).Hash -eq
            (Get-FileHash $replacementPath -Algorithm SHA256).Hash) {
            throw "Compatibility replacement '$path' is byte-identical to canonical content."
        }
    }
    foreach ($path in $exclusions) {
        Assert-OverlayPath `
            -Path $path `
            -Description "Compatibility exclusion"
        $canonicalPath = Resolve-SafePath `
            -Root (Join-Path $WebsiteRoot "docs") `
            -RelativePath $path `
            -Description "Canonical exclusion target"
        if (-not (Test-Path $canonicalPath -PathType Leaf)) {
            throw "Compatibility exclusion '$path' has no canonical page."
        }
    }

    $declaredOverlayFiles = @(
        "overlay.json"
        $replacements
    )
    $unexpectedOverlayFiles = @(
        Get-ChildItem $overlayRoot -File -Recurse |
            ForEach-Object {
                [System.IO.Path]::GetRelativePath(
                    $overlayRoot,
                    $_.FullName
                ).Replace("\", "/")
            } |
            Where-Object { $_ -notin $declaredOverlayFiles }
    )
    if ($unexpectedOverlayFiles.Count -gt 0) {
        throw "Compatibility overlay for $($entry.version) contains undeclared files."
    }

    if ($entry.version -eq "current") {
        if ($entry.source.kind -ne "checkout" -or
            $entry.source.tag -ne "main" -or
            $null -ne $entry.publicApiApproval.PSObject.Properties["tag"] -or
            $entry.route -ne "next" -or
            $entry.published -or
            $entry.latestStable -or
            $entry.apiPackage -ne "Humanizer") {
            throw "The current preview entry is not isolated correctly."
        }
    } else {
        if ($entry.version -notmatch "^\d+\.\d+\.\d+(?:\.\d+)?$" -or
            $entry.source.kind -ne "nuget" -or
            $entry.source.packageVersion -ne $entry.version -or
            $entry.publicApiApproval.tag -ne "v$($entry.version)" -or
            $entry.route -eq "next") {
            throw "Stable version $($entry.version) has invalid NuGet source metadata."
        }
    }

    $immutabilityProperty = $entry.PSObject.Properties["immutability"]
    if ($entry.version -ne "current" -and $entry.published) {
        if ($null -eq $immutabilityProperty -or
            $immutabilityProperty.Value.snapshotSha256 -notmatch "^[A-F0-9]{64}$" -or
            $immutabilityProperty.Value.sidebarSha256 -notmatch "^[A-F0-9]{64}$") {
            throw "Published version $($entry.version) is missing immutable artifact digests."
        }
    } elseif ($null -ne $immutabilityProperty) {
        throw "Unpublished version $($entry.version) cannot declare immutable artifact digests."
    }
}

foreach ($property in @("version", "label", "route")) {
    $duplicates = @(
        $versions |
            Group-Object $property |
            Where-Object Count -gt 1
    )
    if ($duplicates.Count -gt 0) {
        throw "Version manifest values for '$property' must be unique."
    }
}

$currentVersions = @($versions | Where-Object version -eq "current")
if ($currentVersions.Count -ne 1) {
    throw "Exactly one current preview entry is required."
}

$stableVersions = @($versions | Where-Object version -ne "current")
for ($index = 1; $index -lt $stableVersions.Count; $index++) {
    $previous = [System.Management.Automation.SemanticVersion]::Parse(
        $stableVersions[$index - 1].version
    )
    $current = [System.Management.Automation.SemanticVersion]::Parse(
        $stableVersions[$index].version
    )
    if ($previous -ge $current) {
        throw "Stable versions must be ordered from oldest to newest."
    }
}

$latest = @($stableVersions | Where-Object latestStable)
$publishedStableVersions = @($stableVersions | Where-Object published)
if ($latest.Count -ne 1 -or
    $latest[0].route -ne "" -or
    -not $latest[0].published -or
    $publishedStableVersions.Count -eq 0) {
    throw "Exactly one published stable version must own the empty route."
}
foreach ($entry in $stableVersions | Where-Object { -not $_.latestStable }) {
    if ($entry.route -ne $entry.version) {
        throw "Historical version $($entry.version) must use its version route."
    }
}

if (-not $SkipNativeState) {
    $nativeVersionsPath = Join-Path $WebsiteRoot "versions.json"
    if (-not (Test-Path $nativeVersionsPath -PathType Leaf)) {
        throw "Docusaurus versions.json is missing."
    }
    $nativeVersions = @(
        Get-Content -Raw $nativeVersionsPath |
            ConvertFrom-Json
    )
    $expectedNativeVersions = @(
        $stableVersions |
            Where-Object published |
            Sort-Object {
                [System.Management.Automation.SemanticVersion]::Parse(
                    $_.version
                )
            } -Descending |
            ForEach-Object version
    )
    if (($nativeVersions -join "`n") -ne
        ($expectedNativeVersions -join "`n")) {
        throw "Docusaurus versions.json does not match published manifest entries. Actual: $($nativeVersions -join ', '); expected: $($expectedNativeVersions -join ', ')."
    }

    foreach ($version in $nativeVersions) {
        $publishedEntry = @(
            $stableVersions | Where-Object version -eq $version
        )[0]
        $docsPath = Join-Path $WebsiteRoot "versioned_docs/version-$version"
        $sidebarsPath = Join-Path $WebsiteRoot "versioned_sidebars/version-$version-sidebars.json"
        if (-not (Test-Path $docsPath -PathType Container) -or
            -not (Test-Path $sidebarsPath -PathType Leaf)) {
            throw "Published snapshot $version is incomplete."
        }
        $snapshotDigest = Get-SnapshotDirectoryDigest -Path $docsPath
        $sidebarDigest = (Get-FileHash $sidebarsPath -Algorithm SHA256).Hash
        if ($snapshotDigest -ne $publishedEntry.immutability.snapshotSha256 -or
            $sidebarDigest -ne $publishedEntry.immutability.sidebarSha256) {
            throw "Published snapshot $version differs from its immutable artifact digests."
        }
    }

    $materializedVersions = @(
        Get-ChildItem (Join-Path $WebsiteRoot "versioned_docs") `
            -Directory `
            -Filter "version-*" |
            ForEach-Object { $_.Name.Substring("version-".Length) } |
            Sort-Object
    )
    if (($materializedVersions -join "`n") -ne
        (@($nativeVersions | Sort-Object) -join "`n")) {
        throw "Materialized snapshots do not match Docusaurus versions.json."
    }

    $materializedSidebars = @(
        Get-ChildItem (Join-Path $WebsiteRoot "versioned_sidebars") `
            -File `
            -Filter "version-*-sidebars.json" |
            ForEach-Object {
                $_.Name.Substring(
                    "version-".Length,
                    $_.Name.Length -
                        "version-".Length -
                        "-sidebars.json".Length
                )
            } |
            Sort-Object
    )
    if (($materializedSidebars -join "`n") -ne
        (@($nativeVersions | Sort-Object) -join "`n")) {
        throw "Materialized sidebars do not match Docusaurus versions.json."
    }
}

Write-Host "Documentation version manifest passed validation."
