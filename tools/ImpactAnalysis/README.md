# Humanizer v3 Impact Analysis Tool

This tool performs a comprehensive impact analysis for the Humanizer v3 namespace consolidation, identifying repositories and NuGet packages that will be affected by the breaking changes.

## Overview

Humanizer v3 consolidates many sub-namespaces into the root `Humanizer` namespace:

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

## What This Tool Does

1. **GitHub Search**: Searches public GitHub repositories for usage of old namespaces
   - Using directives (`using Humanizer.Bytes;`)
   - Qualified type references (`Humanizer.Bytes.ByteSize`)
   - Package references in .csproj files
   - Binary references (DLL files)

2. **NuGet Analysis**: Identifies packages that depend on Humanizer.Core or Humanizer
   - Total downloads
   - Dependent packages count
   - Repository links

3. **Impact Scoring**: Ranks repositories and packages using a balanced scoring formula
   - Repository metrics (stars, forks, dependents)
   - Usage metrics (occurrences, files affected)
   - Public API exposure
   - Recency and activity

4. **Report Generation**: Produces multiple output formats
   - `all_matches.csv` - All namespace matches found
   - `top_impacted.csv` - Top 50 impacted entities
   - `impact_analysis_report.json` - Complete structured report
   - `IMPACT_SUMMARY.md` - Executive summary with recommendations

## Prerequisites

- .NET 10.0 SDK (or later)
- GitHub Personal Access Token (for API access)
  - Create at: https://github.com/settings/tokens
  - Required scopes: `public_repo`, `read:packages`

## Usage

### Set GitHub Token

```bash
export GITHUB_TOKEN="your-github-token-here"
```

Or on Windows:
```powershell
$env:GITHUB_TOKEN="your-github-token-here"
```

### Run the Analysis

```bash
cd tools/ImpactAnalysis
dotnet run
```

### Run with Script

```bash
./run-impact-analysis.sh
```

Or on Windows:
```powershell
.\run-impact-analysis.ps1
```

## Output Files

All output files are generated in the `./output` directory:

### 1. all_matches.csv
Complete list of all namespace matches found, including:
- Repository information
- File paths and permalinks
- Match types and context
- Metadata (stars, forks, etc.)

### 2. top_impacted.csv
Top 50 most impacted repositories and packages with:
- Ranking and scores
- Impact category (Critical/High/Medium/Low)
- Recommended mitigation strategies

### 3. impact_analysis_report.json
Structured JSON report containing:
- Summary statistics
- Top impacted entities
- Aggregated data by namespace
- Methodology and search queries used
- Prioritized action items

### 4. IMPACT_SUMMARY.md
Executive summary Markdown document with:
- Top 10 detailed analysis
- Top 25 brief summaries
- Recommended migration strategies
- Methodology and limitations

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
