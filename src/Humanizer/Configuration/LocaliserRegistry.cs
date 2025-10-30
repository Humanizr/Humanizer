using System.Runtime.CompilerServices;

namespace Humanizer;

/// <summary>
/// A registry of localised system components with their associated locales
/// </summary>
public class LocaliserRegistry<TLocaliser>
    where TLocaliser : class
{
    readonly Dictionary<string, Func<CultureInfo, TLocaliser>> localisers = [];
    readonly Func<CultureInfo, TLocaliser> defaultLocaliser;
#pragma warning disable IL2091 // We create the value ourselves via factory, not using parameterless constructor
    readonly ConditionalWeakTable<CultureInfo, TLocaliser> resolvedLocalisersCache = new();
#pragma warning restore IL2091

    /// <summary>
    /// Creates a localiser registry with the default localiser set to the provided value
    /// </summary>
    public LocaliserRegistry(TLocaliser defaultLocaliser) =>
        this.defaultLocaliser = _ => defaultLocaliser;

    /// <summary>
    /// Creates a localiser registry with the default localiser factory set to the provided value
    /// </summary>
    public LocaliserRegistry(Func<CultureInfo, TLocaliser> defaultLocaliser) =>
        this.defaultLocaliser = defaultLocaliser;

    /// <summary>
    /// Gets the localiser for the current thread's UI culture
    /// </summary>
    public TLocaliser ResolveForUiCulture() =>
        ResolveForCulture(null);

    /// <summary>
    /// Gets the localiser for the specified culture
    /// </summary>
    /// <param name="culture">The culture to retrieve localiser for. If not specified, current thread's UI culture is used.</param>
    public TLocaliser ResolveForCulture(CultureInfo? culture)
    {
        var cultureInfo = culture ?? CultureInfo.CurrentUICulture;
        return resolvedLocalisersCache.GetValue(cultureInfo, c => FindLocaliser(c)(c));
    }

    /// <summary>
    /// Registers the localiser for the culture provided
    /// </summary>
    public void Register(string localeCode, TLocaliser localiser) =>
        localisers[localeCode] = _ => localiser;

    /// <summary>
    /// Registers the localiser factory for the culture provided
    /// </summary>
    public void Register(string localeCode, Func<CultureInfo, TLocaliser> localiser) =>
        localisers[localeCode] = localiser;

    Func<CultureInfo, TLocaliser> FindLocaliser(CultureInfo culture)
    {
        for (var c = culture; !string.IsNullOrEmpty(c.Name); c = c.Parent)
        {
            if (localisers.TryGetValue(c.Name, out var localiser))
            {
                return localiser;
            }
        }

        return defaultLocaliser;
    }
}