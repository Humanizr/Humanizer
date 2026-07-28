namespace Humanizer.Tests.Localisation.th;

#if NET6_0_OR_GREATER
[UseCulture("th")]
public class ThaiClockNotationTests
{
    [Theory]
    [InlineData(0, "กลางคืนสิบสอง")]
    [InlineData(1, "ตีหนึ่ง")]
    [InlineData(5, "ตีห้า")]
    [InlineData(6, "ตอนเช้าหก")]
    [InlineData(11, "ตอนเช้าสิบเอ็ด")]
    [InlineData(12, "บ่ายสิบสอง")]
    [InlineData(20, "บ่ายแปด")]
    [InlineData(21, "กลางคืนเก้า")]
    public void ToClockNotation_UsesDayPeriodBoundaries(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());
}
#endif