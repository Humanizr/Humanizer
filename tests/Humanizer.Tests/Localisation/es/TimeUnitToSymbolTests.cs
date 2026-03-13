namespace es;

[UseCulture("es-ES")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "h")]
    [InlineData(TimeUnit.Day, "d")]
    [InlineData(TimeUnit.Week, "semana")]
    [InlineData(TimeUnit.Month, "mes")]
    [InlineData(TimeUnit.Year, "año")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
