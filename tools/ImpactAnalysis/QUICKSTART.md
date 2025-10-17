# Quick Start Guide - Humanizer v3 Impact Analysis

## Overview

This tool analyzes the impact of Humanizer v3's namespace consolidation on public GitHub repositories and NuGet packages.

## 5-Minute Quick Start

### 1. Prerequisites

```bash
# Check .NET version (requires 10.0 or later)
dotnet --version

# Should show: 10.0.x or higher
```

### 2. Get GitHub Token

1. Go to: https://github.com/settings/tokens
2. Click "Generate new token" → "Generate new token (classic)"
3. Give it a name like "Humanizer Impact Analysis"
4. Select scopes:
   - ✅ `public_repo` (Access public repositories)
   - ✅ `read:packages` (Read packages)
5. Click "Generate token"
6. Copy the token (you won't see it again!)

### 3. Set Environment Variable

**Linux/Mac:**
```bash
export GITHUB_TOKEN="ghp_your_token_here"
```

**Windows (PowerShell):**
```powershell
$env:GITHUB_TOKEN="ghp_your_token_here"
```

**Windows (CMD):**
```cmd
set GITHUB_TOKEN=ghp_your_token_here
```

### 4. Run the Analysis

**Linux/Mac:**
```bash
cd tools/ImpactAnalysis
./run-impact-analysis.sh
```

**Windows:**
```powershell
cd tools\ImpactAnalysis
.\run-impact-analysis.ps1
```

### 5. View Results

The tool creates 4 output files in `./output/`:

1. **IMPACT_SUMMARY.md** - Start here! Executive summary with top 10 detailed analysis
2. **top_impacted.csv** - Top 50 entities for further analysis
3. **all_matches.csv** - Complete raw data
4. **impact_analysis_report.json** - Machine-readable report

```bash
# View the summary
cat ./output/IMPACT_SUMMARY.md

# Or open in your editor/browser
code ./output/IMPACT_SUMMARY.md
```

## What the Tool Does

### Phase 1: GitHub Search (5-10 minutes)
- Searches for old namespace usage across public repos
- Patterns: `using Humanizer.Bytes;`, `Humanizer.Localisation.`, etc.
- Collects metadata: stars, forks, last commit, etc.

### Phase 2: NuGet Search (2-3 minutes)
- Finds packages depending on Humanizer.Core or Humanizer
- Gathers download counts and dependents
- Links to source repositories

### Phase 3: Scoring & Ranking (< 1 minute)
- Applies weighted scoring formula
- Factors: popularity, usage frequency, recency, public API exposure
- Ranks from Critical → High → Medium → Low

### Phase 4: Report Generation (< 1 minute)
- Produces CSVs, JSON, and Markdown
- Adds migration recommendations
- Includes code samples

## Understanding the Output

### Impact Categories

- **Critical (99-100th percentile)**
  - Top libraries with massive downstream impact
  - Requires immediate coordination with maintainers
  - Example: Major frameworks using Humanizer

- **High (90-99th percentile)**
  - Popular libraries and widely-used packages
  - Should receive analyzer + migration PRs
  - Example: Popular logging or validation libraries

- **Medium (70-90th percentile)**
  - Active projects with moderate usage
  - Migration script + documentation
  - Example: Internal company libraries

- **Low (<70th percentile)**
  - Smaller projects, samples, or inactive repos
  - Documentation only
  - Example: Tutorial repos, archived projects

### CSV Columns Explained

**all_matches.csv:**
- `repo`: Repository full name (owner/repo)
- `file_path`: Path to file with match
- `permalink`: Direct link to the code
- `match_type`: UsingDirective, QualifiedType, PackageReference, etc.
- `occurrences_in_repo`: Total matches in this repo
- `confidence`: How certain we are (1.0 = definite, 0.3 = inferred)

**top_impacted.csv:**
- `rank`: Position (1 = highest impact)
- `category`: Critical/High/Medium/Low
- `percentile_score`: Normalized score (0-100+)
- `recommended_mitigation`: Specific actions to take

## Common Scenarios

### Scenario 1: Running Without Token (Limited)
```bash
# You'll get 60 requests/hour (very limited)
./run-impact-analysis.sh
# Press 'y' when prompted
```

### Scenario 2: Re-running Analysis
```bash
# Delete old output first
rm -rf output/
./run-impact-analysis.sh
```

### Scenario 3: Customizing the Analysis
Edit `Program.cs`:
```csharp
var config = new AnalysisConfig
{
    MaxReposToAnalyze = 1000,  // Default: 500
    MaxFilesPerRepo = 200,     // Default: 100
    Top50Count = 100,          // Default: 50
};
```

Then rebuild:
```bash
dotnet build -c Release
```

### Scenario 4: Analyzing Specific Namespaces Only
Edit `Scoring.cs` → `AnalysisConfig.OldNamespaces`:
```csharp
public string[] OldNamespaces { get; set; } =
[
    "Humanizer.Bytes",
    "Humanizer.Localisation"
    // Comment out ones you don't want
];
```

## Troubleshooting

### "Rate limit exceeded"
**Problem:** Too many API requests too quickly.

**Solution 1:** Wait 60 minutes for rate limit reset.

**Solution 2:** Use a different GitHub account/token.

**Solution 3:** Reduce `MaxReposToAnalyze` in config.

### "Unable to find package Octokit"
**Problem:** NuGet packages not restored.

**Solution:**
```bash
dotnet restore
dotnet build -c Release
```

### "GITHUB_TOKEN not set" warning
**Problem:** No token provided (or expired).

**Solution:** See step 2-3 above to set token.

### Build fails with ".NET 10 not found"
**Problem:** Wrong .NET SDK version.

**Solution:** Install .NET 10 SDK:
- Download from: https://dotnet.microsoft.com/download/dotnet/10.0

### "Output directory not created"
**Problem:** Tool crashed or exited early.

**Solution:** Check console output for error messages. Common causes:
- Invalid GitHub token
- Network connectivity issues
- API rate limit hit immediately

## Advanced Usage

### Export to Excel/Google Sheets
```bash
# CSVs can be opened directly
open ./output/top_impacted.csv
```

### Filter High-Impact Only
```bash
# Using command-line tools
cat ./output/top_impacted.csv | grep "Critical\|High" > high_impact.csv
```

### Search Specific Repository
Modify `GitHubSearcher.cs` to add:
```csharp
queries.Add($"repo:microsoft/AspNetCore \"using Humanizer.Bytes\"");
```

### JSON Processing
```bash
# Pretty-print JSON
cat ./output/impact_analysis_report.json | jq .

# Extract just top 10 names
cat ./output/impact_analysis_report.json | jq '.top_impacted[:10].repo_or_package'
```

## Performance Tips

### Faster Analysis
- **Use authenticated requests**: 5000/hr vs 60/hr
- **Reduce MaxReposToAnalyze**: Trade coverage for speed
- **Run during off-peak hours**: Less contention

### More Complete Analysis
- **Increase MaxReposToAnalyze**: Catch more long-tail repos
- **Increase MaxFilesPerRepo**: Better occurrence counts
- **Run multiple times**: API pagination may miss results

## Next Steps After Analysis

1. **Review IMPACT_SUMMARY.md**
   - Focus on top 10 first
   - Note Critical and High categories

2. **Contact Maintainers**
   - Use GitHub Issues or email
   - Link to migration guide
   - Offer to submit PRs

3. **Create Migration Materials**
   - Roslyn analyzer with code fix
   - PowerShell/bash scripts
   - Step-by-step guide

4. **Monitor Progress**
   - Track which projects have updated
   - Re-run analysis after 30/60/90 days
   - Measure adoption rate

## Getting Help

- **Tool issues**: Open issue in Humanizer repository
- **API problems**: Check GitHub/NuGet status pages
- **Questions**: Ask in GitHub Discussions

---

**Estimated Time:** 10-15 minutes for full analysis
**Rate Limits:** ~500 repos × 2 API calls = 1000 calls (~12 minutes at 5000/hour)
