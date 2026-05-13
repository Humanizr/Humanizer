namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliRelativeDateTests
{
    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "jana")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "kesho")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 siku zilizopita")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "baada ya siku 2")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "mwezi 1 uliopita")]
    [InlineData(2, TimeUnit.Month, Tense.Past, "2 miezi iliyopita")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "sasa")]
    public void DateHumanize_UsesSwahiliPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("sw"));
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesSwahiliNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("kamwe", date.Humanize(culture: new CultureInfo("sw")));
    }
}