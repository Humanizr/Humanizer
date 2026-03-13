namespace it;

[UseCulture("it")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "sec.")]
    [InlineData(TimeUnit.Minute, "min.")]
    [InlineData(TimeUnit.Hour, "ora")]
    [InlineData(TimeUnit.Day, "giorno")]
    [InlineData(TimeUnit.Week, "settimana")]
    [InlineData(TimeUnit.Month, "mese")]
    [InlineData(TimeUnit.Year, "anno")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
