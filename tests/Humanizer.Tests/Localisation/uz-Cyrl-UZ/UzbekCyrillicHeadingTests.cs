namespace Humanizer.Tests.Localisation.uzCyrl;

[UseCulture("uz-Cyrl-UZ")]
public class UzbekCyrillicHeadingTests
{
    static readonly CultureInfo UzCyrl = new("uz-Cyrl-UZ");

    [Theory]
    [InlineData(0, "Ш")]
    [InlineData(45, "ШШ")]
    [InlineData(90, "Шарқ")]
    [InlineData(135, "ЖШ")]
    [InlineData(180, "Ж")]
    [InlineData(225, "ЖҒ")]
    [InlineData(270, "Ғ")]
    [InlineData(315, "ШҒ")]
    public void ToHeading_Abbreviated_UsesCyrillicLetters(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Abbreviated, UzCyrl));
}