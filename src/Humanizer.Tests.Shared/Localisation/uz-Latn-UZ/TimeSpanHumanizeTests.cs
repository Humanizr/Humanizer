using System;
using Xunit;

namespace Humanizer.Tests.Localisation.uzLatn
{
    [UseCulture("uz-Latn-UZ")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 yil")]
        [InlineData(731, "2 yil")]
        [InlineData(1096, "3 yil")]
        [InlineData(4018, "11 yil")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 oy")]
        [InlineData(61, "2 oy")]
        [InlineData(92, "3 oy")]
        [InlineData(335, "11 oy")]
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
        [InlineData(6, "6 kun")]
        [InlineData(2, "2 kun")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 soat")]
        [InlineData(1, "1 soat")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 minut")]
        [InlineData(1, "1 minut")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 sekund")]
        [InlineData(1, "1 sekund")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 millisekund")]
        [InlineData(1, "1 millisekund")]
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
            Assert.Equal("0 millisekund", actual);
        }

        [Fact]
        public void NoTimeToWords()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize(toWords: true);
            Assert.Equal("vaqt yo`q", actual);
        }
    }
}
