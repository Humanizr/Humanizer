using System;
using Xunit;

namespace Humanizer.Tests.Localisation.ar
{
    [UseCulture("ar")]
    public class TimeSpanHumanizeTests
    {
        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "السنة 1")]
        [InlineData(731, "سنتين")]
        [InlineData(1096, "3 سنة")]
        [InlineData(4018, "11 سنة")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "شهر 1")]
        [InlineData(61, "شهرين")]
        [InlineData(92, "3 أشهر")]
        [InlineData(335, "11 أشهر")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "أسبوع واحد")]
        [InlineData(14, "أسبوعين")]
        [InlineData(21, "3 أسابيع")]
        [InlineData(77, "11 أسبوع")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }


        [Theory]
        [InlineData(1, "يوم واحد")]
        [InlineData(2, "يومين")]
        [InlineData(3, "3 أيام")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "ساعة واحدة")]
        [InlineData(2, "ساعتين")]
        [InlineData(3, "3 ساعات")]
        [InlineData(11, "11 ساعة")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "دقيقة واحدة")]
        [InlineData(2, "دقيقتين")]
        [InlineData(3, "3 دقائق")]
        [InlineData(11, "11 دقيقة")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }


        [Theory]
        [InlineData(1, "ثانية واحدة")]
        [InlineData(2, "ثانيتين")]
        [InlineData(3, "3 ثوان")]
        [InlineData(11, "11 ثانية")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "جزء من الثانية")]
        [InlineData(2, "جزئين من الثانية")]
        [InlineData(3, "3 أجزاء من الثانية")]
        [InlineData(11, "11 جزء من الثانية")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 جزء من الثانية", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("حالاً", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
