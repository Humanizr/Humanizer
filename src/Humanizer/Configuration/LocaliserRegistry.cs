namespace Humanizer;

/// <summary>
/// A registry of localised system components with their associated locales
/// </summary>
public class LocaliserRegistry<TLocaliser>
    where TLocaliser : class
{
    readonly Dictionary<string, Func<CultureInfo, TLocaliser>> localisersBuilder = [];
    FrozenDictionary<string, Func<CultureInfo, TLocaliser>>? frozenLocalisers;
    readonly Func<CultureInfo, TLocaliser> defaultLocaliser;
    readonly ConcurrentDictionary<CultureInfo, TLocaliser> resolvedLocalisersCache = new();

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
        return resolvedLocalisersCache.GetOrAdd(cultureInfo, c => FindLocaliser(c)(c));
    }

    /// <summary>
    /// Registers the localiser for the culture provided
    /// </summary>
    public void Register(string localeCode, TLocaliser localiser)
    {
        if (frozenLocalisers != null)
        {
            throw new InvalidOperationException("Cannot register localisers after the registry has been used.");
        }
        localisersBuilder[localeCode] = _ => localiser;
    }

    /// <summary>
    /// Registers the localiser factory for the culture provided
    /// </summary>
    public void Register(string localeCode, Func<CultureInfo, TLocaliser> localiser)
    {
        if (frozenLocalisers != null)
        {
            throw new InvalidOperationException("Cannot register localisers after the registry has been used.");
        }
        localisersBuilder[localeCode] = localiser;
    }

    Func<CultureInfo, TLocaliser> FindLocaliser(CultureInfo culture)
    {
        // Freeze the dictionary on first use for better read performance
        frozenLocalisers ??= localisersBuilder.ToFrozenDictionary();

        for (var c = culture; !string.IsNullOrEmpty(c.Name); c = c.Parent)
        {
            if (frozenLocalisers.TryGetValue(c.Name, out var localiser))
            {
                return localiser;
            }
        }

        return defaultLocaliser;
    }
}