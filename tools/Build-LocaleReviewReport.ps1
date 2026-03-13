param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path $PSScriptRoot -Parent
$reviewsDir = Join-Path $repoRoot "docs/reviews"
$localeReviewsDir = Join-Path $reviewsDir "locales"

$reviewFiles = Get-ChildItem $localeReviewsDir -Filter "*.review.json" | Sort-Object Name
$localeReviews = foreach ($file in $reviewFiles)
{
    Get-Content $file.FullName -Raw | ConvertFrom-Json -Depth 100
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

$aggregate = [ordered]@{
    generated = (Get-Date).ToString("yyyy-MM-dd")
    branch = (git -C $repoRoot branch --show-current).Trim()
    locale_count = $localeReviews.Count
    total_defects = $defects.Count
    total_suspicious = $suspicious.Count
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
$lines.Add("## Master Findings")

if ($defects.Count -eq 0 -and $suspicious.Count -eq 0)
{
    $lines.Add("No findings recorded.")
}
else
{
    foreach ($record in (@($defects + $suspicious) | Sort-Object locale, key))
    {
        $lines.Add("- [$($record.locale)] `$($record.key)`")
        $lines.Add("  Current: $($record.current_value)")
        $lines.Add("  Proposed: $($record.proposed_replacement)")
        $lines.Add("  Status: $($record.status) / Severity: $($record.severity) / Confidence: $($record.confidence)")
        $lines.Add("  Rationale: $($record.native_rationale)")
        if ($record.evidence.Count -gt 0)
        {
            $lines.Add("  Evidence: $([string]::Join('; ', $record.evidence))")
        }
        if ($record.notes)
        {
            $lines.Add("  Notes: $($record.notes)")
        }
    }
}

$lines.Add("")
$lines.Add("## Per-Locale Summary")

foreach ($review in $localeReviews | Sort-Object locale)
{
    $lines.Add("- `$( $review.locale )`: $($review.summary.defect) defect, $($review.summary.suspicious) suspicious, $($review.summary.ok) ok")
}

$lines | Set-Content -Encoding UTF8 (Join-Path $reviewsDir "2026-03-13-locale-adversarial-review.md")
