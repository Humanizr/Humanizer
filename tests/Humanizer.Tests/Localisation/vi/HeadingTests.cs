namespace vi;

[UseCulture("vi")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "B")]
    [InlineData(45, "ĐB")]
    [InlineData(90, "Đ")]
    [InlineData(135, "ĐN")]
    [InlineData(180, "N")]
    [InlineData(225, "TN")]
    [InlineData(270, "T")]
    [InlineData(315, "TB")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "bắc")]
    [InlineData(45, "đông bắc")]
    [InlineData(90, "đông")]
    [InlineData(135, "đông nam")]
    [InlineData(180, "nam")]
    [InlineData(225, "tây nam")]
    [InlineData(270, "tây")]
    [InlineData(315, "tây bắc")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("B", 0)]
    [InlineData("ĐB", 45)]
    [InlineData("Đ", 90)]
    [InlineData("ĐN", 135)]
    [InlineData("N", 180)]
    [InlineData("TN", 225)]
    [InlineData("T", 270)]
    [InlineData("TB", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}
