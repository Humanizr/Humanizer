namespace id;

[UseCulture("id-ID")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "U")]
    [InlineData(45, "TL")]
    [InlineData(90, "T")]
    [InlineData(135, "TG")]
    [InlineData(180, "S")]
    [InlineData(225, "BD")]
    [InlineData(270, "B")]
    [InlineData(315, "BL")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "utara")]
    [InlineData(45, "timur laut")]
    [InlineData(90, "timur")]
    [InlineData(135, "tenggara")]
    [InlineData(180, "selatan")]
    [InlineData(225, "barat daya")]
    [InlineData(270, "barat")]
    [InlineData(315, "barat laut")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
}
