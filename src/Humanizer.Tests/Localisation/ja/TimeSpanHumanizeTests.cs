using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ja
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("ja") { }

        [Theory]
        [InlineData(7, "1 週間")]
        [InlineData(14, "2 週間")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 日間")]
        [InlineData(2, "2 日間")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 時間")]
        [InlineData(2, "2 時間")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "1 分間")]
        [InlineData(2, "2 分間")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "1 秒間")]
        [InlineData(2, "2 秒間")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "1 ミリ秒間")]
        [InlineData(2, "2 ミリ秒間")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 秒間", TimeSpan.Zero.Humanize());
        }
    }
}