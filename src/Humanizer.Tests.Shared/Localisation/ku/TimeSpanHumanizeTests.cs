using System;
using Xunit;

namespace Humanizer.Tests.Localisation.ku
{
    [UseCulture("ku")]
    public class TimeSpanHumanizeTests
    {
        [Theory(Skip = "ku not supported on VS 2022")]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "1 ساڵ")]
        [InlineData(731, "2 ساڵ")]
        [InlineData(1096, "3 ساڵ")]
        [InlineData(4018, "11 ساڵ")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory(Skip = "ku not supported on VS 2022")]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "1 مانگ")]
        [InlineData(61, "2 مانگ")]
        [InlineData(92, "3 مانگ")]
        [InlineData(335, "11 مانگ")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory(Skip = "ku not supported on VS 2022")]
        [InlineData(7, "1 هەفتە")]
        [InlineData(77, "11 هەفتە")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory(Skip = "ku not supported on VS 2022")]
        [InlineData(1, "1 ڕۆژ")]
        [InlineData(3, "3 ڕۆژ")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory(Skip = "ku not supported on VS 2022")]
        [InlineData(1, "1 کاتژمێر")]
        [InlineData(11, "11 کاتژمێر")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory(Skip = "ku not supported on VS 2022")]
        [InlineData(1, "1 خولەک")]
        [InlineData(11, "11 خولەک")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory(Skip = "ku not supported on VS 2022")]
        [InlineData(1, "1 چرکە")]
        [InlineData(11, "11 چرکە")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory(Skip = "ku not supported on VS 2022")]
        [InlineData(1, "1 میلیچرکە")]
        [InlineData(11, "11 میلیچرکە")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact(Skip = "ku not supported on VS 2022")]
        public void NoTime()
        {
            Assert.Equal("0 میلیچرکە", TimeSpan.Zero.Humanize());
        }

        [Fact(Skip = "ku not supported on VS 2022")]
        public void NoTimeToWords()
        {
            Assert.Equal("ئێستا", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
