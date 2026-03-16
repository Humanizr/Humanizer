namespace sk;

[UseCulture("sk-SK")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "h")]
    [InlineData(TimeUnit.Day, "deň")]
    [InlineData(TimeUnit.Week, "týždeň")]
    [InlineData(TimeUnit.Month, "mesiac")]
    [InlineData(TimeUnit.Year, "rok")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
