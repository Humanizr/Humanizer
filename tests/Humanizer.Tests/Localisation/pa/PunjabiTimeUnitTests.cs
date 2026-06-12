namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiTimeUnitTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ਮਿਲੀਸਕਿੰਟ")]
    [InlineData(TimeUnit.Second, "ਸਕਿੰਟ")]
    [InlineData(TimeUnit.Minute, "ਮਿੰਟ")]
    [InlineData(TimeUnit.Hour, "ਘੰਟਾ")]
    [InlineData(TimeUnit.Day, "ਦਿਨ")]
    [InlineData(TimeUnit.Week, "ਹਫ਼ਤਾ")]
    [InlineData(TimeUnit.Month, "ਮਹੀਨਾ")]
    [InlineData(TimeUnit.Year, "ਸਾਲ")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("pa"));
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }
}