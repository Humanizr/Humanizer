namespace filPH;

[UseCulture("fil-PH")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "sero")]
    [InlineData(1, "isa")]
    [InlineData(10, "sampu")]
    [InlineData(11, "labing-isa")]
    [InlineData(21, "dalawampu't isa")]
    [InlineData(58, "limampu't walo")]
    [InlineData(100, "isang daan")]
    [InlineData(105, "isang daan at lima")]
    [InlineData(1000, "isang libo")]
    [InlineData(1001, "isang libo't isa")]
    [InlineData(1000000, "isang milyon")]
    [InlineData(-42, "minus apatnapu't dalawa")]
    public void ToWords(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(1, "una")]
    [InlineData(2, "ikalawa")]
    [InlineData(10, "ikasampu")]
    public void ToOrdinalWords(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords());

    [Theory]
    [InlineData("fil", 21, "dalawampu't isa")]
    [InlineData("fil-PH", 21, "dalawampu't isa")]
    public void Supports_exact_and_specific_locale_names(string cultureName, int number, string expected) =>
        Assert.Equal(expected, number.ToWords(new CultureInfo(cultureName)));
}
