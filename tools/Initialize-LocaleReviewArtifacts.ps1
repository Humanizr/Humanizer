param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path $PSScriptRoot -Parent
$propertiesDir = Join-Path $repoRoot "src/Humanizer/Properties"
$reviewsDir = Join-Path $repoRoot "docs/reviews"
$localeReviewsDir = Join-Path $reviewsDir "locales"
$testsDir = Join-Path $repoRoot "tests/Humanizer.Tests/Localisation"

$childCultureMap = @{
    "de" = @("de-CH", "de-LI")
    "fi" = @("fi-FI")
    "fil" = @("fil-PH")
    "fr" = @("fr-BE", "fr-CH")
    "ko" = @("ko-KR")
    "ms" = @("ms-MY")
    "nb" = @("nb-NO")
    "ro" = @("ro-Ro")
    "ru" = @("ru-RU")
    "th" = @("th-TH")
    "uk" = @("uk-UA")
    "zh-Hant" = @("zh-HK")
}

$headingKeys = @(
    "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE",
    "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW",
    "N_Short", "NNE_Short", "NE_Short", "ENE_Short", "E_Short", "ESE_Short", "SE_Short", "SSE_Short",
    "S_Short", "SSW_Short", "SW_Short", "WSW_Short", "W_Short", "WNW_Short", "NW_Short", "NNW_Short"
)

function Get-ResxMap([string] $path)
{
    $xml = [xml](Get-Content $path)
    $map = @{}

    foreach ($node in @($xml.root.data))
    {
        $map[$node.name] = $node.value
    }

    return $map
}

function Get-SurfaceFamily([string] $key)
{
    if ($key.StartsWith("DateHumanize_"))
    {
        return "DateHumanize"
    }

    if ($key.StartsWith("TimeSpanHumanize_"))
    {
        return "TimeSpanHumanize"
    }

    if ($key.StartsWith("TimeUnit_"))
    {
        return "TimeUnit"
    }

    if ($key.StartsWith("DataUnit_"))
    {
        return "DataUnit"
    }

    if ($headingKeys -contains $key)
    {
        return "Heading"
    }

    return "Other"
}

function Get-RelativePath([string] $basePath, [string] $targetPath)
{
    $baseUri = New-Object System.Uri(($basePath.TrimEnd('\') + '\'))
    $targetUri = New-Object System.Uri($targetPath)
    return [System.Uri]::UnescapeDataString($baseUri.MakeRelativeUri($targetUri).ToString().Replace('/', '\'))
}

New-Item -ItemType Directory -Force -Path $reviewsDir | Out-Null
New-Item -ItemType Directory -Force -Path $localeReviewsDir | Out-Null

$neutralMap = Get-ResxMap (Join-Path $propertiesDir "Resources.resx")
$localeFiles = Get-ChildItem $propertiesDir -Filter "Resources.*.resx" |
    Where-Object { $_.Name -ne "Resources.resx" } |
    Sort-Object Name

$manifest = [System.Collections.Generic.List[object]]::new()

foreach ($localeFile in $localeFiles)
{
    $locale = $localeFile.BaseName.Replace("Resources.", "")
    $localeMap = Get-ResxMap $localeFile.FullName
    $ownedChildren = @()

    if ($childCultureMap.ContainsKey($locale))
    {
        $ownedChildren = $childCultureMap[$locale]
    }

    $records = foreach ($key in ($localeMap.Keys | Sort-Object))
    {
        [ordered]@{
            locale = $locale
            key = $key
            current_value = $localeMap[$key]
            neutral_reference = if ($neutralMap.ContainsKey($key)) { $neutralMap[$key] } else { $null }
            surface_family = Get-SurfaceFamily $key
            status = "ok"
            severity = "P3"
            confidence = "high"
            proposed_replacement = $localeMap[$key]
            replacement_confidence = "high"
            native_rationale = "Accepted pending native review."
            evidence = @()
            notes = ""
        }
    }

    $review = [ordered]@{
        locale = $locale
        resource_file = Get-RelativePath $repoRoot $localeFile.FullName
        owned_child_cultures = $ownedChildren
        direct_test_directories = @(
            if (Test-Path (Join-Path $testsDir $locale)) { Get-RelativePath $repoRoot (Join-Path $testsDir $locale) }
        ) | Where-Object { $_ }
        review_status = "pending"
        summary = [ordered]@{
            total_keys = $records.Count
            ok = $records.Count
            suspicious = 0
            defect = 0
        }
        records = $records
    }

    $reviewPath = Join-Path $localeReviewsDir "$locale.review.json"
    $review | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 $reviewPath

    $manifest.Add([ordered]@{
        locale = $locale
        resource_file = $review.resource_file
        owned_child_cultures = $ownedChildren
        total_keys = $records.Count
        review_path = Get-RelativePath $repoRoot $reviewPath
    })
}

$aggregate = [ordered]@{
    generated = (Get-Date).ToString("yyyy-MM-dd")
    branch = (git -C $repoRoot branch --show-current).Trim()
    locale_count = $manifest.Count
    locales = $manifest
}

$aggregate | ConvertTo-Json -Depth 6 | Set-Content -Encoding UTF8 (Join-Path $reviewsDir "2026-03-13-locale-adversarial-review.json")

$markdown = @"
# Locale Adversarial Review

Generated: $(Get-Date -Format 'yyyy-MM-dd')

## Summary
- Total locales reviewed: $($manifest.Count)
- Defects: pending
- Suspicious items: pending

## Findings
Pending aggregation.
"@

$markdown | Set-Content -Encoding UTF8 (Join-Path $reviewsDir "2026-03-13-locale-adversarial-review.md")
