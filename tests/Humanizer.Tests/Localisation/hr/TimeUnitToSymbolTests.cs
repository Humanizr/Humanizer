namespace hr;

[UseCulture("hr-HR")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "sat")]
    [InlineData(TimeUnit.Day, "dan")]
    [InlineData(TimeUnit.Week, "tjedan")]
    [InlineData(TimeUnit.Month, "mjesec")]
    [InlineData(TimeUnit.Year, "godina")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
