namespace Humanizer.Tests.Localisation.@as;

[UseCulture("as")]
public class AssameseOrdinalTests
{
    static readonly CultureInfo As = new("as");

    [Theory]
    [InlineData(1, "প্ৰথম")]
    [InlineData(2, "দ্বিতীয়")]
    [InlineData(3, "তৃতীয়")]
    [InlineData(4, "চতুৰ্থ")]
    [InlineData(5, "পঞ্চম")]
    [InlineData(6, "ষষ্ঠ")]
    [InlineData(21, "একৈছতম")]
    [InlineData(100, "শতম")]
    public void ToOrdinalWords_ProducesAssameseWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(As));
    }

    [Theory]
    [InlineData(1, "1ম")]
    [InlineData(2, "2য়")]
    [InlineData(4, "4ৰ্থ")]
    [InlineData(6, "6ষ্ঠ")]
    [InlineData(21, "21তম")]
    public void Ordinalize_Int_UsesAssameseSuffixes(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(As));
    }

    [Theory]
    [InlineData("21", GrammaticalGender.Masculine, "21তম")]
    [InlineData("21", GrammaticalGender.Feminine, "21তম")]
    [InlineData("21", GrammaticalGender.Neuter, "21তম")]
    public void Ordinalize_String_GenderedOutput(string numberString, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, numberString.Ordinalize(gender, As));
    }

    [Theory]
    [InlineData(-1, "-1তম")]
    [InlineData(-5, "-5তম")]
    [InlineData(-21, "-21তম")]
    public void NegativeOrdinalize_UsesDefaultSuffix(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(As));
    }
}