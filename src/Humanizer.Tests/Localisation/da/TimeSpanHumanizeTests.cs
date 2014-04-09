using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.da
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("da") { }

        [Theory]
        [InlineData(7, "en uge")]
        [InlineData(14, "2 uger")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "en dag")]
        [InlineData(2, "2 dage")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "en time")]
        [InlineData(2, "2 timer")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "et minut")]
        [InlineData(2, "2 minutter")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "et sekund")]
        [InlineData(2, "2 sekunder")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "et millisekund")]
        [InlineData(2, "2 millisekunder")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Theory]
        [InlineData(0, 3, "ingen tid")]
        [InlineData(0, 2, "ingen tid")]
        [InlineData(10, 2, "10 millisekunder")]
        [InlineData(1400, 2, "1 sekund, 400 millisekunder")]
        [InlineData(2500, 2, "2 sekunder, 500 millisekunder")]
        [InlineData(120000, 2, "2 minutter")]
        [InlineData(62000, 2, "1 minute, 2 sekunder")]
        [InlineData(62020, 2, "1 minute, 2 sekunder")]
        [InlineData(62020, 3, "1 minute, 2 sekunder, 20 millisekunder")]
        [InlineData(3600020, 4, "1 time, 20 millisekunder")]
        [InlineData(3600020, 3, "1 time, 20 millisekunder")]
        [InlineData(3600020, 2, "1 time, 20 millisekunder")]
        [InlineData(3600020, 1, "1 time")]
        [InlineData(3603001, 2, "1 time, 3 sekunder")]
        [InlineData(3603001, 3, "1 time, 3 sekunder, 1 millisekund")]
        [InlineData(86400000, 3, "1 dag")]
        [InlineData(86400000, 2, "1 dag")]
        [InlineData(86400000, 1, "1 dag")]
        [InlineData(86401000, 1, "1 dag")]
        [InlineData(86401000, 2, "1 dag, 1 sekund")]
        [InlineData(86401200, 2, "1 dag, 1 sekund")]
        [InlineData(86401200, 3, "1 dag, 1 sekund, 200 millisekunder")]
        [InlineData(1296000000, 1, "2 weeks")]
        [InlineData(1296000000, 2, "2 weeks, 1 dag")]
        [InlineData(1299600000, 2, "2 weeks, 1 dag")]
        [InlineData(1299600000, 3, "2 weeks, 1 dag, 1 time")]
        [InlineData(1299630020, 3, "2 weeks, 1 dag, 1 time")]
        [InlineData(1299630020, 4, "2 weeks, 1 dag, 1 time, 30 sekunder")]
        [InlineData(1299630020, 5, "2 weeks, 1 dag, 1 time, 30 sekunder, 20 millisekunder")]
        public void TimeSpanWithPrecesion(int milliseconds, int precesion, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precesion);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("ingen tid", TimeSpan.Zero.Humanize());
        }
    }
}