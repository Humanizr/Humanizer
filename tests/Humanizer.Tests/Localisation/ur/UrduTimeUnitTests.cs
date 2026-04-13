namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduTimeUnitTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ملی سیکنڈ")]
    [InlineData(TimeUnit.Second, "سیکنڈ")]
    [InlineData(TimeUnit.Minute, "منٹ")]
    [InlineData(TimeUnit.Hour, "گھنٹہ")]
    [InlineData(TimeUnit.Day, "دن")]
    [InlineData(TimeUnit.Week, "ہفتہ")]
    [InlineData(TimeUnit.Month, "مہینہ")]
    [InlineData(TimeUnit.Year, "سال")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ur"));
        var result = formatter.TimeUnitHumanize(unit);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}
