namespace Humanizer.Tests.Localisation.gu;

[UseCulture("gu")]
public class GujaratiOrdinalTests
{
    static readonly CultureInfo Gu = new("gu");

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "પહેલો")]
    [InlineData(2, GrammaticalGender.Masculine, "બીજો")]
    [InlineData(3, GrammaticalGender.Masculine, "ત્રીજો")]
    [InlineData(4, GrammaticalGender.Masculine, "ચોથો")]
    [InlineData(5, GrammaticalGender.Masculine, "પાંચમો")]
    [InlineData(6, GrammaticalGender.Masculine, "છઠ્ઠો")]
    [InlineData(21, GrammaticalGender.Masculine, "એકવીસમો")]
    [InlineData(100, GrammaticalGender.Masculine, "એકસોમો")]
    [InlineData(1, GrammaticalGender.Feminine, "પહેલી")]
    [InlineData(2, GrammaticalGender.Feminine, "બીજી")]
    [InlineData(3, GrammaticalGender.Feminine, "ત્રીજી")]
    [InlineData(4, GrammaticalGender.Feminine, "ચોથી")]
    [InlineData(5, GrammaticalGender.Feminine, "પાંચમી")]
    [InlineData(6, GrammaticalGender.Feminine, "છઠ્ઠી")]
    [InlineData(21, GrammaticalGender.Feminine, "એકવીસમી")]
    [InlineData(100, GrammaticalGender.Feminine, "એકસોમી")]
    [InlineData(1, GrammaticalGender.Neuter, "પહેલું")]
    [InlineData(2, GrammaticalGender.Neuter, "બીજું")]
    [InlineData(3, GrammaticalGender.Neuter, "ત્રીજું")]
    [InlineData(4, GrammaticalGender.Neuter, "ચોથું")]
    [InlineData(5, GrammaticalGender.Neuter, "પાંચમું")]
    [InlineData(6, GrammaticalGender.Neuter, "છઠ્ઠું")]
    [InlineData(21, GrammaticalGender.Neuter, "એકવીસમું")]
    [InlineData(100, GrammaticalGender.Neuter, "એકસોમું")]
    public void ToOrdinalWords_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Gu));
    }

    [Theory]
    [InlineData(5, "પાંચમો")]
    [InlineData(21, "એકવીસમો")]
    public void ToOrdinalWords_GenderlessDefaultsToMasculine(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Gu));
    }

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "પાંચમો")]
    [InlineData(5, GrammaticalGender.Feminine, "પાંચમી")]
    [InlineData(21, GrammaticalGender.Masculine, "એકવીસમો")]
    [InlineData(21, GrammaticalGender.Feminine, "એકવીસમી")]
    [InlineData(21, GrammaticalGender.Neuter, "એકવીસમું")]
    [InlineData(100, GrammaticalGender.Masculine, "એકસોમો")]
    [InlineData(100, GrammaticalGender.Feminine, "એકસોમી")]
    [InlineData(100, GrammaticalGender.Neuter, "એકસોમું")]
    public void Ordinalize_Int_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Gu));
    }

    [Theory]
    [InlineData(-1, GrammaticalGender.Masculine, "નકારાત્મક પહેલો")]
    [InlineData(-5, GrammaticalGender.Masculine, "નકારાત્મક પાંચમો")]
    [InlineData(-5, GrammaticalGender.Feminine, "નકારાત્મક પાંચમી")]
    [InlineData(-5, GrammaticalGender.Neuter, "નકારાત્મક પાંચમું")]
    [InlineData(-21, GrammaticalGender.Masculine, "નકારાત્મક એકવીસમો")]
    [InlineData(-21, GrammaticalGender.Neuter, "નકારાત્મક એકવીસમું")]
    public void NegativeOrdinals_BothPaths_ProduceConsistentOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Gu));
        Assert.Equal(expected, number.Ordinalize(gender, Gu));
    }

    [Theory]
    [InlineData("પહેલો", 1)]
    [InlineData("પહેલી", 1)]
    [InlineData("પહેલું", 1)]
    [InlineData("પાંચમો", 5)]
    [InlineData("પાંચમી", 5)]
    [InlineData("પાંચમું", 5)]
    [InlineData("એકવીસમી", 21)]
    [InlineData("એકવીસમું", 21)]
    [InlineData("એકસોમો", 100)]
    [InlineData("એકસોમી", 100)]
    [InlineData("એકસો એકમો", 101)]
    [InlineData("એક લાખમો", 100000)]
    public void WordsToNumber_ParsesGujaratiOrdinalWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Gu));
    }
}