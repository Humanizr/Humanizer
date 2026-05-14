namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboTimeUnitTests
{
    [Theory]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "awa")]
    [InlineData(TimeUnit.Day, "ụb")]
    public void TimeUnitHumanize_UsesIgboLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ig"));
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }
}