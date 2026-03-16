namespace cs;

[UseCulture("cs-CZ")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "h")]
    [InlineData(TimeUnit.Day, "den")]
    [InlineData(TimeUnit.Week, "týden")]
    [InlineData(TimeUnit.Month, "měsíc")]
    [InlineData(TimeUnit.Year, "rok")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
