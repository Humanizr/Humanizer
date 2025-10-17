using System.Globalization;
using System.Text;
using System.Text.Json;

namespace ImpactAnalysis;

/// <summary>
/// Generates CSV, JSON, and Markdown reports
/// </summary>
public class ReportGenerator
{
    private readonly AnalysisConfig _config;

    public ReportGenerator(AnalysisConfig config)
    {
        _config = config;
    }

    public void GenerateAllMatchesCsv(List<RepositoryMetadata> repos, string outputPath)
    {
        var csv = new StringBuilder();
        csv.AppendLine("repo,repo_url,file_path,permalink,match_line,context_above,context_below,match_type,occurrences_in_repo,files_with_matches,stars,forks,last_commit_date,used_by_count,nuget_packages_linked,confidence");

        foreach (var repo in repos)
        {
            foreach (var match in repo.Matches.Take(100)) // Limit matches per repo
            {
                var confidence = ScoringCalculator.CalculateConfidence(match.MatchType, true);
                csv.AppendLine(CsvEscape([
                    repo.FullName,
                    repo.Url,
                    match.FilePath,
                    match.Permalink,
                    match.MatchLine,
                    match.ContextAbove,
                    match.ContextBelow,
                    match.MatchType.ToString(),
                    repo.OccurrencesInRepo.ToString(),
                    repo.FilesWithMatches.ToString(),
                    repo.Stars.ToString(),
                    repo.Forks.ToString(),
                    repo.LastCommitDate.ToString("yyyy-MM-dd"),
                    repo.UsedByCount.ToString(),
                    string.Join(";", repo.NuGetPackageIds),
                    confidence.ToString("F2")
                ]));
            }
        }

        File.WriteAllText(outputPath, csv.ToString());
        Console.WriteLine($"Generated all_matches.csv: {outputPath}");
    }

    public void GenerateTopImpactedCsv(List<ImpactEntity> topImpacted, string outputPath)
    {
        var csv = new StringBuilder();
        csv.AppendLine("rank,repo_or_package,type,stars,forks,used_by,nuget_downloads,package_dependents,occurrences,public_api_exposure,recency_days,raw_score,percentile_score,category,recommended_mitigation");

        for (int i = 0; i < topImpacted.Count; i++)
        {
            var entity = topImpacted[i];
            csv.AppendLine(CsvEscape([
                (i + 1).ToString(),
                entity.Name,
                entity.Type.ToString(),
                entity.Stars.ToString(),
                entity.Forks.ToString(),
                entity.UsedByCount.ToString(),
                entity.NuGetDownloads.ToString(),
                entity.PackageDependents.ToString(),
                entity.Occurrences.ToString(),
                entity.PublicApiExposure.ToString("F2"),
                entity.RecencyDays.ToString(),
                entity.RawScore.ToString("F4"),
                entity.PercentileScore.ToString("F2"),
                entity.Category.ToString(),
                entity.RecommendedMitigation
            ]));
        }

        File.WriteAllText(outputPath, csv.ToString());
        Console.WriteLine($"Generated top_impacted.csv: {outputPath}");
    }

    public void GenerateJsonReport(AnalysisReport report, string outputPath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        var json = JsonSerializer.Serialize(report, options);
        File.WriteAllText(outputPath, json);
        Console.WriteLine($"Generated JSON report: {outputPath}");
    }

