using System.Net.Http.Json;
using System.Text.Json;

namespace ImpactAnalysis;

/// <summary>
/// NuGet package search and metadata collection
/// </summary>
public class NuGetSearcher
{
    private readonly HttpClient _httpClient;
    private readonly AnalysisConfig _config;
    private const string NuGetSearchApi = "https://azuresearch-usnc.nuget.org/query";
    private const string NuGetPackageApi = "https://api.nuget.org/v3/registration5-semver1";

    public NuGetSearcher(AnalysisConfig config)
    {
        _config = config;
        _httpClient = new HttpClient();
    }

    public async Task<List<NuGetPackageMetadata>> SearchDependentPackagesAsync()
    {
        var packages = new List<NuGetPackageMetadata>();

        Console.WriteLine("Searching for NuGet packages that depend on Humanizer...");

        // Search for packages depending on Humanizer.Core
        var humanizerCorePackages = await SearchPackagesByDependencyAsync("Humanizer.Core");
        packages.AddRange(humanizerCorePackages);

        // Search for packages depending on Humanizer
        var humanizerPackages = await SearchPackagesByDependencyAsync("Humanizer");
        packages.AddRange(humanizerPackages);

        // Remove duplicates
        var uniquePackages = packages
            .GroupBy(p => p.PackageId)
            .Select(g => g.First())
            .ToList();

        Console.WriteLine($"Found {uniquePackages.Count} unique dependent packages");

        // Enrich with additional metadata
        foreach (var package in uniquePackages)
        {
            await EnrichPackageMetadataAsync(package);
            await Task.Delay(100); // Rate limiting
        }

        return uniquePackages;
    }

    private async Task<List<NuGetPackageMetadata>> SearchPackagesByDependencyAsync(string dependencyId)
    {
        var packages = new List<NuGetPackageMetadata>();

        try
        {
            // NuGet API v3 doesn't directly support dependency search in the query endpoint
            // We'd need to use the catalog or search all packages and filter
            // For this implementation, we'll do a simplified search
            
            var searchUrl = $"{NuGetSearchApi}?q=dependency:{dependencyId}&take=100";
            var response = await _httpClient.GetAsync(searchUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"  Search failed for {dependencyId}: {response.StatusCode}");
                return packages;
            }

            var content = await response.Content.ReadAsStringAsync();
            var searchResult = JsonSerializer.Deserialize<NuGetSearchResult>(content);

            if (searchResult?.Data != null)
            {
                foreach (var item in searchResult.Data)
                {
                    var package = new NuGetPackageMetadata
                    {
                        PackageId = item.Id ?? string.Empty,
                        LatestVersion = item.Version ?? string.Empty,
                        TotalDownloads = item.TotalDownloads,
                        ProjectUrl = item.ProjectUrl,
                    };
                    packages.Add(package);
                }
            }

            Console.WriteLine($"  Found {packages.Count} packages depending on {dependencyId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Error searching for {dependencyId}: {ex.Message}");
        }

        return packages;
    }

    private async Task EnrichPackageMetadataAsync(NuGetPackageMetadata package)
    {
        try
        {
            // Get detailed package info from registration API
            var packageUrl = $"{NuGetPackageApi}/{package.PackageId.ToLowerInvariant()}/index.json";
            var response = await _httpClient.GetAsync(packageUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var registration = JsonSerializer.Deserialize<NuGetRegistration>(content);

                if (registration?.Items != null && registration.Items.Length > 0)
                {
                    var latestPage = registration.Items.Last();
                    if (latestPage.Items != null && latestPage.Items.Length > 0)
                    {
                        var latestVersion = latestPage.Items.Last();
                        var catalogEntry = latestVersion.CatalogEntry;

                        if (catalogEntry != null)
                        {
                            package.ProjectUrl = catalogEntry.ProjectUrl;
                            
                            // Try to extract repository URL
                            if (!string.IsNullOrEmpty(catalogEntry.Repository?.Url))
                            {
                                package.RepositoryUrl = catalogEntry.Repository.Url;
                                package.SourceAvailable = IsPublicGitHubRepo(catalogEntry.Repository.Url);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Error enriching {package.PackageId}: {ex.Message}");
        }
    }

    private bool IsPublicGitHubRepo(string url)
    {
        return !string.IsNullOrEmpty(url) && 
               (url.Contains("github.com", StringComparison.OrdinalIgnoreCase) ||
                url.Contains("gitlab.com", StringComparison.OrdinalIgnoreCase));
    }

    // NuGet API response models
    private class NuGetSearchResult
    {
        public int TotalHits { get; set; }
        public NuGetPackageInfo[]? Data { get; set; }
    }

    private class NuGetPackageInfo
    {
        public string? Id { get; set; }
        public string? Version { get; set; }
        public long TotalDownloads { get; set; }
        public string? ProjectUrl { get; set; }
    }

    private class NuGetRegistration
    {
        public NuGetRegistrationPage[]? Items { get; set; }
    }

    private class NuGetRegistrationPage
    {
        public NuGetRegistrationItem[]? Items { get; set; }
    }

    private class NuGetRegistrationItem
    {
        public NuGetCatalogEntry? CatalogEntry { get; set; }
    }

    private class NuGetCatalogEntry
    {
        public string? ProjectUrl { get; set; }
        public NuGetRepository? Repository { get; set; }
    }

    private class NuGetRepository
    {
        public string? Url { get; set; }
    }
}
