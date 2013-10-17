using System;
using Xunit;

namespace Humanizer.Tests
{
    public class TimeSpanHumanizeExtensionsTests
    {
        [Fact]
        public void TwoWeeks()
        {
            var twoWeeks = TimeSpan.FromDays(14);
            var actual = twoWeeks.Humanize();
            Assert.Equal("2 weeks", actual);
        }

        [Fact]
        public void OneWeek()
        {
            var oneWeek = TimeSpan.FromDays(7);
            var actual = oneWeek.Humanize();
            Assert.Equal("1 week", actual);
        }

        [Fact]
        public void SixDays()
        {
            var sixDays = TimeSpan.FromDays(6);
            var actual = sixDays.Humanize();
            Assert.Equal("6 days", actual);
        }

        [Fact]
        public void TwoDays()
        {
            var twoDays = TimeSpan.FromDays(2);
            var actual = twoDays.Humanize();
            Assert.Equal("2 days", actual);
        }

        [Fact]
        public void OneDay()
        {
            var oneDay = TimeSpan.FromDays(1);
            var actual = oneDay.Humanize();
            Assert.Equal("1 day", actual);
        }

        [Fact]
        public void TwoHours()
        {
            var twoHours = TimeSpan.FromHours(2);
            var actual = twoHours.Humanize();
            Assert.Equal("2 hours", actual);
        }

        [Fact]
        public void OneHour()
        {
            var oneHour = TimeSpan.FromHours(1);
            var actual = oneHour.Humanize();
            Assert.Equal("1 hour", actual);
        }

        [Fact]
        public void TwoMinutes()
        {
            var twoMinutes = TimeSpan.FromMinutes(2);
            var actual = twoMinutes.Humanize();
            Assert.Equal("2 minutes", actual);
        }

        [Fact]
        public void OneMinute()
        {
            var oneMinute = TimeSpan.FromMinutes(1);
            var actual = oneMinute.Humanize();
            Assert.Equal("1 minute", actual);
        }

        [Fact]
        public void TwoSeconds()
        {
            var twoSeconds = TimeSpan.FromSeconds(2);
            var actual = twoSeconds.Humanize();
            Assert.Equal("2 seconds", actual);
        }

        [Fact]
        public void OneSecond()
        {
            var oneSecond = TimeSpan.FromSeconds(1);
            var actual = oneSecond.Humanize();
            Assert.Equal("1 second", actual);
        }

        [Fact]
        public void TwoMilliseconds()
        {
            var twoMilliseconds = TimeSpan.FromMilliseconds(2);
            var actual = twoMilliseconds.Humanize();
            Assert.Equal("2 milliseconds", actual);
        }

        [Fact]
        public void OneMillisecond()
        {
            var oneMillisecond = TimeSpan.FromMilliseconds(1);
            var actual = oneMillisecond.Humanize();
            Assert.Equal("1 millisecond", actual);
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
