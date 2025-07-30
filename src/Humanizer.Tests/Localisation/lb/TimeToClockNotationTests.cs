#if NET6_0_OR_GREATER

namespace lb;

[UseCulture("lb-LU")]
public class TimeToClockNotationTests
{
    [Theory]
    [InlineData(00, 00, "Mëtternuecht")]
    [InlineData(00, 07, "siwe Minutten op zwielef")]
    [InlineData(01, 11, "eelef Minutten op eng")]
    [InlineData(04, 00, "véier Auer")]
    [InlineData(05, 01, "eng Minutt op fënnef")]
    [InlineData(06, 05, "fënnef op sechs")]
    [InlineData(07, 10, "zéng op siwen")]
    [InlineData(08, 15, "Véirel op aacht")]
    [InlineData(09, 20, "zwanzeg op néng")]
    [InlineData(10, 25, "fënnef vir hallwer eelef")]
    [InlineData(11, 30, "hallwer zwielef")]
    [InlineData(12, 00, "Mëtteg")]
    [InlineData(12, 39, "eenanzwanzeg Minutten vir eng")]
    [InlineData(13, 23, "dräianzwanzeg Minutten op eng")]
    [InlineData(14, 32, "zwou Minutten op hallwer dräi")]
    [InlineData(15, 35, "fënnef op hallwer véier")]
    [InlineData(16, 40, "zwanzeg vir fënnef")]
    [InlineData(17, 45, "Véirel vir sechs")]
    [InlineData(18, 50, "zéng vir siwen")]
    [InlineData(19, 52, "aacht Minutten vir aacht")]
    [InlineData(20, 55, "fënnef vir néng")]
    [InlineData(21, 58, "zwou Minutten vir zéng")]
    [InlineData(22, 59, "eng Minutt vir eelef")]
    public void ConvertToClockNotationTimeOnlyString(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation();
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(00, 00, "Mëtternuecht")]
    [InlineData(00, 07, "fënnef op zwielef")]
    [InlineData(01, 11, "zéng op eng")]
    [InlineData(04, 00, "véier Auer")]
    [InlineData(05, 01, "fënnef Auer")]
    [InlineData(06, 05, "fënnef op sechs")]
    [InlineData(07, 10, "zéng op siwen")]
    [InlineData(08, 15, "Véirel op aacht")]
    [InlineData(09, 20, "zwanzeg op néng")]
    [InlineData(10, 25, "fënnef vir hallwer eelef")]
    [InlineData(11, 30, "hallwer zwielef")]
    [InlineData(12, 00, "Mëtteg")]
    [InlineData(12, 39, "zwanzeg vir eng")]
    [InlineData(13, 23, "fënnef vir hallwer zwou")]
    [InlineData(14, 32, "hallwer dräi")]
    [InlineData(15, 35, "fënnef op hallwer véier")]
    [InlineData(16, 40, "zwanzeg vir fënnef")]
    [InlineData(17, 45, "Véirel vir sechs")]
    [InlineData(18, 50, "zéng vir siwen")]
    [InlineData(19, 52, "zéng vir aacht")]
    [InlineData(20, 55, "fënnef vir néng")]
    [InlineData(21, 58, "zéng Auer")]
    [InlineData(22, 59, "eelef Auer")]
    public void ConvertToRoundedClockNotationTimeOnlyString(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
        Assert.Equal(expectedResult, actualResult);
    }
}

#endif
