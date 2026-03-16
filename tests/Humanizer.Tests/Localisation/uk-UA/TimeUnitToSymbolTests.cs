namespace ukUA;

[UseCulture("uk-UA")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "мс.")]
    [InlineData(TimeUnit.Second, "сек.")]
    [InlineData(TimeUnit.Minute, "хв.")]
    [InlineData(TimeUnit.Hour, "год.")]
    [InlineData(TimeUnit.Day, "д.")]
    [InlineData(TimeUnit.Week, "тиж.")]
    [InlineData(TimeUnit.Month, "міс.")]
    [InlineData(TimeUnit.Year, "р.")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
