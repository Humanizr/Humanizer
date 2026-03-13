namespace bg;

[UseCulture("bg-BG")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "мс.")]
    [InlineData(TimeUnit.Second, "сек.")]
    [InlineData(TimeUnit.Minute, "мин.")]
    [InlineData(TimeUnit.Hour, "ч.")]
    [InlineData(TimeUnit.Day, "ден")]
    [InlineData(TimeUnit.Week, "седмица")]
    [InlineData(TimeUnit.Month, "месец")]
    [InlineData(TimeUnit.Year, "година")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
