namespace nl;

[UseCulture("nl-NL")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "sec.")]
    [InlineData(TimeUnit.Minute, "min.")]
    [InlineData(TimeUnit.Hour, "uur")]
    [InlineData(TimeUnit.Day, "dag")]
    [InlineData(TimeUnit.Week, "week")]
    [InlineData(TimeUnit.Month, "maand")]
    [InlineData(TimeUnit.Year, "jaar")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
