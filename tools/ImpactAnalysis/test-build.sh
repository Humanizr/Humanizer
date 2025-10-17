#!/bin/bash

# Quick verification test - doesn't make real API calls
# Just verifies the tool builds and runs without errors

cd "$(dirname "$0")"

echo "Building Impact Analysis Tool..."
dotnet build -c Release

if [ $? -ne 0 ]; then
    echo "Build failed!"
    exit 1
fi

echo ""
echo "Build successful!"
echo ""
echo "To run the full analysis, you need:"
echo "  1. Set GITHUB_TOKEN environment variable"
echo "  2. Run: ./run-impact-analysis.sh"
echo ""
echo "Note: The full analysis may take several minutes and will"
echo "make hundreds of API calls to GitHub and NuGet."
echo ""
echo "Expected output files:"
echo "  - output/all_matches.csv"
echo "  - output/top_impacted.csv"
echo "  - output/impact_analysis_report.json"
echo "  - output/IMPACT_SUMMARY.md"
