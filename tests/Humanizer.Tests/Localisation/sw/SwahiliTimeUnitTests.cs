namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliTimeUnitTests
{
    [Theory]
    [InlineData(TimeUnit.Second, "sekunde")]
    [InlineData(TimeUnit.Minute, "dakika")]
    [InlineData(TimeUnit.Hour, "saa")]
    [InlineData(TimeUnit.Day, "siku")]
    public void TimeUnitHumanize_UsesSwahiliLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("sw"));
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }
}