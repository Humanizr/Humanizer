namespace Humanizer.Tests.Localisation;

public class NumberWordMagnitudeTests
{
    [Theory]
    [MemberData(nameof(LocaleNumberMagnitudeTheoryData.MagnitudeCardinalCases), MemberType = typeof(LocaleNumberMagnitudeTheoryData))]
    public void UsesExpectedMagnitudeCardinalCases(string localeName, long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberMagnitudeTheoryData.ExtendedMagnitudeCardinalCases), MemberType = typeof(LocaleNumberMagnitudeTheoryData))]
    public void UsesExpectedExtendedMagnitudeCardinalCases(string localeName, long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberMagnitudeTheoryData.UnsupportedMagnitudeCardinalCases), MemberType = typeof(LocaleNumberMagnitudeTheoryData))]
    public void UnsupportedMagnitudeCardinalCasesThrowExpectedException(string localeName, long number, Type expectedExceptionType)
    {
        var exception = Record.Exception(() => number.ToWords(GetCulture(localeName)));

        Assert.NotNull(exception);
        Assert.IsType(expectedExceptionType, exception);
    }

    static CultureInfo GetCulture(string localeName) => CultureInfo.GetCultureInfo(localeName);
}
