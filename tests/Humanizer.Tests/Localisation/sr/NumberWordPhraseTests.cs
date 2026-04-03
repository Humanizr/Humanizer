namespace Humanizer.Tests.Localisation;

public class NumberWordPhraseTests_sr
{
    public static TheoryData<int, string> CardinalCases => new()
    {
        { 1, "један" },
        { 21, "двадесет један" },
    };

    public static TheoryData<int, bool, string> CardinalAddAndCases => new()
    {
        { 105, false, "сто пет" },
    };

    public static TheoryData<int, WordForm, string> CardinalWordFormCases => new()
    {
        { 21, WordForm.Abbreviation, "двадесет један" },
    };

    public static TheoryData<int, GrammaticalGender, string> CardinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "један" },
    };

    public static TheoryData<int, WordForm, GrammaticalGender, string> CardinalWordFormGenderCases => new()
    {
        { 21, WordForm.Abbreviation, GrammaticalGender.Feminine, "двадесет један" },
    };

    public static TheoryData<int, string> OrdinalCases => new()
    {
        { 1, "1" },
        { 3, "3" },
    };

    public static TheoryData<int, WordForm, string> OrdinalWordFormCases => new()
    {
        { 3, WordForm.Abbreviation, "3" },
    };

    public static TheoryData<int, GrammaticalGender, string> OrdinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "1" },
    };

    public static TheoryData<int, GrammaticalGender, WordForm, string> OrdinalWordFormGenderCases => new()
    {
        { 3, GrammaticalGender.Feminine, WordForm.Abbreviation, "3" },
    };

    public static TheoryData<int, string> TupleCases => new()
    {
        { 2, "два" },
    };

    [Theory]
    [MemberData(nameof(CardinalCases))]
    public void UsesExpectedCardinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinal("sr", number, expected);

    [Theory]
    [MemberData(nameof(CardinalAddAndCases))]
    public void UsesExpectedCardinalAddAndCases(int number, bool addAnd, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithAddAnd("sr", number, addAnd, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormCases))]
    public void UsesExpectedCardinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordForm("sr", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(CardinalGenderCases))]
    public void UsesExpectedCardinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithGender("sr", number, gender, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormGenderCases))]
    public void UsesExpectedCardinalWordFormGenderCases(int number, WordForm wordForm, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordFormAndGender("sr", number, wordForm, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalCases))]
    public void UsesExpectedOrdinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinal("sr", number, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormCases))]
    public void UsesExpectedOrdinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordForm("sr", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(OrdinalGenderCases))]
    public void UsesExpectedOrdinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithGender("sr", number, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormGenderCases))]
    public void UsesExpectedOrdinalWordFormGenderCases(int number, GrammaticalGender gender, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordFormAndGender("sr", number, gender, wordForm, expected);

    [Theory]
    [MemberData(nameof(TupleCases))]
    public void UsesExpectedTupleCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyTuple("sr", number, expected);
}
