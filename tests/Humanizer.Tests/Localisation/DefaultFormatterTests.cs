namespace Humanizer.Tests.Localisation;

public class DefaultFormatterTests
{
    [Fact]
    [UseCulture("iv")]
    public void FallsBackToDefaultCollectionFormatterForUnsupportedCulture()
    {
        var dates = new[] { DateTime.UtcNow, DateTime.UtcNow.AddDays(10) };
        var humanized = dates.Humanize();

        Assert.Equal(dates[0] + " & " + dates[1], humanized);
    }

    [Theory]
    [InlineData("af", "gister")]
    [InlineData("da", "i går")]
    [InlineData("es", "ayer")]
    [InlineData("ku", "دوێنێ")]
    [InlineData("nl", "gisteren")]
    [InlineData("pt", "ontem")]
    public void DefaultFormatterFallbackLocalesStillResolveLocalizedDateStrings(string cultureName, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new(cultureName));

        Assert.Equal(expected, formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
    }
}
