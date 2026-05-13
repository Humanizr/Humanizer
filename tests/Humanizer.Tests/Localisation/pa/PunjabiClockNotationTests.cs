namespace Humanizer.Tests.Localisation.pa;

#if NET6_0_OR_GREATER
[UseCulture("pa")]
public class PunjabiClockNotationTests
{
    [Theory]
    [InlineData(1, 0, "ਸਵੇਰੇ ਇੱਕ ਵਜੇ")]
    [InlineData(1, 5, "ਸਵੇਰੇ ਇੱਕ ਵੱਜ ਕੇ ਪੰਜ ਮਿੰਟ")]
    [InlineData(7, 5, "ਸਵੇਰੇ ਸੱਤ ਵੱਜ ਕੇ ਪੰਜ ਮਿੰਟ")]
    [InlineData(12, 0, "ਦੁਪਹਿਰ ਬਾਰਾਂ ਵਜੇ")]
    [InlineData(12, 30, "ਦੁਪਹਿਰ ਬਾਰਾਂ ਵੱਜ ਕੇ ਤੀਹ ਮਿੰਟ")]
    [InlineData(13, 0, "ਦੁਪਹਿਰ ਇੱਕ ਵਜੇ")]
    [InlineData(13, 23, "ਦੁਪਹਿਰ ਇੱਕ ਵੱਜ ਕੇ ਤੇਈ ਮਿੰਟ")]
    [InlineData(18, 0, "ਸ਼ਾਮ ਛੇ ਵਜੇ")]
    [InlineData(20, 0, "ਸ਼ਾਮ ਅੱਠ ਵਜੇ")]
    [InlineData(21, 0, "ਰਾਤ ਨੌਂ ਵਜੇ")]
    [InlineData(23, 40, "ਰਾਤ ਗਿਆਰਾਂ ਵੱਜ ਕੇ ਚਾਲੀ ਮਿੰਟ")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Theory]
    [InlineData(13, 23, "ਦੁਪਹਿਰ ਇੱਕ ਵੱਜ ਕੇ ਪੱਚੀ ਮਿੰਟ")]
    public void ToClockNotation_Rounded_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}
#endif