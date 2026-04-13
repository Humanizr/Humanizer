namespace Humanizer.Tests.Localisation.ur;

/// <summary>
/// Verifies both ordinal API families (Ordinalize via IOrdinalizer and ToOrdinalWords via
/// INumberToWordsConverter) produce consistent gendered word-ordinal output for Urdu.
/// Also tests regional variant resolution (ur-PK, ur-IN) via parent culture walk.
/// </summary>
[UseCulture("ur")]
public class UrduOrdinalTests
{
    static readonly CultureInfo Ur = new("ur");
    static readonly CultureInfo UrPk = new("ur-PK");
    static readonly CultureInfo UrIn = new("ur-IN");

    // --- ToOrdinalWords (INumberToWordsConverter path) ---

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "پہلا")]
    [InlineData(2, GrammaticalGender.Masculine, "دوسرا")]
    [InlineData(3, GrammaticalGender.Masculine, "تیسرا")]
    [InlineData(4, GrammaticalGender.Masculine, "چوتھا")]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(6, GrammaticalGender.Masculine, "چھٹا")]
    [InlineData(9, GrammaticalGender.Masculine, "نواں")]
    [InlineData(50, GrammaticalGender.Masculine, "پچاسواں")]
    [InlineData(100, GrammaticalGender.Masculine, "ایک سوواں")]
    [InlineData(101, GrammaticalGender.Masculine, "ایک سو ایکواں")]
    [InlineData(100000, GrammaticalGender.Masculine, "ایک لاکھواں")]
    [InlineData(1, GrammaticalGender.Feminine, "پہلی")]
    [InlineData(2, GrammaticalGender.Feminine, "دوسری")]
    [InlineData(3, GrammaticalGender.Feminine, "تیسری")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    [InlineData(50, GrammaticalGender.Feminine, "پچاسویں")]
    [InlineData(100, GrammaticalGender.Feminine, "ایک سوویں")]
    [InlineData(100000, GrammaticalGender.Feminine, "ایک لاکھویں")]
    public void ToOrdinalWords_GenderedOutput(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(gender, Ur));

    [Theory]
    [InlineData(5, "پانچواں")]
    [InlineData(50, "پچاسواں")]
    public void ToOrdinalWords_GenderlessDefaultsToMasculine(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(Ur));

    [Theory]
    [InlineData(1, GrammaticalGender.Neuter, "پہلا")]
    [InlineData(5, GrammaticalGender.Neuter, "پانچواں")]
    public void ToOrdinalWords_NeuterFallsBackToMasculine(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(gender, Ur));

    // --- Ordinalize (IOrdinalizer path) ---

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    [InlineData(50, GrammaticalGender.Masculine, "پچاسواں")]
    [InlineData(50, GrammaticalGender.Feminine, "پچاسویں")]
    [InlineData(100, GrammaticalGender.Masculine, "ایک سوواں")]
    [InlineData(100, GrammaticalGender.Feminine, "ایک سوویں")]
    [InlineData(100000, GrammaticalGender.Masculine, "ایک لاکھواں")]
    [InlineData(100000, GrammaticalGender.Feminine, "ایک لاکھویں")]
    public void Ordinalize_Int_GenderedOutput(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.Ordinalize(gender, Ur));

    [Theory]
    [InlineData("5", GrammaticalGender.Masculine, "پانچواں")]
    [InlineData("5", GrammaticalGender.Feminine, "پانچویں")]
    public void Ordinalize_String_GenderedOutput(string numberString, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, numberString.Ordinalize(gender, Ur));

    [Theory]
    [InlineData(5, GrammaticalGender.Neuter, "پانچواں")]
    [InlineData(1, GrammaticalGender.Neuter, "پہلا")]
    public void Ordinalize_NeuterFallsBackToMasculine(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.Ordinalize(gender, Ur));

    [Fact]
    public void Ordinalize_GenderlessDefaultsToMasculine() =>
        Assert.Equal("پانچواں", 5.Ordinalize(Ur));

    // --- Both API paths produce consistent output ---

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "پہلا")]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    [InlineData(100, GrammaticalGender.Masculine, "ایک سوواں")]
    public void ToOrdinalWords_And_Ordinalize_ProduceConsistentOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Ur));
        Assert.Equal(expected, number.Ordinalize(gender, Ur));
    }

    // --- Regional variant resolution via parent culture walk ---

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    public void Ordinalize_UrPk_ResolvesViaParentWalk(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.Ordinalize(gender, UrPk));

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    public void Ordinalize_UrIn_ResolvesViaParentWalk(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.Ordinalize(gender, UrIn));
}
