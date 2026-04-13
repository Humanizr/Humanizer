[UseCulture("en-US")]
public class OrdinalizeTests
{
    [Theory]
    [InlineData("0", "0th")]
    [InlineData("1", "1st")]
    [InlineData("2", "2nd")]
    [InlineData("3", "3rd")]
    [InlineData("4", "4th")]
    [InlineData("5", "5th")]
    [InlineData("6", "6th")]
    [InlineData("7", "7th")]
    [InlineData("8", "8th")]
    [InlineData("9", "9th")]
    [InlineData("10", "10th")]
    [InlineData("11", "11th")]
    [InlineData("12", "12th")]
    [InlineData("13", "13th")]
    [InlineData("14", "14th")]
    [InlineData("20", "20th")]
    [InlineData("21", "21st")]
    [InlineData("22", "22nd")]
    [InlineData("23", "23rd")]
    [InlineData("24", "24th")]
    [InlineData("100", "100th")]
    [InlineData("101", "101st")]
    [InlineData("102", "102nd")]
    [InlineData("103", "103rd")]
    [InlineData("104", "104th")]
    [InlineData("110", "110th")]
    [InlineData("1000", "1000th")]
    [InlineData("1001", "1001st")]
    public void OrdinalizeString(string number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(), ordinalized);

