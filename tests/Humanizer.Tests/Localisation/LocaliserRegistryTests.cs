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
    public void ResolveForCultureNullUsesCurrentCulture()
    {
        var registry = new LocaliserRegistry<string>(culture => $"default:{culture.Name}");
        registry.Register("fr", culture => $"fr:{culture.Name}");

        using var _ = new DistinctCultureSwap(new("en-US"), new("fr-CH"));

        Assert.Equal("default:en-US", registry.ResolveForCulture(null));
    }

    [Fact]
    public void DefaultRegistryCallersUseCurrentCulture()
    {
        using var _ = new DistinctCultureSwap(new("en-US"), new("de-DE"));

        Assert.Multiple(() =>
        {
            Assert.Equal("one", 1.ToWords());
            Assert.Equal("1st", 1.Ordinalize());
            Assert.Equal("January 1st, 2015", new DateTime(2015, 1, 1).ToOrdinalWords());
            Assert.Equal("January 1st, 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());
            Assert.Equal("a and b", new List<string> { "a", "b" }.Humanize());
            Assert.Equal("1 day", TimeSpan.FromDays(1).Humanize());
            Assert.Equal("one twenty-three", new TimeOnly(13, 23).ToClockNotation());
        });
    }

    [Fact]
    public void OrdinalizeNullCultureUsesCurrentCultureForNumberFormatting()
    {
        var culture = new CultureInfo("en-US");
        culture.NumberFormat.NegativeSign = "~";
        var uiCulture = new CultureInfo("en-US");
        uiCulture.NumberFormat.NegativeSign = "!";
        using var _ = new DistinctCultureSwap(culture, uiCulture);

        Assert.Equal("~1st", (-1).Ordinalize(null!));
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