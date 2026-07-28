param(
    [Parameter(Mandatory = $true)]
    [string[]] $Reports,

    [int] $Top = 25,

    [string] $JsonOutputPath
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = 'Stop'

function Get-AttributeValue {
    param(
        [System.Xml.XmlElement] $Element,
        [string] $Name
    )

    $value = $Element.GetAttribute($Name)
    if ([string]::IsNullOrWhiteSpace($value)) {
        return $null
    }

    return $value
}

$files = foreach ($report in $Reports) {
    Get-ChildItem -Path $report -File
}

if (-not $files) {
    throw "No Cobertura reports matched: $($Reports -join ', ')"
}

$byFile = @{}

foreach ($file in $files) {
    [xml] $document = Get-Content -LiteralPath $file.FullName -Raw
    foreach ($class in $document.coverage.packages.package.classes.class) {
        $sourceFile = Get-AttributeValue $class 'filename'
        if ([string]::IsNullOrWhiteSpace($sourceFile)) {
            continue
        }

        if (-not $byFile.ContainsKey($sourceFile)) {
            $byFile[$sourceFile] = [ordered]@{
                file = $sourceFile
                coveredBranches = 0
                totalBranches = 0
                missedBranches = 0
                lines = @{}
            }
        }

        foreach ($line in $class.lines.line) {
            if ((Get-AttributeValue $line 'branch') -ne 'True') {
                continue
            }

            $coverage = Get-AttributeValue $line 'condition-coverage'
            if ($coverage -notmatch '\((\d+)/(\d+)\)') {
                continue
            }

            $covered = [int] $Matches[1]
            $total = [int] $Matches[2]
            $missed = $total - $covered
            $lineNumber = [int] (Get-AttributeValue $line 'number')
            $entry = $byFile[$sourceFile]
            $entry.coveredBranches += $covered
            $entry.totalBranches += $total
            $entry.missedBranches += $missed

            if ($missed -gt 0) {
                if (-not $entry.lines.ContainsKey($lineNumber)) {
                    $entry.lines[$lineNumber] = [ordered]@{
                        line = $lineNumber
                        coveredBranches = 0
                        totalBranches = 0
                        missedBranches = 0
                    }
                }

                $lineEntry = $entry.lines[$lineNumber]
                $lineEntry.coveredBranches += $covered
                $lineEntry.totalBranches += $total
                $lineEntry.missedBranches += $missed
            }
        }
    }
}

$hotspots = $byFile.Values |
    Where-Object { $_.missedBranches -gt 0 } |
    ForEach-Object {
        [pscustomobject] [ordered]@{
            file = $_.file
            branchCoverage = [Math]::Round(100.0 * $_.coveredBranches / $_.totalBranches, 2)
            coveredBranches = $_.coveredBranches
            totalBranches = $_.totalBranches
            missedBranches = $_.missedBranches
            missedLines = @($_.lines.Values |
                ForEach-Object { [pscustomobject] $_ } |
                Sort-Object @{ Expression = 'missedBranches'; Descending = $true }, @{ Expression = 'line'; Ascending = $true } |
                Select-Object -First 10)
        }
    } |
    Sort-Object @{ Expression = 'missedBranches'; Descending = $true }, @{ Expression = 'totalBranches'; Descending = $true } |
    Select-Object -First $Top

$summary = [ordered]@{
    reports = @($files.FullName)
    totalFilesWithMissedBranches = ($byFile.Values | Where-Object { $_.missedBranches -gt 0 }).Count
    hotspots = @($hotspots)
}

if ($JsonOutputPath) {
    $summary | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $JsonOutputPath -Encoding UTF8
}

"Missed Branch Hotspots"
"======================"
foreach ($hotspot in $hotspots) {
    "{0,5} missed  {1,5}/{2,-5}  {3,6:N2}%  {4}" -f $hotspot.missedBranches, $hotspot.coveredBranches, $hotspot.totalBranches, $hotspot.branchCoverage, $hotspot.file
    foreach ($line in $hotspot.missedLines) {
        "       L{0}: {1}/{2} covered, {3} missed" -f $line.line, $line.coveredBranches, $line.totalBranches, $line.missedBranches
    }
}
