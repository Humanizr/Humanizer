namespace sr;

[UseCulture("sr")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "мс")]
    [InlineData(TimeUnit.Second, "с")]
    [InlineData(TimeUnit.Minute, "мин")]
    [InlineData(TimeUnit.Hour, "ч")]
    [InlineData(TimeUnit.Day, "дан")]
    [InlineData(TimeUnit.Week, "нед")]
    [InlineData(TimeUnit.Month, "мес")]
    [InlineData(TimeUnit.Year, "год")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
