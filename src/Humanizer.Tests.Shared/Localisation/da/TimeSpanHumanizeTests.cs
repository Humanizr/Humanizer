using System;
using Xunit;

namespace Humanizer.Tests.Localisation.da
{
    [UseCulture("da-DK")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "et år")]
        [InlineData(731, "2 år")]
        [InlineData(1096, "3 år")]
        [InlineData(4018, "11 år")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "en måned")]
        [InlineData(61, "2 måneder")]
        [InlineData(92, "3 måneder")]
        [InlineData(335, "11 måneder")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "en uge")]
        [InlineData(14, "2 uger")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "en dag")]
        [InlineData(2, "2 dage")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "en time")]
        [InlineData(2, "2 timer")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "et minut")]
        [InlineData(2, "2 minutter")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "et sekund")]
        [InlineData(2, "2 sekunder")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "et millisekund")]
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
