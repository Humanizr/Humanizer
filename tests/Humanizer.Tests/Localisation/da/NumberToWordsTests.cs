namespace da;

[UseCulture("da-DK")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "nul")]
    [InlineData(1, "en")]
    [InlineData(11, "elleve")]
    [InlineData(21, "enogtyve")]
    [InlineData(58, "otteoghalvtreds")]
    [InlineData(100, "et hundrede")]
    [InlineData(105, "et hundrede fem")]
    [InlineData(1000, "et tusind")]
    [InlineData(2001, "to tusind en")]
    [InlineData(1000000, "en million")]
    [InlineData(-42, "minus toogfyrre")]
    public void ToWords(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(0, "nulte")]
    [InlineData(1, "første")]
    [InlineData(2, "anden")]
    [InlineData(3, "tredje")]
    [InlineData(10, "tiende")]
    public void ToOrdinalWords(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords());
}
