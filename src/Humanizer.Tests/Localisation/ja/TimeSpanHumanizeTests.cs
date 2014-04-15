using System;
using Xunit;

namespace Humanizer.Tests.Localisation.ja
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("ja") { }

        [Fact]
        public void TwoWeeks()
        {
            Assert.Equal("2 週間", TimeSpan.FromDays(14).Humanize());
        }

        [Fact]
        public void OneWeek()
        {
            Assert.Equal("1 週間", TimeSpan.FromDays(7).Humanize());
        }

        [Fact]
        public void SixDays()
        {
            Assert.Equal("6 日間", TimeSpan.FromDays(6).Humanize());
        }

        [Fact]
        public void TwoDays()
        {
            Assert.Equal("2 日間", TimeSpan.FromDays(2).Humanize());
        }

        [Fact]
        public void OneDay()
        {
            Assert.Equal("1 日間", TimeSpan.FromDays(1).Humanize());
        }

        [Fact]
        public void TwoHours()
        {
            Assert.Equal("2 時間", TimeSpan.FromHours(2).Humanize());
        }

        [Fact]
        public void OneHour()
        {
            Assert.Equal("1 時間", TimeSpan.FromHours(1).Humanize());
        }

        [Fact]
        public void TwoMinutes()
        {
            Assert.Equal("2 分間", TimeSpan.FromMinutes(2).Humanize());
        }

        [Fact]
        public void OneMinute()
        {
            Assert.Equal("1 分間", TimeSpan.FromMinutes(1).Humanize());
        }

        [Fact]
        public void TwoSeconds()
        {
            Assert.Equal("2 秒間", TimeSpan.FromSeconds(2).Humanize());
        }

        [Fact]
        public void OneSecond()
        {
            Assert.Equal("1 秒間", TimeSpan.FromSeconds(1).Humanize());
        }

        [Fact]
        public void TwoMilliseconds()
        {
            Assert.Equal("2 ミリ秒間", TimeSpan.FromMilliseconds(2).Humanize());
        }

        [Fact]
        public void OneMillisecond()
        {
            Assert.Equal("1 ミリ秒間", TimeSpan.FromMilliseconds(1).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("時間なし", TimeSpan.Zero.Humanize());
        }
    }
}