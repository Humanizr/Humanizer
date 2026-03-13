namespace lv;

[UseCulture("lv")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "milisekunde")]
    [InlineData(TimeUnit.Second, "sekunde")]
    [InlineData(TimeUnit.Minute, "minūte")]
    [InlineData(TimeUnit.Hour, "stunda")]
    [InlineData(TimeUnit.Day, "diena")]
    [InlineData(TimeUnit.Week, "nedēļa")]
    [InlineData(TimeUnit.Month, "mēnesis")]
    [InlineData(TimeUnit.Year, "gads")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
