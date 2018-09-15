using System;
using Xunit;

namespace Humanizer.Tests.Localisation.hu
{
    [UseCulture("hu-HU")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "egy év")]
        [InlineData(731, "2 év")]
        [InlineData(1096, "3 év")]
        [InlineData(4018, "11 év")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "egy hónap")]
        [InlineData(61, "2 hónap")]
        [InlineData(92, "3 hónap")]
        [InlineData(335, "11 hónap")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(14, "2 hét")]
        [InlineData(7, "1 hét")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(2, "2 nap")]
        [InlineData(1, "1 nap")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(2, "2 óra")]
        [InlineData(1, "1 óra")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(2, "2 perc")]
        [InlineData(1, "1 perc")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(2, "2 másodperc")]
        [InlineData(1, "1 másodperc")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("0 ezredmásodperc", actual);
        }

        [Fact]
        public void NoTimeToWords()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize(toWords: true);
            Assert.Equal("nincs idő", actual);
        }
    }
}
