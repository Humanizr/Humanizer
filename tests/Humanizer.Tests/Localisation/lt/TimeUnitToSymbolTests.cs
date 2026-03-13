namespace lt;

[UseCulture("lt")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min.")]
    [InlineData(TimeUnit.Hour, "val.")]
    [InlineData(TimeUnit.Day, "d.")]
    [InlineData(TimeUnit.Week, "sav.")]
    [InlineData(TimeUnit.Month, "mėn.")]
    [InlineData(TimeUnit.Year, "m.")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
