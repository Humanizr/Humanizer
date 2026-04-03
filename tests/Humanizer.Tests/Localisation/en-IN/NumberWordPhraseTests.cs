namespace Humanizer.Tests.Localisation;

public class NumberWordPhraseTests_en_IN
{
    public static TheoryData<int, string> CardinalCases => new()
    {
        { 1, "one" },
        { 21, "twenty one" },
    };

    public static TheoryData<int, bool, string> CardinalAddAndCases => new()
    {
        { 105, false, "one hundred and five" },
    };

    public static TheoryData<int, WordForm, string> CardinalWordFormCases => new()
    {
        { 21, WordForm.Abbreviation, "twenty one" },
    };

    public static TheoryData<int, GrammaticalGender, string> CardinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "one" },
    };

    public static TheoryData<int, WordForm, GrammaticalGender, string> CardinalWordFormGenderCases => new()
    {
        { 21, WordForm.Abbreviation, GrammaticalGender.Feminine, "twenty one" },
    };

    public static TheoryData<int, string> OrdinalCases => new()
    {
        { 1, "one" },
        { 3, "three" },
    };

    public static TheoryData<int, WordForm, string> OrdinalWordFormCases => new()
    {
        { 3, WordForm.Abbreviation, "three" },
    };

    public static TheoryData<int, GrammaticalGender, string> OrdinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "one" },
    };

    public static TheoryData<int, GrammaticalGender, WordForm, string> OrdinalWordFormGenderCases => new()
    {
        { 3, GrammaticalGender.Feminine, WordForm.Abbreviation, "three" },
    };

    public static TheoryData<int, string> TupleCases => new()
    {
        { 2, "two" },
    };

    [Theory]
    [MemberData(nameof(CardinalCases))]
    public void UsesExpectedCardinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinal("en-IN", number, expected);

    [Theory]
    [MemberData(nameof(CardinalAddAndCases))]
    public void UsesExpectedCardinalAddAndCases(int number, bool addAnd, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithAddAnd("en-IN", number, addAnd, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormCases))]
    public void UsesExpectedCardinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordForm("en-IN", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(CardinalGenderCases))]
    public void UsesExpectedCardinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithGender("en-IN", number, gender, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormGenderCases))]
    public void UsesExpectedCardinalWordFormGenderCases(int number, WordForm wordForm, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordFormAndGender("en-IN", number, wordForm, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalCases))]
    public void UsesExpectedOrdinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinal("en-IN", number, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormCases))]
    public void UsesExpectedOrdinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordForm("en-IN", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(OrdinalGenderCases))]
    public void UsesExpectedOrdinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithGender("en-IN", number, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormGenderCases))]
    public void UsesExpectedOrdinalWordFormGenderCases(int number, GrammaticalGender gender, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordFormAndGender("en-IN", number, gender, wordForm, expected);

    [Theory]
    [MemberData(nameof(TupleCases))]
    public void UsesExpectedTupleCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyTuple("en-IN", number, expected);
}
