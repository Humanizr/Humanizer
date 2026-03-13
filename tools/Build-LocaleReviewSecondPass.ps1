param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path $PSScriptRoot -Parent
$adjudicationDir = Join-Path $repoRoot "docs/reviews/second-pass/locales"
$reportDir = Join-Path $repoRoot "docs/reviews/second-pass"

function Read-Json([string] $path)
{
    $json = Get-Content $path -Raw
    if ($json.Length -gt 0 -and $json[0] -eq [char]0xFEFF)
    {
        $json = $json.Substring(1)
    }

    return $json | ConvertFrom-Json
}

$files = Get-ChildItem $adjudicationDir -Filter "*.adjudication.json" | Sort-Object Name
$reviews = foreach ($file in $files)
{
    Read-Json $file.FullName
}

$confirmed = @()
$modified = @()
$rejected = @()

foreach ($review in $reviews)
{
    $review.summary.pending = @($review.findings | Where-Object { $_.adjudication -eq "pending" }).Count
    $review.summary.confirmed = @($review.findings | Where-Object { $_.adjudication -eq "confirmed" }).Count
    $review.summary.modified = @($review.findings | Where-Object { $_.adjudication -eq "modified" }).Count
    $review.summary.rejected = @($review.findings | Where-Object { $_.adjudication -eq "rejected" }).Count

    $confirmed += @($review.findings | Where-Object { $_.adjudication -eq "confirmed" })
    $modified += @($review.findings | Where-Object { $_.adjudication -eq "modified" })
    $rejected += @($review.findings | Where-Object { $_.adjudication -eq "rejected" })
}

$aggregate = [ordered]@{
    generated = (Get-Date).ToString("yyyy-MM-dd")
    branch = (git -C $repoRoot branch --show-current).Trim()
    locale_count = $reviews.Count
    total_confirmed = $confirmed.Count
    total_modified = $modified.Count
    total_rejected = $rejected.Count
    locales = $reviews
}

$aggregate | ConvertTo-Json -Depth 12 | Set-Content -Encoding UTF8 (Join-Path $reportDir "2026-03-13-locale-adversarial-second-pass.json")

$lines = [System.Collections.Generic.List[string]]::new()
$lines.Add("# Locale Adversarial Review Second Pass")
$lines.Add("")
$lines.Add("Generated: $(Get-Date -Format 'yyyy-MM-dd')")
$lines.Add("")
$lines.Add("## Summary")
$lines.Add(("- Locales adjudicated: {0}" -f $reviews.Count))
$lines.Add(("- Confirmed findings: {0}" -f $confirmed.Count))
$lines.Add(("- Modified findings: {0}" -f $modified.Count))
$lines.Add(("- Rejected findings: {0}" -f $rejected.Count))
$lines.Add("")
$lines.Add("## Adjudication")

foreach ($bucket in @(
    @{ Name = "Modified"; Items = $modified },
    @{ Name = "Rejected"; Items = $rejected },
    @{ Name = "Confirmed"; Items = $confirmed }
))
{
    $lines.Add(("### {0}" -f $bucket.Name))
    if ($bucket.Items.Count -eq 0)
    {
        $lines.Add("None.")
    }
    else
    {
        foreach ($item in $bucket.Items | Sort-Object locale, key)
        {
            $lines.Add(("- [{0}] `{1}`" -f $item.locale, $item.key))
            $lines.Add(("  Original: {0} -> {1}" -f $item.current_value, $item.proposed_replacement))
            $lines.Add(("  Final: {0}" -f $item.adjudicated_replacement))
            $lines.Add(("  Decision: {0}" -f $item.adjudication))
            $lines.Add(("  Rationale: {0}" -f $item.adjudicated_rationale))
            if ($item.adjudicated_evidence.Count -gt 0)
            {
                $lines.Add(("  Evidence: {0}" -f ([string]::Join("; ", $item.adjudicated_evidence))))
            }
            if ($item.adjudicated_notes)
            {
                $lines.Add(("  Notes: {0}" -f $item.adjudicated_notes))
            }
        }
    }
    $lines.Add("")
}

$lines | Set-Content -Encoding UTF8 (Join-Path $reportDir "2026-03-13-locale-adversarial-second-pass.md")
