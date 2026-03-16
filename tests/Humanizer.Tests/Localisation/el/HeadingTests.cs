namespace el;

[UseCulture("el")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "Β")]
    [InlineData(45, "ΒΑ")]
    [InlineData(90, "Α")]
    [InlineData(135, "ΝΑ")]
    [InlineData(180, "Ν")]
    [InlineData(225, "ΝΔ")]
    [InlineData(270, "Δ")]
    [InlineData(315, "ΒΔ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "βορράς")]
    [InlineData(45, "βορειοανατολικά")]
    [InlineData(90, "ανατολή")]
    [InlineData(135, "νοτιοανατολικά")]
    [InlineData(180, "νότος")]
    [InlineData(225, "νοτιοδυτικά")]
    [InlineData(270, "δύση")]
    [InlineData(315, "βορειοδυτικά")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("Β", 0)]
    [InlineData("ΒΑ", 45)]
    [InlineData("Α", 90)]
    [InlineData("ΝΑ", 135)]
    [InlineData("Ν", 180)]
    [InlineData("ΝΔ", 225)]
    [InlineData("Δ", 270)]
    [InlineData("ΒΔ", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}
