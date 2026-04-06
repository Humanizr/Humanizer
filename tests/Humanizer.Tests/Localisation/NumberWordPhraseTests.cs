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
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalCardinalCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalCardinalCases(string localeName, long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalCardinalGenderCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalCardinalGenderCases(string localeName, long number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalCardinalWordFormCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalCardinalWordFormCases(string localeName, long number, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.ToWords(wordForm, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalCardinalWordFormGenderCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalCardinalWordFormGenderCases(string localeName, long number, WordForm wordForm, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(wordForm, gender, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalOrdinalCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalOrdinalCases(string localeName, int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalOrdinalGenderCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalOrdinalGenderCases(string localeName, int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalOrdinalWordFormCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalOrdinalWordFormCases(string localeName, int number, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(wordForm, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalOrdinalWordFormGenderCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalOrdinalWordFormGenderCases(string localeName, int number, WordForm wordForm, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, wordForm, GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalTupleCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalTupleCases(string localeName, int number, string expected)
    {
        Assert.Equal(expected, number.ToTuple(GetCulture(localeName)));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalConverterCardinalCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalConverterCardinalCases(string localeName, long number, string expected)
    {
        var converter = Configurator.NumberToWordsConverters.ResolveForCulture(GetCulture(localeName));

        Assert.Equal(expected, converter.Convert(number));
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalNumberTheoryData.AdditionalConverterOrdinalCases), MemberType = typeof(LocaleAdditionalNumberTheoryData))]
    public void UsesExpectedAdditionalConverterOrdinalCases(string localeName, int number, string expected)
    {
        var converter = Configurator.NumberToWordsConverters.ResolveForCulture(GetCulture(localeName));

        Assert.Equal(expected, converter.ConvertToOrdinal(number));
    }

    [Theory]
    [MemberData(nameof(LocaleNumberTheoryData.WordsToNumberCases), MemberType = typeof(LocaleNumberTheoryData))]
    public void UsesExpectedWordsToNumberCases(string localeName, string words, long expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected, words.ToNumber(culture));
        Assert.True(words.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }

    static CultureInfo GetCulture(string localeName) =>
        localeName == "invariant"
            ? CultureInfo.InvariantCulture
            : CultureInfo.GetCultureInfo(localeName);
}


