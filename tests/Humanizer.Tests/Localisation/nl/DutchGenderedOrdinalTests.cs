namespace Humanizer.Tests.Localisation.nl;

[UseCulture("nl")]
public class DutchGenderedOrdinalTests
{
    static readonly CultureInfo Nl = new("nl");

    [Theory]
    [InlineData(0, GrammaticalGender.Masculine, "0")]
    [InlineData(1, GrammaticalGender.Masculine, "1e")]
    [InlineData(2, GrammaticalGender.Masculine, "2e")]
    [InlineData(23, GrammaticalGender.Masculine, "23e")]
    [InlineData(0, GrammaticalGender.Feminine, "0")]
    [InlineData(1, GrammaticalGender.Feminine, "1e")]
    [InlineData(2, GrammaticalGender.Feminine, "2e")]
    [InlineData(23, GrammaticalGender.Feminine, "23e")]
    [InlineData(0, GrammaticalGender.Neuter, "0")]
    [InlineData(1, GrammaticalGender.Neuter, "1e")]
    [InlineData(2, GrammaticalGender.Neuter, "2e")]
    [InlineData(23, GrammaticalGender.Neuter, "23e")]
    public void Ordinalize_AllGenders_ProduceESuffix(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Nl));
        Assert.Equal(expected, number.ToString(Nl).Ordinalize(gender, Nl));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine)]
    [InlineData(1, GrammaticalGender.Feminine)]
    [InlineData(1, GrammaticalGender.Neuter)]
    public void Ordinalize_GenderInvariant_AllGendersMatch(int number, GrammaticalGender gender)
    {
        var masculine = number.Ordinalize(GrammaticalGender.Masculine, Nl);
        var result = number.Ordinalize(gender, Nl);
        Assert.Equal(masculine, result);
    }
}
