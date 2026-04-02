namespace Humanizer.Tests.Localisation;

public class ResourcesTests
{
    [Fact]
    public void GetResourceUsesCurrentCultureWhenCultureIsNull()
    {
        using (LocaleCoverageData.UseCulture("ro"))
        {
            var format = Resources.GetResource("DateHumanize_MultipleYearsAgo");
            Assert.Equal("acum {0}{1} ani", format);
        }
    }

    [Fact]
    public void GetResourceUsesExplicitExactCulture()
    {
        var format = Resources.GetResource("DateHumanize_MultipleYearsAgo", new("ro"));
        Assert.Equal("acum {0}{1} ani", format);
    }

    [Fact]
    public void TryGetResourceUsesCurrentUiCultureWhenCultureIsNull()
    {
        using (LocaleCoverageData.UseCulture("ro"))
        {
            Assert.True(Resources.TryGetResource("DateHumanize_SingleYearAgo", null, out var result));
            Assert.Equal("acum un an", result);
        }
    }

    [Fact]
    public void GetResourceFallsBackToParentCulture()
    {
        var format = Resources.GetResource("DateHumanize_SingleYearAgo", new("ro-RO"));
        Assert.Equal("acum un an", format);
    }

    [Fact]
    public void GetResourceFallsBackToNeutralResourcesWhenNoCultureSpecificValueExists()
    {
        var format = Resources.GetResource("DateHumanize_SingleYearAgo", new("zu-ZA"));
        Assert.Equal("one year ago", format);
    }

    [Fact]
    public void GetResourceThrowsForMissingKey()
    {
        var exception = Assert.Throws<ArgumentException>(() => Resources.GetResource("DefinitelyMissingResourceKey", new("zu-ZA")));
        Assert.Equal("resourceKey", exception.ParamName);
    }

    [Fact]
    public void TryGetResourceUsesExactCultureOnly()
    {
        var culture = new CultureInfo("ro");
        var localizedKeys = LocaleCoverageData.LocalizedResourceKeysByLocale["ro"];
        var missingKey = LocaleCoverageData.NeutralResourceKeys.First(key => !localizedKeys.Contains(key, StringComparer.Ordinal));

        Assert.False(Resources.TryGetResource(missingKey, culture, out var exactResult));
        Assert.Null(exactResult);

        var fallbackCulture = new CultureInfo("zu-ZA");
        Assert.Equal(Resources.GetResource(missingKey, culture), Resources.GetResource(missingKey, fallbackCulture));

        Assert.True(Resources.TryGetResource("DateHumanize_SingleYearAgo", culture, out var loadedExactResult));
        Assert.Equal("acum un an", loadedExactResult);

        Assert.Equal("acum un an", Resources.GetResource("DateHumanize_SingleYearAgo", culture));

        Assert.True(Resources.TryGetResource("DateHumanize_SingleYearAgo", culture, out exactResult));
        Assert.Equal("acum un an", exactResult);
    }

    [Fact]
    public void TryGetResourceDoesNotFallBackToNeutralResources()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_SingleYearAgo", new("zu-ZA"), out var result));
        Assert.Equal("one year ago", result);
    }
}
