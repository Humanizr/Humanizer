namespace el;

[UseCulture("el")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "χιλ.δ.")]
    [InlineData(TimeUnit.Second, "δευτ.")]
    [InlineData(TimeUnit.Minute, "λεπτ.")]
    [InlineData(TimeUnit.Hour, "ώρα")]
    [InlineData(TimeUnit.Day, "ημέρα")]
    [InlineData(TimeUnit.Week, "εβδομάδα")]
    [InlineData(TimeUnit.Month, "μήνας")]
    [InlineData(TimeUnit.Year, "χρόνος")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
