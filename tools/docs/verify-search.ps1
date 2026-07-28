param(
    [string]$BuildDirectory,
    [string]$RecordVersion
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
. (Join-Path $PSScriptRoot "snapshot-state.ps1")
if (-not $BuildDirectory) {
    $BuildDirectory = Join-Path $repoRoot "website/build"
}

$manifest = Get-Content -Raw (Join-Path $repoRoot "website/humanizer-versions.json") |
    ConvertFrom-Json -Depth 20
$baselinePath = Join-Path $repoRoot "website/search-index-baselines.json"
$baselines = Get-Content -Raw $baselinePath |
    ConvertFrom-Json -Depth 20
$latest = @($manifest.versions | Where-Object latestStable)
$preview = @($manifest.versions | Where-Object version -eq "current")
$searchableVersions = @(
    $manifest.versions |
        Where-Object { $_.published -or $_.version -eq "current" }
)
if ($latest.Count -ne 1 -or $preview.Count -ne 1 -or $baselines.schemaVersion -ne 1) {
    throw "Search verification metadata is invalid."
}

if (-not (Test-Path $BuildDirectory -PathType Container)) {
    throw "Docusaurus build output not found: $BuildDirectory"
}

$allSearchDirectory = Join-Path $BuildDirectory "pagefind"
$recordBaseline = $false
if ($RecordVersion) {
    $recordEntries = @(
        $manifest.versions |
            Where-Object version -eq $RecordVersion
    )
    $existingBaseline = $baselines.versions.PSObject.Properties[$RecordVersion]
    $recordIndex = Join-Path $BuildDirectory (
        "search-index-docs-default-$RecordVersion.json"
    )
    if ($recordEntries.Count -ne 1 -or
        $RecordVersion -eq "current" -or
        -not $recordEntries[0].published) {
        throw "A search baseline can be recorded only for one published stable version."
    }
    if ($null -ne $existingBaseline) {
        throw "Search baseline already exists for $RecordVersion."
    }
    if (-not (Test-Path $recordIndex -PathType Leaf) -or
        -not (Test-Path $allSearchDirectory -PathType Container)) {
        throw "Build search output is incomplete for $RecordVersion."
    }

    $releaseTag = [string]$recordEntries[0].publicApiApproval.tag
    if ($releaseTag -ne "v$RecordVersion") {
        throw "Search baseline release tag is missing or malformed for $RecordVersion."
    }
    $tagCommit = @(
        & git -C $repoRoot rev-parse --verify "$releaseTag^{commit}" 2>$null
    )
    if ($LASTEXITCODE -ne 0 -or
        $tagCommit.Count -ne 1 -or
        $tagCommit[0] -notmatch "^[0-9a-fA-F]{40,64}$") {
        throw "Search baseline release tag is unavailable: $releaseTag"
    }
    $reviewedAt = @(
        & git -C $repoRoot show -s --format=%cs $tagCommit[0] 2>$null
    )
    $parsedReviewDate = [DateTime]::MinValue
    if ($LASTEXITCODE -ne 0 -or
        $reviewedAt.Count -ne 1 -or
        -not [DateTime]::TryParseExact(
            $reviewedAt[0],
            "yyyy-MM-dd",
            [System.Globalization.CultureInfo]::InvariantCulture,
            [System.Globalization.DateTimeStyles]::None,
            [ref]$parsedReviewDate
        )) {
        throw "Search baseline release commit date is missing or malformed: $releaseTag"
    }

    $allSearchSize = (
        Get-ChildItem $allSearchDirectory -Recurse -File |
            Measure-Object -Property Length -Sum
    ).Sum
    $baselines.versions |
        Add-Member `
            -NotePropertyName $RecordVersion `
            -NotePropertyValue (Get-Item $recordIndex).Length
    $baselines.allVersionsBytes = $allSearchSize
    $baselines.reviewedAt = $reviewedAt[0]
    $baselines.reviewedCorpus = (
        "Humanizer $RecordVersion immutable snapshot and every manifest " +
        "context; Pagefind indexed the complete corpus."
    )
    $recordBaseline = $true
}

$indexes = @($searchableVersions | ForEach-Object {
    $routeSegment = if ($_.route) { "$($_.route)/" } else { "" }
    @{
        Version = $_.label
        ManifestVersion = $_.version
        Path = Join-Path $BuildDirectory "search-index-docs-default-$($_.version).json"
        RequiredRoute = "/docs/${routeSegment}start/quick-start"
        RequiredApiRoute = "/docs/${routeSegment}api/Humanizer.StringHumanizeExtensions/"
    }
})

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

& node (Join-Path $repoRoot "website/scripts/verify-contextual-search.mjs") $BuildDirectory
if ($LASTEXITCODE -ne 0) {
    throw "Contextual search query verification failed."
}

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

$versionedPages = @($searchableVersions | ForEach-Object {
    $relativePath = if ($_.route) {
        "docs/$($_.route)/start/quick-start/index.html"
    }
    else {
        "docs/start/quick-start/index.html"
    }
    @{
        Label = $_.label
        Path = Join-Path $BuildDirectory $relativePath
    }
})
foreach ($page in $versionedPages) {
    $html = Get-Content -Raw $page.Path
    if ($html -notmatch 'data-pagefind-filter="version"' -or
        $html -notmatch [Regex]::Escape($page.Label) -or
        $html -notmatch 'class="humanizerAllVersionsTrigger"' -or
        $html -notmatch '<pagefind-modal reset-on-close>' -or
        $html -match '<(?:link|script)[^>]+/pagefind/pagefind-component-ui\.(?:css|js)') {
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
Write-Host "All-version search passed: $($searchableVersions.Count) versions ($allSearchSize / $maximumAllSearchSize bytes)"

$redirectPath = Join-Path $BuildDirectory "quick-start/index.html"
if (-not (Test-Path $redirectPath -PathType Leaf) -or
    (Get-Content -Raw $redirectPath) -notmatch "/docs/start/quick-start") {
    throw "The static quick-start redirect page is missing or invalid."
}

if (-not (Test-Path (Join-Path $BuildDirectory ".nojekyll") -PathType Leaf)) {
    throw "The local Pages artifact is missing .nojekyll."
}

if ($recordBaseline) {
    Write-SnapshotJson -Value $baselines -Path $baselinePath
    Write-Host "Recorded reviewed search baselines for $RecordVersion."
}

Write-Host "All manifest contextual indexes, lazy all-version data, budgets, and Pages artifact proof passed."
