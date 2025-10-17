# Humanizer v3 Impact Analysis Tool

A comprehensive tool for analyzing the impact of Humanizer v3's namespace consolidation on public GitHub repositories and NuGet packages.

## What This Tool Does

Humanizer v3 consolidates many sub-namespaces into the root `Humanizer` namespace, which is a source-breaking change. This tool:

1. **Searches GitHub** for repositories using old namespaces
2. **Analyzes NuGet** packages depending on Humanizer
3. **Scores & Ranks** impact using a weighted formula
4. **Generates Reports** in CSV, JSON, and Markdown formats
5. **Recommends Actions** for different impact levels

## Quick Start

```bash
# Set your GitHub token
export GITHUB_TOKEN="your-token-here"

# Run the analysis
cd tools/ImpactAnalysis
./run-impact-analysis.sh

# View results
cat ./output/IMPACT_SUMMARY.md
```

**📖 [Read the Complete Quick Start Guide](QUICKSTART.md)** for detailed setup instructions.

**📄 [See Sample Output](SAMPLE_OUTPUT.md)** to understand what the reports look like.

## Old Namespaces Being Consolidated

All of these are moving to the root `Humanizer` namespace in v3:

- `Humanizer.Bytes` → `Humanizer`
- `Humanizer.Localisation` → `Humanizer`
- `Humanizer.Localisation.Formatters` → `Humanizer`
- `Humanizer.Localisation.NumberToWords` → `Humanizer`
- `Humanizer.DateTimeHumanizeStrategy` → `Humanizer`
- `Humanizer.Configuration` → `Humanizer`
- `Humanizer.Localisation.DateToOrdinalWords` → `Humanizer`
- `Humanizer.Localisation.Ordinalizers` → `Humanizer`
- `Humanizer.Inflections` → `Humanizer`
- `Humanizer.Localisation.CollectionFormatters` → `Humanizer`
- `Humanizer.Localisation.TimeToClockNotation` → `Humanizer`

## How It Works

### Phase 1: GitHub Search
Searches public GitHub repositories for:
- Using directives: `using Humanizer.Bytes;`
- Qualified type references: `Humanizer.Bytes.ByteSize`
- Package references in .csproj files
- Binary references (DLL files)

### Phase 2: NuGet Analysis
Identifies packages that depend on Humanizer.Core or Humanizer:
- Total downloads and recent downloads
- Dependent packages count
- Repository links

### Phase 3: Impact Scoring
Ranks repositories and packages using a balanced scoring formula:
- Repository metrics (stars, forks, dependents)
- Usage metrics (occurrences, files affected)
- Public API exposure
- Recency and activity

### Phase 4: Report Generation
Produces multiple output formats:
- `IMPACT_SUMMARY.md` - Executive summary with recommendations ⭐
- `top_impacted.csv` - Top 50 impacted entities
- `all_matches.csv` - All namespace matches found
- `impact_analysis_report.json` - Complete structured report

## Prerequisites

- .NET 10.0 SDK or later
- GitHub Personal Access Token ([get one here](https://github.com/settings/tokens))
  - Required scopes: `public_repo`, `read:packages`

## Running the Analysis

### Option 1: Using the Scripts (Recommended)

**Linux/Mac:**
```bash
export GITHUB_TOKEN="your-token-here"
./run-impact-analysis.sh
```

**Windows:**
```powershell
$env:GITHUB_TOKEN="your-token-here"
.\run-impact-analysis.ps1
```

### Option 2: Direct Execution

```bash
export GITHUB_TOKEN="your-token-here"
dotnet run -c Release
```

## Output Files

All files are generated in `./output/`:

### 1. IMPACT_SUMMARY.md ⭐ Start Here!

Executive summary with:
- Top 10 detailed analysis with code samples
- Top 25 brief summaries
- Recommended migration strategies
- Impact breakdown by namespace
- Methodology and limitations

### 2. top_impacted.csv

Top 50 entities with rankings and scores:
- Repository/package name and type
- Impact category (Critical/High/Medium/Low)
- Metrics: stars, forks, downloads, occurrences
- Recommended mitigation strategies

### 3. all_matches.csv

Complete raw data:
- All namespace matches found
- Repository information and metadata
- File paths with permalinks
- Match types and confidence levels

### 4. impact_analysis_report.json

Machine-readable structured report:
- Summary statistics
- Top impacted entities with details
- Aggregated data by namespace
- Search queries and methodology
- Prioritized action items

## Scoring Methodology

The tool uses a balanced scoring formula with log-scaling:

### For Repositories
```
score = R * (0.30*LS(dependents) + 0.20*LS(nuget_downloads) + 
             0.15*LS(stars) + 0.15*LS(occurrences) + 
             0.12*public_api_exposure + 0.08*confidence)
```

### For Packages
```
score = R * (0.40*LS(total_downloads) + 0.30*LS(package_dependents) + 
             0.15*LS(occurrences) + 0.10*LS(recent_downloads) + 
             0.05*confidence)
```

Where:
- `LS(x) = log10(x + 1)` (log scaling)
- `R` = recency factor (1.0 for recent, 0.4 for old)
- Scores are converted to percentiles and categorized

### Impact Categories
- **Critical**: 99-100th percentile - Immediate outreach required
- **High**: 90-99th percentile - Analyzer + migration PRs
- **Medium**: 70-90th percentile - Migration scripts + docs
- **Low**: <70th percentile - Documentation only

## Limitations

- **Rate Limits**: GitHub API has rate limits (5000/hour authenticated)
- **Private Code**: Only analyzes public repositories
- **Public API Detection**: Automated detection may not be 100% accurate
- **Code Context**: Limited to 3-line excerpts around matches
- **NuGet Data**: Reverse dependency data may be incomplete

## Configuration

Edit `AnalysisConfig` in `Program.cs` to customize:

```csharp
var config = new AnalysisConfig
{
    GitHubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN"),
    MaxReposToAnalyze = 500,    // Maximum repositories to analyze
    MaxFilesPerRepo = 100,       // Maximum files to sample per repo
    Top50Count = 50,             // Number of top entities to report
    Top25Count = 25,             // Number for detailed verification
    OutputDirectory = "./output" // Output directory path
};
```

## Extending the Tool

The tool is modular and can be extended:

- **GitHubSearcher.cs**: Add more search patterns or GraphQL queries
- **NuGetSearcher.cs**: Enhance NuGet API integration
- **Scoring.cs**: Adjust scoring formulas or add new metrics
- **ReportGenerator.cs**: Add new output formats or visualizations

## Troubleshooting

### Rate Limit Errors
If you encounter rate limits:
1. Ensure you're using a valid GitHub token
2. Wait for rate limit reset (shown in error message)
3. Reduce `MaxReposToAnalyze` in config

### Missing Dependencies
```bash
dotnet restore
```

### Build Errors
Ensure .NET 10.0 SDK is installed:
```bash
dotnet --version
```

## Contributing

This tool was created to support the Humanizer v3 migration. Improvements and bug fixes are welcome!

## License

Same as Humanizer - MIT License
