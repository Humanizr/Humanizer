namespace ca;

[UseCulture("ca")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "h")]
    [InlineData(TimeUnit.Day, "dia")]
    [InlineData(TimeUnit.Week, "setmana")]
    [InlineData(TimeUnit.Month, "mes")]
    [InlineData(TimeUnit.Year, "any")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
