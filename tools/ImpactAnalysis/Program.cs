using System.Text;
using ImpactAnalysis;

// Configuration
var config = new AnalysisConfig
{
    GitHubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN"),
    OutputDirectory = "./output"
};

// Ensure output directory exists
Directory.CreateDirectory(config.OutputDirectory);

Console.WriteLine("=================================================");
Console.WriteLine("Humanizer v3 Namespace Consolidation");
Console.WriteLine("Impact Analysis Tool");
Console.WriteLine("=================================================");
Console.WriteLine();

// Step 1: Search GitHub repositories
Console.WriteLine("Phase 1: GitHub Repository Search");
Console.WriteLine("-------------------------------------------------");
var githubSearcher = new GitHubSearcher(config);
var repositories = await githubSearcher.SearchRepositoriesAsync();
Console.WriteLine($"Completed: Found {repositories.Count} repositories");
Console.WriteLine();

// Step 2: Search NuGet packages
Console.WriteLine("Phase 2: NuGet Package Search");
Console.WriteLine("-------------------------------------------------");
var nugetSearcher = new NuGetSearcher(config);
var packages = await nugetSearcher.SearchDependentPackagesAsync();
Console.WriteLine($"Completed: Found {packages.Count} dependent packages");
Console.WriteLine();

// Step 3: Score and rank entities
Console.WriteLine("Phase 3: Scoring and Ranking");
Console.WriteLine("-------------------------------------------------");
var allEntities = new List<ImpactEntity>();

// Convert repositories to impact entities
foreach (var repo in repositories)
{
    var recencyFactor = ScoringCalculator.CalculateRecencyFactor(repo.LastCommitDate);
    var recencyDays = ScoringCalculator.GetRecencyDays(repo.LastCommitDate);
    
    // Determine confidence (average of all matches)
    var avgConfidence = repo.Matches.Any() 
        ? repo.Matches.Average(m => ScoringCalculator.CalculateConfidence(m.MatchType, true))
        : 0.6;

    // Determine public API exposure (simplified)
    var publicApiExposure = ScoringCalculator.DeterminePublicApiExposure(
        repo.HasNuGetPackage, 
        false, // We'd need deeper analysis for this
        ClassificationType.Application);

    // Calculate raw score
    var rawScore = ScoringCalculator.CalculateRepoScore(
        repo.Stars,
        repo.UsedByCount,
        0, // We'd sum NuGet downloads if we had them
        repo.OccurrencesInRepo,
        publicApiExposure,
        avgConfidence,
        recencyFactor);

    var entity = new ImpactEntity
    {
        Name = repo.FullName,
        Type = EntityType.Repository,
        Stars = repo.Stars,
        Forks = repo.Forks,
        UsedByCount = repo.UsedByCount,
        Occurrences = repo.OccurrencesInRepo,
        PublicApiExposure = publicApiExposure,
        Confidence = avgConfidence,
        RecencyFactor = recencyFactor,
        RecencyDays = recencyDays,
        RawScore = rawScore,
        Repository = repo,
        Classification = DetermineClassification(repo)
    };

    allEntities.Add(entity);
}

// Convert packages to impact entities
foreach (var pkg in packages)
{
    var recencyFactor = 1.0; // Default if we don't have repo
    var recencyDays = 0;

    if (pkg.SourceRepo != null)
    {
        recencyFactor = ScoringCalculator.CalculateRecencyFactor(pkg.SourceRepo.LastCommitDate);
        recencyDays = ScoringCalculator.GetRecencyDays(pkg.SourceRepo.LastCommitDate);
    }

    var confidence = pkg.SourceAvailable ? 0.8 : 0.6;

    var rawScore = ScoringCalculator.CalculatePackageScore(
        pkg.TotalDownloads,
        pkg.PackageDependentsCount,
        pkg.SourceRepo?.OccurrencesInRepo ?? 0,
        pkg.RecentDownloads30d,
        confidence,
        recencyFactor);

    var entity = new ImpactEntity
    {
        Name = pkg.PackageId,
        Type = EntityType.Package,
        NuGetDownloads = pkg.TotalDownloads,
        PackageDependents = pkg.PackageDependentsCount,
        Occurrences = pkg.SourceRepo?.OccurrencesInRepo ?? 0,
        PublicApiExposure = 0.5, // Packages likely expose in public API
        Confidence = confidence,
        RecencyFactor = recencyFactor,
        RecencyDays = recencyDays,
        RawScore = rawScore,
        Package = pkg,
        Classification = ClassificationType.LibraryPublicApi
    };

    allEntities.Add(entity);
}

