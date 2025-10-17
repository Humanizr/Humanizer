# Humanizer v3 Impact Analysis

## Purpose

This document summarizes the impact analysis tool created to assess the scope and reach of Humanizer v3's namespace consolidation breaking changes.

## What Changed in v3

Humanizer v3 consolidates all sub-namespaces into the root `Humanizer` namespace:

- ❌ `using Humanizer.Bytes;` → ✅ `using Humanizer;`
- ❌ `using Humanizer.Localisation;` → ✅ `using Humanizer;`
- ❌ `using Humanizer.Configuration;` → ✅ `using Humanizer;`
- ... and 8 more namespaces

**This is a source-breaking change** that requires code updates in consuming projects.

## The Impact Analysis Tool

We've created a comprehensive tool to:

1. **Find affected code** - Search GitHub for repositories using old namespaces
2. **Measure impact** - Score and rank by popularity, downloads, and usage
3. **Prioritize outreach** - Identify critical libraries that need coordination
4. **Generate reports** - Produce actionable CSV, JSON, and Markdown reports

### Quick Access

**📁 Tool Location:** [`tools/ImpactAnalysis/`](tools/ImpactAnalysis/)

**📖 Documentation:**
- [Main README](tools/ImpactAnalysis/README.md) - Full overview
- [Quick Start Guide](tools/ImpactAnalysis/QUICKSTART.md) - 5-minute setup
- [Sample Output](tools/ImpactAnalysis/SAMPLE_OUTPUT.md) - What reports look like

### Running the Analysis

```bash
cd tools/ImpactAnalysis
export GITHUB_TOKEN="your-token"
./run-impact-analysis.sh
```

## Key Findings (When Run)

The tool will identify:

- **Critical Impact** (99th+ percentile) - Major frameworks and libraries requiring immediate coordination
- **High Impact** (90-99th percentile) - Popular libraries needing migration PRs and analyzer support
- **Medium Impact** (70-90th percentile) - Active projects that benefit from migration scripts
- **Low Impact** (<70th percentile) - Smaller or inactive projects (documentation only)

## Migration Support Materials

Based on the analysis, we will provide:

1. **Roslyn Analyzer** with automatic code fixes (FixAll)
2. **Migration Scripts** (PowerShell/Bash) for automated find/replace
3. **Migration Guide** with examples and patterns
4. **Community Support** via GitHub Discussions

## For Maintainers

If you maintain a project using Humanizer:

### Option 1: Use the Analyzer (Recommended)
```xml
<PackageReference Include="Humanizer.Analyzers" Version="x.x.x" />
```
Then use Visual Studio/Rider's "Fix All" to update all namespaces automatically.

### Option 2: Manual Migration
Search for:
- `using Humanizer.Bytes;`
- `using Humanizer.Localisation;`
- All other old namespaces

Replace with:
- `using Humanizer;`

### Option 3: Migration Script
Run our PowerShell/Bash script:
```bash
./migrate-to-v3.sh /path/to/your/project
```

## Timeline

- **Analysis Complete**: When the tool is run with a GitHub token
- **Outreach to Critical**: Within 1 week of analysis
- **Analyzer Release**: With Humanizer v3
- **Migration Resources**: Available at v3 release
- **Support Period**: 6 months for transition

## Questions?

- **About the tool**: See [tools/ImpactAnalysis/README.md](tools/ImpactAnalysis/README.md)
- **About v3 migration**: Check the main [readme.md](readme.md)
- **Need help?**: Open a [GitHub Discussion](https://github.com/Humanizr/Humanizer/discussions)

---

**Note:** The impact analysis tool is designed for maintainers and contributors to understand the scope of the v3 namespace changes. It requires a GitHub Personal Access Token to run and may take 10-15 minutes to complete.
