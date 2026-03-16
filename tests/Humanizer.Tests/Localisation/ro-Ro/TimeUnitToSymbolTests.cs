namespace roRO;

[UseCulture("ro-RO")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "sec.")]
    [InlineData(TimeUnit.Minute, "min.")]
    [InlineData(TimeUnit.Hour, "oră")]
    [InlineData(TimeUnit.Day, "zi")]
    [InlineData(TimeUnit.Week, "săptămână")]
    [InlineData(TimeUnit.Month, "lună")]
    [InlineData(TimeUnit.Year, "an")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
