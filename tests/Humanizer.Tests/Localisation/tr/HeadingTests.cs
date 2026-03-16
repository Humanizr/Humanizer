namespace tr;

[UseCulture("tr")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "K")]
    [InlineData(45, "KD")]
    [InlineData(90, "D")]
    [InlineData(135, "GD")]
    [InlineData(180, "G")]
    [InlineData(225, "GB")]
    [InlineData(270, "B")]
    [InlineData(315, "KB")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "kuzey")]
    [InlineData(45, "kuzeydoğu")]
    [InlineData(90, "doğu")]
    [InlineData(135, "güneydoğu")]
    [InlineData(180, "güney")]
    [InlineData(225, "güneybatı")]
    [InlineData(270, "batı")]
    [InlineData(315, "kuzeybatı")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("K", 0)]
    [InlineData("KD", 45)]
    [InlineData("D", 90)]
    [InlineData("GD", 135)]
    [InlineData("G", 180)]
    [InlineData("GB", 225)]
    [InlineData("B", 270)]
    [InlineData("KB", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading(new("tr")));
}
