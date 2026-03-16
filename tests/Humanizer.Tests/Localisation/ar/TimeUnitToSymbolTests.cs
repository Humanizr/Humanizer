namespace ar;

[UseCulture("ar")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "مللي ثانية")]
    [InlineData(TimeUnit.Second, "ثانية")]
    [InlineData(TimeUnit.Minute, "دقيقة")]
    [InlineData(TimeUnit.Hour, "ساعة")]
    [InlineData(TimeUnit.Day, "يوم")]
    [InlineData(TimeUnit.Week, "أسبوع")]
    [InlineData(TimeUnit.Month, "شهر")]
    [InlineData(TimeUnit.Year, "سنة")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
