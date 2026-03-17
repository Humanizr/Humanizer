namespace msMY;

[UseCulture("ms-MY")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "kosong")]
    [InlineData(1, "satu")]
    [InlineData(-1, "minus satu")]
    [InlineData(11, "sebelas")]
    [InlineData(21, "dua puluh satu")]
    [InlineData(105, "seratus lima")]
    [InlineData(569, "lima ratus enam puluh sembilan")]
    [InlineData(1000, "seribu")]
    [InlineData(1234, "seribu dua ratus tiga puluh empat")]
    [InlineData(1000000, "satu juta")]
    [InlineData(2147483647, "dua bilion seratus empat puluh tujuh juta empat ratus lapan puluh tiga ribu enam ratus empat puluh tujuh")]
    public void ToWords(long number, string expected) =>
        Assert.Equal(expected, number.ToWords());
}
