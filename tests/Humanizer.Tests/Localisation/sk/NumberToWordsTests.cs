namespace sk;

[UseCulture("sk-SK")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "nula")]
    [InlineData(1, "jeden")]
    [InlineData(2, "dva")]
    [InlineData(5, "päť")]
    [InlineData(11, "jedenásť")]
    [InlineData(21, "dvadsať jeden")]
    [InlineData(42, "štyridsať dva")]
    [InlineData(100, "sto")]
    [InlineData(105, "sto päť")]
    [InlineData(1000, "tisíc")]
    [InlineData(2000, "dva tisíce")]
    [InlineData(5000, "päť tisíc")]
    [InlineData(1000000, "jeden milión")]
    [InlineData(-42, "mínus štyridsať dva")]
    public void ToWords(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(1, "1")]
    [InlineData(21, "21")]
    public void ToOrdinalWords(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords());
}
