using Xunit;

namespace Humanizer.Tests.Localisation.de
{
    [UseCulture("de-DE")]
    public class HeadingTests
    {
        [InlineData(0, "N")]
        [InlineData(11.2, "N")]
        [InlineData(11.3, "NNO")]
        [InlineData(22.5, "NNO")]
        [InlineData(33.7, "NNO")]
        [InlineData(33.8, "NO")]
        [InlineData(45, "NO")]
        [InlineData(56.2, "NO")]
        [InlineData(56.3, "ONO")]
        [InlineData(67.5, "ONO")]
        [InlineData(78.7, "ONO")]
        [InlineData(78.8, "O")]
        [InlineData(90, "O")]
        [InlineData(101.2, "O")]
        [InlineData(101.3, "OSO")]
        [InlineData(112.5, "OSO")]
        [InlineData(123.7, "OSO")]
        [InlineData(123.8, "SO")]
        [InlineData(135, "SO")]
        [InlineData(146.2, "SO")]
        [InlineData(146.3, "SSO")]
        [InlineData(157.5, "SSO")]
        [InlineData(168.7, "SSO")]
        [InlineData(168.8, "S")]
        [InlineData(180, "S")]
        [InlineData(191.2, "S")]
        [InlineData(191.3, "SSW")]
        [InlineData(202.5, "SSW")]
        [InlineData(213.7, "SSW")]
        [InlineData(213.8, "SW")]
        [InlineData(225, "SW")]
        [InlineData(236.2, "SW")]
        [InlineData(236.3, "WSW")]
        [InlineData(247.5, "WSW")]
        [InlineData(258.7, "WSW")]
        [InlineData(258.8, "W")]
        [InlineData(270, "W")]
        [InlineData(281.2, "W")]
        [InlineData(281.3, "WNW")]
        [InlineData(292.5, "WNW")]
        [InlineData(303.7, "WNW")]
        [InlineData(303.8, "NW")]
        [InlineData(315, "NW")]
        [InlineData(326.2, "NW")]
        [InlineData(326.3, "NNW")]
        [InlineData(337.5, "NNW")]
        [InlineData(348.7, "NNW")]
        [InlineData(348.8, "N")]
        [InlineData(720, "N")]
        [Theory]
        public void ToHeadingAbbreviated(double heading, string expected)
        {
            Assert.Equal(expected, heading.ToHeading());
        }

        [InlineData(0, "Nord")]
        [InlineData(22.5, "Nordnordost")]
        [InlineData(45, "Nordost")]
        [InlineData(67.5, "Ostnordost")]
        [InlineData(90, "Ost")]
        [InlineData(112.5, "Ostsüdost")]
        [InlineData(135, "Südost")]
        [InlineData(157.5, "Südsüdost")]
        [InlineData(180, "Süd")]
        [InlineData(202.5, "Südsüdwest")]
        [InlineData(225, "Südwest")]
        [InlineData(247.5, "Westsüdwest")]
        [InlineData(270, "West")]
        [InlineData(292.5, "Westnordwest")]
        [InlineData(315, "Nordwest")]
        [InlineData(337.5, "Nordnordwest")]
        [InlineData(720, "Nord")]
        [Theory]
        public void ToHeading(double heading, string expected)
        {
            Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
        }

        [InlineData("N", 0)]
        [InlineData("NNO", 22.5)]
        [InlineData("NO", 45)]
        [InlineData("ONO", 67.5)]
        [InlineData("O", 90)]
        [InlineData("OSO", 112.5)]
        [InlineData("SO", 135)]
        [InlineData("SSO", 157.5)]
        [InlineData("S", 180)]
        [InlineData("SSW", 202.5)]
        [InlineData("SW", 225)]
        [InlineData("WSW", 247.5)]
        [InlineData("W", 270)]
        [InlineData("WNW", 292.5)]
        [InlineData("NW", 315)]
        [InlineData("NNW", 337.5)]
        [Theory]
        public void FromShortHeading(string heading, double expected)
        {
            Assert.Equal(expected, heading.FromAbbreviatedHeading());
        }
    }
}
