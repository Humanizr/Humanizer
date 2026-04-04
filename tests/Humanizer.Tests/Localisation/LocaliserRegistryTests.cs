namespace Humanizer.Tests.Localisation;

public class LocaliserRegistryTests
{
    [Fact]
    public void ResolveForUiCultureUsesCurrentUiCultureAndParentLocaleFallback()
    {
        var registry = new LocaliserRegistry<string>(culture => $"default:{culture.Name}");
        registry.Register("fr", culture => $"fr:{culture.Name}");

        using var _ = new DistinctCultureSwap(new("en-US"), new("fr-CH"));

        Assert.Equal("fr:fr-CH", registry.ResolveForUiCulture());
    }

    [Fact]
    public void ResolveForCultureNullUsesCurrentUiCulture()
    {
        var registry = new LocaliserRegistry<string>(culture => $"default:{culture.Name}");
        registry.Register("fr", culture => $"fr:{culture.Name}");

        using var _ = new DistinctCultureSwap(new("en-US"), new("fr-CH"));

        Assert.Equal("fr:fr-CH", registry.ResolveForCulture(null));
    }

    [Fact]
    public void ResolveForCulturePrefersExactLocaleOverParentLocale()
    {
        var registry = new LocaliserRegistry<string>(culture => $"default:{culture.Name}");
        registry.Register("fr", culture => $"fr:{culture.Name}");
        registry.Register("fr-CH", culture => $"fr-CH:{culture.Name}");

        Assert.Equal("fr-CH:fr-CH", registry.ResolveForCulture(new("fr-CH")));
    }

    [Fact]
    public void ResolveForCultureFallsBackToDefaultWhenNoLocaleMatches()
    {
        var registry = new LocaliserRegistry<string>(culture => $"default:{culture.Name}");
        registry.Register("fr", culture => $"fr:{culture.Name}");

        Assert.Equal("default:eo", registry.ResolveForCulture(new("eo")));
    }

    [Fact]
    public void RegisterInstanceThrowsAfterRegistryHasBeenUsed()
    {
        var registry = new LocaliserRegistry<string>("default");
        _ = registry.ResolveForCulture(new("en"));

        var exception = Assert.Throws<InvalidOperationException>(() => registry.Register("fr", "bonjour"));
        Assert.Equal("Cannot register localisers after the registry has been used.", exception.Message);
    }

    [Fact]
    public void RegisterFactoryThrowsAfterRegistryHasBeenUsed()
    {
        var registry = new LocaliserRegistry<string>("default");
        _ = registry.ResolveForCulture(new("en"));

        var exception = Assert.Throws<InvalidOperationException>(() => registry.Register("fr", _ => "bonjour"));
        Assert.Equal("Cannot register localisers after the registry has been used.", exception.Message);
    }

    [Fact]
    public void GetRegisteredLocaleCodesReturnsSortedRegisteredLocales()
    {
        var registry = new LocaliserRegistry<string>("default");
        registry.Register("fr-CH", "fr-CH");
        registry.Register("en", "en");
        registry.Register("fr", "fr");

        Assert.Equal(["en", "fr", "fr-CH"], registry.GetRegisteredLocaleCodes());
    }
}

sealed class DistinctCultureSwap : IDisposable
{
    readonly CultureInfo originalCulture = CultureInfo.CurrentCulture;
    readonly CultureInfo originalUiCulture = CultureInfo.CurrentUICulture;

    public DistinctCultureSwap(CultureInfo culture, CultureInfo uiCulture)
    {
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = uiCulture;
    }

    public void Dispose()
    {
        CultureInfo.CurrentCulture = originalCulture;
        CultureInfo.CurrentUICulture = originalUiCulture;
    }
}
