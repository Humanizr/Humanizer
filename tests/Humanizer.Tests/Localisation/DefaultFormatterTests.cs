namespace Humanizer.Tests.Localisation;

public class DefaultFormatterTests
{
    [Fact]
    [UseCulture("iv")]
    public void HandlesNotImplementedCollectionFormattersGracefully()
    {
        var a = new[] { DateTime.UtcNow, DateTime.UtcNow.AddDays(10) };
        var b = a.Humanize();

        Assert.Equal(a[0] + " & " + a[1], b);
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
