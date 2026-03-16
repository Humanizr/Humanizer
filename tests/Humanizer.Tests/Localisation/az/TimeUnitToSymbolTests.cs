namespace az;

[UseCulture("az")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "saniyə")]
    [InlineData(TimeUnit.Minute, "dəqiqə")]
    [InlineData(TimeUnit.Hour, "saat")]
    [InlineData(TimeUnit.Day, "gün")]
    [InlineData(TimeUnit.Week, "həftə")]
    [InlineData(TimeUnit.Month, "ay")]
    [InlineData(TimeUnit.Year, "il")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
