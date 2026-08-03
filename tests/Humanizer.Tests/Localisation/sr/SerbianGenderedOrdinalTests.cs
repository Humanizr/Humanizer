namespace Humanizer.Tests.Localisation.sr;

[UseCulture("sr")]
public class SerbianGenderedOrdinalTests
{
    static readonly CultureInfo Sr = new("sr");

    [Theory]
    [InlineData(1, "једна")]
    [InlineData(2, "две")]
    [InlineData(21, "двадесет једна")]
    [InlineData(22, "двадесет две")]
    public void ToWords_ProducesSerbianFeminineCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine, Sr));
        Assert.Equal(expected, number.ToWords(WordForm.Normal, GrammaticalGender.Feminine, Sr));
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData("sr", 1, "пет и један")]
    [InlineData("sr", 2, "пет и два")]
    [InlineData("sr", 21, "пет и двадесет један")]
    [InlineData("sr", 22, "пет и двадесет два")]
    [InlineData("sr-Cyrl-RS", 22, "пет и двадесет два")]
    public void ToClockNotation_KeepsSerbianMasculineConvention(string cultureName, int minutes, string expected)
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
        Assert.Equal(expected, number.Ordinalize(gender, Sr));
        Assert.Equal(expected, number.ToString(Sr).Ordinalize(gender, Sr));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine)]
    [InlineData(1, GrammaticalGender.Feminine)]
    [InlineData(1, GrammaticalGender.Neuter)]
    public void Ordinalize_GenderInvariant_AllGendersMatch(int number, GrammaticalGender gender)
    {
        var masculine = number.Ordinalize(GrammaticalGender.Masculine, Sr);
        var result = number.Ordinalize(gender, Sr);
        Assert.Equal(masculine, result);
    }

    [Theory]
    [InlineData(0, GrammaticalGender.Masculine, "нулти")]
    [InlineData(1, GrammaticalGender.Masculine, "први")]
    [InlineData(2, GrammaticalGender.Feminine, "друга")]
    [InlineData(3, GrammaticalGender.Neuter, "треће")]
    [InlineData(11, GrammaticalGender.Masculine, "једанаести")]
    [InlineData(20, GrammaticalGender.Feminine, "двадесета")]
    [InlineData(21, GrammaticalGender.Neuter, "двадесет прво")]
    [InlineData(100, GrammaticalGender.Masculine, "стоти")]
    [InlineData(101, GrammaticalGender.Feminine, "сто прва")]
    [InlineData(200, GrammaticalGender.Neuter, "двестото")]
    [InlineData(1000, GrammaticalGender.Masculine, "хиљадити")]
    [InlineData(2000, GrammaticalGender.Masculine, "двехиљадити")]
    [InlineData(21000, GrammaticalGender.Masculine, "двадесетједнохиљадити")]
    [InlineData(2000000, GrammaticalGender.Masculine, "двомилионити")]
    [InlineData(22000000, GrammaticalGender.Masculine, "двадесетдвомилионити")]
    [InlineData(102000000, GrammaticalGender.Masculine, "стодвомилионити")]
    [InlineData(2001, GrammaticalGender.Masculine, "две хиљаде први")]
    [InlineData(-1, GrammaticalGender.Masculine, "- први")]
    public void ToOrdinalWords_ProducesSerbianWords(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Sr));
        if (gender == GrammaticalGender.Masculine)
        {
            Assert.Equal(expected, number.ToOrdinalWords(Sr));
        }
    }
}