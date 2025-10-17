using System.Text.Json.Serialization;

namespace ImpactAnalysis;

/// <summary>
/// Configuration for the impact analysis
/// </summary>
public class AnalysisConfig
{
    public string[] OldNamespaces { get; set; } =
    [
        "Humanizer.Bytes",
        "Humanizer.Localisation",
        "Humanizer.Localisation.Formatters",
        "Humanizer.Localisation.NumberToWords",
        "Humanizer.DateTimeHumanizeStrategy",
        "Humanizer.Configuration",
        "Humanizer.Localisation.DateToOrdinalWords",
        "Humanizer.Localisation.Ordinalizers",
        "Humanizer.Inflections",
        "Humanizer.Localisation.CollectionFormatters",
        "Humanizer.Localisation.TimeToClockNotation"
    ];

    public string? GitHubToken { get; set; }
    public int MaxReposToAnalyze { get; set; } = 500;
    public int MaxFilesPerRepo { get; set; } = 100;
    public int Top50Count { get; set; } = 50;
    public int Top25Count { get; set; } = 25;
    public string OutputDirectory { get; set; } = "./output";
}

/// <summary>
/// Scoring calculator using the balanced defaults
/// </summary>
public static class ScoringCalculator
{
    // Log scaling: LS(x) = log10(x + 1)
    public static double LS(double x) => Math.Log10(x + 1);

    public static double CalculateRecencyFactor(DateTime lastCommit)
    {
        var daysSince = (DateTime.UtcNow - lastCommit).TotalDays;
        return daysSince switch
        {
            <= 365 => 1.0,
            <= 1095 => 0.8,
            <= 1825 => 0.6,
            _ => 0.4
        };
    }

    public static int GetRecencyDays(DateTime lastCommit)
    {
        return (int)(DateTime.UtcNow - lastCommit).TotalDays;
    }

    public static double CalculateRepoScore(
        int stars,
        int usedByCount,
        long nugetTotalDownloads,
        int occurrences,
        double publicApiExposure,
        double confidence,
        double recencyFactor)
    {
        // repo_score_raw = R * ( 0.30*LS(Dg) + 0.20*LS(Nu) + 0.15*LS(S) + 0.15*LS(O) + 0.12*P + 0.08*C )
        return recencyFactor * (
            0.30 * LS(usedByCount) +
            0.20 * LS(nugetTotalDownloads) +
            0.15 * LS(stars) +
            0.15 * LS(occurrences) +
            0.12 * publicApiExposure +
            0.08 * confidence
        );
    }

    public static double CalculatePackageScore(
        long totalDownloads,
        int packageDependents,
        int repoOccurrences,
        long recentDownloads,
        double confidence,
        double recencyFactor)
    {
        // pkg_score_raw = R * ( 0.40*LS(TD) + 0.30*LS(PD) + 0.15*LS(RO) + 0.10*LS(RD) + 0.05*C )
        return recencyFactor * (
            0.40 * LS(totalDownloads) +
            0.30 * LS(packageDependents) +
            0.15 * LS(repoOccurrences) +
            0.10 * LS(recentDownloads) +
            0.05 * confidence
        );
    }

    public static double CalculatePercentile(double value, List<double> allValues)
    {
        if (allValues.Count == 0) return 0;
        var sorted = allValues.OrderBy(v => v).ToList();
        var index = sorted.BinarySearch(value);
        if (index < 0) index = ~index;
        return (double)index / sorted.Count * 100.0;
    }

    public static ImpactCategory DetermineCategory(double percentile)
    {
        return percentile switch
        {
            >= 99 => ImpactCategory.Critical,
            >= 90 => ImpactCategory.High,
            >= 70 => ImpactCategory.Medium,
            _ => ImpactCategory.Low
        };
    }

    public static double CalculateConfidence(MatchType matchType, bool sourceAvailable)
    {
        return matchType switch
        {
            MatchType.UsingDirective => 1.0,
            MatchType.QualifiedType => 1.0,
            MatchType.PackageReference when sourceAvailable => 1.0,
            MatchType.PackageReference when !sourceAvailable => 0.6,
            MatchType.BinaryArtifact => 0.3,
            _ => 0.3
        };
    }

    public static double DeterminePublicApiExposure(
        bool publishesNuGet,
        bool hasPublicSignatures,
        ClassificationType classification)
    {
        if (publishesNuGet && hasPublicSignatures)
            return 1.0;
        if (classification == ClassificationType.LibraryPublicApi)
            return 1.0;
        if (classification == ClassificationType.LibraryInternal)
            return 0.5;
        return 0.0;
    }
}

/// <summary>
/// Final analysis report structure
/// </summary>
public class AnalysisReport
{
    [JsonPropertyName("summary")]
    public ReportSummary Summary { get; set; } = new();

    [JsonPropertyName("top_impacted")]
    public List<ImpactEntityReport> TopImpacted { get; set; } = [];

    [JsonPropertyName("aggregates")]
    public ReportAggregates Aggregates { get; set; } = new();

    [JsonPropertyName("methodology")]
    public ReportMethodology Methodology { get; set; } = new();

    [JsonPropertyName("actions")]
    public List<string> Actions { get; set; } = [];
}

public class ReportSummary
{
    [JsonPropertyName("total_repos")]
    public int TotalRepos { get; set; }

    [JsonPropertyName("total_matches")]
    public int TotalMatches { get; set; }

    [JsonPropertyName("total_nuget_dependents")]
    public int TotalNuGetDependents { get; set; }

    [JsonPropertyName("total_nuget_downloads")]
    public long TotalNuGetDownloads { get; set; }
}

public class ImpactEntityReport
{
    [JsonPropertyName("repo_or_package")]
    public string RepoOrPackage { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("stars")]
    public int Stars { get; set; }

    [JsonPropertyName("forks")]
    public int Forks { get; set; }

    [JsonPropertyName("nuget_downloads")]
    public long NuGetDownloads { get; set; }

    [JsonPropertyName("occurrences")]
    public int Occurrences { get; set; }

    [JsonPropertyName("recency")]
    public int Recency { get; set; }

    [JsonPropertyName("impact_score")]
    public double ImpactScore { get; set; }

    [JsonPropertyName("sample_files")]
    public List<CodeSample> SampleFiles { get; set; } = [];

    [JsonPropertyName("recommended_mitigation")]
    public string RecommendedMitigation { get; set; } = string.Empty;
}

public class ReportAggregates
{
    [JsonPropertyName("matches_by_namespace")]
    public Dictionary<string, int> MatchesByNamespace { get; set; } = [];

    [JsonPropertyName("matches_by_country")]
    public Dictionary<string, int> MatchesByCountry { get; set; } = [];
}

public class ReportMethodology
{
    [JsonPropertyName("search_queries_used")]
    public List<string> SearchQueriesUsed { get; set; } = [];

    [JsonPropertyName("weighting_formula")]
    public string WeightingFormula { get; set; } = string.Empty;

    [JsonPropertyName("date_run")]
    public string DateRun { get; set; } = string.Empty;
}
