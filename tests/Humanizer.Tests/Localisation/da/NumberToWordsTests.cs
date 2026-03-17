namespace da;

[UseCulture("da")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "nul")]
    [InlineData(1, "en")]
    [InlineData(11, "elleve")]
    [InlineData(21, "enogtyve")]
    [InlineData(40, "fyrre")]
    [InlineData(55, "femoghalvtreds")]
    [InlineData(73, "treoghalvfjerds")]
    [InlineData(99, "nioghalvfems")]
    [InlineData(100, "et hundrede")]
    [InlineData(101, "et hundrede og en")]
    [InlineData(151, "et hundrede og enoghalvtreds")]
    [InlineData(262, "to hundrede og toogtres")]
    [InlineData(373, "tre hundrede og treoghalvfjerds")]
    [InlineData(484, "fire hundrede og fireogfirs")]
    [InlineData(595, "fem hundrede og femoghalvfems")]
    [InlineData(1000, "et tusind")]
    [InlineData(1001, "et tusind og en")]
    [InlineData(2000, "to tusind")]
    [InlineData(1000000, "en million")]
    [InlineData(2000000, "to millioner")]
    [InlineData(1000000000, "en milliard")]
    [InlineData(-55, "minus femoghalvtreds")]
    public void ToWords(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());
}
