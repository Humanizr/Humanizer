namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiOrdinalTests
{
    static readonly CultureInfo Pa = new("pa");

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "ਪਹਿਲਾ")]
    [InlineData(2, GrammaticalGender.Masculine, "ਦੂਜਾ")]
    [InlineData(3, GrammaticalGender.Masculine, "ਤੀਜਾ")]
    [InlineData(4, GrammaticalGender.Masculine, "ਚੌਥਾ")]
    [InlineData(5, GrammaticalGender.Masculine, "ਪੰਜਵਾਂ")]
    [InlineData(6, GrammaticalGender.Masculine, "ਛੇਵਾਂ")]
    [InlineData(10, GrammaticalGender.Masculine, "ਦਸਵਾਂ")]
    [InlineData(21, GrammaticalGender.Masculine, "ਇੱਕੀਵਾਂ")]
    [InlineData(100, GrammaticalGender.Masculine, "ਇੱਕ ਸੌਵਾਂ")]
    [InlineData(1, GrammaticalGender.Feminine, "ਪਹਿਲੀ")]
    [InlineData(2, GrammaticalGender.Feminine, "ਦੂਜੀ")]
    [InlineData(3, GrammaticalGender.Feminine, "ਤੀਜੀ")]
    [InlineData(4, GrammaticalGender.Feminine, "ਚੌਥੀ")]
    [InlineData(5, GrammaticalGender.Feminine, "ਪੰਜਵੀਂ")]
    [InlineData(6, GrammaticalGender.Feminine, "ਛੇਵੀਂ")]
    [InlineData(10, GrammaticalGender.Feminine, "ਦਸਵੀਂ")]
    [InlineData(21, GrammaticalGender.Feminine, "ਇੱਕੀਵੀਂ")]
    [InlineData(100, GrammaticalGender.Feminine, "ਇੱਕ ਸੌਵੀਂ")]
    public void ToOrdinalWords_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Pa));
    }

    [Theory]
    [InlineData(5, "ਪੰਜਵਾਂ")]
    [InlineData(21, "ਇੱਕੀਵਾਂ")]
    public void ToOrdinalWords_GenderlessDefaultsToMasculine(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Pa));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Neuter, "ਪਹਿਲਾ")]
    [InlineData(21, GrammaticalGender.Neuter, "ਇੱਕੀਵਾਂ")]
    public void ToOrdinalWords_NeuterFallsBackToMasculine(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Pa));
    }

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "ਪੰਜਵਾਂ")]
    [InlineData(5, GrammaticalGender.Feminine, "ਪੰਜਵੀਂ")]
    [InlineData(21, GrammaticalGender.Masculine, "ਇੱਕੀਵਾਂ")]
    [InlineData(21, GrammaticalGender.Feminine, "ਇੱਕੀਵੀਂ")]
    [InlineData(100, GrammaticalGender.Masculine, "ਇੱਕ ਸੌਵਾਂ")]
    [InlineData(100, GrammaticalGender.Feminine, "ਇੱਕ ਸੌਵੀਂ")]
    public void Ordinalize_Int_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Pa));
    }

    [Theory]
    [InlineData("21", GrammaticalGender.Masculine, "ਇੱਕੀਵਾਂ")]
    [InlineData("21", GrammaticalGender.Feminine, "ਇੱਕੀਵੀਂ")]
    [InlineData("21", GrammaticalGender.Neuter, "ਇੱਕੀਵਾਂ")]
    public void Ordinalize_String_GenderedOutput(string numberString, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, numberString.Ordinalize(gender, Pa));
    }

    [Theory]
    [InlineData(-1, GrammaticalGender.Masculine, "ਰਿਣਾਤਮਕ ਪਹਿਲਾ")]
    [InlineData(-5, GrammaticalGender.Masculine, "ਰਿਣਾਤਮਕ ਪੰਜਵਾਂ")]
    [InlineData(-5, GrammaticalGender.Feminine, "ਰਿਣਾਤਮਕ ਪੰਜਵੀਂ")]
    [InlineData(-21, GrammaticalGender.Masculine, "ਰਿਣਾਤਮਕ ਇੱਕੀਵਾਂ")]
    public void NegativeOrdinals_BothPaths_ProduceConsistentOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Pa));
        Assert.Equal(expected, number.Ordinalize(gender, Pa));
    }

    [Theory]
    [InlineData("ਪਹਿਲਾ", 1)]
    [InlineData("ਪੰਜਵਾਂ", 5)]
    [InlineData("ਇੱਕੀਵੀਂ", 21)]
    [InlineData("ਇੱਕ ਸੌਵਾਂ", 100)]
    [InlineData("ਇੱਕ ਸੌਵੀਂ", 100)]
    [InlineData("ਇੱਕ ਸੌ ਇੱਕਵਾਂ", 101)]
    [InlineData("ਇੱਕ ਲੱਖਵਾਂ", 100000)]
    public void WordsToNumber_ParsesPunjabiOrdinalWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Pa));
    }
}