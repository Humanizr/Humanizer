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
    [InlineData(105, "isang daan lima")]
    [InlineData(1000, "isang libo")]
    [InlineData(1000000, "isang milyon")]
    [InlineData(-42, "minus apatnapu't dalawa")]
    public void ToWords(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(1, "una")]
    [InlineData(2, "ika-dalawa")]
    [InlineData(10, "ika-sampu")]
    public void ToOrdinalWords(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords());
}
