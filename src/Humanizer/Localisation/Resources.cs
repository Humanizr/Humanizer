using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

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
        // We avoid populating ResourceManager's fallback cache unless absolutely necessary.
        // ResourceManager.GetString follows the full fallback chain (culture -> parent -> neutral)
        // and will create/cache ResourceSets for whichever culture supplies the string. On
        // environments with lazy satellite assembly loading (Blazor WebAssembly), this can
        // result in an incorrect fallback to the neutral culture and that fallback being cached
        // for subsequent lookups. To minimize that risk and improve performance, we:
        //   1) Attempt to read the exact culture's ResourceSet without creating or probing parents.
        //   2) If not found, check already-loaded parent ResourceSets without creating new ones.
        //   3) Only as a last resort call ResourceManager.GetString which may perform fallback
        //      and populate the cache.

        string? resource = null;

        // Only call GetResourceSet when we have an explicit, non-null culture to pass 
        // This preserves the previous behavior for callers that pass null (they fall through to GetString).
        if (culture != null)
        {
            // Exact culture only, don't try parents and don't create resource sets.
            var exactResourceSet = ResourceManager.GetResourceSet(culture, createIfNotExists: false, tryParents: false);
            resource = exactResourceSet?.GetString(resourceKey);

            if (resource == null)
            {
                // Check already-loaded parents without creating/adding new ResourceSets to the cache.
                var parentResourceSet = ResourceManager.GetResourceSet(culture, createIfNotExists: false, tryParents: true);
                resource = parentResourceSet?.GetString(resourceKey);
            }
        }

        // Last resort: use the standard GetString which performs the full fallback chain and accepts null culture.
        if (resource == null)
        {
            resource = ResourceManager.GetString(resourceKey, culture);
        }

        if (resource == null || string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($@"The resource object with key '{resourceKey}' was not found", nameof(resourceKey));
        }

        return resource;
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