// Calculate percentiles
var rawScores = allEntities.Select(e => e.RawScore).ToList();
foreach (var entity in allEntities)
{
    entity.PercentileScore = ScoringCalculator.CalculatePercentile(entity.RawScore, rawScores);
    
    // Apply multiplier for published packages with public API exposure
    if (entity.Type == EntityType.Package && entity.PublicApiExposure > 0.7)
    {
        entity.PercentileScore *= 1.25;
    }

    entity.Category = ScoringCalculator.DetermineCategory(entity.PercentileScore);
}

// Sort by percentile score
allEntities = allEntities.OrderByDescending(e => e.PercentileScore).ToList();

Console.WriteLine($"Scored {allEntities.Count} entities");
Console.WriteLine($"  Critical: {allEntities.Count(e => e.Category == ImpactCategory.Critical)}");
Console.WriteLine($"  High: {allEntities.Count(e => e.Category == ImpactCategory.High)}");
Console.WriteLine($"  Medium: {allEntities.Count(e => e.Category == ImpactCategory.Medium)}");
Console.WriteLine($"  Low: {allEntities.Count(e => e.Category == ImpactCategory.Low)}");
Console.WriteLine();

// Step 4: Add recommended mitigations
Console.WriteLine("Phase 4: Generating Recommendations");
Console.WriteLine("-------------------------------------------------");
foreach (var entity in allEntities.Take(config.Top50Count))
{
    entity.RecommendedMitigation = GenerateRecommendation(entity);
    
    // Add sample files from matches
    if (entity.Repository != null)
    {
        entity.SampleFiles = entity.Repository.Matches
            .Take(3)
            .Select(m => new CodeSample
            {
                Path = m.FilePath,
                Permalink = m.Permalink,
                Excerpt = $"{m.ContextAbove}\n{m.MatchLine}\n{m.ContextBelow}"
            })
            .ToList();
    }
}
Console.WriteLine($"Generated recommendations for top {config.Top50Count}");
Console.WriteLine();

// Step 5: Generate reports
Console.WriteLine("Phase 5: Report Generation");
Console.WriteLine("-------------------------------------------------");
var reportGenerator = new ReportGenerator(config);

// Generate CSVs
reportGenerator.GenerateAllMatchesCsv(
    repositories,
    Path.Combine(config.OutputDirectory, "all_matches.csv"));

reportGenerator.GenerateTopImpactedCsv(
    allEntities.Take(config.Top50Count).ToList(),
    Path.Combine(config.OutputDirectory, "top_impacted.csv"));

// Generate JSON report
var report = CreateAnalysisReport(allEntities, repositories, packages);
reportGenerator.GenerateJsonReport(
    report,
    Path.Combine(config.OutputDirectory, "impact_analysis_report.json"));

// Generate Markdown summary
reportGenerator.GenerateMarkdownSummary(
    report,
    allEntities.Take(config.Top50Count).ToList(),
    Path.Combine(config.OutputDirectory, "IMPACT_SUMMARY.md"));

Console.WriteLine();
Console.WriteLine("=================================================");
Console.WriteLine("Analysis Complete!");
Console.WriteLine("=================================================");
Console.WriteLine($"Output directory: {Path.GetFullPath(config.OutputDirectory)}");
Console.WriteLine();
Console.WriteLine("Generated files:");
Console.WriteLine("  - all_matches.csv");
Console.WriteLine("  - top_impacted.csv");
Console.WriteLine("  - impact_analysis_report.json");
Console.WriteLine("  - IMPACT_SUMMARY.md");
Console.WriteLine();

// Helper functions
ClassificationType DetermineClassification(RepositoryMetadata repo)
{
    if (repo.HasNuGetPackage)
        return ClassificationType.LibraryPublicApi;
    
    // Check file patterns for test/sample/docs
    var testPatterns = new[] { "test", "sample", "example", "demo", "doc" };
    if (testPatterns.Any(p => repo.FullName.Contains(p, StringComparison.OrdinalIgnoreCase)))
        return ClassificationType.TestSample;

    // Default to application
    return ClassificationType.Application;
}

