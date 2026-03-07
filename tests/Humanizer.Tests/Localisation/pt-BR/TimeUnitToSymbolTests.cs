namespace pt_BR;

[UseCulture("pt-BR")]
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
    [InlineData(TimeUnit.Month, "m")]
    [InlineData(TimeUnit.Year, "a")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
