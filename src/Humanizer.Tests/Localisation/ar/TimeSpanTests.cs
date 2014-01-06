using System;
using Humanizer.Tests;
using Xunit;

namespace Humanizer.Tests.Localisation.ar
{
    public class TimeSpanHumanizeExtensionsTests : AmbientCulture
    {
        public TimeSpanHumanizeExtensionsTests() : base("ar") { }

        [Fact]
        public void OneWeek()
        {
            Assert.Equal("أسبوع واحد", TimeSpan.FromDays(7).Humanize());
        }

        [Fact]
        public void OneDay()
        {
            Assert.Equal("يوم واحد", TimeSpan.FromDays(1).Humanize());
        }

        [Fact]
        public void OneHour()
        {
            Assert.Equal("ساعة واحدة", TimeSpan.FromHours(1).Humanize());
        }

        [Fact]
        public void OneMinute()
        {
            Assert.Equal("دقيقة واحدة", TimeSpan.FromMinutes(1).Humanize());
        }

        [Fact]
        public void OneSecond()
        {
            Assert.Equal("ثانية واحدة", TimeSpan.FromSeconds(1).Humanize());
        }

        [Fact]
        public void OneMillisecond()
        {
            Assert.Equal("جزء من الثانية", TimeSpan.FromMilliseconds(1).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("حالاً", TimeSpan.Zero.Humanize());
        }
    }
}