namespace ruRU;

[UseCulture("ru-RU")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(TimeUnit.Millisecond, "мс.")]
    [InlineData(TimeUnit.Second, "сек.")]
    [InlineData(TimeUnit.Minute, "мин.")]
    [InlineData(TimeUnit.Hour, "ч.")]
    [InlineData(TimeUnit.Day, "д.")]
    [InlineData(TimeUnit.Week, "нед.")]
    [InlineData(TimeUnit.Month, "мес.")]
    [InlineData(TimeUnit.Year, "г.")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}