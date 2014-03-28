using System;
using Xunit;

namespace Humanizer.Tests.Localisation.nl
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("nl-NL") { }

        [Fact]
        public void TwoWeeks()
        {
            Assert.Equal("2 weken", TimeSpan.FromDays(14).Humanize());
        }

        [Fact]
        public void OneWeek()
        {
            Assert.Equal("één week", TimeSpan.FromDays(7).Humanize());
        }

        [Fact]
        public void SixDays()
        {
            Assert.Equal("6 dagen", TimeSpan.FromDays(6).Humanize());
        }

        [Fact]
        public void TwoDays()
        {
            Assert.Equal("2 dagen", TimeSpan.FromDays(2).Humanize());
        }

        [Fact]
        public void OneDay()
        {
            Assert.Equal("één dag", TimeSpan.FromDays(1).Humanize());
        }

        [Fact]
        public void TwoHours()
        {
            Assert.Equal("2 uur", TimeSpan.FromHours(2).Humanize());
        }

        [Fact]
        public void OneHour()
        {
            Assert.Equal("één uur", TimeSpan.FromHours(1).Humanize());
        }

        [Fact]
        public void TwoMinutes()
        {
            Assert.Equal("2 minuten", TimeSpan.FromMinutes(2).Humanize());
        }

        [Fact]
        public void OneMinute()
        {
            Assert.Equal("één minuut", TimeSpan.FromMinutes(1).Humanize());
        }

        [Fact]
        public void TwoSeconds()
        {
            Assert.Equal("2 seconden", TimeSpan.FromSeconds(2).Humanize());
        }

        [Fact]
        public void OneSecond()
        {
            Assert.Equal("één seconde", TimeSpan.FromSeconds(1).Humanize());
        }

        [Fact]
        public void TwoMilliseconds()
        {
            Assert.Equal("2 milliseconden", TimeSpan.FromMilliseconds(2).Humanize());
        }

        [Fact]
        public void OneMillisecond()
        {
            Assert.Equal("één milliseconde", TimeSpan.FromMilliseconds(1).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("geen tijd", TimeSpan.Zero.Humanize());
        }
    }
}