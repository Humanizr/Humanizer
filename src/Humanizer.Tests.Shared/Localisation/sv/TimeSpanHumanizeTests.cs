using System;
using Xunit;

namespace Humanizer.Tests.Localisation.sv
{
    [UseCulture("sv-SE")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [InlineData(1, "1 millisekund")]
        [InlineData(2, "2 millisekunder")]
        public void Milliseconds(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 sekund")]
        [InlineData(2, "2 sekunder")]
        public void Seconds(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minut")]
        [InlineData(2, "2 minuter")]
        public void Minutes(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 timma")]
        [InlineData(2, "2 timmar")]
        public void Hours(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 dag")]
        [InlineData(2, "2 dagar")]
        public void Days(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 vecka")]
        [InlineData(2, "2 veckor")]
        public void Weeks(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number * 7).Humanize());
        }
    }
}
