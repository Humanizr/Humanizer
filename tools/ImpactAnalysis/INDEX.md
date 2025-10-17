# Impact Analysis Tool - Documentation Index

Welcome to the Humanizer v3 Impact Analysis Tool documentation!

## Getting Started

1. **[README.md](README.md)** - Main overview and technical details
2. **[QUICKSTART.md](QUICKSTART.md)** - 5-minute setup and usage guide
3. **[SAMPLE_OUTPUT.md](SAMPLE_OUTPUT.md)** - Example of what the reports look like

## Running the Tool

### Quick Commands

```bash
# Set your token
export GITHUB_TOKEN="your-token-here"

# Run the analysis
./run-impact-analysis.sh

# View results
cat ./output/IMPACT_SUMMARY.md
```

### Available Scripts

- `run-impact-analysis.sh` - Main runner script (Linux/Mac)
- `run-impact-analysis.ps1` - Main runner script (Windows)
- `test-build.sh` - Quick build verification

## Understanding the Output

The tool generates 4 files in `./output/`:

| File | Purpose | Start Here? |
|------|---------|------------|
| `IMPACT_SUMMARY.md` | Executive summary with top 10 detailed analysis | ✅ YES |
| `top_impacted.csv` | Top 50 entities for Excel/analysis | 📊 |
| `all_matches.csv` | Complete raw data | 📋 |
| `impact_analysis_report.json` | Machine-readable report | 🤖 |

## Key Concepts

### Impact Categories

- **Critical** (99-100th percentile) - Immediate action required
- **High** (90-99th percentile) - High priority migration
- **Medium** (70-90th percentile) - Standard migration path
- **Low** (<70th percentile) - Documentation only

### Old Namespaces

All moving to root `Humanizer` namespace:

- Humanizer.Bytes
- Humanizer.Localisation (and all sub-namespaces)
- Humanizer.Configuration
- Humanizer.Inflections
- Humanizer.DateTimeHumanizeStrategy

## Files in This Directory

### Source Code
- `Program.cs` - Main orchestration logic
- `GitHubSearcher.cs` - GitHub API search
- `NuGetSearcher.cs` - NuGet API search
- `Scoring.cs` - Scoring algorithms and formulas
- `Models.cs` - Data models
- `ReportGenerator.cs` - CSV, JSON, Markdown generation

### Configuration
- `ImpactAnalysis.csproj` - Project file
- `.gitignore` - Excludes build and output

### Documentation
- `README.md` - Main documentation
- `QUICKSTART.md` - Quick start guide
- `SAMPLE_OUTPUT.md` - Example output
- `INDEX.md` - This file

### Scripts
- `run-impact-analysis.sh` - Linux/Mac runner
- `run-impact-analysis.ps1` - Windows runner
- `test-build.sh` - Build verification

## Troubleshooting

Common issues and solutions:

| Problem | Solution |
|---------|----------|
| Rate limit exceeded | Wait 60min or use different token |
| Build errors | Run `dotnet restore` |
| No output | Check token and network |
| Wrong .NET version | Install .NET 10.0 SDK |

See [QUICKSTART.md](QUICKSTART.md) for detailed troubleshooting.

## Support

- **Tool Issues**: Open issue in Humanizer repository
- **Questions**: GitHub Discussions
- **API Problems**: Check GitHub/NuGet status

---

**Quick Links:**
- [Main README](README.md)
- [Quick Start](QUICKSTART.md)
- [Sample Output](SAMPLE_OUTPUT.md)
- [Humanizer Repository](https://github.com/Humanizr/Humanizer)
