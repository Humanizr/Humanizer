param(
    [string]$ManifestPath,
    [string]$UpgradeDataPath
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
$websiteRoot = Join-Path $repoRoot "website"
. (Join-Path $PSScriptRoot "nuget-package.ps1")
if (-not $ManifestPath) {
    $ManifestPath = Join-Path $websiteRoot "humanizer-versions.json"
}
if (-not $UpgradeDataPath) {
    $UpgradeDataPath = Join-Path $websiteRoot "docs/upgrading/boundaries.json"
}

function Assert-EqualSequence {
    param(
        [Parameter(Mandatory = $true)][object[]]$Actual,
        [Parameter(Mandatory = $true)][object[]]$Expected,
        [Parameter(Mandatory = $true)][string]$Description
    )

    if (($Actual -join "`n") -ne ($Expected -join "`n")) {
        throw "$Description. Expected '$($Expected -join ", ")'; found '$($Actual -join ", ")'."
    }
}

function Assert-UpgradeChain {
    param(
        [Parameter(Mandatory = $true)]$Data,
        [Parameter(Mandatory = $true)][string[]]$ManifestVersions,
        [Parameter(Mandatory = $true)][string]$DocsRoot
    )

    if ($Data.schemaVersion -ne 1) {
        throw "Unsupported upgrade-boundary schema."
    }

    $boundaries = @($Data.boundaries)
    if ($boundaries.Count -eq 0) {
        throw "The upgrade boundary sequence is empty."
    }

    $versions = @($boundaries[0].from)
    $versions += @($boundaries | ForEach-Object to)
    Assert-EqualSequence `
        -Actual $versions `
        -Expected $ManifestVersions `
        -Description "Upgrade versions do not match the stable manifest"

    for ($index = 0; $index -lt $boundaries.Count; $index++) {
        $boundary = $boundaries[$index]
        if ($index -gt 0 -and
            $boundary.from -ne $boundaries[$index - 1].to) {
            throw "Upgrade boundary $index does not connect adjacent supported versions."
        }
        foreach ($property in @("title", "summary", "guide")) {
            if ([string]::IsNullOrWhiteSpace($boundary.$property)) {
                throw "Upgrade boundary $($boundary.from) -> $($boundary.to) is missing '$property'."
            }
        }
        if ($boundary.guide -notmatch "^\./([a-z0-9-]+)(?:#([a-z0-9-]+))?$") {
            throw "Upgrade guide must be a local version-preserving route: $($boundary.guide)"
        }
        $guideName = $Matches[1]
        $fragment = $Matches[2]
        $guidePath = Join-Path $DocsRoot "$guideName.mdx"
        if (-not (Test-Path $guidePath -PathType Leaf)) {
            throw "Upgrade guide does not exist: $guidePath"
        }
        if ($fragment) {
            $guide = Get-Content -Raw $guidePath
            $escapedFragment = [regex]::Escape($fragment)
            if ($guide -notmatch "(?m)^#{1,6}\s+.+\{#$escapedFragment\}\s*$") {
                throw "Upgrade guide fragment does not resolve to an explicit heading ID: $($boundary.guide)"
            }
        }
    }
}

function Get-NuGetPackageEvidence {
    param([Parameter(Mandatory = $true)]$Entry)

    $package = Get-DocsNuGetPackage -Entry $Entry -RepoRoot $repoRoot
    $entries = @(
        Get-ChildItem $package.ExtractPath -Recurse -File |
            ForEach-Object {
                [System.IO.Path]::GetRelativePath(
                    $package.ExtractPath,
                    $_.FullName
                ).Replace('\', '/')
            }
    )
    $targetsPath = Join-Path $package.ExtractPath "buildTransitive/Humanizer.Core.targets"
    $xmlPath = Join-Path $package.ExtractPath "lib/$($Entry.referenceTfm)/Humanizer.xml"
    if (-not (Test-Path $xmlPath -PathType Leaf)) {
        throw "Published API XML is missing for $($Entry.version): $xmlPath"
    }
    $targets = if (Test-Path $targetsPath -PathType Leaf) {
        Get-Content -Raw $targetsPath
    } else {
        ""
    }

    return [PSCustomObject]@{
        Entries = $entries
        Targets = $targets
        Xml = Get-Content -Raw $xmlPath
    }
}

function Get-ManifestVersion {
    param(
        [Parameter(Mandatory = $true)]$Manifest,
        [Parameter(Mandatory = $true)][string]$Version
    )

    $entries = @($Manifest.versions | Where-Object version -eq $Version)
    if ($entries.Count -ne 1) {
        throw "Expected exactly one manifest entry for $Version."
    }
    return $entries[0]
}

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
$data = Get-Content -Raw $UpgradeDataPath | ConvertFrom-Json -Depth 20
$stableManifestVersions = @(
    $manifest.versions |
        Where-Object version -ne "current" |
        ForEach-Object version
)
$upgradeDocsRoot = Join-Path $websiteRoot "docs/upgrading"
Assert-UpgradeChain `
    -Data $data `
    -ManifestVersions $stableManifestVersions `
    -DocsRoot $upgradeDocsRoot

$missingBoundary = $data | ConvertTo-Json -Depth 20 | ConvertFrom-Json -Depth 20
$missingBoundary.boundaries = @($missingBoundary.boundaries | Select-Object -Skip 1)
$missingBoundaryRejected = $false
try {
    Assert-UpgradeChain `
        -Data $missingBoundary `
        -ManifestVersions $stableManifestVersions `
        -DocsRoot $upgradeDocsRoot
} catch {
    $missingBoundaryRejected = $true
}
if (-not $missingBoundaryRejected) {
    throw "Upgrade graph validation accepted a missing intermediate boundary."
}

$wrongTarget = $data | ConvertTo-Json -Depth 20 | ConvertFrom-Json -Depth 20
$wrongTarget.boundaries[0].to = $wrongTarget.boundaries[1].to
$wrongTargetRejected = $false
try {
    Assert-UpgradeChain `
        -Data $wrongTarget `
        -ManifestVersions $stableManifestVersions `
        -DocsRoot $upgradeDocsRoot
} catch {
    $wrongTargetRejected = $true
}
if (-not $wrongTargetRejected) {
    throw "Upgrade graph validation accepted a non-adjacent boundary."
}

$badFragment = $data | ConvertTo-Json -Depth 20 | ConvertFrom-Json -Depth 20
$badFragment.boundaries[0].guide = "./2-x-lines#missing-boundary"
$badFragmentRejected = $false
try {
    Assert-UpgradeChain `
        -Data $badFragment `
        -ManifestVersions $stableManifestVersions `
        -DocsRoot $upgradeDocsRoot
} catch {
    $badFragmentRejected = $true
}
if (-not $badFragmentRejected) {
    throw "Upgrade graph validation accepted an unresolved guide fragment."
}

$analyzerSource = Get-Content -Raw (
    Join-Path $repoRoot "src/Humanizer.Analyzers/NamespaceMigrationAnalyzer.cs"
)
if ($analyzerSource -notmatch 'DiagnosticSeverity\.Error' -or
    $analyzerSource -notmatch 'isEnabledByDefault:\s*true') {
    throw "HUMANIZER001 source severity no longer matches the consumer guidance."
}
$analyzerGuide = Get-Content -Raw (
    Join-Path $websiteRoot "docs/analyzer/index.mdx"
)
if ($analyzerGuide -notmatch 'HUMANIZER001' -or
    $analyzerGuide -notmatch 'severity `error`') {
    throw "Analyzer guidance does not state the built HUMANIZER001 severity."
}

$package301 = Get-NuGetPackageEvidence -Entry (
    Get-ManifestVersion -Manifest $manifest -Version "3.0.1"
)
$package308 = Get-NuGetPackageEvidence -Entry (
    Get-ManifestVersion -Manifest $manifest -Version "3.0.8"
)
$package3010 = Get-NuGetPackageEvidence -Entry (
    Get-ManifestVersion -Manifest $manifest -Version "3.0.10"
)
$analyzerPattern = "^analyzers/dotnet/roslyn[^/]+/cs/Humanizer\.Analyzers\.dll$"
Assert-EqualSequence `
    -Actual @($package301.Entries | Where-Object { $_ -match $analyzerPattern } | Sort-Object) `
    -Expected @("analyzers/dotnet/roslyn3.8/cs/Humanizer.Analyzers.dll") `
    -Description "Humanizer.Core 3.0.1 analyzer assets changed"
Assert-EqualSequence `
    -Actual @($package308.Entries | Where-Object { $_ -match $analyzerPattern } | Sort-Object) `
    -Expected @(
        "analyzers/dotnet/roslyn3.8/cs/Humanizer.Analyzers.dll"
        "analyzers/dotnet/roslyn4.14/cs/Humanizer.Analyzers.dll"
        "analyzers/dotnet/roslyn4.8/cs/Humanizer.Analyzers.dll"
    ) `
    -Description "Humanizer.Core 3.0.8 analyzer assets changed"
Assert-EqualSequence `
    -Actual @($package3010.Entries | Where-Object { $_ -match $analyzerPattern } | Sort-Object) `
    -Expected @(
        "analyzers/dotnet/roslyn3.8/cs/Humanizer.Analyzers.dll"
        "analyzers/dotnet/roslyn4.14/cs/Humanizer.Analyzers.dll"
        "analyzers/dotnet/roslyn4.8/cs/Humanizer.Analyzers.dll"
    ) `
    -Description "Humanizer.Core 3.0.10 analyzer assets changed"

$toQuantitySignature = "ToQuantity(System.String,System.Int32"
if ($package301.Xml.Contains($toQuantitySignature) -or
    -not $package308.Xml.Contains($toQuantitySignature) -or
    -not $package3010.Xml.Contains($toQuantitySignature)) {
    throw "The 3.0.8 ToQuantity compatibility boundary no longer matches published XML documentation."
}
if ($package308.Targets -notmatch "<ItemGroup" -or
    $package308.Targets -match "SelectHumanizerAnalyzerFallback" -or
    $package3010.Targets -notmatch "SelectHumanizerAnalyzerFallback") {
    throw "The 3.0.10 analyzer fallback boundary no longer matches published package targets."
}

$u5Files = @(
    Get-ChildItem (Join-Path $websiteRoot "docs/upgrading") -File
    Get-ChildItem (Join-Path $websiteRoot "docs/analyzer") -File
)
$prohibitedProductName = @("Flow", "Next") -join " "
foreach ($file in $u5Files) {
    if ((Get-Content -Raw $file.FullName).Contains($prohibitedProductName)) {
        throw "Prohibited product wording found in $($file.FullName)."
    }
}

Write-Host "Upgrade paths passed validation for $($stableManifestVersions -join ', ')."
