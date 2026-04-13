<#
.SYNOPSIS
    Enforces code coverage thresholds by parsing ReportGenerator's Summary.xml.

.DESCRIPTION
    This is the canonical source of coverage threshold numbers for the Humanizer project.
    All documentation (CLAUDE.md, AGENTS.md, docs/adding-a-locale.md) references this
    header rather than duplicating threshold values.

    Thresholds:
      Humanizer           — line >= 95, branch >= 88, method >= 85
      Humanizer.Analyzers — line >= 95, branch >= 85
      Humanizer.SourceGenerators — report-only (not gated)

    The gate reads Summary.xml exactly as ReportGenerator publishes it.
    No line-range subtraction, no per-class carve-out list.

.PARAMETER SummaryXmlPath
    Path to the ReportGenerator Summary.xml file (XmlSummary report type).

.EXAMPLE
    pwsh scripts/coverage-gate.ps1 -SummaryXmlPath artifacts/local-coverage/Summary.xml
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$SummaryXmlPath
)

$ErrorActionPreference = "Stop"

# ---------------------------------------------------------------------------
# Validate input
# ---------------------------------------------------------------------------
if (-not (Test-Path $SummaryXmlPath)) {
    Write-Error "Summary.xml not found at path: $SummaryXmlPath"
    exit 1
}

try {
    [xml]$summary = Get-Content -Path $SummaryXmlPath -Raw
}
catch {
    Write-Error "Failed to parse Summary.xml as valid XML: $_"
    exit 1
}

$coverageNode = $summary.CoverageReport.Coverage
if (-not $coverageNode) {
    Write-Error "Summary.xml is missing the <CoverageReport><Coverage> element."
    exit 1
}

# ---------------------------------------------------------------------------
# Threshold definitions
# ---------------------------------------------------------------------------
$gatedAssemblies = @{
    "Humanizer" = @{
        Line   = 95
        Branch = 88
        Method = 85
    }
    "Humanizer.Analyzers" = @{
        Line   = 95
        Branch = 85
    }
}

$reportOnlyAssemblies = @("Humanizer.SourceGenerators")

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------
function Get-AssemblyNode {
    param([System.Xml.XmlElement]$CoverageNode, [string]$Name)
    $nodes = $CoverageNode.SelectNodes("Assembly[@name='$Name']")
    if ($nodes.Count -eq 0) { return $null }
    return $nodes[0]
}

function Format-Metric {
    param([string]$Label, [double]$Actual, [double]$Threshold, [bool]$Pass)
    $status = if ($Pass) { "PASS" } else { "FAIL" }
    return "  $Label : $Actual% (threshold: $Threshold%) [$status]"
}

# ---------------------------------------------------------------------------
# Gate evaluation
# ---------------------------------------------------------------------------
$failures = @()
$allOutput = @()

foreach ($assemblyName in $gatedAssemblies.Keys | Sort-Object) {
    $thresholds = $gatedAssemblies[$assemblyName]
    $node = Get-AssemblyNode -CoverageNode $coverageNode -Name $assemblyName

    if (-not $node) {
        $msg = "Required assembly '$assemblyName' not found in Summary.xml."
        $failures += $msg
        $allOutput += "[$assemblyName]"
        $allOutput += "  ERROR: $msg"
        continue
    }

    $allOutput += "[$assemblyName]"

    # Line coverage
    $lineCov = [double]$node.coverage
    $linePass = $lineCov -ge $thresholds.Line
    $allOutput += (Format-Metric -Label "Line  " -Actual $lineCov -Threshold $thresholds.Line -Pass $linePass)
    if (-not $linePass) {
        $failures += "$assemblyName line coverage $lineCov% < $($thresholds.Line)%"
    }

    # Branch coverage
    $branchCov = [double]$node.branchcoverage
    $branchPass = $branchCov -ge $thresholds.Branch
    $allOutput += (Format-Metric -Label "Branch" -Actual $branchCov -Threshold $thresholds.Branch -Pass $branchPass)
    if (-not $branchPass) {
        $failures += "$assemblyName branch coverage $branchCov% < $($thresholds.Branch)%"
    }

    # Method coverage (only if threshold defined)
    if ($thresholds.ContainsKey("Method")) {
        $methodCov = [double]$node.methodcoverage
        $methodPass = $methodCov -ge $thresholds.Method
        $allOutput += (Format-Metric -Label "Method" -Actual $methodCov -Threshold $thresholds.Method -Pass $methodPass)
        if (-not $methodPass) {
            $failures += "$assemblyName method coverage $methodCov% < $($thresholds.Method)%"
        }
    }

    $allOutput += ""
}

# Report-only assemblies
foreach ($assemblyName in $reportOnlyAssemblies) {
    $node = Get-AssemblyNode -CoverageNode $coverageNode -Name $assemblyName

    $allOutput += "[$assemblyName] (report-only, not gated)"

    if (-not $node) {
        $allOutput += "  Not found in Summary.xml (skipped)."
    }
    else {
        $lineCov = [double]$node.coverage
        $branchCov = [double]$node.branchcoverage
        $allOutput += "  Line  : $lineCov%"
        $allOutput += "  Branch: $branchCov%"
    }
    $allOutput += ""
}

# ---------------------------------------------------------------------------
# Output and exit
# ---------------------------------------------------------------------------
Write-Host ""
Write-Host "=== Coverage Gate ==="
foreach ($line in $allOutput) {
    Write-Host $line
}

if ($failures.Count -gt 0) {
    Write-Host "--- GATE FAILED ---"
    foreach ($f in $failures) {
        Write-Host "  * $f"
    }
    Write-Host ""
    exit 1
}
else {
    Write-Host "--- GATE PASSED ---"
    Write-Host ""
    exit 0
}
