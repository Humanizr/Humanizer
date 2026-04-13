namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduNumberToWordsTests
{
    static readonly CultureInfo Ur = new("ur");

    [Theory]
    [InlineData(0, "صفر")]
    [InlineData(21, "اکیس")]
    [InlineData(99, "ننانوے")]
    [InlineData(100, "ایک سو")]
    [InlineData(1234, "ایک ہزار دو سو چونتیس")]
    [InlineData(100000, "ایک لاکھ")]
    [InlineData(1234567, "بارہ لاکھ چونتیس ہزار پانچ سو سڑسٹھ")]
    [InlineData(10000000, "ایک کروڑ")]
    [InlineData(1_000_000_000, "ایک ارب")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        var result = number.ToWords(Ur);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(21, "اکیس")]
    [InlineData(101, "ایک سو ایک")]
    [InlineData(1001, "ایک ہزار ایک")]
    [InlineData(100000, "ایک لاکھ")]
    [InlineData(1234567, "بارہ لاکھ چونتیس ہزار پانچ سو سڑسٹھ")]
    public void NumberToWords_RoundTrip(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Ur));
    }
}