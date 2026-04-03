namespace Humanizer.Tests.Localisation;

public class NumberWordPhraseTests
{
    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.CardinalCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedCardinalCases(string localeName, int number, string expected)
    {
        Assert.Equal(expected, number.ToWords(GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.CardinalAddAndCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedCardinalAddAndCases(string localeName, int number, bool addAnd, string expected)
    {
        Assert.Equal(expected, number.ToWords(addAnd, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.CardinalWordFormCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedCardinalWordFormCases(string localeName, int number, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.ToWords(wordForm, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.CardinalGenderCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedCardinalGenderCases(string localeName, int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.CardinalWordFormGenderCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedCardinalWordFormGenderCases(string localeName, int number, WordForm wordForm, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(wordForm, gender, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.OrdinalCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedOrdinalCases(string localeName, int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.OrdinalWordFormCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedOrdinalWordFormCases(string localeName, int number, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(wordForm, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.OrdinalGenderCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedOrdinalGenderCases(string localeName, int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.OrdinalWordFormGenderCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedOrdinalWordFormGenderCases(string localeName, int number, GrammaticalGender gender, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, wordForm, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.TupleCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedTupleCases(string localeName, int number, string expected)
    {
        Assert.Equal(expected, number.ToTuple(GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.WordsToNumberEnglishCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedWordsToNumberEnglishCases(string localeName, string words, int expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected, words.ToNumber(culture));
        Assert.True(words.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.WordsToNumberUnsupportedLocaleCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedWordsToNumberUnsupportedLocaleCases(string localeName, string words)
    {
        var culture = GetCulture(localeName);

        Assert.Throws<NotSupportedException>(() => words.ToNumber(culture));
    }

    static CultureInfo GetCulture(string localeName) => CultureInfo.GetCultureInfo(localeName);
}
