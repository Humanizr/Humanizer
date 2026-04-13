<#
.SYNOPSIS
    Unit tests for coverage-gate.ps1 using synthesized Summary.xml fixtures.

.DESCRIPTION
    Runs the coverage gate script against each fixture in coverage-gate-fixtures/
    and asserts the expected exit code and output content.

.EXAMPLE
    pwsh scripts/coverage-gate.Tests.ps1
#>

$ErrorActionPreference = "Stop"

$scriptDir = $PSScriptRoot
$gatePath = Join-Path $scriptDir "coverage-gate.ps1"
$fixturesDir = Join-Path $scriptDir "coverage-gate-fixtures"

$passed = 0
$failed = 0
$testResults = @()

function Run-GateTest {
    param(
        [string]$Name,
        [string]$FixturePath,
        [int]$ExpectedExitCode,
        [string[]]$ExpectedOutputContains,
        [string[]]$ExpectedOutputNotContains
    )

    $output = $null
    $exitCode = $null

    try {
        $output = & pwsh -NoProfile -NonInteractive -File $gatePath -SummaryXmlPath $FixturePath 2>&1 | Out-String
        $exitCode = $LASTEXITCODE
    }
    catch {
        $output = $_.Exception.Message
        $exitCode = 1
    }

    $testPassed = $true
    $reasons = @()

    if ($exitCode -ne $ExpectedExitCode) {
        $testPassed = $false
        $reasons += "Expected exit code $ExpectedExitCode but got $exitCode"
    }

    foreach ($expected in $ExpectedOutputContains) {
        if ($output -notmatch [regex]::Escape($expected)) {
            $testPassed = $false
            $reasons += "Expected output to contain '$expected'"
        }
    }

    foreach ($notExpected in $ExpectedOutputNotContains) {
        if ($output -match [regex]::Escape($notExpected)) {
            $testPassed = $false
            $reasons += "Expected output NOT to contain '$notExpected'"
        }
    }

    if ($testPassed) {
        Write-Host "  PASS: $Name" -ForegroundColor Green
        $script:passed++
    }
    else {
        Write-Host "  FAIL: $Name" -ForegroundColor Red
        foreach ($r in $reasons) {
            Write-Host "        $r" -ForegroundColor Yellow
        }
        Write-Host "        Output: $output" -ForegroundColor DarkGray
        $script:failed++
    }
}

Write-Host ""
Write-Host "=== Coverage Gate Tests ==="
Write-Host ""

# Test 1: Passing fixture
Run-GateTest `
    -Name "All assemblies pass thresholds" `
    -FixturePath (Join-Path $fixturesDir "passing.xml") `
    -ExpectedExitCode 0 `
    -ExpectedOutputContains @("GATE PASSED", "[Humanizer]", "[Humanizer.Analyzers]", "[Humanizer.SourceGenerators] (report-only, not gated)") `
    -ExpectedOutputNotContains @("FAIL", "GATE FAILED")

# Test 2: Humanizer line coverage fails
Run-GateTest `
    -Name "Humanizer line coverage below threshold" `
    -FixturePath (Join-Path $fixturesDir "fail-humanizer-line.xml") `
    -ExpectedExitCode 1 `
    -ExpectedOutputContains @("GATE FAILED", "Humanizer line coverage") `
    -ExpectedOutputNotContains @()

# Test 3: Humanizer branch coverage fails
Run-GateTest `
    -Name "Humanizer branch coverage below threshold" `
    -FixturePath (Join-Path $fixturesDir "fail-humanizer-branch.xml") `
    -ExpectedExitCode 1 `
    -ExpectedOutputContains @("GATE FAILED", "Humanizer branch coverage") `
    -ExpectedOutputNotContains @()

# Test 4: Humanizer method coverage fails
Run-GateTest `
    -Name "Humanizer method coverage below threshold" `
    -FixturePath (Join-Path $fixturesDir "fail-humanizer-method.xml") `
    -ExpectedExitCode 1 `
    -ExpectedOutputContains @("GATE FAILED", "Humanizer method coverage") `
    -ExpectedOutputNotContains @()

# Test 5: Analyzers line coverage fails
Run-GateTest `
    -Name "Humanizer.Analyzers line coverage below threshold" `
    -FixturePath (Join-Path $fixturesDir "fail-analyzers-line.xml") `
    -ExpectedExitCode 1 `
    -ExpectedOutputContains @("GATE FAILED", "Humanizer.Analyzers line coverage") `
    -ExpectedOutputNotContains @()

# Test 6: Analyzers branch coverage fails
Run-GateTest `
    -Name "Humanizer.Analyzers branch coverage below threshold" `
    -FixturePath (Join-Path $fixturesDir "fail-analyzers-branch.xml") `
    -ExpectedExitCode 1 `
    -ExpectedOutputContains @("GATE FAILED", "Humanizer.Analyzers branch coverage") `
    -ExpectedOutputNotContains @()

# Test 7: Missing required assembly
Run-GateTest `
    -Name "Required assembly missing from Summary.xml" `
    -FixturePath (Join-Path $fixturesDir "missing-assembly.xml") `
    -ExpectedExitCode 1 `
    -ExpectedOutputContains @("GATE FAILED", "Humanizer.Analyzers", "not found") `
    -ExpectedOutputNotContains @()

# Test 8: Malformed XML
Run-GateTest `
    -Name "Malformed XML input" `
    -FixturePath (Join-Path $fixturesDir "malformed.xml") `
    -ExpectedExitCode 1 `
    -ExpectedOutputContains @() `
    -ExpectedOutputNotContains @("GATE PASSED")

# Test 9: Missing file
Run-GateTest `
    -Name "Summary.xml file does not exist" `
    -FixturePath (Join-Path $fixturesDir "nonexistent.xml") `
    -ExpectedExitCode 1 `
    -ExpectedOutputContains @() `
    -ExpectedOutputNotContains @("GATE PASSED")

# Test 10: SourceGenerators is report-only (never fails even with low coverage)
Run-GateTest `
    -Name "SourceGenerators low coverage does not fail gate" `
    -FixturePath (Join-Path $fixturesDir "passing.xml") `
    -ExpectedExitCode 0 `
    -ExpectedOutputContains @("report-only, not gated") `
    -ExpectedOutputNotContains @("GATE FAILED")

Write-Host ""
Write-Host "=== Results: $passed passed, $failed failed ==="
Write-Host ""

if ($failed -gt 0) {
    exit 1
}

exit 0