string GenerateRecommendation(ImpactEntity entity)
{
    var sb = new StringBuilder();

    switch (entity.Category)
    {
        case ImpactCategory.Critical:
            if (entity.Type == EntityType.Package)
            {
                sb.Append("Immediate coordination required. Contact maintainers to plan coordinated v3 release. ");
                sb.Append("Offer Roslyn analyzer with automatic code fix. ");
                sb.Append("Consider temporary compatibility adapter package.");
            }
            else if (entity.Classification == ClassificationType.LibraryPublicApi)
            {
                sb.Append("High-priority library with public API exposure. ");
                sb.Append("Reach out with migration guide and offer to draft PR with namespace changes. ");
                sb.Append("Recommend maintaining compatibility branch.");
            }
            else
            {
                sb.Append("High-visibility repository. ");
                sb.Append("Provide migration script and analyzer. ");
                sb.Append("Offer assistance with migration PR.");
            }
            break;

        case ImpactCategory.High:
            if (entity.Type == EntityType.Package || entity.Classification == ClassificationType.LibraryPublicApi)
            {
                sb.Append("Distribute Roslyn analyzer with FixAll support. ");
                sb.Append("Provide detailed migration guide. ");
                sb.Append("Consider opening issue with migration steps.");
            }
            else
            {
                sb.Append("Provide automated migration script (PowerShell/bash). ");
                sb.Append("Include in migration documentation as example. ");
                sb.Append("Offer analyzer for automated fix.");
            }
            break;

        case ImpactCategory.Medium:
            sb.Append("Include in migration documentation. ");
            sb.Append("Provide find/replace patterns and migration script. ");
            sb.Append("Analyzer will catch issues during build.");
            break;

        case ImpactCategory.Low:
            sb.Append("Document in migration guide. ");
            sb.Append("Users can apply manual fixes or use analyzer. ");
            sb.Append("No direct outreach needed.");
            break;
    }

    return sb.ToString();
}

AnalysisReport CreateAnalysisReport(
    List<ImpactEntity> entities,
    List<RepositoryMetadata> repos,
    List<NuGetPackageMetadata> packages)
{
    var report = new AnalysisReport
    {
        Summary = new ReportSummary
        {
            TotalRepos = repos.Count,
            TotalMatches = repos.Sum(r => r.OccurrencesInRepo),
            TotalNuGetDependents = packages.Count,
            TotalNuGetDownloads = packages.Sum(p => p.TotalDownloads)
        },
        TopImpacted = entities.Take(50).Select(e => new ImpactEntityReport
        {
            RepoOrPackage = e.Name,
            Type = e.Type.ToString(),
            Stars = e.Stars,
            Forks = e.Forks,
            NuGetDownloads = e.NuGetDownloads,
            Occurrences = e.Occurrences,
            Recency = e.RecencyDays,
            ImpactScore = e.PercentileScore,
            SampleFiles = e.SampleFiles,
            RecommendedMitigation = e.RecommendedMitigation
        }).ToList(),
        Methodology = new ReportMethodology
        {
            DateRun = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"),
            WeightingFormula = "Repository: R * (0.30*LS(Dg) + 0.20*LS(Nu) + 0.15*LS(S) + 0.15*LS(O) + 0.12*P + 0.08*C)\nPackage: R * (0.40*LS(TD) + 0.30*LS(PD) + 0.15*LS(RO) + 0.10*LS(RD) + 0.05*C)\nwhere LS(x) = log10(x+1)",
            SearchQueriesUsed = new GitHubSearcher(new AnalysisConfig()).GetType()
                .GetMethod("GenerateSearchQueries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(new GitHubSearcher(new AnalysisConfig()), null) as List<string> ?? []
        },
        Actions = GeneratePrioritizedActions(entities)
    };

    // Aggregate matches by namespace
    foreach (var repo in repos)
    {
        foreach (var match in repo.Matches)
        {
            if (!string.IsNullOrEmpty(match.Namespace))
            {
                if (!report.Aggregates.MatchesByNamespace.ContainsKey(match.Namespace))
                    report.Aggregates.MatchesByNamespace[match.Namespace] = 0;
                report.Aggregates.MatchesByNamespace[match.Namespace]++;
            }
        }
    }

    return report;
}

List<string> GeneratePrioritizedActions(List<ImpactEntity> entities)
{
    var actions = new List<string>();

    var critical = entities.Where(e => e.Category == ImpactCategory.Critical).ToList();
    var highImpactLibraries = entities.Where(e => 
        e.Category == ImpactCategory.High && 
        e.Classification == ClassificationType.LibraryPublicApi).ToList();

    actions.Add($"Develop Roslyn analyzer with automatic namespace migration (FixAll support)");
    actions.Add($"Create PowerShell/Bash migration script for automated find/replace");
    actions.Add($"Write comprehensive migration guide with examples and common patterns");
    
    if (critical.Any())
    {
        actions.Add($"Immediate outreach to {critical.Count} critical-impact maintainers for coordinated releases");
    }

    if (highImpactLibraries.Any())
    {
        actions.Add($"Contact {highImpactLibraries.Count} high-impact library maintainers with migration PRs");
    }

    actions.Add($"Consider publishing temporary compatibility adapter package (Humanizer.Migration)");
    actions.Add($"Update Humanizer documentation with prominent v3 migration section");
    actions.Add($"Create GitHub Discussions thread for community migration support");
    actions.Add($"Monitor top {entities.Take(25).Count()} repositories for migration completion");
    actions.Add($"Prepare blog post announcing v3 with migration timeline and resources");

    return actions;
}
