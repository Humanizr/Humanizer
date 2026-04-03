namespace Humanizer.Tests.Localisation;

public class WordsToNumberPhraseTests
{
    public static TheoryData<string, int> EnglishCases => new()
    {
        { "one", 1 },
        { "twenty-one", 21 },
        { "one hundred and five", 105 },
        { "one thousand two hundred thirty-four", 1234 },
    };

    public static TheoryData<string> UnsupportedCultureCases => new()
    {
        { "af" }, { "ar" }, { "az" }, { "bg" }, { "bn" }, { "ca" }, { "cs" }, { "de" }, { "el" }, { "es" }, { "fa" }, { "fi" }, { "fr" }, { "he" }, { "hr" }, { "hu" }, { "hy" }, { "id" }, { "is" }, { "it" }, { "ja" }, { "ko" }, { "ku" }, { "lb" }, { "lt" }, { "lv" }, { "mt" }, { "nb" }, { "nl" }, { "pl" }, { "pt" }, { "pt-BR" }, { "ro" }, { "ru" }, { "sk" }, { "sl" }, { "sr" }, { "sr-Latn" }, { "sv" }, { "ta" }, { "th" }, { "tr" }, { "uk" }, { "uz-Cyrl-UZ" }, { "uz-Latn-UZ" }, { "vi" }, { "zh-CN" }, { "zh-Hans" }, { "zh-Hant" }
    };

    [Theory]
    [MemberData(nameof(EnglishCases))]
    public void ParsesEnglishPhrases(string words, int expected)
    {
        var culture = CultureInfo.GetCultureInfo("en");
        Assert.Equal(expected, words.ToNumber(culture));
        Assert.True(words.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [MemberData(nameof(UnsupportedCultureCases))]
    public void RejectsUnsupportedCultures(string localeName)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Throws<NotSupportedException>(() => "one".ToNumber(culture));
        Assert.Throws<NotSupportedException>(() => "one".TryToNumber(out _, culture, out _));
    }
}
