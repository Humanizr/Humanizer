namespace he;

[UseCulture("he")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "מילישנייה")]
    [InlineData(TimeUnit.Second, "שנייה")]
    [InlineData(TimeUnit.Minute, "דקה")]
    [InlineData(TimeUnit.Hour, "שעה")]
    [InlineData(TimeUnit.Day, "יום")]
    [InlineData(TimeUnit.Week, "שבוע")]
    [InlineData(TimeUnit.Month, "חודש")]
    [InlineData(TimeUnit.Year, "שנה")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
