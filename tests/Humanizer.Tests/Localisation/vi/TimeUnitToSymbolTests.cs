namespace vi;

[UseCulture("vi")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "mili giây")]
    [InlineData(TimeUnit.Second, "giây")]
    [InlineData(TimeUnit.Minute, "phút")]
    [InlineData(TimeUnit.Hour, "giờ")]
    [InlineData(TimeUnit.Day, "ngày")]
    [InlineData(TimeUnit.Week, "tuần")]
    [InlineData(TimeUnit.Month, "tháng")]
    [InlineData(TimeUnit.Year, "năm")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
