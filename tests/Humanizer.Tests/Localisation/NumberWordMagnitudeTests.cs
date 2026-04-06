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
    [MemberData(nameof(LocaleNumberMagnitudeTheoryData.HighRangeRoundTripCases), MemberType = typeof(LocaleNumberMagnitudeTheoryData))]
    public void UsesExpectedHighRangeRoundTripCases(string localeName, long number, string expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected, number.ToWords(culture));
        Assert.Equal(number, expected.ToNumber(culture));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberMagnitudeTheoryData.HighRangeRoundTripOnlyCases), MemberType = typeof(LocaleNumberMagnitudeTheoryData))]
    public void UsesExpectedHighRangeRoundTripOnlyCases(string localeName, long number)
    {
        var culture = GetCulture(localeName);
        var words = number.ToWords(culture);

        Assert.Equal(number, words.ToNumber(culture));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalHighRangeRoundTripOnlyCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalHighRangeRoundTripOnlyCases(string localeName, long number)
    {
        var culture = GetCulture(localeName);
        var words = number.ToWords(culture);

        Assert.Equal(number, words.ToNumber(culture));
    }

    static CultureInfo GetCulture(string localeName) => CultureInfo.GetCultureInfo(localeName);
}
