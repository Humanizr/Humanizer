using System;
using Xunit;

namespace Humanizer.Tests.Localisation.filPH
{
    [UseCulture("fil-PH")]
    public class TimeSpanHumanizeTests
    {
        [Theory]
        [Trait("Translation", "Google Translate")]
        [InlineData(366, "1 taon")]
        [InlineData(731, "2 taon")]
        [InlineData(1096, "3 taon")]
        [InlineData(4018, "11 taon")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google Translate")]
        [InlineData(31, "1 buwan")]
        [InlineData(61, "2 buwan")]
        [InlineData(92, "3 buwan")]
        [InlineData(335, "11 buwan")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google Translate")]
        [InlineData(7, "1 linggo")]
        [InlineData(14, "2 linggo")]
        [InlineData(21, "3 linggo")]
        [InlineData(77, "11 linggo")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google Translate")]
        [InlineData(1, "1 araw")]
        [InlineData(2, "2 araw")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google Translate")]
        [InlineData(1, "1 oras")]
        [InlineData(2, "2 oras")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google Translate")]
        [InlineData(1, "1 minuto")]
        [InlineData(2, "2 minuto")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google Translate")]
        [InlineData(1, "1 segundo")]
        [InlineData(2, "2 segundo")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google Translate")]
        [InlineData(1, "1 millisecond")]
        [InlineData(2, "2 milliseconds")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 milliseconds", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("walang oras", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
