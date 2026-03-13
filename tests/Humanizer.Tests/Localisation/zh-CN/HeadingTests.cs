namespace zhCN;

[UseCulture("zh-CN")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "北")]
    [InlineData(90, "东")]
    [InlineData(180, "南")]
    [InlineData(270, "西")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());
}
