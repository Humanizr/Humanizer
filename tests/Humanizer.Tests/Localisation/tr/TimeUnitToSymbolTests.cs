namespace tr;

[UseCulture("tr")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "saniye")]
    [InlineData(TimeUnit.Minute, "dakika")]
    [InlineData(TimeUnit.Hour, "saat")]
    [InlineData(TimeUnit.Day, "gün")]
    [InlineData(TimeUnit.Week, "hafta")]
    [InlineData(TimeUnit.Month, "ay")]
    [InlineData(TimeUnit.Year, "yıl")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
