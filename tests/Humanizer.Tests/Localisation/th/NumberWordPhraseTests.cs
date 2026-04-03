namespace Humanizer.Tests.Localisation;

public class NumberWordPhraseTests_th
{
    public static TheoryData<int, string> CardinalCases => new()
    {
        { 1, "หนึ่ง" },
        { 21, "ยี่สิบหนึ่ง" },
    };

    public static TheoryData<int, bool, string> CardinalAddAndCases => new()
    {
        { 105, false, "หนึ่งร้อยห้า" },
    };

    public static TheoryData<int, WordForm, string> CardinalWordFormCases => new()
    {
        { 21, WordForm.Abbreviation, "ยี่สิบหนึ่ง" },
    };

    public static TheoryData<int, GrammaticalGender, string> CardinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "หนึ่ง" },
    };

    public static TheoryData<int, WordForm, GrammaticalGender, string> CardinalWordFormGenderCases => new()
    {
        { 21, WordForm.Abbreviation, GrammaticalGender.Feminine, "ยี่สิบหนึ่ง" },
    };

    public static TheoryData<int, string> OrdinalCases => new()
    {
        { 1, "หนึ่ง" },
        { 3, "สาม" },
    };

    public static TheoryData<int, WordForm, string> OrdinalWordFormCases => new()
    {
        { 3, WordForm.Abbreviation, "สาม" },
    };

    public static TheoryData<int, GrammaticalGender, string> OrdinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "หนึ่ง" },
    };

    public static TheoryData<int, GrammaticalGender, WordForm, string> OrdinalWordFormGenderCases => new()
    {
        { 3, GrammaticalGender.Feminine, WordForm.Abbreviation, "สาม" },
    };

    public static TheoryData<int, string> TupleCases => new()
    {
        { 2, "สอง" },
    };

    [Theory]
    [MemberData(nameof(CardinalCases))]
    public void UsesExpectedCardinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinal("th", number, expected);

    [Theory]
    [MemberData(nameof(CardinalAddAndCases))]
    public void UsesExpectedCardinalAddAndCases(int number, bool addAnd, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithAddAnd("th", number, addAnd, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormCases))]
    public void UsesExpectedCardinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordForm("th", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(CardinalGenderCases))]
    public void UsesExpectedCardinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithGender("th", number, gender, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormGenderCases))]
    public void UsesExpectedCardinalWordFormGenderCases(int number, WordForm wordForm, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordFormAndGender("th", number, wordForm, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalCases))]
    public void UsesExpectedOrdinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinal("th", number, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormCases))]
    public void UsesExpectedOrdinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordForm("th", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(OrdinalGenderCases))]
    public void UsesExpectedOrdinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithGender("th", number, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormGenderCases))]
    public void UsesExpectedOrdinalWordFormGenderCases(int number, GrammaticalGender gender, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordFormAndGender("th", number, gender, wordForm, expected);

    [Theory]
    [MemberData(nameof(TupleCases))]
    public void UsesExpectedTupleCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyTuple("th", number, expected);
}
