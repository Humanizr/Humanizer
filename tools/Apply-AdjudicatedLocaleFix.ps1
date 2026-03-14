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
    $json = [System.IO.File]::ReadAllText($path, [System.Text.Encoding]::UTF8)
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

$dirSep = [string][System.IO.Path]::DirectorySeparatorChar

$adjudication = Read-Json $secondPassPath
$sourceReviewPath = Join-Path $repoRoot ($adjudication.source_review -replace "\\\\", $dirSep)
$sourceReview = Read-Json $sourceReviewPath
$resourcePath = Join-Path $repoRoot ($sourceReview.resource_file -replace "\\\\", $dirSep)
$testsRoot = Join-Path $repoRoot "tests/Humanizer.Tests/Localisation"

if (!(Test-Path $resourcePath))
{
    throw "Resource file not found: $resourcePath"
}

[xml] $xml = [System.IO.File]::ReadAllText($resourcePath, [System.Text.Encoding]::UTF8)
$changedKeys = [System.Collections.Generic.List[string]]::new()
$replacements = [System.Collections.Generic.List[object]]::new()

function Normalize-ToArray($value)
{
    if ($null -eq $value)
    {
        return @()
    }

    if ($value -is [System.Array])
    {
        return @($value)
    }

    return @($value)
}

function Repair-Mojibake([string] $value)
{
    if ([string]::IsNullOrEmpty($value))
    {
        return $value
    }

    if ($value -notmatch 'Ã|Ð|Ñ|Ø|Ù|à')
    {
        return $value
    }

    $latin1 = [System.Text.Encoding]::GetEncoding(28591)
    $bytes = $latin1.GetBytes($value)
    return [System.Text.Encoding]::UTF8.GetString($bytes)
}

foreach ($finding in @($adjudication.findings))
{
    if ($finding.adjudication -notin @("confirmed", "modified"))
    {
        continue
    }

    $currentValue = Repair-Mojibake ([string] $finding.current_value)
    $replacementValue = Repair-Mojibake ([string] $finding.adjudicated_replacement)

    if ($currentValue -eq $replacementValue)
    {
        continue
    }

    $replacements.Add([pscustomobject]@{
        Current = $currentValue
        Replacement = $replacementValue
    })

    $node = @($xml.root.data | Where-Object { $_.name -eq $finding.key }) | Select-Object -First 1
    if ($null -eq $node)
    {
        continue
    }

    $node.value = $replacementValue
    $changedKeys.Add($finding.key)
}

$xml.Save($resourcePath)

$testDirectories = [System.Collections.Generic.List[string]]::new()
foreach ($dir in Normalize-ToArray $sourceReview.direct_test_directories)
{
    $normalizedDir = $dir -replace "\\\\", $dirSep
    $testDirectories.Add((Join-Path $repoRoot $normalizedDir))
}

foreach ($child in Normalize-ToArray $sourceReview.owned_child_cultures)
{
    $childDir = Join-Path $testsRoot $child
    if (Test-Path $childDir)
    {
        $testDirectories.Add($childDir)
    }
}

$updatedTestFiles = [System.Collections.Generic.HashSet[string]]::new()
foreach ($dir in ($testDirectories | Sort-Object -Unique))
{
    if (!(Test-Path $dir))
    {
        continue
    }

    foreach ($file in Get-ChildItem $dir -Recurse -Filter *.cs)
    {
        $content = [System.IO.File]::ReadAllText($file.FullName, [System.Text.Encoding]::UTF8)
        $original = $content
        foreach ($replacement in $replacements)
        {
            $content = $content.Replace($replacement.Current, $replacement.Replacement)
        }

        if ($content -ne $original)
        {
            [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8)
            $updatedTestFiles.Add($file.FullName) | Out-Null
        }
    }
}

if ($changedKeys.Count -eq 0)
{
    Write-Output "No direct resource replacements applied for $Locale."
}
else
{
    Write-Output "Applied direct resource replacements for ${Locale}:"
    $changedKeys | Sort-Object | ForEach-Object { Write-Output "- $_" }
}

if ($updatedTestFiles.Count -gt 0)
{
    Write-Output "Updated locale test files for ${Locale}:"
    $updatedTestFiles | Sort-Object | ForEach-Object { Write-Output "- $_" }
}
