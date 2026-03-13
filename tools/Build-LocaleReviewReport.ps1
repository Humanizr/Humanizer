param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path $PSScriptRoot -Parent
$reviewsDir = Join-Path $repoRoot "docs/reviews"
$localeReviewsDir = Join-Path $reviewsDir "locales"

$reviewFiles = Get-ChildItem $localeReviewsDir -Filter "*.review.json" | Sort-Object Name
$localeReviews = foreach ($file in $reviewFiles)
{
    $json = Get-Content $file.FullName -Raw
    if ($json.Length -gt 0 -and $json[0] -eq [char]0xFEFF)
    {
        $json = $json.Substring(1)
    }

    $json | ConvertFrom-Json
}

$defects = @()
$suspicious = @()

foreach ($review in $localeReviews)
{
    foreach ($record in @($review.records))
    {
        switch ($record.status)
        {
            "defect" { $defects += $record }
            "suspicious" { $suspicious += $record }
        }
    }

    $review.summary.ok = @($review.records | Where-Object { $_.status -eq "ok" }).Count
    $review.summary.suspicious = @($review.records | Where-Object { $_.status -eq "suspicious" }).Count
    $review.summary.defect = @($review.records | Where-Object { $_.status -eq "defect" }).Count
}

$nonOk = @($defects + $suspicious)
$crossLocaleThemes = @(
    $nonOk |
        Group-Object key |
        Where-Object { $_.Count -gt 1 } |
        Sort-Object Count -Descending |
        ForEach-Object {
            [ordered]@{
                key = $_.Name
                count = $_.Count
                locales = @($_.Group | ForEach-Object { $_.locale } | Sort-Object -Unique)
            }
        }
)

$aggregate = [ordered]@{
    generated = (Get-Date).ToString("yyyy-MM-dd")
    branch = (git -C $repoRoot branch --show-current).Trim()
    locale_count = $localeReviews.Count
    total_defects = $defects.Count
    total_suspicious = $suspicious.Count
    cross_locale_themes = $crossLocaleThemes
    locales = $localeReviews
}

$aggregate | ConvertTo-Json -Depth 100 | Set-Content -Encoding UTF8 (Join-Path $reviewsDir "2026-03-13-locale-adversarial-review.json")

$lines = [System.Collections.Generic.List[string]]::new()
$lines.Add("# Locale Adversarial Review")
$lines.Add("")
$lines.Add("Generated: $(Get-Date -Format 'yyyy-MM-dd')")
$lines.Add("")
$lines.Add("## Summary")
$lines.Add("- Total locales reviewed: $($localeReviews.Count)")
$lines.Add("- Defects: $($defects.Count)")
$lines.Add("- Suspicious items: $($suspicious.Count)")
$lines.Add("")
$lines.Add("## Cross-Locale Themes")

if ($crossLocaleThemes.Count -eq 0)
{
    $lines.Add("No repeated non-ok keys across multiple locales.")
}
else
{
    foreach ($theme in $crossLocaleThemes)
    {
        $localeList = [string]::Join(", ", $theme.locales)
        $lines.Add(('- `{0}`: {1} locales ({2})' -f $theme.key, $theme.count, $localeList))
    }
}

$lines.Add("")
$lines.Add("## Master Findings")

if ($defects.Count -eq 0 -and $suspicious.Count -eq 0)
{
    $lines.Add("No findings recorded.")
}
else
{
    foreach ($record in (@($defects + $suspicious) | Sort-Object locale, key))
    {
        $lines.Add(('- [{0}] `{1}`' -f $record.locale, $record.key))
        $lines.Add(("  Current: {0}" -f $record.current_value))
        $lines.Add(("  Proposed: {0}" -f $record.proposed_replacement))
        $lines.Add(("  Status: {0} / Severity: {1} / Confidence: {2}" -f $record.status, $record.severity, $record.confidence))
        $lines.Add(("  Rationale: {0}" -f $record.native_rationale))
        if ($record.evidence.Count -gt 0)
        {
            $lines.Add(("  Evidence: {0}" -f ([string]::Join("; ", $record.evidence))))
        }
        if ($record.notes)
        {
            $lines.Add(("  Notes: {0}" -f $record.notes))
        }
    }
}

$lines.Add("")
$lines.Add("## Per-Locale Summary")

foreach ($review in $localeReviews | Sort-Object locale)
{
    $lines.Add(('- `{0}`: {1} defect, {2} suspicious, {3} ok' -f $review.locale, $review.summary.defect, $review.summary.suspicious, $review.summary.ok))
}

$lines | Set-Content -Encoding UTF8 (Join-Path $reviewsDir "2026-03-13-locale-adversarial-review.md")