    [Theory]
    [InlineData(0, "0th")]
    [InlineData(1, "1st")]
    [InlineData(2, "2nd")]
    [InlineData(3, "3rd")]
    [InlineData(4, "4th")]
    [InlineData(5, "5th")]
    [InlineData(6, "6th")]
    [InlineData(7, "7th")]
    [InlineData(8, "8th")]
    [InlineData(9, "9th")]
    [InlineData(10, "10th")]
    [InlineData(11, "11th")]
    [InlineData(12, "12th")]
    [InlineData(13, "13th")]
    [InlineData(14, "14th")]
    [InlineData(20, "20th")]
    [InlineData(21, "21st")]
    [InlineData(22, "22nd")]
    [InlineData(23, "23rd")]
    [InlineData(24, "24th")]
    [InlineData(100, "100th")]
    [InlineData(101, "101st")]
    [InlineData(102, "102nd")]
    [InlineData(103, "103rd")]
    [InlineData(104, "104th")]
    [InlineData(110, "110th")]
    [InlineData(1000, "1000th")]
    [InlineData(1001, "1001st")]
    public void OrdinalizeNumber(int number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(), ordinalized);

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(8)]
    public void OrdinalizeNumberGenderIsImmaterial(int number)
    {
        var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine);
        var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine);
        Assert.Equal(masculineOrdinalized, feminineOrdinalized);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("8")]
    public void OrdinalizeStringGenderIsImmaterial(string number)
    {
        var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine);
        var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine);
        Assert.Equal(masculineOrdinalized, feminineOrdinalized);
    }

    [Theory]
    [InlineData("en-US", "1", "1st")]
    [InlineData("nl-NL", "1", "1e")]
    public void OrdinalizeStringWithSpecifiedCultureInsteadOfCurrentCulture(string cultureName, string number, string ordinalized)
    {
        var culture = new CultureInfo(cultureName);
        Assert.Equal(number.Ordinalize(culture), ordinalized);
    }

    [Theory]
    [InlineData("en-US", 1, "1st")]
    [InlineData("nl-NL", 1, "1e")]
    public void OrdinalizeNumberWithSpecifiedCultureInsteadOfCurrentCulture(string cultureName, int number, string ordinalized)
    {
        var culture = new CultureInfo(cultureName);
        Assert.Equal(number.Ordinalize(culture), ordinalized);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(8)]
    public void OrdinalizeNumberWithSpecifiedCultureGenderIsImmaterial(int number)
    {
        var culture = new CultureInfo("nl-NL");
        var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine, culture);
        var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine, culture);
        Assert.Equal(masculineOrdinalized, feminineOrdinalized);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("8")]
    public void OrdinalizeStringWithSpecifiedGenderIsImmaterial(string number)
    {
        var culture = new CultureInfo("nl-NL");
        var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine, culture);
        var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine, culture);
        Assert.Equal(masculineOrdinalized, feminineOrdinalized);
    }

    [Theory]
    [InlineData(1, WordForm.Normal, "es-ES", "1.º")]
    [InlineData(1, WordForm.Abbreviation, "es-ES", "1.er")]
    [InlineData(1, WordForm.Normal, "en-US", "1st")]
    [InlineData(1, WordForm.Abbreviation, "en-US", "1st")]
    public void OrdinalizeNumberWithSpecifiedCultureAndSpecificForm(int number, WordForm wordForm, string cultureName, string expected)
    {
        var culture = new CultureInfo(cultureName);
        Assert.Equal(expected, number.Ordinalize(culture, wordForm));
        Assert.Equal(expected, number
            .ToString(culture)
            .Ordinalize(culture, wordForm));
    }

    [Theory]
    [InlineData(1, WordForm.Normal, GrammaticalGender.Masculine, "es-ES", "1.º")]
    [InlineData(1, WordForm.Abbreviation, GrammaticalGender.Masculine, "es-ES", "1.er")]
    [InlineData(1, WordForm.Normal, GrammaticalGender.Feminine, "es-ES", "1.ª")]
    [InlineData(1, WordForm.Abbreviation, GrammaticalGender.Feminine, "es-ES", "1.ª")]
    [InlineData(1, WordForm.Normal, GrammaticalGender.Masculine, "en-US", "1st")]
    [InlineData(1, WordForm.Normal, GrammaticalGender.Feminine, "en-US", "1st")]
    public void OrdinalizeNumberWithSpecifiedCultureAndGenderAndForm(
        int number,
        WordForm wordForm,
        GrammaticalGender gender,
        string cultureName,
        string expected)
    {
        var culture = new CultureInfo(cultureName);
        Assert.Equal(expected, number.Ordinalize(gender, culture, wordForm));
        Assert.Equal(expected, number
            .ToString(culture)
            .Ordinalize(gender, culture, wordForm));
    }

    [Theory]
    [InlineData("1", WordForm.Normal, "1st")]
    [InlineData("2", WordForm.Normal, "2nd")]
    [InlineData("3", WordForm.Abbreviation, "3rd")]
    public void OrdinalizeStringWithWordForm(string number, WordForm wordForm, string expected) =>
        Assert.Equal(expected, number.Ordinalize(wordForm));

    [Theory]
    [InlineData(1, WordForm.Normal, "1st")]
    [InlineData(2, WordForm.Normal, "2nd")]
    [InlineData(3, WordForm.Abbreviation, "3rd")]
    public void OrdinalizeNumberWithWordForm(int number, WordForm wordForm, string expected) =>
        Assert.Equal(expected, number.Ordinalize(wordForm));

    [Theory]
    [InlineData("1", GrammaticalGender.Masculine, WordForm.Normal, "1st")]
    [InlineData("1", GrammaticalGender.Feminine, WordForm.Normal, "1st")]
    [InlineData("2", GrammaticalGender.Masculine, WordForm.Abbreviation, "2nd")]
    public void OrdinalizeStringWithGenderAndWordForm(string number, GrammaticalGender gender, WordForm wordForm, string expected) =>
        Assert.Equal(expected, number.Ordinalize(gender, wordForm));

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, WordForm.Normal, "1st")]
    [InlineData(1, GrammaticalGender.Feminine, WordForm.Normal, "1st")]
    [InlineData(2, GrammaticalGender.Masculine, WordForm.Abbreviation, "2nd")]
    public void OrdinalizeNumberWithGenderAndWordForm(int number, GrammaticalGender gender, WordForm wordForm, string expected) =>
        Assert.Equal(expected, number.Ordinalize(gender, wordForm));

    [Fact]
    public void OrdinalizeStringWithNullCultureUsesCurrentUICulture()
    {
        var result = "1".Ordinalize((CultureInfo)null!);
        Assert.Equal("1st", result);
    }

    [Fact]
    public void OrdinalizeStringWithNullCultureAndWordFormUsesCurrentUICulture()
    {
        var result = "1".Ordinalize((CultureInfo)null!, WordForm.Normal);
        Assert.Equal("1st", result);
    }

    [Fact]
    public void OrdinalizeStringWithNullCultureAndGenderUsesCurrentUICulture()
    {
        var result = "1".Ordinalize(GrammaticalGender.Masculine, (CultureInfo)null!);
        Assert.Equal("1st", result);
    }

    [Fact]
    public void OrdinalizeStringWithNullCultureAndGenderAndWordFormUsesCurrentUICulture()
    {
        var result = "1".Ordinalize(GrammaticalGender.Masculine, (CultureInfo)null!, WordForm.Normal);
        Assert.Equal("1st", result);
    }

    [Fact]
    public void OrdinalizeNumberWithNullCultureUsesCurrentUICulture()
    {
        var result = 1.Ordinalize((CultureInfo)null!);
        Assert.Equal("1st", result);
    }

    [Fact]
    public void OrdinalizeNumberWithNullCultureAndWordFormUsesCurrentUICulture()
    {
        var result = 1.Ordinalize((CultureInfo)null!, WordForm.Normal);
        Assert.Equal("1st", result);
    }

    [Fact]
    public void OrdinalizeNumberWithNullCultureAndGenderUsesCurrentUICulture()
    {
        var result = 1.Ordinalize(GrammaticalGender.Masculine, (CultureInfo)null!);
        Assert.Equal("1st", result);
    }

    [Fact]
    public void OrdinalizeNumberWithNullCultureAndGenderAndWordFormUsesCurrentUICulture()
    {
        var result = 1.Ordinalize(GrammaticalGender.Masculine, (CultureInfo)null!, WordForm.Normal);
        Assert.Equal("1st", result);
    }

    [Fact]
    public void NormalizeOrdinalNumberStringPreservesPlainNumbers()
    {
        // No format characters: NormalizeOrdinalNumberString returns original string
        var result = "42".Ordinalize();
        Assert.Equal("42nd", result);
    }

    [Fact]
    public void OrdinalizeNumberWithCultureExercisesNormalizePath()
    {
        // This exercises the culture-aware int.Ordinalize path which calls
        // NormalizeOrdinalNumberString(number.ToString(NumberFormatInfo))
        var culture = new CultureInfo("en-US");
        var result = 42.Ordinalize(culture);
        Assert.Equal("42nd", result);
    }
}