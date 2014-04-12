using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.bnBD
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("bn-BD") { }

        [Theory]
        [InlineData(7, "এক সপ্তাহ")]
        [InlineData(14, "2 সপ্তাহ")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "এক দিন")]
        [InlineData(2, "2 দিন")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "এক ঘণ্টা")]
        [InlineData(2, "2 ঘণ্টা")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "এক মিনিট")]
        [InlineData(2, "2 মিনিট")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "এক সেকেন্ড")]
        [InlineData(2, "2 সেকেন্ড")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "এক মিলিসেকেন্ড")]
        [InlineData(2, "2 মিলিসেকেন্ড")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            // This one really doesn't make a lot of sense but again... w/e
            Assert.Equal("শুন্য সময়", TimeSpan.Zero.Humanize());
        }
    }
}
        public TimeSpanHumanizeTests() : base("fa") { }

        [Theory]
        [InlineData(7, "یک هفته")]
        [InlineData(77, "11 هفته")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "یک روز")]
        [InlineData(3, "3 روز")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "یک ساعت")]
        [InlineData(11, "11 ساعت")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "یک دقیقه")]
        [InlineData(11, "11 دقیقه")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "یک ثانیه")]
        [InlineData(11, "11 ثانیه")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "یک میلی ثانیه")]
        [InlineData(11, "11 میلی ثانیه")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("الآن", TimeSpan.Zero.Humanize());
        }
    }
}