using System.Globalization;

namespace Humanizer.Tests.Localisation;

public class LocaleRegistrySweepTests
{
    [Theory]
    [MemberData(nameof(LocaleCoverageData.NumberToWordsLocaleTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void NumberToWords_RegisteredLocales_ReturnCardinalAndOrdinalOutput(string localeName)
    {
        var culture = new CultureInfo(localeName);

        var cardinal = 123.ToWords(culture);
        var ordinal = 123.ToOrdinalWords(culture);

        Assert.False(string.IsNullOrWhiteSpace(cardinal));
        Assert.False(string.IsNullOrWhiteSpace(ordinal));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.NumberToWordsLocaleTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void NumberToWords_RegisteredLocales_ExposeTupleConversion(string localeName)
    {
        var culture = new CultureInfo(localeName);

        _ = 123.ToTuple(culture);
    }

    [Theory]
    [MemberData(nameof(NumberToWordsRegressionTheoryData))]
    public void NumberToWords_CustomLocales_UseExpectedCardinalForms(string localeName, long number, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.ToWords(culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.NumberToWordsOrdinalExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void NumberToWords_CustomLocales_UseExpectedOrdinalForms(string localeName, int number, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.ToOrdinalWords(GrammaticalGender.Feminine, culture));
    }

    [Fact]
    public void NumberToWords_BrazilianPortuguese_UsesGenderedForms()
    {
        var culture = new CultureInfo("pt-BR");

        Assert.Equal("uma", 1.ToWords(GrammaticalGender.Feminine, culture));
    }

    public static TheoryData<string, long, string> NumberToWordsRegressionTheoryData => new()
    {
        { "de-CH", 30, "dreissig" },
        { "de-LI", 30, "dreissig" },
        { "en-IN", 100000, "one lakh" },
        { "fr-CH", 80, "octante" },
        { "ta", 100, "நூறு" }
    };

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerLocaleTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_RegisteredLocales_ReturnIntAndStringOverloadOutput(string localeName)
    {
        var culture = new CultureInfo(localeName);

        var ordinalFromInt = 123.Ordinalize(culture);
        var ordinalFromString = "123".Ordinalize(culture);

        Assert.Equal(ordinalFromInt, ordinalFromString);
        Assert.False(string.IsNullOrWhiteSpace(ordinalFromInt));
    }

    [Theory]
    [MemberData(nameof(OrdinalizerRegressionTheoryData))]
    public void Ordinalizer_CustomLocales_UseExpectedForms(string localeName, int number, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.Ordinalize(culture));
    }

    public static TheoryData<string, int, string> OrdinalizerRegressionTheoryData => new()
    {
        { "ca", 1, "1r" },
        { "de", 1, "1." },
        { "en", 1, "1st" },
        { "es", 1, "1.º" },
        { "fr", 1, "1er" },
        { "lb", 1, "1." },
        { "pt", 1, "1º" },
        { "ro", 1, "primul" }
    };

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWordsLocaleTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_RegisteredLocales_ReturnOutput(string localeName)
    {
        using var _ = LocaleCoverageData.UseCulture(localeName);

        var defaultOrdinalWords = new DateTime(2015, 1, 1).ToOrdinalWords();
        var nominativeOrdinalWords = new DateTime(2015, 1, 1).ToOrdinalWords(GrammaticalCase.Nominative);

        Assert.False(string.IsNullOrWhiteSpace(defaultOrdinalWords));
        Assert.False(string.IsNullOrWhiteSpace(nominativeOrdinalWords));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWordsExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_CustomLocales_UseExpectedForms(string localeName, string expected)
    {
        using var _ = LocaleCoverageData.UseCulture(localeName);

        Assert.Equal(expected, new DateTime(2015, 1, 1).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWordsLocaleTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_RegisteredLocales_ReturnOutput(string localeName)
    {
        using var _ = LocaleCoverageData.UseCulture(localeName);

        var defaultOrdinalWords = new DateOnly(2015, 1, 1).ToOrdinalWords();
        var nominativeOrdinalWords = new DateOnly(2015, 1, 1).ToOrdinalWords(GrammaticalCase.Nominative);

        Assert.False(string.IsNullOrWhiteSpace(defaultOrdinalWords));
        Assert.False(string.IsNullOrWhiteSpace(nominativeOrdinalWords));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWordsExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_CustomLocales_UseExpectedForms(string localeName, string expected)
    {
        using var _ = LocaleCoverageData.UseCulture(localeName);

        Assert.Equal(expected, new DateOnly(2015, 1, 1).ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyToClockNotationExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeOnlyToClockNotation_CustomLocales_UseExpectedRoundedForms(string localeName, int hours, int minutes, string expected)
    {
        using var _ = LocaleCoverageData.UseCulture(localeName);

        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyToClockNotationLocaleTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeOnlyToClockNotation_RegisteredLocales_ReturnDefaultAndRoundedOutput(string localeName)
    {
        using var _ = LocaleCoverageData.UseCulture(localeName);

        var exact = new TimeOnly(13, 23).ToClockNotation();
        var rounded = new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);

        Assert.False(string.IsNullOrWhiteSpace(exact));
        Assert.False(string.IsNullOrWhiteSpace(rounded));
    }
#endif

    [Theory]
    [MemberData(nameof(LocaleCoverageData.WordsToNumberLocaleTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void WordsToNumber_RegisteredLocales_RoundTripNativeWords(string localeName)
    {
        var culture = new CultureInfo(localeName);
        const long expected = 105;
        var words = expected.ToWords(culture);

        Assert.Equal(expected, words.ToNumber(culture));
        Assert.True(words.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.WordsToNumberUnsupportedExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void WordsToNumber_UnsupportedCultures_UseDefaultLexicon(string localeName, string words, long expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, words.ToNumber(culture));
        Assert.True(words.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }
}
