using System.Resources;

namespace Humanizer;

/// <summary>
/// Provides access to the resources of Humanizer
/// </summary>
public static class Resources
{
    static readonly ResourceManager ResourceManager = new("Humanizer.Properties.Resources", typeof(Resources).GetTypeInfo()
        .Assembly);

    /// <summary>
    /// Returns the value of the specified string resource
    /// </summary>
    /// <param name="resourceKey">The name of the resource to retrieve.</param>
    /// <param name="culture">The culture of the resource to retrieve. If not specified, current thread's UI culture is used.</param>
    /// <returns>The value of the resource localized for the specified culture.</returns>
    public static string GetResource(string resourceKey, CultureInfo? culture = null)
    {
        var resource = GetResourceInternal(resourceKey, culture);

        if (resource == null || string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($@"The resource object with key '{resourceKey}' was not found", nameof(resourceKey));
        }

        return resource;
    }

    /// <summary>
    /// Internal method to get resource with enhanced Blazor WebAssembly support
    /// </summary>
    /// <param name="resourceKey">The name of the resource to retrieve.</param>
    /// <param name="culture">The culture of the resource to retrieve.</param>
    /// <returns>The value of the resource localized for the specified culture, or null if not found.</returns>
    static string? GetResourceInternal(string resourceKey, CultureInfo? culture)
    {
        if (culture == null)
        {
            return ResourceManager.GetString(resourceKey);
        }

        // First, try the standard ResourceManager approach
        var resource = ResourceManager.GetString(resourceKey, culture);
        
        // In Blazor WebAssembly, ResourceManager.GetString might fallback to neutral culture
        // even when a specific culture is requested. We need to validate that we got the 
        // resource for the correct culture, not a fallback.
        if (resource != null && IsCorrectCultureResource(resourceKey, resource, culture))
        {
            return resource;
        }

        // If ResourceManager.GetString didn't work or returned wrong culture,
        // try alternative approach using GetResourceSet with exact culture match
        return TryGetResourceWithoutFallback(resourceKey, culture);
    }

    /// <summary>
    /// Tries to get resource for exact culture without fallback to parent cultures
    /// </summary>
    /// <param name="resourceKey">The resource key</param>
    /// <param name="culture">The specific culture</param>
    /// <returns>Resource string for exact culture, or null if not found</returns>
    static string? TryGetResourceWithoutFallback(string resourceKey, CultureInfo culture)
    {
        try
        {
            // Try to get the resource set for the specific culture without fallback
            var resourceSet = ResourceManager.GetResourceSet(culture, createIfNotExists: false, tryParents: false);
            return resourceSet?.GetString(resourceKey);
        }
        catch
        {
            // If ResourceSet approach fails, return null to allow normal fallback behavior
            return null;
        }
    }

    /// <summary>
    /// Validates that the returned resource is actually for the requested culture
    /// by checking if it's different from the neutral culture resource
    /// </summary>
    /// <param name="resourceKey">The resource key</param>
    /// <param name="resource">The resource value returned</param>
    /// <param name="culture">The requested culture</param>
    /// <returns>True if the resource appears to be for the correct culture</returns>
    static bool IsCorrectCultureResource(string resourceKey, string resource, CultureInfo culture)
    {
        // If requesting neutral culture or invariant culture, any result is correct
        if (string.IsNullOrEmpty(culture.Name) || culture.Equals(CultureInfo.InvariantCulture))
        {
            return true;
        }

        try
        {
            // Get the neutral culture resource for comparison
            var neutralResource = ResourceManager.GetString(resourceKey, CultureInfo.InvariantCulture);
            
            // If we got the same string as neutral culture, it might be a fallback
            // This is a heuristic - if the localized resource is identical to neutral,
            // it could mean ResourceManager fell back to neutral culture
            if (neutralResource != null && string.Equals(resource, neutralResource, StringComparison.Ordinal))
            {
                // Double-check by trying to get the resource set directly
                var cultureResource = TryGetResourceWithoutFallback(resourceKey, culture);
                return cultureResource != null && !string.Equals(cultureResource, neutralResource, StringComparison.Ordinal);
            }

            // If it's different from neutral, it's likely the correct culture
            return true;
        }
        catch
        {
            // If we can't validate, assume it's correct to avoid breaking existing functionality
            return true;
        }
    }

    /// <summary>
    /// Tries to get the value of the specified string resource, without fallback
    /// </summary>
    /// <param name="resourceKey">The name of the resource to retrieve.</param>
    /// <param name="culture">The culture of the resource to retrieve. If not specified, current thread's UI culture is used.</param>
    /// <param name="result">The value of the resource localized for the specified culture if found; null otherwise.</param>
    /// <returns>true if the specified string resource was found for the given culture; otherwise, false.</returns>
    public static bool TryGetResource(string resourceKey, CultureInfo? culture, [NotNullWhen(true)] out string? result)
    {
        culture ??= CultureInfo.CurrentUICulture;
        var resourceSet = ResourceManager.GetResourceSet(culture, createIfNotExists: false, tryParents: false);
        result = resourceSet?.GetString(resourceKey);
        return result is not null;
    }
}