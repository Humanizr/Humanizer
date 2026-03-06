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
}
