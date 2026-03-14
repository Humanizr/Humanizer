param(
    [Parameter(Mandatory = $true)]
    [string] $Locale
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Split-Path $PSScriptRoot -Parent
$secondPassPath = Join-Path $repoRoot "docs/reviews/second-pass/locales/$Locale.adjudication.json"

function Read-Json([string] $path)
{
    $json = Get-Content $path -Raw
    if ($json.Length -gt 0 -and $json[0] -eq [char]0xFEFF)
    {
        $json = $json.Substring(1)
    }

    return $json | ConvertFrom-Json
}

if (!(Test-Path $secondPassPath))
{
    throw "Adjudication file not found: $secondPassPath"
}

$adjudication = Read-Json $secondPassPath
$sourceReviewPath = Join-Path $repoRoot ($adjudication.source_review -replace '\\\\', '\')
$sourceReview = Read-Json $sourceReviewPath
$resourcePath = Join-Path $repoRoot ($sourceReview.resource_file -replace '\\\\', '\')

if (!(Test-Path $resourcePath))
{
    throw "Resource file not found: $resourcePath"
}

[xml] $xml = Get-Content $resourcePath
$changedKeys = [System.Collections.Generic.List[string]]::new()

foreach ($finding in @($adjudication.findings))
{
    if ($finding.adjudication -notin @("confirmed", "modified"))
    {
        continue
    }

    if ($finding.current_value -eq $finding.adjudicated_replacement)
    {
        continue
    }

    $node = @($xml.root.data | Where-Object { $_.name -eq $finding.key }) | Select-Object -First 1
    if ($null -eq $node)
    {
        continue
    }

    $node.value = [string] $finding.adjudicated_replacement
    $changedKeys.Add($finding.key)
}

$xml.Save($resourcePath)

if ($changedKeys.Count -eq 0)
{
    Write-Output "No direct resource replacements applied for $Locale."
}
else
{
    Write-Output "Applied direct resource replacements for $Locale:"
    $changedKeys | Sort-Object | ForEach-Object { Write-Output "- $_" }
}
