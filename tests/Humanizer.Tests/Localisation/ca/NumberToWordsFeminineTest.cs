namespace ca;

[UseCulture("ca")]
public class NumberToWordsFeminineTests
{
    [Theory]
    [InlineData(1, "una")]
    [InlineData(21, "vint-i-una")]
    [InlineData(31, "trenta-una")]
    [InlineData(81, "vuitanta-una")]
    [InlineData(500, "cinc-centes")]
    [InlineData(701, "set-centes una")]
    [InlineData(3500, "tres mil cinc-centes")]
    [InlineData(200121, "dues-centes mil cent vint-i-una")]
    [InlineData(200000121, "dos-cents milions cent vint-i-una")]
    [InlineData(1000001, "un milió una")]
    public void ToWords(int number, string expected) =>
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
}