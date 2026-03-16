namespace bnBD;

[UseCulture("bn-BD")]
public class TimeUnitToSymbolTests
{
    [Theory]
    [InlineData(TimeUnit.Millisecond, "মিলিসেকেন্ড")]
    [InlineData(TimeUnit.Second, "সেকেন্ড")]
    [InlineData(TimeUnit.Minute, "মিনিট")]
    [InlineData(TimeUnit.Hour, "ঘণ্টা")]
    [InlineData(TimeUnit.Day, "দিন")]
    [InlineData(TimeUnit.Week, "সপ্তাহ")]
    [InlineData(TimeUnit.Month, "মাস")]
    [InlineData(TimeUnit.Year, "বছর")]
    public void ToSymbol(TimeUnit unit, string expected) =>
        Assert.Equal(expected, unit.ToSymbol());
}
