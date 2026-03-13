namespace mt;

[UseCulture("mt")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "sie")]
    [InlineData(TimeUnit.Day, "jum")]
    [InlineData(TimeUnit.Week, "ġimgħa")]
    [InlineData(TimeUnit.Month, "xahar")]
    [InlineData(TimeUnit.Year, "sena")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
