using System;
using Xunit;

namespace Humanizer.Tests.Localisation.lv
{
    [UseCulture("lv")]
    public class TimeSpanHumanizeTests
    {
        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 gads")]
        [InlineData(731, "2 gadi")]
        [InlineData(1096, "3 gadi")]
        [InlineData(4018, "11 gadi")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 mēnesis")]
        [InlineData(61, "2 mēneši")]
        [InlineData(92, "3 mēneši")]
        [InlineData(335, "11 mēneši")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(7, "1 nedēļa")]
        [InlineData(14, "2 nedēļas")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 diena")]
        [InlineData(2, "2 dienas")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 stunda")]
        [InlineData(2, "2 stundas")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 minūte")]
        [InlineData(2, "2 minūtes")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 sekunde")]
        [InlineData(2, "2 sekundes")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 milisekunde")]
        [InlineData(2, "2 milisekundes")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        [Trait("Translation", "Google")]
        public void NoTime()
        {
            Assert.Equal("0 milisekundes", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("bez laika", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
