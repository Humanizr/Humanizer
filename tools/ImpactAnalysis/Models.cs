namespace ImpactAnalysis;

/// <summary>
/// Represents a matched occurrence of an old namespace in a repository
/// </summary>
public class NamespaceMatch
{
    public string RepoFullName { get; set; } = string.Empty;
    public string RepoUrl { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Permalink { get; set; } = string.Empty;
    public string MatchLine { get; set; } = string.Empty;
    public string ContextAbove { get; set; } = string.Empty;
    public string ContextBelow { get; set; } = string.Empty;
    public MatchType MatchType { get; set; }
    public string Namespace { get; set; } = string.Empty;
}

public enum MatchType
{
    UsingDirective,
    QualifiedType,
    PackageReference,
    BinaryArtifact,
    Other
}

/// <summary>
/// Represents repository metadata
/// </summary>
public class RepositoryMetadata
{
    public string FullName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int Stars { get; set; }
    public int Forks { get; set; }
    public int Watchers { get; set; }
    public string PrimaryLanguage { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastCommitDate { get; set; }
    public int UsedByCount { get; set; }
    public bool IsFork { get; set; }
    public string? UpstreamParent { get; set; }
    public bool HasCIWorkflows { get; set; }
    public bool HasNuGetPackage { get; set; }
    public List<string> NuGetPackageIds { get; set; } = [];
    public int OccurrencesInRepo { get; set; }
    public int FilesWithMatches { get; set; }
    public List<NamespaceMatch> Matches { get; set; } = [];
}

/// <summary>
/// Represents NuGet package metadata
/// </summary>
public class NuGetPackageMetadata
{
    public string PackageId { get; set; } = string.Empty;
    public string LatestVersion { get; set; } = string.Empty;
    public long TotalDownloads { get; set; }
    public long RecentDownloads30d { get; set; }
    public long RecentDownloads90d { get; set; }
    public long RecentDownloads365d { get; set; }
    public int PackageDependentsCount { get; set; }
    public string? ProjectUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public bool SourceAvailable { get; set; }
    public RepositoryMetadata? SourceRepo { get; set; }
}

/// <summary>
/// Represents a scored impact entity (repo or package)
/// </summary>
public class ImpactEntity
{
    public string Name { get; set; } = string.Empty;
    public EntityType Type { get; set; }
    public int Stars { get; set; }
    public int Forks { get; set; }
    public int UsedByCount { get; set; }
    public long NuGetDownloads { get; set; }
    public int PackageDependents { get; set; }
    public int Occurrences { get; set; }
    public double PublicApiExposure { get; set; }
    public double Confidence { get; set; }
    public double RecencyFactor { get; set; }
    public int RecencyDays { get; set; }
    public double RawScore { get; set; }
    public double PercentileScore { get; set; }
    public ImpactCategory Category { get; set; }
    public string RecommendedMitigation { get; set; } = string.Empty;
    public List<CodeSample> SampleFiles { get; set; } = [];
    public RepositoryMetadata? Repository { get; set; }
    public NuGetPackageMetadata? Package { get; set; }
    public ClassificationType Classification { get; set; }
}

public enum EntityType
{
    Repository,
    Package
}

public enum ImpactCategory
{
    Critical,
    High,
    Medium,
    Low
}

public enum ClassificationType
{
    LibraryPublicApi,
    LibraryInternal,
    Application,
    TestSample,
    Documentation
}

public class CodeSample
{
    public string Path { get; set; } = string.Empty;
    public string Permalink { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
}
