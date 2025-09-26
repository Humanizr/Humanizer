namespace ca;

[UseCulture("ca")]
public class OrdinalizeTests
{
    [Theory]
    [InlineData(1, "1r")]
    [InlineData(3, "3r")]
    public void OrdinalizeDefaultGender(int number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(), ordinalized);

    [Theory]
    [InlineData(-1, "1r")]
    [InlineData(int.MinValue, "0")]
    public void OrdinalizeZeroOrNegativeNumber(int number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(), ordinalized);

    [Theory]
    [InlineData(1, WordForm.Abbreviation, "1r")]
    [InlineData(1, WordForm.Normal, "1r")]
    [InlineData(2, WordForm.Abbreviation, "2n")]
    [InlineData(2, WordForm.Normal, "2n")]
    [InlineData(3, WordForm.Abbreviation, "3r")]
    [InlineData(3, WordForm.Normal, "3r")]
    [InlineData(21, WordForm.Abbreviation, "21r")]
    [InlineData(21, WordForm.Normal, "21r")]
    public void OrdinalizeWithWordForm(int number, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(wordForm));
        Assert.Equal(expected, number.ToString(CultureInfo.CurrentUICulture).Ordinalize(wordForm));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, WordForm.Abbreviation, "1r")]
    [InlineData(1, GrammaticalGender.Masculine, WordForm.Normal, "1r")]
    [InlineData(1, GrammaticalGender.Feminine, WordForm.Abbreviation, "1a")]
    [InlineData(1, GrammaticalGender.Feminine, WordForm.Normal, "1a")]
    [InlineData(1, GrammaticalGender.Neuter, WordForm.Abbreviation, "1r")]
    [InlineData(1, GrammaticalGender.Neuter, WordForm.Normal, "1r")]
    public void OrdinalizeWithWordFormAndGender(int number, GrammaticalGender gender, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, wordForm));
        Assert.Equal(expected, number.ToString(CultureInfo.CurrentUICulture).Ordinalize(gender, wordForm));
    }

    [Theory]
    [InlineData("1", "1r")]
    [InlineData("2", "2n")]
    [InlineData("3", "3r")]
    [InlineData("4", "4t")]
    [InlineData("5", "5è")]
    [InlineData("6", "6è")]
    [InlineData("23", "23r")]
    [InlineData("100", "100è")]
    [InlineData("101", "101r")]
    [InlineData("102", "102n")]
    [InlineData("103", "103r")]
    [InlineData("1001", "1001r")]
    public void OrdinalizeString(string number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);

    [Theory]
    [InlineData("0", "0")]
    [InlineData("1", "1a")]
    [InlineData("2", "2a")]
    [InlineData("3", "3a")]
    [InlineData("4", "4a")]
    [InlineData("5", "5a")]
    [InlineData("6", "6a")]
    [InlineData("23", "23a")]
    [InlineData("100", "100a")]
    [InlineData("101", "101a")]
    [InlineData("102", "102a")]
    [InlineData("103", "103a")]
    [InlineData("1001", "1001a")]
    public void OrdinalizeStringFeminine(string number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1r")]
    [InlineData(2, "2n")]
    [InlineData(3, "3r")]
    [InlineData(4, "4t")]
    [InlineData(5, "5è")]
    [InlineData(6, "6è")]
    [InlineData(10, "10è")]
    [InlineData(23, "23r")]
    [InlineData(100, "100è")]
    [InlineData(101, "101r")]
    [InlineData(102, "102n")]
    [InlineData(103, "103r")]
    [InlineData(1001, "1001r")]
    public void OrdinalizeNumber(int number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1a")]
    [InlineData(2, "2a")]
    [InlineData(3, "3a")]
    [InlineData(4, "4a")]
    [InlineData(5, "5a")]
    [InlineData(6, "6a")]
    [InlineData(10, "10a")]
    [InlineData(23, "23a")]
    [InlineData(100, "100a")]
    [InlineData(101, "101a")]
    [InlineData(102, "102a")]
    [InlineData(103, "103a")]
    [InlineData(1001, "1001a")]
    public void OrdinalizeNumberFeminine(int number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
}