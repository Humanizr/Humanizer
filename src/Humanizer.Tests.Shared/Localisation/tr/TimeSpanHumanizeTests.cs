using System;
using Xunit;

namespace Humanizer.Tests.Localisation.tr
{
    [UseCulture("tr")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 yıl")]
        [InlineData(731, "2 yıl")]
        [InlineData(1096, "3 yıl")]
        [InlineData(4018, "11 yıl")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 ay")]
        [InlineData(61, "2 ay")]
        [InlineData(92, "3 ay")]
        [InlineData(335, "11 ay")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(14, "2 hafta")]
        [InlineData(7, "1 hafta")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 gün")]
        [InlineData(2, "2 gün")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 saat")]
        [InlineData(1, "1 saat")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 dakika")]
        [InlineData(1, "1 dakika")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 saniye")]
        [InlineData(1, "1 saniye")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 milisaniye")]
        [InlineData(1, "1 milisaniye")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("0 milisaniye", actual);
        }

        [Fact]
        public void NoTimeToWords()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize(toWords: true);
            Assert.Equal("zaman farkı yok", actual);
        }
    }
}
