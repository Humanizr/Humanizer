namespace id;

[UseCulture("id-ID")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "milidetik")]
    [InlineData(TimeUnit.Second, "detik")]
    [InlineData(TimeUnit.Minute, "menit")]
    [InlineData(TimeUnit.Hour, "jam")]
    [InlineData(TimeUnit.Day, "hari")]
    [InlineData(TimeUnit.Week, "minggu")]
    [InlineData(TimeUnit.Month, "bulan")]
    [InlineData(TimeUnit.Year, "tahun")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
