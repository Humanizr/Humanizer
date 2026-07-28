param(
    [string]$BuildDirectory
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
if (-not $BuildDirectory) {
    $BuildDirectory = Join-Path $repoRoot "website/build"
}

$manifest = Get-Content -Raw (Join-Path $repoRoot "website/humanizer-versions.json") |
    ConvertFrom-Json -Depth 20
$baselines = Get-Content -Raw (Join-Path $repoRoot "website/search-index-baselines.json") |
    ConvertFrom-Json -Depth 20
$latest = @($manifest.versions | Where-Object latestStable)
$preview = @($manifest.versions | Where-Object version -eq "current")
if ($latest.Count -ne 1 -or $preview.Count -ne 1 -or $baselines.schemaVersion -ne 1) {
    throw "Search verification metadata is invalid."
}

if (-not (Test-Path $BuildDirectory -PathType Container)) {
    throw "Docusaurus build output not found: $BuildDirectory"
}

$indexes = @(
    @{
        Version = $latest[0].label
        ManifestVersion = $latest[0].version
        Path = Join-Path $BuildDirectory "search-index-docs-default-$($latest[0].version).json"
        RequiredRoute = "/docs/$($latest[0].route)start/quick-start"
        RequiredApiRoute = "/docs/$($latest[0].route)api/Humanizer.StringHumanizeExtensions/"
        ForbiddenRoute = "/docs/$($preview[0].route)/"
    },
    @{
        Version = $preview[0].label
        ManifestVersion = $preview[0].version
        Path = Join-Path $BuildDirectory "search-index-docs-default-current.json"
        RequiredRoute = "/docs/$($preview[0].route)/start/quick-start"
        RequiredApiRoute = "/docs/$($preview[0].route)/api/Humanizer.StringHumanizeExtensions/"
        ForbiddenRoute = "/docs/$($latest[0].route)start/quick-start"
    }
)

foreach ($index in $indexes) {
    if (-not (Test-Path $index.Path -PathType Leaf)) {
        throw "Search index for $($index.Version) is missing: $($index.Path)"
    }

    $data = Get-Content -Raw $index.Path | ConvertFrom-Json -Depth 100
    $routes = @($data.documents.sectionRoute)
    if (-not ($routes | Where-Object { $_ -like "$($index.RequiredRoute)*" })) {
        throw "Search index for $($index.Version) does not contain its quick-start route."
    }
    if (-not ($routes | Where-Object { $_ -like "$($index.RequiredApiRoute)*" })) {
        throw "Search index for $($index.Version) does not contain its generated API route."
    }
    if ($routes | Where-Object { $_ -like "$($index.ForbiddenRoute)*" }) {
        throw "Search index for $($index.Version) contains another version's route."
    }

    $size = (Get-Item $index.Path).Length
    $baselineProperty = $baselines.versions.PSObject.Properties[$index.ManifestVersion]
    if ($null -eq $baselineProperty -or $baselineProperty.Value -le 0) {
        throw "Search index for $($index.Version) has no positive reviewed baseline."
    }

    $maximumSize = [Math]::Floor(
        $baselineProperty.Value * (1 + ($baselines.maximumIncreasePercent / 100))
    )
    if ($size -gt $maximumSize) {
        throw "Search index for $($index.Version) grew beyond its reviewed budget: $size > $maximumSize bytes."
    }

    Write-Host "Contextual search index passed: $($index.Version) ($size / $maximumSize bytes)"
}

$allSearchDirectory = Join-Path $BuildDirectory "pagefind"
$requiredAllSearchAssets = @(
    "pagefind.js",
    "pagefind-entry.json",
    "pagefind-component-ui.js",
    "pagefind-component-ui.css"
)
foreach ($asset in $requiredAllSearchAssets) {
    $assetPath = Join-Path $allSearchDirectory $asset
    if (-not (Test-Path $assetPath -PathType Leaf)) {
        throw "All-version search asset is missing: $assetPath"
    }
}

$versionedPages = @(
    @{
        Label = $latest[0].label
        Path = Join-Path $BuildDirectory "docs/$($latest[0].route)start/quick-start/index.html"
    },
    @{
        Label = $preview[0].label
        Path = Join-Path $BuildDirectory "docs/$($preview[0].route)/start/quick-start/index.html"
    }
)
foreach ($page in $versionedPages) {
    $html = Get-Content -Raw $page.Path
    if ($html -notmatch 'data-pagefind-filter="version"' -or
        $html -notmatch [Regex]::Escape($page.Label) -or
        $html -notmatch "<pagefind-modal-trigger") {
        throw "All-version search metadata is invalid for $($page.Label)."
    }
}

& node (Join-Path $repoRoot "website/scripts/verify-pagefind.mjs") $BuildDirectory
if ($LASTEXITCODE -ne 0) {
    throw "All-version search query verification failed."
}

$allSearchSize = (
    Get-ChildItem $allSearchDirectory -Recurse -File |
        Measure-Object -Property Length -Sum
).Sum
if ($baselines.allVersionsBytes -le 0) {
    throw "All-version search has no positive reviewed baseline."
}
$maximumAllSearchSize = [Math]::Floor(
    $baselines.allVersionsBytes * (1 + ($baselines.maximumIncreasePercent / 100))
)
if ($allSearchSize -gt $maximumAllSearchSize) {
    throw "All-version search grew beyond its reviewed budget: $allSearchSize > $maximumAllSearchSize bytes."
}
Write-Host "All-version search passed: stable + preview ($allSearchSize / $maximumAllSearchSize bytes)"

$redirectPath = Join-Path $BuildDirectory "quick-start/index.html"
if (-not (Test-Path $redirectPath -PathType Leaf) -or
    (Get-Content -Raw $redirectPath) -notmatch "/docs/start/quick-start") {
    throw "The static quick-start redirect page is missing or invalid."
}

if (-not (Test-Path (Join-Path $BuildDirectory ".nojekyll") -PathType Leaf)) {
    throw "The local Pages artifact is missing .nojekyll."
}

Write-Host "Search isolation, cross-version indexing, static redirect, and Pages artifact proof passed."
