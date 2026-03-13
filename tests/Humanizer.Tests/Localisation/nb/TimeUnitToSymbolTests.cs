namespace nb;

[UseCulture("nb")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "sek.")]
    [InlineData(TimeUnit.Minute, "min.")]
    [InlineData(TimeUnit.Hour, "time")]
    [InlineData(TimeUnit.Day, "dag")]
    [InlineData(TimeUnit.Week, "uke")]
    [InlineData(TimeUnit.Month, "måned")]
    [InlineData(TimeUnit.Year, "år")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
