using System;
using Xunit;

namespace Humanizer.Tests.Localisation.fa
{
    [UseCulture("fa")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "یک سال")]
        [InlineData(731, "2 سال")]
        [InlineData(1096, "3 سال")]
        [InlineData(4018, "11 سال")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "یک ماه")]
        [InlineData(61, "2 ماه")]
        [InlineData(92, "3 ماه")]
        [InlineData(335, "11 ماه")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

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
            Assert.Equal("0 میلی ثانیه", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("الآن", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
