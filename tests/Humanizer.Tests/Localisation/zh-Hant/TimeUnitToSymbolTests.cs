namespace zhHant;

[UseCulture("zh-Hant")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "毫秒")]
    [InlineData(TimeUnit.Second, "秒")]
    [InlineData(TimeUnit.Minute, "分")]
    [InlineData(TimeUnit.Hour, "小時")]
    [InlineData(TimeUnit.Day, "天")]
    [InlineData(TimeUnit.Week, "周")]
    [InlineData(TimeUnit.Month, "月")]
    [InlineData(TimeUnit.Year, "年")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
