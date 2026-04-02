namespace Humanizer.Tests.Localisation;

public class LocaliserRegistryTests
{
    [Fact]
    public void ResolveForUiCultureUsesCurrentUiCultureAndParentLocaleFallback()
    {
        var registry = new LocaliserRegistry<string>(culture => $"default:{culture.Name}");
        registry.Register("fr", culture => $"fr:{culture.Name}");

        using var _ = LocaleCoverageData.UseCulture("fr-CH");

        Assert.Equal("fr:fr-CH", registry.ResolveForUiCulture());
    }

    [Fact]
    public void ResolveForCultureNullUsesCurrentUiCulture()
    {
        var registry = new LocaliserRegistry<string>(culture => $"default:{culture.Name}");
        registry.Register("fr", culture => $"fr:{culture.Name}");

        using var _ = LocaleCoverageData.UseCulture("fr-CH");

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

        Assert.Equal("default:zu-ZA", registry.ResolveForCulture(new("zu-ZA")));
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
}
