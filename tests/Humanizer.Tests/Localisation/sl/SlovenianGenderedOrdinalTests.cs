namespace Humanizer.Tests.Localisation.sl;

[UseCulture("sl")]
public class SlovenianGenderedOrdinalTests
{
    static readonly CultureInfo Sl = new("sl");

    [Theory]
    [InlineData(2, "dve")]
    [InlineData(22, "dvaindvajset")]
    public void ToWords_ProducesSlovenianFeminineCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine, Sl));
        Assert.Equal(expected, number.ToWords(WordForm.Normal, GrammaticalGender.Feminine, Sl));
    }

    [Fact]
    public void ToWords_InvertedCompoundScaleCount_IsGenderInvariant()
    {
        const long number = 22_000_000_000;
        const string expected = "dvaindvajset milijard";

        Assert.Equal(expected, number.ToWords(Sl));
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Masculine, Sl));
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Neuter, Sl));
    }

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
        Assert.Equal(expected, number.Ordinalize(gender, Sl));
        Assert.Equal(expected, number.ToString(Sl).Ordinalize(gender, Sl));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine)]
    [InlineData(1, GrammaticalGender.Feminine)]
    [InlineData(1, GrammaticalGender.Neuter)]
    public void Ordinalize_GenderInvariant_AllGendersMatch(int number, GrammaticalGender gender)
    {
        var masculine = number.Ordinalize(GrammaticalGender.Masculine, Sl);
        var result = number.Ordinalize(gender, Sl);
        Assert.Equal(masculine, result);
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "prvi")]
    [InlineData(2, GrammaticalGender.Feminine, "druga")]
    [InlineData(3, GrammaticalGender.Neuter, "tretje")]
    [InlineData(21, GrammaticalGender.Masculine, "enaindvajseti")]
    [InlineData(101, GrammaticalGender.Feminine, "stoprva")]
    [InlineData(1000, GrammaticalGender.Neuter, "tisoče")]
    [InlineData(21000, GrammaticalGender.Masculine, "enaindvajsettisoči")]
    [InlineData(2000000, GrammaticalGender.Masculine, "dvomilijonti")]
    [InlineData(22000000, GrammaticalGender.Masculine, "dvaindvajsetmilijonti")]
    public void ToOrdinalWords_ProducesSlovenianWords(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(gender, WordForm.Normal, Sl));

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2, 2, "dve dve")]
    [InlineData(22, 22, "dvaindvajset dvaindvajset")]
    public void ToClockNotation_UsesFeminineHourAndMinuteValues(int hours, int minutes, string expected) =>
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());

    [Fact]
    public void ToClockNotation_RegionalCultureUsesSlovenianProfile() =>
        Assert.Equal(
            "dve dve",
            new TimeOnly(2, 2).ToClockNotation(ClockNotationRounding.None, new("sl-SI")));
#endif
}