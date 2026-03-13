param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path $PSScriptRoot -Parent
$sourceDir = Join-Path $repoRoot "docs/reviews/locales"
$adjudicationDir = Join-Path $repoRoot "docs/reviews/second-pass/locales"
$reportDir = Join-Path $repoRoot "docs/reviews/second-pass"

New-Item -ItemType Directory -Force -Path $reportDir | Out-Null
New-Item -ItemType Directory -Force -Path $adjudicationDir | Out-Null

function Read-Json([string] $path)
{
    $json = Get-Content $path -Raw
    if ($json.Length -gt 0 -and $json[0] -eq [char]0xFEFF)
    {
        $json = $json.Substring(1)
    }

    return $json | ConvertFrom-Json
}

$manifest = [System.Collections.Generic.List[object]]::new()

foreach ($file in Get-ChildItem $sourceDir -Filter "*.review.json" | Sort-Object Name)
{
    $review = Read-Json $file.FullName
    $flagged = @($review.records | Where-Object { $_.status -ne "ok" })
    if ($flagged.Count -eq 0)
    {
        continue
    }

    $adjudication = [ordered]@{
        locale = $review.locale
        source_review = "docs\\reviews\\locales\\$($file.Name)"
        adjudication_status = "pending"
        summary = [ordered]@{
            pending = $flagged.Count
            confirmed = 0
            modified = 0
            rejected = 0
        }
        findings = @(
            foreach ($record in $flagged)
            {
                [ordered]@{
                    locale = $record.locale
                    key = $record.key
                    original_status = $record.status
                    original_severity = $record.severity
                    original_confidence = $record.confidence
                    current_value = $record.current_value
                    proposed_replacement = $record.proposed_replacement
                    native_rationale = $record.native_rationale
                    evidence = @($record.evidence)
                    notes = $record.notes
                    adjudication = "pending"
                    adjudicated_status = $record.status
                    adjudicated_severity = $record.severity
                    adjudicated_confidence = $record.confidence
                    adjudicated_replacement = $record.proposed_replacement
                    adjudicated_rationale = ""
                    adjudicated_evidence = @()
                    adjudicated_notes = ""
                }
            }
        )
    }

    $outPath = Join-Path $adjudicationDir "$($review.locale).adjudication.json"
    $adjudication | ConvertTo-Json -Depth 10 | Set-Content -Encoding UTF8 $outPath

    $manifest.Add([ordered]@{
        locale = $review.locale
        source_review = $adjudication.source_review
        adjudication_review = "docs\\reviews\\second-pass\\locales\\$($review.locale).adjudication.json"
        finding_count = $flagged.Count
    })
}

$pendingCount = 0
foreach ($item in $manifest)
{
    $pendingCount += [int]$item.finding_count
}

$aggregate = [ordered]@{
    generated = (Get-Date).ToString("yyyy-MM-dd")
    branch = (git -C $repoRoot branch --show-current).Trim()
    locale_count = $manifest.Count
    locales = $manifest
}

$aggregate | ConvertTo-Json -Depth 6 | Set-Content -Encoding UTF8 (Join-Path $reportDir "2026-03-13-locale-adversarial-second-pass.json")

$markdown = @"
# Locale Adversarial Review Second Pass

Generated: $(Get-Date -Format 'yyyy-MM-dd')

## Summary
- Locales requiring adjudication: $($manifest.Count)
- Pending adjudications: $pendingCount

## Findings
Pending adjudication.
"@

$markdown | Set-Content -Encoding UTF8 (Join-Path $reportDir "2026-03-13-locale-adversarial-second-pass.md")
