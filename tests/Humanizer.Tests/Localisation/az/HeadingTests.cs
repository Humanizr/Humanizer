namespace az;

[UseCulture("az")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "Ş")]
    [InlineData(22.5, "ŞŞŞ")]
    [InlineData(45, "ŞŞ")]
    [InlineData(67.5, "Ş-ŞŞ")]
    [InlineData(90, "Şər")]
    [InlineData(112.5, "Ş-CŞ")]
    [InlineData(135, "CŞ")]
    [InlineData(157.5, "CCŞ")]
    [InlineData(180, "C")]
    [InlineData(202.5, "CCQ")]
    [InlineData(225, "CQ")]
    [InlineData(247.5, "Q-CQ")]
    [InlineData(270, "Q")]
    [InlineData(292.5, "Q-ŞQ")]
    [InlineData(315, "ŞQ")]
    [InlineData(337.5, "ŞŞQ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "şimal")]
    [InlineData(22.5, "şimal-şimal-şərq")]
    [InlineData(45, "şimal-şərq")]
    [InlineData(67.5, "şərq-şimal-şərq")]
    [InlineData(90, "şərq")]
    [InlineData(112.5, "şərq-cənub-şərq")]
    [InlineData(135, "cənub-şərq")]
    [InlineData(157.5, "cənub-cənub-şərq")]
    [InlineData(180, "cənub")]
    [InlineData(202.5, "cənub-cənub-qərb")]
    [InlineData(225, "cənub-qərb")]
    [InlineData(247.5, "qərb-cənub-qərb")]
    [InlineData(270, "qərb")]
    [InlineData(292.5, "qərb-şimal-qərb")]
    [InlineData(315, "şimal-qərb")]
    [InlineData(337.5, "şimal-şimal-qərb")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("Ş", 0)]
    [InlineData("ŞŞŞ", 22.5)]
    [InlineData("ŞŞ", 45)]
    [InlineData("Ş-ŞŞ", 67.5)]
    [InlineData("Şər", 90)]
    [InlineData("Ş-CŞ", 112.5)]
    [InlineData("CŞ", 135)]
    [InlineData("CCŞ", 157.5)]
    [InlineData("C", 180)]
    [InlineData("CCQ", 202.5)]
    [InlineData("CQ", 225)]
    [InlineData("Q-CQ", 247.5)]
    [InlineData("Q", 270)]
    [InlineData("Q-ŞQ", 292.5)]
    [InlineData("ŞQ", 315)]
    [InlineData("ŞŞQ", 337.5)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}
