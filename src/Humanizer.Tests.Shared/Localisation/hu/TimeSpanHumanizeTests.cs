using System;
using Xunit;

namespace Humanizer.Tests.Localisation.hu
{
    [UseCulture("hu-HU")]
    public class TimeSpanHumanizeTests 
    {

        [Theory]
        [InlineData(14, "2 hét")]
        [InlineData(7, "1 hét")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(2, "2 nap")]
        [InlineData(1, "1 nap")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(2, "2 óra")]
        [InlineData(1, "1 óra")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(2, "2 perc")]
        [InlineData(1, "1 perc")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(2, "2 másodperc")]
        [InlineData(1, "1 másodperc")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("nincs idő", actual);
        }
    }
}
