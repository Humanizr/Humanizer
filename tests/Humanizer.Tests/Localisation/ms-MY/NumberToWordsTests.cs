namespace msMY;

[UseCulture("ms-MY")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "kosong")]
    [InlineData(1, "satu")]
    [InlineData(11, "sebelas")]
    [InlineData(21, "dua puluh satu")]
    [InlineData(100, "seratus")]
    [InlineData(1000, "seribu")]
    [InlineData(1000000, "satu juta")]
    [InlineData(-42, "minus empat puluh dua")]
    public void ToWords(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(1, "pertama")]
    [InlineData(2, "kedua")]
    [InlineData(10, "kesepuluh")]
    public void ToOrdinalWords(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords());
}
