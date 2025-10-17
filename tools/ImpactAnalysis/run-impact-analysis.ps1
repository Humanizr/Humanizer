# Humanizer v3 Impact Analysis Runner
# This script runs the impact analysis tool and generates reports

$ErrorActionPreference = "Stop"

Write-Host "=================================================" -ForegroundColor Cyan
Write-Host "Humanizer v3 Impact Analysis" -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan
Write-Host ""

# Check if GitHub token is set
if (-not $env:GITHUB_TOKEN) {
    Write-Host "WARNING: GITHUB_TOKEN environment variable is not set." -ForegroundColor Yellow
    Write-Host "The tool will run with limited API access (60 requests/hour)." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "To set your token:" -ForegroundColor Yellow
    Write-Host '  $env:GITHUB_TOKEN="your-token-here"' -ForegroundColor Yellow
    Write-Host ""
    $response = Read-Host "Continue without token? (y/n)"
    if ($response -ne "y" -and $response -ne "Y") {
        exit 1
    }
}

# Navigate to the tool directory
Set-Location $PSScriptRoot

# Restore dependencies
Write-Host "Restoring dependencies..." -ForegroundColor Green
dotnet restore

# Build the project
Write-Host "Building project..." -ForegroundColor Green
dotnet build -c Release

# Run the analysis
Write-Host ""
Write-Host "Running impact analysis..." -ForegroundColor Green
Write-Host ""
dotnet run -c Release

# Check if output was generated
if (Test-Path "./output") {
    Write-Host ""
    Write-Host "=================================================" -ForegroundColor Cyan
    Write-Host "Analysis complete! Output files:" -ForegroundColor Cyan
    Write-Host "=================================================" -ForegroundColor Cyan
    Get-ChildItem ./output/ | Format-Table Name, Length, LastWriteTime
    Write-Host ""
    Write-Host "To view the summary:" -ForegroundColor Green
    Write-Host "  Get-Content ./output/IMPACT_SUMMARY.md" -ForegroundColor White
    Write-Host ""
    Write-Host "Or open in your browser/editor:" -ForegroundColor Green
    Write-Host "  - ./output/IMPACT_SUMMARY.md" -ForegroundColor White
    Write-Host "  - ./output/impact_analysis_report.json" -ForegroundColor White
    Write-Host "  - ./output/top_impacted.csv" -ForegroundColor White
    Write-Host "  - ./output/all_matches.csv" -ForegroundColor White
    Write-Host ""
}
else {
    Write-Host ""
    Write-Host "ERROR: Output directory not created. Check for errors above." -ForegroundColor Red
    exit 1
}
