namespace uzLatn;

[UseCulture("uz-Latn-UZ")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "millisekund")]
    [InlineData(TimeUnit.Second, "sekund")]
    [InlineData(TimeUnit.Minute, "minut")]
    [InlineData(TimeUnit.Hour, "soat")]
    [InlineData(TimeUnit.Day, "kun")]
    [InlineData(TimeUnit.Week, "hafta")]
    [InlineData(TimeUnit.Month, "oy")]
    [InlineData(TimeUnit.Year, "yil")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
