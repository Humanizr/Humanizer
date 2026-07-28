namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliTimeUnitTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "मिलिसेकेन्ड")]
    [InlineData(TimeUnit.Second, "सेकेन्ड")]
    [InlineData(TimeUnit.Minute, "मिनेट")]
    [InlineData(TimeUnit.Hour, "घण्टा")]
    [InlineData(TimeUnit.Day, "दिन")]
    [InlineData(TimeUnit.Week, "हप्ता")]
    [InlineData(TimeUnit.Month, "महिना")]
    [InlineData(TimeUnit.Year, "वर्ष")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ne"));
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }
}