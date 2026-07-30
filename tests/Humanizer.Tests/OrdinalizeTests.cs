[UseCulture("en-US")]
public class OrdinalizeTests
{
    public static IEnumerable<object[]> ShippedLocaleRows =>
        Humanizer.Tests.Localisation.LocaleCoverageData.ShippedLocales
            .Select(static localeName => new object[] { localeName });

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
    [InlineData(2_147_483_651L, "2147483651st")]
    [InlineData(-2_147_483_651L, "-2147483651st")]
    [InlineData(long.MinValue, "-9223372036854775808th")]
    public void OrdinalizeLongNumber(long number, string ordinalized) =>
        Assert.Equal(number.Ordinalize(), ordinalized);

    [Fact]
    public void OrdinalizeLongNumberUsesCultureSpecificRules()
    {
        const long number = 4_294_967_297;

        Assert.Equal("4294967297ème", number.Ordinalize(new CultureInfo("fr-FR")));
        Assert.Equal(
            "2147483651.er",
            2_147_483_651L.Ordinalize(
                GrammaticalGender.Masculine,
                new CultureInfo("es-ES"),
                WordForm.Abbreviation));
    }

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void OrdinalizeLongNumberMatchesIntOverloadForEverySupportedLocale(string localeName)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(21.Ordinalize(culture), 21L.Ordinalize(culture));
    }

    [Fact]
    public void OrdinalizeLongMinValueUsesCultureSpecificRules()
    {
        Assert.Equal(
            "9223372036854775808.º",
            long.MinValue.Ordinalize(new CultureInfo("es-ES"), WordForm.Normal));
        Assert.Equal(
            "9223372036854775808è",
            long.MinValue.Ordinalize(new CultureInfo("ca-ES")));
    }

    [Fact]
    public void OrdinalizeLongNumberUsesNumberWordSuffixFallback()
    {
        const long number = 2_147_483_648;
        var culture = new CultureInfo("hi-IN");

        Assert.Equal(
            number.ToWords(GrammaticalGender.Masculine, culture) + "वाँ",
            number.Ordinalize(GrammaticalGender.Masculine, culture));
    }

    [Fact]
    public void OrdinalizeLongOverloads()
    {
        const long number = 2_147_483_651;
        var culture = new CultureInfo("en-US");

        Assert.Equal("2147483651st", number.Ordinalize());
        Assert.Equal("2147483651st", number.Ordinalize(WordForm.Normal));
        Assert.Equal("2147483651st", number.Ordinalize(culture));
        Assert.Equal("2147483651st", number.Ordinalize(culture, WordForm.Normal));
        Assert.Equal("2147483651st", number.Ordinalize(GrammaticalGender.Masculine));
        Assert.Equal("2147483651st", number.Ordinalize(GrammaticalGender.Masculine, WordForm.Normal));
        Assert.Equal("2147483651st", number.Ordinalize(GrammaticalGender.Masculine, culture));
        Assert.Equal("2147483651st", number.Ordinalize(GrammaticalGender.Masculine, culture, WordForm.Normal));
    }

    [Fact]
    public void OrdinalizeStringRemainsIntBounded() =>
        Assert.Throws<OverflowException>(() => "2147483648".Ordinalize());

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

    [Fact]
    public void OrdinalizeNegativeNumberWithCustomCultureNegativeSign()
    {
        var culture = (CultureInfo)CultureInfo.GetCultureInfo("en-US").Clone();
        culture.NumberFormat.NegativeSign = "!";

        Assert.Equal("!1st", (-1).Ordinalize(culture));
    }

    [Fact]
    public void OrdinalizeNegativeNumberWithCustomCultureNegativePatternMatchesStringOverload()
    {
        var culture = (CultureInfo)CultureInfo.GetCultureInfo("en-US").Clone();
        culture.NumberFormat.NumberNegativePattern = 3;

        Assert.Equal((-1).ToString(culture).Ordinalize(culture), (-1).Ordinalize(culture));
    }

    [Theory]
    [InlineData("ar-SA", 42)]
    [InlineData("ar-SA", -42)]
    public void OrdinalizeNumberWithSpecifiedCultureMatchesStringOverload(string cultureName, int number)
    {
        var culture = new CultureInfo(cultureName);

        Assert.Equal(number.ToString(culture).Ordinalize(culture), number.Ordinalize(culture));
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
}