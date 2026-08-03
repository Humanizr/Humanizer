namespace Humanizer.Tests.Localisation.sr_Latn;

[UseCulture("sr-Latn")]
public class SerbianLatinGenderedOrdinalTests
{
    static readonly CultureInfo SrLatn = new("sr-Latn");

    [Theory]
    [InlineData(1, "jedna")]
    [InlineData(2, "dve")]
    [InlineData(21, "dvadeset jedna")]
    [InlineData(22, "dvadeset dve")]
    public void ToWords_ProducesSerbianFeminineCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine, SrLatn));
        Assert.Equal(expected, number.ToWords(WordForm.Normal, GrammaticalGender.Feminine, SrLatn));
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData("sr-Latn", 1, "pet i jedan")]
    [InlineData("sr-Latn", 2, "pet i dva")]
    [InlineData("sr-Latn", 21, "pet i dvadeset jedan")]
    [InlineData("sr-Latn", 22, "pet i dvadeset dva")]
    [InlineData("sr-Latn-RS", 22, "pet i dvadeset dva")]
    public void ToClockNotation_KeepsSerbianLatinMasculineConvention(string cultureName, int minutes, string expected)
    {
        Assert.Equal(
            expected,
            new TimeOnly(5, minutes).ToClockNotation(
                ClockNotationRounding.None,
                new(cultureName)));
    }
#endif

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
        Assert.Equal(expected, number.Ordinalize(gender, SrLatn));
        Assert.Equal(expected, number.ToString(SrLatn).Ordinalize(gender, SrLatn));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine)]
    [InlineData(1, GrammaticalGender.Feminine)]
    [InlineData(1, GrammaticalGender.Neuter)]
    public void Ordinalize_GenderInvariant_AllGendersMatch(int number, GrammaticalGender gender)
    {
        var masculine = number.Ordinalize(GrammaticalGender.Masculine, SrLatn);
        var result = number.Ordinalize(gender, SrLatn);
        Assert.Equal(masculine, result);
    }

    [Theory]
    [InlineData(0, GrammaticalGender.Masculine, "nulti")]
    [InlineData(1, GrammaticalGender.Masculine, "prvi")]
    [InlineData(2, GrammaticalGender.Feminine, "druga")]
    [InlineData(3, GrammaticalGender.Neuter, "treće")]
    [InlineData(11, GrammaticalGender.Masculine, "jedanaesti")]
    [InlineData(20, GrammaticalGender.Feminine, "dvadeseta")]
    [InlineData(21, GrammaticalGender.Neuter, "dvadeset prvo")]
    [InlineData(100, GrammaticalGender.Masculine, "stoti")]
    [InlineData(101, GrammaticalGender.Feminine, "sto prva")]
    [InlineData(200, GrammaticalGender.Neuter, "dvestoto")]
    [InlineData(1000, GrammaticalGender.Masculine, "hiljaditi")]
    [InlineData(2000, GrammaticalGender.Masculine, "dvehiljaditi")]
    [InlineData(21000, GrammaticalGender.Masculine, "dvadesetjednohiljaditi")]
    [InlineData(2000000, GrammaticalGender.Masculine, "dvomilioniti")]
    [InlineData(22000000, GrammaticalGender.Masculine, "dvadesetdvomilioniti")]
    [InlineData(102000000, GrammaticalGender.Masculine, "stodvomilioniti")]
    [InlineData(2001, GrammaticalGender.Masculine, "dve hiljade prvi")]
    [InlineData(-1, GrammaticalGender.Masculine, "- prvi")]
    public void ToOrdinalWords_ProducesSerbianWords(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, SrLatn));
        if (gender == GrammaticalGender.Masculine)
        {
            Assert.Equal(expected, number.ToOrdinalWords(SrLatn));
        }
    }
}