namespace Humanizer.Tests.Localisation;

public class NumberWordPhraseTests_it
{
    public static TheoryData<int, string> CardinalCases => new()
    {
        { 1, "uno" },
        { 21, "ventuno" },
    };

    public static TheoryData<int, bool, string> CardinalAddAndCases => new()
    {
        { 105, false, "centocinque" },
    };

    public static TheoryData<int, WordForm, string> CardinalWordFormCases => new()
    {
        { 21, WordForm.Abbreviation, "ventuno" },
    };

    public static TheoryData<int, GrammaticalGender, string> CardinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "una" },
    };

    public static TheoryData<int, WordForm, GrammaticalGender, string> CardinalWordFormGenderCases => new()
    {
        { 21, WordForm.Abbreviation, GrammaticalGender.Feminine, "ventuno" },
    };

    public static TheoryData<int, string> OrdinalCases => new()
    {
        { 1, "primo" },
        { 3, "terzo" },
    };

    public static TheoryData<int, WordForm, string> OrdinalWordFormCases => new()
    {
        { 3, WordForm.Abbreviation, "terzo" },
    };

    public static TheoryData<int, GrammaticalGender, string> OrdinalGenderCases => new()
    {
        { 1, GrammaticalGender.Feminine, "prima" },
    };

    public static TheoryData<int, GrammaticalGender, WordForm, string> OrdinalWordFormGenderCases => new()
    {
        { 3, GrammaticalGender.Feminine, WordForm.Abbreviation, "terza" },
    };

    public static TheoryData<int, string> TupleCases => new()
    {
        { 2, "due" },
    };

    [Theory]
    [MemberData(nameof(CardinalCases))]
    public void UsesExpectedCardinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinal("it", number, expected);

    [Theory]
    [MemberData(nameof(CardinalAddAndCases))]
    public void UsesExpectedCardinalAddAndCases(int number, bool addAnd, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithAddAnd("it", number, addAnd, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormCases))]
    public void UsesExpectedCardinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordForm("it", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(CardinalGenderCases))]
    public void UsesExpectedCardinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithGender("it", number, gender, expected);

    [Theory]
    [MemberData(nameof(CardinalWordFormGenderCases))]
    public void UsesExpectedCardinalWordFormGenderCases(int number, WordForm wordForm, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyCardinalWithWordFormAndGender("it", number, wordForm, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalCases))]
    public void UsesExpectedOrdinalCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinal("it", number, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormCases))]
    public void UsesExpectedOrdinalWordFormCases(int number, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordForm("it", number, wordForm, expected);

    [Theory]
    [MemberData(nameof(OrdinalGenderCases))]
    public void UsesExpectedOrdinalGenderCases(int number, GrammaticalGender gender, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithGender("it", number, gender, expected);

    [Theory]
    [MemberData(nameof(OrdinalWordFormGenderCases))]
    public void UsesExpectedOrdinalWordFormGenderCases(int number, GrammaticalGender gender, WordForm wordForm, string expected) =>
        LocaleNumberPhraseAssertions.VerifyOrdinalWithWordFormAndGender("it", number, gender, wordForm, expected);

    [Theory]
    [MemberData(nameof(TupleCases))]
    public void UsesExpectedTupleCases(int number, string expected) =>
        LocaleNumberPhraseAssertions.VerifyTuple("it", number, expected);
}
