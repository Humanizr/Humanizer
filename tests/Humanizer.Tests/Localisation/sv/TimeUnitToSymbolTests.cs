namespace sv;

[UseCulture("sv-SE")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "sek.")]
    [InlineData(TimeUnit.Minute, "min.")]
    [InlineData(TimeUnit.Hour, "timme")]
    [InlineData(TimeUnit.Day, "dag")]
    [InlineData(TimeUnit.Week, "vecka")]
    [InlineData(TimeUnit.Month, "månad")]
    [InlineData(TimeUnit.Year, "år")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
