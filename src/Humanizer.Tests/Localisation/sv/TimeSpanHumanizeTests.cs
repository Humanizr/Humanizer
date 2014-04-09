using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.sv
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests()
            : base("sv-SE")
        {
        }

        [Theory]
        [InlineData(1, "1 millisekund")]
        [InlineData(2, "2 millisekunder")]
        [InlineData(3, "3 millisekunder")]
        [InlineData(4, "4 millisekunder")]
        [InlineData(5, "5 millisekunder")]
        [InlineData(6, "6 millisekunder")]
        [InlineData(10, "10 millisekunder")]
        public void Milliseconds(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 sekund")]
        [InlineData(2, "2 sekunder")]
        [InlineData(3, "3 sekunder")]
        [InlineData(4, "4 sekunder")]
        [InlineData(5, "5 sekunder")]
        [InlineData(6, "6 sekunder")]
        [InlineData(10, "10 sekunder")]
        public void Seconds(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minut")]
        [InlineData(2, "2 minuter")]
        [InlineData(3, "3 minuter")]
        [InlineData(4, "4 minuter")]
        [InlineData(5, "5 minuter")]
        [InlineData(6, "6 minuter")]
        [InlineData(10, "10 minuter")]
        public void Minutes(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 timma")]
        [InlineData(2, "2 timmar")]
        [InlineData(3, "3 timmar")]
        [InlineData(4, "4 timmar")]
        [InlineData(5, "5 timmar")]
        [InlineData(6, "6 timmar")]
        [InlineData(10, "10 timmar")]
        public void Hours(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 dag")]
        [InlineData(2, "2 dagar")]
        [InlineData(3, "3 dagar")]
        [InlineData(4, "4 dagar")]
        [InlineData(5, "5 dagar")]
        [InlineData(6, "6 dagar")]
        public void Days(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 vecka")]
        [InlineData(2, "2 veckor")]
        [InlineData(3, "3 veckor")]
        [InlineData(4, "4 veckor")]
        [InlineData(5, "5 veckor")]
        [InlineData(6, "6 veckor")]
        public void Weeks(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number*7).Humanize());
        }
    }
}
