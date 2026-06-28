namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaTimeUnitTests
{
    [Theory]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "sa'a")]
    [InlineData(TimeUnit.Day, "kw")]
    public void TimeUnitHumanize_UsesHausaLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ha"));
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }
}