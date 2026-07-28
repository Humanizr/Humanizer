namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiOrdinalTests
{
    static readonly CultureInfo Hi = new("hi");

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "पहला")]
    [InlineData(2, GrammaticalGender.Masculine, "दूसरा")]
    [InlineData(3, GrammaticalGender.Masculine, "तीसरा")]
    [InlineData(4, GrammaticalGender.Masculine, "चौथा")]
    [InlineData(5, GrammaticalGender.Masculine, "पाँचवाँ")]
    [InlineData(6, GrammaticalGender.Masculine, "छठा")]
    [InlineData(10, GrammaticalGender.Masculine, "दसवाँ")]
    [InlineData(21, GrammaticalGender.Masculine, "इक्कीसवाँ")]
    [InlineData(100, GrammaticalGender.Masculine, "एक सौवाँ")]
    [InlineData(1, GrammaticalGender.Feminine, "पहली")]
    [InlineData(2, GrammaticalGender.Feminine, "दूसरी")]
    [InlineData(3, GrammaticalGender.Feminine, "तीसरी")]
    [InlineData(4, GrammaticalGender.Feminine, "चौथी")]
    [InlineData(5, GrammaticalGender.Feminine, "पाँचवीं")]
    [InlineData(6, GrammaticalGender.Feminine, "छठी")]
    [InlineData(10, GrammaticalGender.Feminine, "दसवीं")]
    [InlineData(21, GrammaticalGender.Feminine, "इक्कीसवीं")]
    [InlineData(100, GrammaticalGender.Feminine, "एक सौवीं")]
    public void ToOrdinalWords_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Hi));
    }

    [Theory]
    [InlineData(5, "पाँचवाँ")]
    [InlineData(21, "इक्कीसवाँ")]
    public void ToOrdinalWords_GenderlessDefaultsToMasculine(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Hi));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Neuter, "पहला")]
    [InlineData(21, GrammaticalGender.Neuter, "इक्कीसवाँ")]
    public void ToOrdinalWords_NeuterFallsBackToMasculine(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Hi));
    }

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "पाँचवाँ")]
    [InlineData(5, GrammaticalGender.Feminine, "पाँचवीं")]
    [InlineData(21, GrammaticalGender.Masculine, "इक्कीसवाँ")]
    [InlineData(21, GrammaticalGender.Feminine, "इक्कीसवीं")]
    [InlineData(100, GrammaticalGender.Masculine, "एक सौवाँ")]
    [InlineData(100, GrammaticalGender.Feminine, "एक सौवीं")]
    public void Ordinalize_Int_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Hi));
    }

    [Theory]
    [InlineData("21", GrammaticalGender.Masculine, "इक्कीसवाँ")]
    [InlineData("21", GrammaticalGender.Feminine, "इक्कीसवीं")]
    [InlineData("21", GrammaticalGender.Neuter, "इक्कीसवाँ")]
    public void Ordinalize_String_GenderedOutput(string numberString, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, numberString.Ordinalize(gender, Hi));
    }

    [Theory]
    [InlineData(-1, GrammaticalGender.Masculine, "ऋणात्मक पहला")]
    [InlineData(-5, GrammaticalGender.Masculine, "ऋणात्मक पाँचवाँ")]
    [InlineData(-5, GrammaticalGender.Feminine, "ऋणात्मक पाँचवीं")]
    [InlineData(-21, GrammaticalGender.Masculine, "ऋणात्मक इक्कीसवाँ")]
    public void NegativeOrdinals_BothPaths_ProduceConsistentOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Hi));
        Assert.Equal(expected, number.Ordinalize(gender, Hi));
    }

    [Theory]
    [InlineData("पहला", 1)]
    [InlineData("पाँचवाँ", 5)]
    [InlineData("पांचवां", 5)]
    [InlineData("इक्कीसवीं", 21)]
    [InlineData("एक सौवाँ", 100)]
    [InlineData("एक सौवीं", 100)]
    [InlineData("एक सौ एकवाँ", 101)]
    [InlineData("एक लाखवाँ", 100000)]
    public void WordsToNumber_ParsesHindiOrdinalWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Hi));
    }

    [Fact]
    public void Ordinalize_UsesExplicitTargetCultureWhenAmbientCultureDiffers()
    {
        using var _ = new CultureSwap(new CultureInfo("en-US"));

        Assert.Equal("इक्कीसवाँ", 21.Ordinalize(Hi));
        Assert.Equal("इक्कीसवीं", 21.Ordinalize(GrammaticalGender.Feminine, Hi));
    }
}