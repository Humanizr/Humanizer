using System;
using Xunit;

namespace Humanizer.Tests.Localisation.zhHant
{
    [UseCulture("zh-Hant")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "1 年")]
        [InlineData(731, "2 年")]
        [InlineData(1096, "3 年")]
        [InlineData(4018, "11 年")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "1 個月")]
        [InlineData(61, "2 個月")]
        [InlineData(92, "3 個月")]
        [InlineData(335, "11 個月")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "1 周")]
        [InlineData(14, "2 周")]
        [InlineData(21, "3 周")]
        [InlineData(77, "11 周")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }


        [Theory]
        [InlineData(1, "1 天")]
        [InlineData(2, "2 天")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 小時")]
        [InlineData(2, "2 小時")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "1 分")]
        [InlineData(2, "2 分")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }


        [Theory]
        [InlineData(1, "1 秒")]
        [InlineData(2, "2 秒")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "1 毫秒")]
        [InlineData(2, "2 毫秒")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 毫秒", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("沒有時間", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
