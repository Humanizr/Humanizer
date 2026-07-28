param(
    [string]$Version,
    [switch]$ValidateOnly,
    [ValidateSet("Validate")][string]$Mode,
    [string]$ManifestPath
)

$ErrorActionPreference = "Stop"
& (Join-Path $PSScriptRoot "generate-language-coverage.ps1") -Check
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
foreach ($root in @(
    (Join-Path $websiteRoot "docs"),
    (Join-Path $websiteRoot "versioned_docs/version-$($latest[0].version)")
)) {
    foreach ($required in @(
        "start",
        "scenarios",
        "upgrading",
        "languages",
        "api/index.md",
        "api/Humanizer.StringHumanizeExtensions.md"
    )) {
        if (-not (Test-Path (Join-Path $root $required))) {
            throw "The documentation root is incomplete under $root`: $required"
        }
    }
}

if (-not (Test-Path (Join-Path $websiteRoot "static/.nojekyll") -PathType Leaf)) {
    throw "The Pages artifact input is missing .nojekyll."
}

if (-not $ValidateOnly -and $Mode -ne "Validate") {
    throw "Use -ValidateOnly for structural checks or -Mode Validate for the complete documentation gate."
}

if ($Mode -eq "Validate") {
    & (Join-Path $PSScriptRoot "snapshot.ps1") `
        -All `
        -Check `
        -ManifestPath $ManifestPath
    & (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version current `
        -Check `
        -ManifestPath $ManifestPath
    & (Join-Path $PSScriptRoot "verify-examples.ps1") `
        -Version current `
        -ManifestPath $ManifestPath
    & (Join-Path $PSScriptRoot "verify-upgrade-paths.ps1") `
        -ManifestPath $ManifestPath
}

Write-Host "Documentation inputs and generated outputs passed validation."
