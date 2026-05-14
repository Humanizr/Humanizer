namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliOrdinalTests
{
    static readonly CultureInfo Ne = new("ne");

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "पहिलो")]
    [InlineData(2, GrammaticalGender.Masculine, "दोस्रो")]
    [InlineData(3, GrammaticalGender.Masculine, "तेस्रो")]
    [InlineData(4, GrammaticalGender.Masculine, "चौथो")]
    [InlineData(5, GrammaticalGender.Masculine, "पाँचौँ")]
    [InlineData(6, GrammaticalGender.Masculine, "छैटौँ")]
    [InlineData(10, GrammaticalGender.Masculine, "दसौँ")]
    [InlineData(21, GrammaticalGender.Masculine, "एक्काइसौँ")]
    [InlineData(60, GrammaticalGender.Masculine, "साठिऔँ")]
    [InlineData(100, GrammaticalGender.Masculine, "एक सयौँ")]
    [InlineData(1, GrammaticalGender.Feminine, "पहिली")]
    [InlineData(2, GrammaticalGender.Feminine, "दोस्री")]
    [InlineData(3, GrammaticalGender.Feminine, "तेस्री")]
    [InlineData(4, GrammaticalGender.Feminine, "चौथी")]
    [InlineData(5, GrammaticalGender.Feminine, "पाँचौँ")]
    [InlineData(6, GrammaticalGender.Feminine, "छैटौँ")]
    [InlineData(10, GrammaticalGender.Feminine, "दसौँ")]
    [InlineData(21, GrammaticalGender.Feminine, "एक्काइसौँ")]
    [InlineData(100, GrammaticalGender.Feminine, "एक सयौँ")]
    public void ToOrdinalWords_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Ne));
    }

    [Theory]
    [InlineData(5, "पाँचौँ")]
    [InlineData(21, "एक्काइसौँ")]
    public void ToOrdinalWords_GenderlessDefaultsToMasculine(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Ne));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Neuter, "पहिलो")]
    [InlineData(21, GrammaticalGender.Neuter, "एक्काइसौँ")]
    public void ToOrdinalWords_NeuterFallsBackToMasculine(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Ne));
    }

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "पाँचौँ")]
    [InlineData(5, GrammaticalGender.Feminine, "पाँचौँ")]
    [InlineData(21, GrammaticalGender.Masculine, "एक्काइसौँ")]
    [InlineData(21, GrammaticalGender.Feminine, "एक्काइसौँ")]
    [InlineData(100, GrammaticalGender.Masculine, "एक सयौँ")]
    [InlineData(100, GrammaticalGender.Feminine, "एक सयौँ")]
    public void Ordinalize_Int_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Ne));
    }

    [Theory]
    [InlineData("21", GrammaticalGender.Masculine, "एक्काइसौँ")]
    [InlineData("21", GrammaticalGender.Feminine, "एक्काइसौँ")]
    [InlineData("21", GrammaticalGender.Neuter, "एक्काइसौँ")]
    public void Ordinalize_String_GenderedOutput(string numberString, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, numberString.Ordinalize(gender, Ne));
    }

    [Theory]
    [InlineData(-1, GrammaticalGender.Masculine, "माइनस पहिलो")]
    [InlineData(-5, GrammaticalGender.Masculine, "माइनस पाँचौँ")]
    [InlineData(-5, GrammaticalGender.Feminine, "माइनस पाँचौँ")]
    [InlineData(-21, GrammaticalGender.Masculine, "माइनस एक्काइसौँ")]
    public void NegativeOrdinals_BothPaths_ProduceConsistentOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Ne));
        Assert.Equal(expected, number.Ordinalize(gender, Ne));
    }

    [Theory]
    [InlineData("पहिलो", 1)]
    [InlineData("पाँचौँ", 5)]
    [InlineData("एक्काइसौँ", 21)]
    [InlineData("एक सयौँ", 100)]
    [InlineData("एक सय एकौँ", 101)]
    [InlineData("एक लाखौँ", 100000)]
    public void WordsToNumber_ParsesNepaliOrdinalWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ne));
    }

    [Fact]
    public void Ordinalize_UsesExplicitTargetCultureWhenAmbientCultureDiffers()
    {
        using var _ = new CultureSwap(new CultureInfo("en-US"));

        Assert.Equal("एक्काइसौँ", 21.Ordinalize(Ne));
        Assert.Equal("एक्काइसौँ", 21.Ordinalize(GrammaticalGender.Feminine, Ne));
    }
}