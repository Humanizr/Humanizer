namespace sk;

[UseCulture("sk")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "nula")]
    [InlineData(1, "jeden")]
    [InlineData(2, "dva")]
    [InlineData(5, "päť")]
    [InlineData(11, "jedenásť")]
    [InlineData(19, "devätnásť")]
    [InlineData(21, "dvadsať jeden")]
    [InlineData(34, "tridsať štyri")]
    [InlineData(47, "štyridsať sedem")]
    [InlineData(99, "deväťdesiat deväť")]
    [InlineData(100, "sto")]
    [InlineData(101, "sto jeden")]
    [InlineData(200, "dvesto")]
    [InlineData(305, "tristo päť")]
    [InlineData(999, "deväťsto deväťdesiat deväť")]
    [InlineData(1000, "tisíc")]
    [InlineData(2000, "dva tisíce")]
    [InlineData(5000, "päť tisíc")]
    [InlineData(1000000, "jeden milión")]
    [InlineData(2000000, "dva milióny")]
    [InlineData(5000000, "päť miliónov")]
    [InlineData(1000000000, "jedna miliarda")]
    [InlineData(2147483647, "dve miliardy sto štyridsať sedem miliónov štyristo osemdesiat tri tisíc šesťsto štyridsať sedem")]
    [InlineData(-99, "mínus deväťdesiat deväť")]
    public void ToWords(long number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(1, "jedna")]
    [InlineData(2, "dve")]
    [InlineData(21, "dvadsať jedna")]
    public void ToWordsFeminine(int number, string expected) =>
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));

    [Theory]
    [InlineData(1, "jedno")]
    [InlineData(2, "dva")]
    [InlineData(21, "dvadsať jedno")]
    public void ToWordsNeuter(int number, string expected) =>
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Neuter));
}
