namespace Humanizer.Tests.Localisation.pl;

[UseCulture("pl")]
public class PolishGenderedOrdinalTests
{
    static readonly CultureInfo Pl = new("pl");

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "1.")]
    [InlineData(2, GrammaticalGender.Masculine, "2.")]
    [InlineData(23, GrammaticalGender.Masculine, "23.")]
    [InlineData(100, GrammaticalGender.Masculine, "100.")]
    [InlineData(101, GrammaticalGender.Masculine, "101.")]
    [InlineData(1, GrammaticalGender.Feminine, "1.")]
    [InlineData(2, GrammaticalGender.Feminine, "2.")]
    [InlineData(23, GrammaticalGender.Feminine, "23.")]
    [InlineData(100, GrammaticalGender.Feminine, "100.")]
    [InlineData(101, GrammaticalGender.Feminine, "101.")]
    [InlineData(1, GrammaticalGender.Neuter, "1.")]
    [InlineData(2, GrammaticalGender.Neuter, "2.")]
    [InlineData(23, GrammaticalGender.Neuter, "23.")]
    [InlineData(100, GrammaticalGender.Neuter, "100.")]
    [InlineData(101, GrammaticalGender.Neuter, "101.")]
    public void Ordinalize_AllGenders_ProduceDotSuffix(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Pl));
        Assert.Equal(expected, number.ToString(Pl).Ordinalize(gender, Pl));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine)]
    [InlineData(1, GrammaticalGender.Feminine)]
    [InlineData(1, GrammaticalGender.Neuter)]
    public void Ordinalize_GenderInvariant_AllGendersMatch(int number, GrammaticalGender gender)
    {
        var masculine = number.Ordinalize(GrammaticalGender.Masculine, Pl);
        var result = number.Ordinalize(gender, Pl);
        Assert.Equal(masculine, result);
    }
}
