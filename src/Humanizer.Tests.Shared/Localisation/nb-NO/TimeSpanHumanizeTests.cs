using System;
using Xunit;

namespace Humanizer.Tests.Localisation.nbNO
{
    [UseCulture("nb-NO")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "1 år")]
        [InlineData(731, "2 år")]
        [InlineData(1096, "3 år")]
        [InlineData(4018, "11 år")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "1 måned")]
        [InlineData(61, "2 måneder")]
        [InlineData(92, "3 måneder")]
        [InlineData(335, "11 måneder")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "1 uke")]
        [InlineData(14, "2 uker")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 dag")]
        [InlineData(2, "2 dager")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 time")]
        [InlineData(2, "2 timer")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minutt")]
        [InlineData(2, "2 minutter")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "1 sekund")]
        [InlineData(2, "2 sekunder")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "1 millisekund")]
        [InlineData(2, "2 millisekunder")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 millisekunder", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("ingen tid", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
