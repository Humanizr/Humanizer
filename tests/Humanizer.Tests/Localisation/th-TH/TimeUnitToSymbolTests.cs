namespace thTH;

[UseCulture("th-TH")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "มิลลิวินาที")]
    [InlineData(TimeUnit.Second, "วินาที")]
    [InlineData(TimeUnit.Minute, "นาที")]
    [InlineData(TimeUnit.Hour, "ชั่วโมง")]
    [InlineData(TimeUnit.Day, "วัน")]
    [InlineData(TimeUnit.Week, "สัปดาห์")]
    [InlineData(TimeUnit.Month, "เดือน")]
    [InlineData(TimeUnit.Year, "ปี")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
