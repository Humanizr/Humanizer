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
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 days")]
        [InlineData(2, "2 days")]
        [InlineData(1, "1 day")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 hours")]
        [InlineData(1, "1 hour")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 minutes")]
        [InlineData(1, "1 minute")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(135, "2 minutes")]
        [InlineData(60, "1 minute")]
        [InlineData(2, "2 seconds")]
        [InlineData(1, "1 second")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 seconds")]
        [InlineData(1400, "1 second")]
        [InlineData(2, "2 milliseconds")]
        [InlineData(1, "1 millisecond")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "no time")]
        [InlineData(0, 2, "no time")]
        [InlineData(10, 2, "10 milliseconds")]
        [InlineData(1400, 2, "1 second, 400 milliseconds")]
        [InlineData(2500, 2, "2 seconds, 500 milliseconds")]
        [InlineData(120000, 2, "2 minutes")]
        [InlineData(62000, 2, "1 minute, 2 seconds")]
        [InlineData(62020, 2, "1 minute, 2 seconds")]
        [InlineData(62020, 3, "1 minute, 2 seconds, 20 milliseconds")]
        [InlineData(3600020, 4, "1 hour, 20 milliseconds")]
        [InlineData(3600020, 3, "1 hour, 20 milliseconds")]
        [InlineData(3600020, 2, "1 hour, 20 milliseconds")]
        [InlineData(3600020, 1, "1 hour")]
        [InlineData(3603001, 2, "1 hour, 3 seconds")]
        [InlineData(3603001, 3, "1 hour, 3 seconds, 1 millisecond")]
        [InlineData(86400000, 3, "1 day")]
        [InlineData(86400000, 2, "1 day")]
        [InlineData(86400000, 1, "1 day")]
        [InlineData(86401000, 1, "1 day")]
        [InlineData(86401000, 2, "1 day, 1 second")]
        [InlineData(86401200, 2, "1 day, 1 second")]
        [InlineData(86401200, 3, "1 day, 1 second, 200 milliseconds")]
        [InlineData(1296000000, 1, "2 weeks")]
        [InlineData(1296000000, 2, "2 weeks, 1 day")]
        [InlineData(1299600000, 2, "2 weeks, 1 day")]
        [InlineData(1299600000, 3, "2 weeks, 1 day, 1 hour")]
        [InlineData(1299630020, 3, "2 weeks, 1 day, 1 hour")]
        [InlineData(1299630020, 4, "2 weeks, 1 day, 1 hour, 30 seconds")]
        [InlineData(1299630020, 5, "2 weeks, 1 day, 1 hour, 30 seconds, 20 milliseconds")]
        public void TimeSpanWithPrecesion(int milliseconds, int precesion, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precesion);
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
