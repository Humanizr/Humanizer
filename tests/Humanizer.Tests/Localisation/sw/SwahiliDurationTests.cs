namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliDurationTests
{
    [Theory]
    [InlineData(TimeUnit.Day, 1, "siku 1")]
    [InlineData(TimeUnit.Day, 2, "siku 2")]
    [InlineData(TimeUnit.Month, 1, "mwezi 1")]
    [InlineData(TimeUnit.Month, 2, "miezi 2")]
    public void TimeSpanHumanize_UsesSwahiliDurationPhrases(TimeUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("sw"));
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count));
    }

    [Fact]
    public void TimeSpanHumanize_ToWords_UsesSwahiliSingleWordForm()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("sw"));
        Assert.Equal("siku moja", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Fact]
    public void ToAge_UsesSwahiliTemplate()
    {
        Assert.Equal("umri wa mwaka 1", TimeSpan.FromDays(366).ToAge(new CultureInfo("sw")));
    }
}