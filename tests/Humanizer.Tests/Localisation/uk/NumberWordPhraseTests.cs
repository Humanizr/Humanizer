namespace Humanizer.Tests.Localisation;

public class NumberWordPhraseTests_uk
{
    public static TheoryData<int, string> CardinalCases => new()
    {
        { 1, "один" },
        { 21, "двадцять один" },
    };

    public static TheoryData<int, bool, string> CardinalAddAndCases => new()
    {
        { 105, false, "сто п'ять" },
    };

    public static TheoryData<int, WordForm, string> CardinalWordFormCases => new()
    {
        { 21, WordForm.Abbreviation, "двадцять один" },
    };

    public static TheoryData<int, GrammaticalGender, string> CardinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "одна" },
    };

    public static TheoryData<int, WordForm, GrammaticalGender, string> CardinalWordFormGenderCases => new()
    {
        { 21, WordForm.Abbreviation, GrammaticalGender.Feminine, "двадцять одна" },
    };

    public static TheoryData<int, string> OrdinalCases => new()
    {
        { 1, "перший" },
        { 3, "третій" },
    };

    public static TheoryData<int, WordForm, string> OrdinalWordFormCases => new()
    {
        { 3, WordForm.Abbreviation, "третій" },
    };

    public static TheoryData<int, GrammaticalGender, string> OrdinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "перша" },
    };

    public static TheoryData<int, GrammaticalGender, WordForm, string> OrdinalWordFormGenderCases => new()
    {
        { 3, GrammaticalGender.Feminine, WordForm.Abbreviation, "третя" },
    };

    public static TheoryData<int, string> TupleCases => new()
    {
        { 2, "два" },
    };

    [Theory]
    [MemberData(nameof(CardinalCases))]
    public void UsesExpectedCardinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinal("uk", number, expected);

    [Theory]
    [MemberData(nameof(CardinalAddAndCases))]
    public void UsesExpectedCardinalAddAndCases(int number, bool addAnd, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithAddAnd("uk", number, addAnd, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormCases))]
    public void UsesExpectedCardinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordForm("uk", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(CardinalGenderCases))]
    public void UsesExpectedCardinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithGender("uk", number, gender, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormGenderCases))]
    public void UsesExpectedCardinalWordFormGenderCases(int number, WordForm wordForm, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordFormAndGender("uk", number, wordForm, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalCases))]
    public void UsesExpectedOrdinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinal("uk", number, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormCases))]
    public void UsesExpectedOrdinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordForm("uk", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(OrdinalGenderCases))]
    public void UsesExpectedOrdinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithGender("uk", number, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormGenderCases))]
    public void UsesExpectedOrdinalWordFormGenderCases(int number, GrammaticalGender gender, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordFormAndGender("uk", number, gender, wordForm, expected);

    [Theory]
    [MemberData(nameof(TupleCases))]
    public void UsesExpectedTupleCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyTuple("uk", number, expected);
}
