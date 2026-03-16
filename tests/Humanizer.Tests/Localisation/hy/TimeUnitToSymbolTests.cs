namespace hy;

[UseCulture("hy")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "մվ")]
    [InlineData(TimeUnit.Second, "վ")]
    [InlineData(TimeUnit.Minute, "ր")]
    [InlineData(TimeUnit.Hour, "ժամ")]
    [InlineData(TimeUnit.Day, "օր")]
    [InlineData(TimeUnit.Week, "շաբաթ")]
    [InlineData(TimeUnit.Month, "ամիս")]
    [InlineData(TimeUnit.Year, "տարի")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
