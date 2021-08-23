using System.Globalization;

using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class HeadingTests
    {
        [InlineData(0, "N")]
        [InlineData(11.2, "N")]
        [InlineData(11.3, "NNE")]
        [InlineData(22.5, "NNE")]
        [InlineData(33.7, "NNE")]
        [InlineData(33.8, "NE")]
        [InlineData(45, "NE")]
        [InlineData(56.2, "NE")]
        [InlineData(56.3, "ENE")]
        [InlineData(67.5, "ENE")]
        [InlineData(78.7, "ENE")]
        [InlineData(78.8, "E")]
        [InlineData(90, "E")]
        [InlineData(101.2, "E")]
        [InlineData(101.3, "ESE")]
        [InlineData(112.5, "ESE")]
        [InlineData(123.7, "ESE")]
        [InlineData(123.8, "SE")]
        [InlineData(135, "SE")]
        [InlineData(146.2, "SE")]
        [InlineData(146.3, "SSE")]
        [InlineData(157.5, "SSE")]
        [InlineData(168.7, "SSE")]
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

        [InlineData(0, "north")]
        [InlineData(22.5, "north-northeast")]
        [InlineData(45, "northeast")]
        [InlineData(67.5, "east-northeast")]
        [InlineData(90, "east")]
        [InlineData(112.5, "east-southeast")]
        [InlineData(135, "southeast")]
        [InlineData(157.5, "south-southeast")]
        [InlineData(180, "south")]
        [InlineData(202.5, "south-southwest")]
        [InlineData(225, "southwest")]
        [InlineData(247.5, "west-southwest")]
        [InlineData(270, "west")]
        [InlineData(292.5, "west-northwest")]
        [InlineData(315, "northwest")]
        [InlineData(337.5, "north-northwest")]
        [InlineData(720, "north")]
        [Theory]
        public void ToHeading(double heading, string expected)
        {
            Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
        }

        [InlineData("N", 0)]
        [InlineData("NNE", 22.5)]
        [InlineData("NE", 45)]
        [InlineData("ENE", 67.5)]
        [InlineData("E", 90)]
        [InlineData("ESE", 112.5)]
        [InlineData("SE", 135)]
        [InlineData("SSE", 157.5)]
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

        [InlineData(0, '↑')]
        [InlineData(11.2, '↑')]
        [InlineData(11.3, '↑')]
        [InlineData(22.5, '↗')]
        [InlineData(33.7, '↗')]
        [InlineData(33.8, '↗')]
        [InlineData(45, '↗')]
        [InlineData(56.2, '↗')]
        [InlineData(56.3, '↗')]
        [InlineData(67.5, '→')]
        [InlineData(78.7, '→')]
        [InlineData(78.8, '→')]
        [InlineData(90, '→')]
        [InlineData(101.2, '→')]
        [InlineData(101.3, '→')]
        [InlineData(112.5, '↘')]
        [InlineData(123.7, '↘')]
        [InlineData(123.8, '↘')]
        [InlineData(135, '↘')]
        [InlineData(146.2, '↘')]
        [InlineData(146.3, '↘')]
        [InlineData(157.5, '↓')]
        [InlineData(168.7, '↓')]
        [InlineData(168.8, '↓')]
        [InlineData(180, '↓')]
        [InlineData(191.2, '↓')]
        [InlineData(191.3, '↓')]
        [InlineData(202.5, '↙')]
        [InlineData(213.7, '↙')]
        [InlineData(213.8, '↙')]
        [InlineData(225, '↙')]
        [InlineData(236.2, '↙')]
        [InlineData(236.3, '↙')]
        [InlineData(247.5, '←')]
        [InlineData(258.7, '←')]
        [InlineData(258.8, '←')]
        [InlineData(270, '←')]
        [InlineData(281.2, '←')]
        [InlineData(281.3, '←')]
        [InlineData(292.5, '↖')]
        [InlineData(303.7, '↖')]
        [InlineData(303.8, '↖')]
        [InlineData(315, '↖')]
        [InlineData(326.2, '↖')]
        [InlineData(326.3, '↖')]
        [InlineData(337.5, '↑')]
        [InlineData(348.7, '↑')]
        [InlineData(348.8, '↑')]
        [Theory]
        public void ToHeadingArrow(double heading, char expected)
        {
            Assert.Equal(expected, heading.ToHeadingArrow());
        }

        [InlineData('↑', 0)]
        [InlineData('↗', 45)]
        [InlineData('→', 90)]
        [InlineData('↘', 135)]
        [InlineData('↓', 180)]
        [InlineData('↙', 225)]
        [InlineData('←', 270)]
        [InlineData('↖', 315)]
        [InlineData('\n', -1)]
        [Theory]
        public void FromHeadingArrow(char heading, double expected)
        {
            Assert.Equal(expected, heading.FromHeadingArrow());
        }

        [InlineData("↑", 0)]
        [InlineData("↗", 45)]
        [InlineData("→", 90)]
        [InlineData("↘", 135)]
        [InlineData("↓", 180)]
        [InlineData("↙", 225)]
        [InlineData("←", 270)]
        [InlineData("↖", 315)]
        [InlineData("", -1)]
        [InlineData("xyz", -1)]
        [Theory]
        public void FromHeadingArrow_Also_Works_With_Strings(string heading, double expected)
        {
            Assert.Equal(expected, heading.FromHeadingArrow());
        }

        [InlineData("NNW", "en-US", 337.5)]
        [InlineData("ØNØ", "da", 67.5)]
        [InlineData("O", "de-DE", 90.0)]
        [Theory]
        public void FromShortHeading_CanSpecifyCultureExplicitly(string heading, string culture, double expected)
        {
            Assert.Equal(expected, heading.FromAbbreviatedHeading(new CultureInfo(culture)));
        }
    }
}
