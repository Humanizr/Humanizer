using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("en-US") { }

        [Theory]
        [InlineData(14, "2 weeks")]
        [InlineData(7, "1 week")]
        public void Weeks(int days, string expected)
        {
            var twoWeeks = TimeSpan.FromDays(days);
            var actual = twoWeeks.Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 days")]
        [InlineData(2, "2 days")]
        [InlineData(1, "1 day")]
        public void Days(int days, string expected)
        {
            var sixDays = TimeSpan.FromDays(days);
            var actual = sixDays.Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 hours")]
        [InlineData(1, "1 hour")]
        public void Hours(int hours, string expected)
        {
            var twoHours = TimeSpan.FromHours(hours);
            var actual = twoHours.Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 minutes")]
        [InlineData(1, "1 minute")]
        public void Minutes(int minutes, string expected)
        {
            var twoMinutes = TimeSpan.FromMinutes(minutes);
            var actual = twoMinutes.Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 seconds")]
        [InlineData(1, "1 second")]
        public void Seconds(int seconds, string expected)
        {
            var twoSeconds = TimeSpan.FromSeconds(seconds);
            var actual = twoSeconds.Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 milliseconds")]
        [InlineData(1, "1 millisecond")]
        public void Milliseconds(int ms, string expected)
        {
            var twoMilliseconds = TimeSpan.FromMilliseconds(ms);
            var actual = twoMilliseconds.Humanize();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("no time", actual);
        }
    }
}
