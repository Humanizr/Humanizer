param(
    [string]$Version,
    [switch]$ValidateOnly,
    [ValidateSet("Validate")][string]$Mode,
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

& (Join-Path $PSScriptRoot "verify-manifest.ps1") `
    -ManifestPath $ManifestPath

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
$versions = @($manifest.versions)
$latest = @($versions | Where-Object latestStable)

if ($Version -and -not ($versions.version -contains $Version)) {
    throw "Version '$Version' is not declared."
}

$websiteRoot = Join-Path $repoRoot "website"
$proofRoots = @(
    (Join-Path $websiteRoot "docs"),
    (Join-Path $websiteRoot "versioned_docs/version-$($latest[0].version)")
)
foreach ($proofRoot in $proofRoots) {
    $guidePath = Join-Path $proofRoot "proof.mdx"
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

if (-not $ValidateOnly -and $Mode -ne "Validate") {
    throw "Use -ValidateOnly for structural checks or -Mode Validate for the complete documentation gate."
}

if ($Mode -eq "Validate") {
    foreach ($entry in $versions | Where-Object published) {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version $entry.version `
            -Check `
            -ManifestPath $ManifestPath
    }
    & (Join-Path $PSScriptRoot "verify-api.ps1") `
        -All `
        -ManifestPath $ManifestPath
    & (Join-Path $PSScriptRoot "verify-examples.ps1") `
        -All `
        -ManifestPath $ManifestPath
}

Write-Host "Documentation inputs and generated outputs passed validation."