    public void GenerateMarkdownSummary(
        AnalysisReport report,
        List<ImpactEntity> allEntities,
        string outputPath)
    {
        var md = new StringBuilder();

        // Title
        md.AppendLine("# Humanizer v3 Namespace Consolidation - Impact Analysis Report");
        md.AppendLine();
        md.AppendLine($"**Analysis Date:** {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
        md.AppendLine();

        // Executive Summary
        md.AppendLine("## Executive Summary");
        md.AppendLine();
        md.AppendLine($"- **Total Repositories Analyzed:** {report.Summary.TotalRepos:N0}");
        md.AppendLine($"- **Total Namespace Matches:** {report.Summary.TotalMatches:N0}");
        md.AppendLine($"- **Total NuGet Dependent Packages:** {report.Summary.TotalNuGetDependents:N0}");
        md.AppendLine($"- **Total NuGet Downloads (Dependents):** {report.Summary.TotalNuGetDownloads:N0}");
        md.AppendLine();

        // Top 10 Impacted
        md.AppendLine("## Top 10 Most Impacted Repositories and Packages");
        md.AppendLine();
        
        var top10 = allEntities.Take(10).ToList();
        for (int i = 0; i < top10.Count; i++)
        {
            var entity = top10[i];
            md.AppendLine($"### {i + 1}. {entity.Name}");
            md.AppendLine();
            md.AppendLine($"- **Type:** {entity.Type}");
            md.AppendLine($"- **Impact Category:** {entity.Category}");
            md.AppendLine($"- **Impact Score:** {entity.PercentileScore:F1}th percentile");
            
            if (entity.Type == EntityType.Repository)
            {
                md.AppendLine($"- **Stars:** {entity.Stars:N0}");
                md.AppendLine($"- **Forks:** {entity.Forks:N0}");
                md.AppendLine($"- **Used By:** {entity.UsedByCount:N0} repositories");
            }
            else
            {
                md.AppendLine($"- **Total Downloads:** {entity.NuGetDownloads:N0}");
                md.AppendLine($"- **Package Dependents:** {entity.PackageDependents:N0}");
            }
            
            md.AppendLine($"- **Namespace Occurrences:** {entity.Occurrences:N0}");
            md.AppendLine($"- **Last Updated:** {entity.RecencyDays} days ago");
            md.AppendLine($"- **Public API Exposure:** {entity.PublicApiExposure * 100:F0}%");
            md.AppendLine($"- **Classification:** {entity.Classification}");
            md.AppendLine();
            md.AppendLine($"**Recommended Actions:** {entity.RecommendedMitigation}");
            md.AppendLine();

            if (entity.SampleFiles.Any())
            {
                md.AppendLine("**Sample Matches:**");
                foreach (var sample in entity.SampleFiles.Take(3))
                {
                    md.AppendLine($"- [{sample.Path}]({sample.Permalink})");
                    md.AppendLine($"  ```");
                    md.AppendLine($"  {sample.Excerpt}");
                    md.AppendLine($"  ```");
                }
                md.AppendLine();
            }
        }

        // Top 25 Brief Summary
        md.AppendLine("## Top 25 Impacted Entities");
        md.AppendLine();
        
        var top25 = allEntities.Skip(10).Take(15).ToList();
        foreach (var entity in top25)
        {
            md.AppendLine($"**{allEntities.IndexOf(entity) + 1}. {entity.Name}** ({entity.Type}, {entity.Category})");
            md.AppendLine($"   - {entity.RecommendedMitigation}");
            md.AppendLine();
        }

        // Recommended Mitigations
        md.AppendLine("## Recommended Migration Strategy");
        md.AppendLine();
        md.AppendLine("### For Library Maintainers (Critical/High Impact)");
        md.AppendLine();
        md.AppendLine("1. **Immediate Coordination Required** - Contact maintainers of top 10 libraries");
        md.AppendLine("2. **Roslyn Analyzer + Code Fix** - Develop and distribute analyzer with automatic namespace fix");
        md.AppendLine("3. **Compatibility Shims** - Consider temporary type-forwarding adapter package");
        md.AppendLine("4. **Documentation** - Provide clear migration guide with before/after examples");
        md.AppendLine();

        md.AppendLine("### For Application Developers (Medium/Low Impact)");
        md.AppendLine();
        md.AppendLine("1. **Migration Script** - Provide PowerShell/batch script for automated namespace replacement");
        md.AppendLine("2. **Find/Replace Guide** - Document simple regex patterns for manual migration");
        md.AppendLine("3. **CI/CD Integration** - Add analyzer to catch issues during build");
        md.AppendLine();

        md.AppendLine("### For Package Consumers");
        md.AppendLine();
        md.AppendLine("1. **Version Pinning** - Pin to Humanizer v2.x until dependencies update");
        md.AppendLine("2. **Multi-targeting** - Support both v2 and v3 during transition period");
        md.AppendLine("3. **Adapter Package** - Use compatibility adapter if needed");
        md.AppendLine();

        // Aggregates
        md.AppendLine("## Impact by Namespace");
        md.AppendLine();
        md.AppendLine("| Namespace | Occurrences |");
        md.AppendLine("|-----------|-------------|");
        
        foreach (var kvp in report.Aggregates.MatchesByNamespace.OrderByDescending(x => x.Value))
        {
            md.AppendLine($"| {kvp.Key} | {kvp.Value:N0} |");
        }
        md.AppendLine();

        // Methodology
        md.AppendLine("## Methodology");
        md.AppendLine();
        md.AppendLine($"**Analysis Date:** {report.Methodology.DateRun}");
        md.AppendLine();
        md.AppendLine("**Search Queries Used:**");
        foreach (var query in report.Methodology.SearchQueriesUsed.Take(20))
        {
            md.AppendLine($"- `{query}`");
        }
        md.AppendLine();
        md.AppendLine("**Scoring Formula:**");
        md.AppendLine($"```");
        md.AppendLine(report.Methodology.WeightingFormula);
        md.AppendLine($"```");
        md.AppendLine();

        // Confidence and Limitations
        md.AppendLine("## Confidence and Limitations");
        md.AppendLine();
        md.AppendLine("- **GitHub API Rate Limits:** Some repositories may have been missed due to rate limiting");
        md.AppendLine("- **Private Code:** Analysis only covers public repositories and packages");
        md.AppendLine("- **Public API Detection:** Automated detection may not catch all public API exposures");
        md.AppendLine("- **NuGet Dependency Data:** Reverse dependency counts may be incomplete");
        md.AppendLine("- **Code Context:** Limited context lines may not fully capture usage patterns");
        md.AppendLine();

        // Prioritized Actions
        md.AppendLine("## Prioritized Action Items");
        md.AppendLine();
        int actionNum = 1;
        foreach (var action in report.Actions)
        {
            md.AppendLine($"{actionNum}. {action}");
            actionNum++;
        }
        md.AppendLine();

        // Footer
        md.AppendLine("---");
        md.AppendLine();
        md.AppendLine("*This report was generated automatically by the Humanizer Impact Analysis Tool.*");

        File.WriteAllText(outputPath, md.ToString());
        Console.WriteLine($"Generated Markdown summary: {outputPath}");
    }

    private string CsvEscape(string[] fields)
    {
        var escaped = fields.Select(f =>
        {
            if (string.IsNullOrEmpty(f)) return "\"\"";
            if (f.Contains('"') || f.Contains(',') || f.Contains('\n'))
            {
                return $"\"{f.Replace("\"", "\"\"")}\"";
            }
            return f;
        });
        return string.Join(",", escaped);
    }
}
