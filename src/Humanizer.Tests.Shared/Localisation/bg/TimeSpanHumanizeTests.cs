using System;
using Xunit;

namespace Humanizer.Tests.Localisation.bg
{
    [UseCulture("bg-BG")]
    public class TimeSpanHumanizeTests
    {
        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "една година")]
        [InlineData(731, "2 години")]
        [InlineData(1096, "3 години")]
        [InlineData(4018, "11 години")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "един месец")]
        [InlineData(61, "2 месеца")]
        [InlineData(92, "3 месеца")]
        [InlineData(335, "11 месеца")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "една седмица")]
        [InlineData(14, "2 седмици")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "един ден")]
        [InlineData(2, "2 дена")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "един час")]
        [InlineData(2, "2 часа")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "една минута")]
        [InlineData(2, "2 минути")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "една секунда")]
        [InlineData(2, "2 секунди")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "една милисекунда")]
        [InlineData(2, "2 милисекунди")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("0 милисекунди", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("няма време", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
