namespace zhHant;

[UseCulture("zh-Hant")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "北")]
    [InlineData(90, "東")]
    [InlineData(180, "南")]
    [InlineData(270, "西")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());
}
