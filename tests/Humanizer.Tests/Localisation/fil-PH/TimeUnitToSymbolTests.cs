namespace filPH;

[UseCulture("fil-PH")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [Trait("Translation", "Google Translate")]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "oras")]
    [InlineData(TimeUnit.Day, "araw")]
    [InlineData(TimeUnit.Week, "linggo")]
    [InlineData(TimeUnit.Month, "buwan")]
    [InlineData(TimeUnit.Year, "taon")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
