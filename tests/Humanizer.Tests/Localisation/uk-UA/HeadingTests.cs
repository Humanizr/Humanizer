namespace ukUA;

[UseCulture("uk-UA")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "Пн")]
    [InlineData(22.5, "ПнПнСх")]
    [InlineData(45, "ПнСх")]
    [InlineData(67.5, "СхПнСх")]
    [InlineData(90, "Сх")]
    [InlineData(112.5, "СхПдСх")]
    [InlineData(135, "ПдСх")]
    [InlineData(157.5, "ПдПдСх")]
    [InlineData(180, "Пд")]
    [InlineData(202.5, "ПдПдЗх")]
    [InlineData(225, "ПдЗх")]
    [InlineData(247.5, "ЗхПдЗх")]
    [InlineData(270, "Зх")]
    [InlineData(292.5, "ЗхПнЗх")]
    [InlineData(315, "ПнЗх")]
    [InlineData(337.5, "ПнПнЗх")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "північ")]
    [InlineData(22.5, "північ-північний схід")]
    [InlineData(45, "північний схід")]
    [InlineData(67.5, "схід-північний схід")]
    [InlineData(90, "схід")]
    [InlineData(112.5, "схід-південний схід")]
    [InlineData(135, "південний схід")]
    [InlineData(157.5, "південь-південний схід")]
    [InlineData(180, "південь")]
    [InlineData(202.5, "південь-південний захід")]
    [InlineData(225, "південний захід")]
    [InlineData(247.5, "захід-південний захід")]
    [InlineData(270, "захід")]
    [InlineData(292.5, "захід-північний захід")]
    [InlineData(315, "північний захід")]
    [InlineData(337.5, "північ-північний захід")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("Пн", 0)]
    [InlineData("ПнПнСх", 22.5)]
    [InlineData("ПнСх", 45)]
    [InlineData("СхПнСх", 67.5)]
    [InlineData("Сх", 90)]
    [InlineData("СхПдСх", 112.5)]
    [InlineData("ПдСх", 135)]
    [InlineData("ПдПдСх", 157.5)]
    [InlineData("Пд", 180)]
    [InlineData("ПдПдЗх", 202.5)]
    [InlineData("ПдЗх", 225)]
    [InlineData("ЗхПдЗх", 247.5)]
    [InlineData("Зх", 270)]
    [InlineData("ЗхПнЗх", 292.5)]
    [InlineData("ПнЗх", 315)]
    [InlineData("ПнПнЗх", 337.5)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}
