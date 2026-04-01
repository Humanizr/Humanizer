using System.Resources;

namespace Humanizer;

/// <summary>
/// Provides access to Humanizer's localized string resources.
/// </summary>
public static class Resources
{
    static readonly ResourceManager ResourceManager = new("Humanizer.Properties.Resources", typeof(Resources).GetTypeInfo()
        .Assembly);

    /// <summary>
    /// Returns the value of the specified string resource.
    /// </summary>
    /// <param name="resourceKey">The name of the resource to retrieve.</param>
    /// <param name="culture">
    /// The culture of the resource to retrieve. If not specified, the current thread's UI culture is used.
    /// </param>
    /// <returns>The value of the resource localized for the specified culture.</returns>
    /// <exception cref="ArgumentException">Thrown when the resource key cannot be resolved for the requested culture.</exception>
    public static string GetResource(string resourceKey, CultureInfo? culture = null)
    {
        // When an explicit culture is provided, we must ensure the exact culture's resource set
        // is loaded before falling back. In Blazor WebAssembly, satellite assemblies are loaded
        // lazily, and ResourceManager.GetString's fallback logic may cache an incorrect neutral
        // culture result before the satellite assembly loads. To prevent this, we:
        //   1) Load the exact culture's ResourceSet (createIfNotExists: true, tryParents: false)
        //   2) Only fall back to GetString if the exact culture's resource set doesn't contain the key

        string? resource = null;

        // Only attempt to load the exact culture's resource set when an explicit culture is provided.
        // This preserves the previous behavior for callers that pass null (they fall through to GetString).
        if (culture != null)
        {
            // Load the exact culture's resource set without trying parents.
            // This ensures the satellite assembly is loaded in Blazor WebAssembly scenarios.
            var exactResourceSet = ResourceManager.GetResourceSet(culture, createIfNotExists: true, tryParents: false);
            resource = exactResourceSet?.GetString(resourceKey);
        }

        // If not found in the exact culture (or culture was null), use the standard GetString
        // which performs the full fallback chain and accepts null culture.
        resource ??= ResourceManager.GetString(resourceKey, culture);

        if (resource == null || string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($@"The resource object with key '{resourceKey}' was not found", nameof(resourceKey));
        }

        return resource;
    }

    /// <summary>
    /// Tries to get the value of the specified string resource using the standard fallback chain.
    /// </summary>
    /// <param name="resourceKey">The name of the resource to retrieve.</param>
    /// <param name="culture">
    /// The culture of the resource to retrieve. If not specified, the current thread's UI culture is used.
    /// </param>
    /// <param name="result">The localized value if the resource is found; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the specified string resource was found for the given culture; otherwise, <c>false</c>.</returns>
    public static bool TryGetResource(string resourceKey, CultureInfo? culture, [NotNullWhen(true)] out string? result)
    {
        string? resource = null;

        // Keep the exact-culture preload here too. The non-throwing path shares the same Blazor
        // WebAssembly satellite-loading pitfall as GetResource; only the final missing-resource
        // behavior differs.
        if (culture != null)
        {
            var exactResourceSet = ResourceManager.GetResourceSet(culture, createIfNotExists: true, tryParents: false);
            resource = exactResourceSet?.GetString(resourceKey);
        }

        // Once the exact culture had a chance to populate its satellite assembly, fall back to the
        // normal ResourceManager chain for parents and neutral resources.
        result = resource ?? ResourceManager.GetString(resourceKey, culture);
        return result is not null;
    }

    /// <inheritdoc cref="TryGetResource"/>
    internal static bool TryGetResourceWithFallback(string resourceKey, CultureInfo? culture, [NotNullWhen(true)] out string? result)
    {
        // Keep the call site descriptive without duplicating the lookup path or changing fallback semantics.
        return TryGetResource(resourceKey, culture, out result);
    }
}
