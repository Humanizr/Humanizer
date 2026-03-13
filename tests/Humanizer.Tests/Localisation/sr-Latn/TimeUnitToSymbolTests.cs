namespace srLatn;

[UseCulture("sr-Latn")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "č")]
    [InlineData(TimeUnit.Day, "dan")]
    [InlineData(TimeUnit.Week, "ned")]
    [InlineData(TimeUnit.Month, "mes")]
    [InlineData(TimeUnit.Year, "god")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
