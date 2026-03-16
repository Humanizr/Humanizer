namespace msMY;

[UseCulture("ms-MY")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "milisaat")]
    [InlineData(TimeUnit.Second, "saat")]
    [InlineData(TimeUnit.Minute, "minit")]
    [InlineData(TimeUnit.Hour, "jam")]
    [InlineData(TimeUnit.Day, "hari")]
    [InlineData(TimeUnit.Week, "minggu")]
    [InlineData(TimeUnit.Month, "bulan")]
    [InlineData(TimeUnit.Year, "tahun")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
