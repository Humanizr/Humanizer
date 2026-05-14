namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboDurationTests
{
    [Theory]
    [InlineData(TimeUnit.Day, 1, "ụbọchị 1")]
    [InlineData(TimeUnit.Day, 2, "ụbọchị 2")]
    [InlineData(TimeUnit.Month, 1, "ọnwa 1")]
    [InlineData(TimeUnit.Month, 2, "ọnwa 2")]
    public void TimeSpanHumanize_UsesIgboDurationPhrases(TimeUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ig"));
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count));
    }

    [Fact]
    public void TimeSpanHumanize_ToWords_UsesIgboSingleWordForm()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ig"));
        Assert.Equal("otu ụbọchị", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Fact]
    public void ToAge_UsesIgboTemplate()
    {
        Assert.Equal("afọ 1", TimeSpan.FromDays(366).ToAge(new CultureInfo("ig")));
    }
}