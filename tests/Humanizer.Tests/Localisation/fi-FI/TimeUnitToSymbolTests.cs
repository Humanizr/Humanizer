namespace fiFI;

[UseCulture("fi-FI")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "tunti")]
    [InlineData(TimeUnit.Day, "päivä")]
    [InlineData(TimeUnit.Week, "viikko")]
    [InlineData(TimeUnit.Month, "kuukausi")]
    [InlineData(TimeUnit.Year, "vuosi")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
