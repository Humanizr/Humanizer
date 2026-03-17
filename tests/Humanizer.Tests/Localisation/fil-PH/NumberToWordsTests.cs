namespace filPH;

[UseCulture("fil-PH")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "sero")]
    [InlineData(1, "isa")]
    [InlineData(-3, "minus tatlo")]
    [InlineData(11, "labing-isa")]
    [InlineData(21, "dalawampu't isa")]
    [InlineData(105, "isang daan at lima")]
    [InlineData(569, "limang daan at animnapu't siyam")]
    [InlineData(1000, "isang libo")]
    [InlineData(1234, "isang libo dalawang daan at tatlumpu't apat")]
    [InlineData(2000000, "dalawang milyon")]
[InlineData(2147483647, "dalawang bilyon isang daan at apatnapu't pitong milyon apat na daan at walumpu't tatlong libo anim na daan at apatnapu't pito")]
    public void ToWords(long number, string expected) =>
        Assert.Equal(expected, number.ToWords());
}
