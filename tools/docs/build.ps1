param(
    [string]$Version,
    [switch]$ValidateOnly,
    [string]$ManifestPath
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
}

$latest = @($versions | Where-Object latestStable)
if ($latest.Count -ne 1 -or $latest[0].route -ne "") {
    throw "Exactly one latest stable version must own the empty route."
}

$preview = @($versions | Where-Object version -eq "current")
if ($preview.Count -ne 1 -or $preview[0].route -ne "next" -or $preview[0].published) {
    throw "The unpublished current version must own the next route."
}

$duplicateVersions = @($versions | Group-Object version | Where-Object Count -gt 1)
$duplicateRoutes = @($versions | Group-Object route | Where-Object Count -gt 1)
if ($duplicateVersions.Count -gt 0 -or $duplicateRoutes.Count -gt 0) {
    throw "Version names and routes must be unique."
}

if ($Version -and -not ($versions.version -contains $Version)) {
    throw "Version '$Version' is not declared."
}

$websiteRoot = Join-Path $repoRoot "website"
$nativeVersions = @(Get-Content -Raw (Join-Path $websiteRoot "versions.json") | ConvertFrom-Json)
if ($nativeVersions.Count -ne 1 -or $nativeVersions[0] -ne $latest[0].version) {
    throw "The U1 native Docusaurus snapshot must contain only the latest stable version."
}

$proofRoots = @(
    (Join-Path $websiteRoot "docs"),
    (Join-Path $websiteRoot "versioned_docs/version-$($latest[0].version)")
)
foreach ($proofRoot in $proofRoots) {
    $guidePath = Join-Path $proofRoot "proof.md"
    $apiPath = Join-Path $proofRoot "api/Humanizer.StringHumanizeExtensions.md"
    if (-not (Test-Path $guidePath -PathType Leaf) -or -not (Test-Path $apiPath -PathType Leaf)) {
        throw "The versioned guide-to-API proof is incomplete under $proofRoot."
    }

    $guide = Get-Content -Raw $guidePath
    if ($guide -notmatch "\(\./api/Humanizer\.StringHumanizeExtensions\.md\)") {
        throw "The guide under $proofRoot does not use a relative same-version API link."
    }
}

if (-not (Test-Path (Join-Path $websiteRoot "static/.nojekyll") -PathType Leaf)) {
    throw "The Pages artifact input is missing .nojekyll."
}

if (-not $ValidateOnly) {
    throw "U1 only supports fail-closed validation; snapshot mutation belongs to U2."
}

Write-Host "Manifest, single-plugin version routes, relative API links, and Pages inputs passed validation."
