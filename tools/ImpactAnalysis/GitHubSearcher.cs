using Octokit;
using System.Text.RegularExpressions;

namespace ImpactAnalysis;

/// <summary>
/// GitHub search and data collection
/// </summary>
public class GitHubSearcher
{
    private readonly GitHubClient _client;
    private readonly AnalysisConfig _config;

    public GitHubSearcher(AnalysisConfig config)
    {
        _config = config;
        _client = new GitHubClient(new ProductHeaderValue("HumanizerImpactAnalysis"));
        
        if (!string.IsNullOrEmpty(config.GitHubToken))
        {
            _client.Credentials = new Credentials(config.GitHubToken);
        }
    }

    public async Task<List<RepositoryMetadata>> SearchRepositoriesAsync()
    {
        var allRepos = new Dictionary<string, RepositoryMetadata>();
        var searchQueries = GenerateSearchQueries();

        Console.WriteLine($"Starting GitHub search with {searchQueries.Count} queries...");

        foreach (var query in searchQueries)
        {
            Console.WriteLine($"Executing query: {query}");
            try
            {
                await SearchWithQueryAsync(query, allRepos);
                // Rate limiting: wait between queries
                await Task.Delay(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query '{query}': {ex.Message}");
            }
        }

        Console.WriteLine($"Found {allRepos.Count} unique repositories");
        return allRepos.Values.ToList();
    }

    private List<string> GenerateSearchQueries()
    {
        var queries = new List<string>();

        // Search for using directives
        foreach (var ns in _config.OldNamespaces)
        {
            queries.Add($"\"using {ns}\" language:C#");
            queries.Add($"\"{ns}.\" language:C#");
        }

        // Search for package references
        queries.Add("\"PackageReference Include=\\\"Humanizer.Core\\\"\" language:XML");
        queries.Add("\"PackageReference Include=\\\"Humanizer\\\"\" language:XML");
        
        // Search for DLL references
        queries.Add("\"Humanizer.Core.dll\"");
        queries.Add("\"Humanizer.dll\"");

        return queries;
    }

    private async Task SearchWithQueryAsync(string query, Dictionary<string, RepositoryMetadata> allRepos)
    {
        try
        {
            var request = new SearchCodeRequest(query)
            {
                PerPage = 100
            };

            var results = await _client.Search.SearchCode(request);
            
            Console.WriteLine($"  Found {results.TotalCount} code results");

            foreach (var item in results.Items.Take(100))
            {
                var repoFullName = item.Repository.FullName;
                
                if (!allRepos.ContainsKey(repoFullName))
                {
                    var repoMeta = await GetRepositoryMetadataAsync(item.Repository);
                    allRepos[repoFullName] = repoMeta;
                }

                var match = CreateMatchFromCodeResult(item, query);
                allRepos[repoFullName].Matches.Add(match);
                allRepos[repoFullName].OccurrencesInRepo++;
                
                if (!allRepos[repoFullName].Matches.Any(m => m.FilePath == item.Path))
                {
                    allRepos[repoFullName].FilesWithMatches++;
                }
            }
        }
        catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            Console.WriteLine($"  Rate limit exceeded. Waiting 60 seconds...");
            await Task.Delay(60000);
        }
    }

    private async Task<RepositoryMetadata> GetRepositoryMetadataAsync(Repository repo)
    {
        var metadata = new RepositoryMetadata
        {
            FullName = repo.FullName,
            Url = repo.HtmlUrl,
            Stars = repo.StargazersCount,
            Forks = repo.ForksCount,
            Watchers = repo.SubscribersCount,
            PrimaryLanguage = repo.Language ?? "Unknown",
            Size = repo.Size,
            LastCommitDate = repo.PushedAt?.UtcDateTime ?? DateTime.UtcNow.AddYears(-10),
            IsFork = repo.Fork
        };

        if (repo.Fork && repo.Parent != null)
        {
            metadata.UpstreamParent = repo.Parent.FullName;
        }

        // Check for CI workflows
        try
        {
            var workflowsPath = ".github/workflows";
            var contents = await _client.Repository.Content.GetAllContents(repo.Id, workflowsPath);
            metadata.HasCIWorkflows = contents.Any();
        }
        catch
        {
            metadata.HasCIWorkflows = false;
        }

        // Check for NuGet package indicators
        await CheckForNuGetPackageAsync(metadata, repo);

        return metadata;
    }

    private async Task CheckForNuGetPackageAsync(RepositoryMetadata metadata, Repository repo)
    {
        try
        {
            // Search for .csproj files with Package properties
            var request = new SearchCodeRequest($"repo:{repo.FullName} PackageId filename:csproj")
            {
                PerPage = 10
            };
            var results = await _client.Search.SearchCode(request);
            
            if (results.TotalCount > 0)
            {
                metadata.HasNuGetPackage = true;
                
                // Try to extract package IDs
                foreach (var item in results.Items.Take(5))
                {
                    try
                    {
                        var content = await _client.Repository.Content.GetAllContents(repo.Id, item.Path);
                        var csprojContent = content[0].Content;
                        
                        var packageIdMatch = Regex.Match(csprojContent, @"<PackageId>([^<]+)</PackageId>");
                        if (packageIdMatch.Success)
                        {
                            metadata.NuGetPackageIds.Add(packageIdMatch.Groups[1].Value);
                        }
                    }
                    catch { }
                }
            }

            // Also check for .nuspec files
            var nuspecRequest = new SearchCodeRequest($"repo:{repo.FullName} extension:nuspec")
            {
                PerPage = 5
            };
            var nuspecResults = await _client.Search.SearchCode(nuspecRequest);
            
            if (nuspecResults.TotalCount > 0)
            {
                metadata.HasNuGetPackage = true;
            }
        }
        catch
        {
            // If we can't determine, assume false
        }
    }

    private NamespaceMatch CreateMatchFromCodeResult(SearchCode item, string query)
    {
        var match = new NamespaceMatch
        {
            RepoFullName = item.Repository.FullName,
            RepoUrl = item.Repository.HtmlUrl,
            FilePath = item.Path,
            Permalink = item.HtmlUrl,
            MatchLine = item.Name, // This is limited, we'd need to fetch content for actual lines
            MatchType = DetermineMatchType(query, item.Path)
        };

        // Determine namespace from query
        foreach (var ns in _config.OldNamespaces)
        {
            if (query.Contains(ns))
            {
                match.Namespace = ns;
                break;
            }
        }

        return match;
    }

    private MatchType DetermineMatchType(string query, string filePath)
    {
        if (query.Contains("using "))
            return MatchType.UsingDirective;
        if (filePath.EndsWith(".csproj"))
            return MatchType.PackageReference;
        if (query.Contains(".dll"))
            return MatchType.BinaryArtifact;
        if (query.Contains("."))
            return MatchType.QualifiedType;
        return MatchType.Other;
    }

    public async Task<int> GetDependentsCountAsync(string repoFullName)
    {
        // This would require GraphQL API
        // For now, return 0 as placeholder
        // In a full implementation, we'd use Octokit.GraphQL
        return 0;
    }
}
