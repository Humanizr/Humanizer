namespace sl;

[UseCulture("sl-SI")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "h")]
    [InlineData(TimeUnit.Day, "d.")]
    [InlineData(TimeUnit.Week, "ted.")]
    [InlineData(TimeUnit.Month, "mes.")]
    [InlineData(TimeUnit.Year, "l.")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
