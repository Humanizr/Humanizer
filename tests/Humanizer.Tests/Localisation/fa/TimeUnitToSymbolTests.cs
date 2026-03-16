namespace fa;

[UseCulture("fa")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "میلی‌ثانیه")]
    [InlineData(TimeUnit.Second, "ثانیه")]
    [InlineData(TimeUnit.Minute, "دقیقه")]
    [InlineData(TimeUnit.Hour, "ساعت")]
    [InlineData(TimeUnit.Day, "روز")]
    [InlineData(TimeUnit.Week, "هفته")]
    [InlineData(TimeUnit.Month, "ماه")]
    [InlineData(TimeUnit.Year, "سال")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
