namespace bnBD;

[UseCulture("bn-BD")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "উ")]
    [InlineData(45, "উ-পূ")]
    [InlineData(90, "পূ")]
    [InlineData(135, "দ-পূ")]
    [InlineData(180, "দ")]
    [InlineData(225, "দ-প")]
    [InlineData(270, "প")]
    [InlineData(315, "উ-প")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "উত্তর")]
    [InlineData(45, "উত্তর-পূর্ব")]
    [InlineData(90, "পূর্ব")]
    [InlineData(135, "দক্ষিণ-পূর্ব")]
    [InlineData(180, "দক্ষিণ")]
    [InlineData(225, "দক্ষিণ-পশ্চিম")]
    [InlineData(270, "পশ্চিম")]
    [InlineData(315, "উত্তর-পশ্চিম")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("উ", 0)]
    [InlineData("উ-পূ", 45)]
    [InlineData("পূ", 90)]
    [InlineData("দ-পূ", 135)]
    [InlineData("দ", 180)]
    [InlineData("দ-প", 225)]
    [InlineData("প", 270)]
    [InlineData("উ-প", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}
