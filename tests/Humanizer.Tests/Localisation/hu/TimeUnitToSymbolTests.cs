namespace hu;

[UseCulture("hu")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "m")]
    [InlineData(TimeUnit.Minute, "p")]
    [InlineData(TimeUnit.Hour, "ó")]
    [InlineData(TimeUnit.Day, "n")]
    [InlineData(TimeUnit.Week, "hét")]
    [InlineData(TimeUnit.Month, "h")]
    [InlineData(TimeUnit.Year, "é")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
