#!/bin/bash

# Humanizer v3 Impact Analysis Runner
# This script runs the impact analysis tool and generates reports

set -e

echo "================================================="
echo "Humanizer v3 Impact Analysis"
echo "================================================="
echo ""

# Check if GitHub token is set
if [ -z "$GITHUB_TOKEN" ]; then
    echo "WARNING: GITHUB_TOKEN environment variable is not set."
    echo "The tool will run with limited API access (60 requests/hour)."
    echo ""
    echo "To set your token:"
    echo "  export GITHUB_TOKEN='your-token-here'"
    echo ""
    read -p "Continue without token? (y/n) " -n 1 -r
    echo ""
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi

# Navigate to the tool directory
cd "$(dirname "$0")"

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore

# Build the project
echo "Building project..."
dotnet build -c Release

# Run the analysis
echo ""
echo "Running impact analysis..."
echo ""
dotnet run -c Release

# Check if output was generated
if [ -d "./output" ]; then
    echo ""
    echo "================================================="
    echo "Analysis complete! Output files:"
    echo "================================================="
    ls -lh ./output/
    echo ""
    echo "To view the summary:"
    echo "  cat ./output/IMPACT_SUMMARY.md"
    echo ""
    echo "Or open in your browser/editor:"
    echo "  - ./output/IMPACT_SUMMARY.md"
    echo "  - ./output/impact_analysis_report.json"
    echo "  - ./output/top_impacted.csv"
    echo "  - ./output/all_matches.csv"
    echo ""
else
    echo ""
    echo "ERROR: Output directory not created. Check for errors above."
    exit 1
fi
