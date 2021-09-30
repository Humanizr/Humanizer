using Xunit;

namespace Humanizer.Tests.Localisation.@is
{
    [UseCulture("is")]
    public class HeadingTests
    {
        [InlineData(0, "N")]
        [InlineData(11.2, "N")]
        [InlineData(11.3, "NNA")]
        [InlineData(22.5, "NNA")]
        [InlineData(33.7, "NNA")]
        [InlineData(33.8, "NA")]
        [InlineData(45, "NA")]
        [InlineData(56.2, "NA")]
        [InlineData(56.3, "ANA")]
        [InlineData(67.5, "ANA")]
        [InlineData(78.7, "ANA")]
        [InlineData(78.8, "A")]
        [InlineData(90, "A")]
        [InlineData(101.2, "A")]
        [InlineData(101.3, "ASA")]
        [InlineData(112.5, "ASA")]
        [InlineData(123.7, "ASA")]
        [InlineData(123.8, "SA")]
        [InlineData(135, "SA")]
        [InlineData(146.2, "SA")]
        [InlineData(146.3, "SSA")]
        [InlineData(157.5, "SSA")]
        [InlineData(168.7, "SSA")]
        [InlineData(168.8, "S")]
        [InlineData(180, "S")]
        [InlineData(191.2, "S")]
        [InlineData(191.3, "SSV")]
        [InlineData(202.5, "SSV")]
        [InlineData(213.7, "SSV")]
        [InlineData(213.8, "SV")]
        [InlineData(225, "SV")]
        [InlineData(236.2, "SV")]
        [InlineData(236.3, "VSV")]
        [InlineData(247.5, "VSV")]
        [InlineData(258.7, "VSV")]
        [InlineData(258.8, "V")]
        [InlineData(270, "V")]
        [InlineData(281.2, "V")]
        [InlineData(281.3, "VNV")]
        [InlineData(292.5, "VNV")]
        [InlineData(303.7, "VNV")]
        [InlineData(303.8, "NV")]
        [InlineData(315, "NV")]
        [InlineData(326.2, "NV")]
        [InlineData(326.3, "NNV")]
        [InlineData(337.5, "NNV")]
        [InlineData(348.7, "NNV")]
        [InlineData(348.8, "N")]
        [InlineData(720, "N")]
        [Theory]
        public void ToHeadingAbbreviated(double heading, string expected)
        {
            Assert.Equal(expected, heading.ToHeading());
        }

        [InlineData(0, "norður")]
        [InlineData(45, "norðaustur")]
        [InlineData(67.5, "austnorðaustur")]
        [InlineData(90, "austur")]
        [InlineData(112.5, "austsuðaustur")]
        [InlineData(135, "suðaustur")]
        [InlineData(157.5, "suðsuðaustur")]
        [InlineData(180, "suður")]
        [InlineData(202.5, "suðsuðvestur")]
        [InlineData(225, "suðvestur")]
        [InlineData(247.5, "vestsuðvestur")]
        [InlineData(270, "vestur")]
        [InlineData(292.5, "vestnorðvestur")]
        [InlineData(315, "norðvestur")]
        [InlineData(337.5, "norðnorðvestur")]
        [InlineData(720, "norður")]
        [Theory]
        public void ToHeading(double heading, string expected)
        {
            Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
        }
    }
}
