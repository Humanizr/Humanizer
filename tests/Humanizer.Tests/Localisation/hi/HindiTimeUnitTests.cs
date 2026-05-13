namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiTimeUnitTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "मिलीसेकंड")]
    [InlineData(TimeUnit.Second, "सेकंड")]
    [InlineData(TimeUnit.Minute, "मिनट")]
    [InlineData(TimeUnit.Hour, "घंटा")]
    [InlineData(TimeUnit.Day, "दिन")]
    [InlineData(TimeUnit.Week, "सप्ताह")]
    [InlineData(TimeUnit.Month, "महीना")]
    [InlineData(TimeUnit.Year, "साल")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("hi"));
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }
}