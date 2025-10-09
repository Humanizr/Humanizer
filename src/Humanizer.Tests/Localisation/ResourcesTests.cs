namespace Humanizer.Tests.Localisation;

public class ResourcesTests
{
    [Fact]
    [UseCulture("ro")]
    public void CanGetCultureSpecificTranslationsWithImplicitCulture()
    {
        var format = Resources.GetResource("DateHumanize_MultipleYearsAgo");
        Assert.Equal("acum {0}{1} ani", format);
    }

    [Fact]
    public void CanGetCultureSpecificTranslationsWithExplicitCulture()
    {
        var format = Resources.GetResource("DateHumanize_MultipleYearsAgo", new("ro"));
        Assert.Equal("acum {0}{1} ani", format);
    }

    [Fact]
    public void ExplicitCultureShouldNotFallbackToEnglish()
    {
        // This test verifies the fix for the Blazor WebAssembly issue where explicit culture parameters were ignored
        // Test with German culture
        var germanFormat = Resources.GetResource("DateHumanize_MultipleHoursAgo", new CultureInfo("de"));
        Assert.NotEqual("before {0}{1} hours", germanFormat); // Should not be English
        Assert.Equal("vor {0}{1} Stunden", germanFormat); // Should be German

        // Test with French culture
        var frenchFormat = Resources.GetResource("DateHumanize_MultipleHoursAgo", new CultureInfo("fr"));
        Assert.NotEqual("before {0}{1} hours", frenchFormat); // Should not be English
        Assert.Equal("il y a {0}{1} heures", frenchFormat); // Should be French
    }
}