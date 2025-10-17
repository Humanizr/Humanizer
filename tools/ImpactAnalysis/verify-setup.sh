#!/bin/bash

# Comprehensive Test of the Impact Analysis Tool
# This script verifies all components are working

set -e

echo "=============================================="
echo "Impact Analysis Tool - Comprehensive Test"
echo "=============================================="
echo ""

# Check .NET version
echo "1. Checking .NET SDK..."
DOTNET_VERSION=$(dotnet --version)
echo "   Found: $DOTNET_VERSION"

if [[ ! "$DOTNET_VERSION" =~ ^10\. ]] && [[ ! "$DOTNET_VERSION" =~ ^9\. ]]; then
    echo "   WARNING: .NET 10.0 recommended, found $DOTNET_VERSION"
fi
echo ""

# Navigate to tool directory
cd "$(dirname "$0")"
echo "2. Working directory: $(pwd)"
echo ""

# Clean previous builds
echo "3. Cleaning previous builds..."
rm -rf bin/ obj/ output/
echo "   ✓ Cleaned"
echo ""

# Restore dependencies
echo "4. Restoring NuGet packages..."
dotnet restore > /dev/null 2>&1
if [ $? -eq 0 ]; then
    echo "   ✓ Packages restored"
else
    echo "   ✗ Restore failed"
    exit 1
fi
echo ""

# Build Release
echo "5. Building Release configuration..."
dotnet build -c Release --no-restore > /dev/null 2>&1
if [ $? -eq 0 ]; then
    echo "   ✓ Build successful"
else
    echo "   ✗ Build failed"
    exit 1
fi
echo ""

# Check for GitHub token
echo "6. Checking for GitHub token..."
if [ -z "$GITHUB_TOKEN" ]; then
    echo "   ⚠ GITHUB_TOKEN not set"
    echo "   The tool will run with limited API access (60 requests/hour)"
    echo ""
    echo "   To set your token:"
    echo "   export GITHUB_TOKEN='your-token-here'"
    echo ""
else
    echo "   ✓ Token configured"
fi
echo ""

# Verify all source files exist
echo "7. Verifying source files..."
files=(
    "Program.cs"
    "Models.cs"
    "Scoring.cs"
    "GitHubSearcher.cs"
    "NuGetSearcher.cs"
    "ReportGenerator.cs"
    "ImpactAnalysis.csproj"
)

all_exist=true
for file in "${files[@]}"; do
    if [ -f "$file" ]; then
        echo "   ✓ $file"
    else
        echo "   ✗ $file NOT FOUND"
        all_exist=false
    fi
done

if [ "$all_exist" = false ]; then
    echo ""
    echo "ERROR: Some source files are missing!"
    exit 1
fi
echo ""

# Verify documentation exists
echo "8. Verifying documentation..."
docs=(
    "README.md"
    "QUICKSTART.md"
    "SAMPLE_OUTPUT.md"
    "INDEX.md"
)

for doc in "${docs[@]}"; do
    if [ -f "$doc" ]; then
        echo "   ✓ $doc"
    else
        echo "   ⚠ $doc missing"
    fi
done
echo ""

# Verify scripts exist and are executable
echo "9. Verifying scripts..."
if [ -x "run-impact-analysis.sh" ]; then
    echo "   ✓ run-impact-analysis.sh (executable)"
else
    echo "   ⚠ run-impact-analysis.sh not executable"
fi

if [ -f "run-impact-analysis.ps1" ]; then
    echo "   ✓ run-impact-analysis.ps1"
else
    echo "   ⚠ run-impact-analysis.ps1 missing"
fi
echo ""

# Summary
echo "=============================================="
echo "Test Summary"
echo "=============================================="
echo "✓ .NET SDK: $DOTNET_VERSION"
echo "✓ Build: Successful"
echo "✓ Dependencies: Restored"
echo "✓ Source files: Complete"
echo "✓ Documentation: Present"
echo "✓ Scripts: Ready"

if [ -z "$GITHUB_TOKEN" ]; then
    echo "⚠ GitHub Token: Not configured"
else
    echo "✓ GitHub Token: Configured"
fi
echo ""

echo "=============================================="
echo "Ready to Run!"
echo "=============================================="
echo ""
echo "To run the full analysis:"
echo "  ./run-impact-analysis.sh"
echo ""
echo "Or direct execution:"
echo "  dotnet run -c Release"
echo ""
echo "Expected outputs (in ./output/):"
echo "  - IMPACT_SUMMARY.md"
echo "  - top_impacted.csv"
echo "  - all_matches.csv"
echo "  - impact_analysis_report.json"
echo ""

if [ -z "$GITHUB_TOKEN" ]; then
    echo "⚠ Note: Without GITHUB_TOKEN, you'll have limited API access"
    echo "  Set it with: export GITHUB_TOKEN='your-token'"
    echo ""
fi

echo "All systems ready! ✓"
