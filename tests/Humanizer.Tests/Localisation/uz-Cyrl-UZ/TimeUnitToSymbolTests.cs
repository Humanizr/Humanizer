namespace uzCyrl;

[UseCulture("uz-Cyrl-UZ")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "миллисекунд")]
    [InlineData(TimeUnit.Second, "сония")]
    [InlineData(TimeUnit.Minute, "дақиқа")]
    [InlineData(TimeUnit.Hour, "соат")]
    [InlineData(TimeUnit.Day, "кун")]
    [InlineData(TimeUnit.Week, "ҳафта")]
    [InlineData(TimeUnit.Month, "ой")]
    [InlineData(TimeUnit.Year, "йил")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
