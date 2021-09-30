using System;
using Xunit;

namespace Humanizer.Tests.Localisation.msMY
{
    [UseCulture("ms-MY")]
    public class TimeSpanHumanizeTests
    {
        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 tahun")]
        [InlineData(731, "2 tahun")]
        [InlineData(1096, "3 tahun")]
        [InlineData(4018, "11 tahun")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 bulan")]
        [InlineData(61, "2 bulan")]
        [InlineData(92, "3 bulan")]
        [InlineData(335, "11 bulan")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(7, "1 minggu")]
        [InlineData(14, "2 minggu")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 hari")]
        [InlineData(2, "2 hari")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 jam")]
        [InlineData(2, "2 jam")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 minit")]
        [InlineData(2, "2 minit")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 saat")]
        [InlineData(2, "2 saat")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1 milisaat")]
        [InlineData(2, "2 milisaat")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        [Trait("Translation", "Google")]
        public void NoTime()
        {
            Assert.Equal("0 milisaat", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("tiada masa", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
