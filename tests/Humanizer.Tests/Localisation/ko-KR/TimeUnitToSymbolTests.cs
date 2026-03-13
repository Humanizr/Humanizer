namespace koKR;

[UseCulture("ko-KR")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "밀리초")]
    [InlineData(TimeUnit.Second, "초")]
    [InlineData(TimeUnit.Minute, "분")]
    [InlineData(TimeUnit.Hour, "시간")]
    [InlineData(TimeUnit.Day, "일")]
    [InlineData(TimeUnit.Week, "주")]
    [InlineData(TimeUnit.Month, "개월")]
    [InlineData(TimeUnit.Year, "년")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
