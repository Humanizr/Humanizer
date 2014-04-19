using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.tr
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("tr") { }

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
        [InlineData(1, "1 gün")]
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
        [InlineData(135, "2 dakika")]
        [InlineData(60, "1 dakika")]
        [InlineData(2, "2 saniye")]
        [InlineData(1, "1 saniye")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 saniye")]
        [InlineData(1400, "1 saniye")]
        [InlineData(2, "2 milisaniye")]
        [InlineData(1, "1 milisaniye")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "zaman farkı yok")]
        [InlineData(0, 2, "zaman farkı yok")]
        [InlineData(10, 2, "10 milisaniye")]
        [InlineData(1400, 2, "1 saniye, 400 milisaniye")]
        [InlineData(2500, 2, "2 saniye, 500 milisaniye")]
        [InlineData(120000, 2, "2 dakika")]
        [InlineData(62000, 2, "1 dakika, 2 saniye")]
        [InlineData(62020, 2, "1 dakika, 2 saniye")]
        [InlineData(62020, 3, "1 dakika, 2 saniye, 20 milisaniye")]
        [InlineData(3600020, 4, "1 saat, 20 milisaniye")]
        [InlineData(3600020, 3, "1 saat, 20 milisaniye")]
        [InlineData(3600020, 2, "1 saat, 20 milisaniye")]
        [InlineData(3600020, 1, "1 saat")]
        [InlineData(3603001, 2, "1 saat, 3 saniye")]
        [InlineData(3603001, 3, "1 saat, 3 saniye, 1 milisaniye")]
        [InlineData(86400000, 3, "1 gün")]
        [InlineData(86400000, 2, "1 gün")]
        [InlineData(86400000, 1, "1 gün")]
        [InlineData(86401000, 1, "1 gün")]
        [InlineData(86401000, 2, "1 gün, 1 saniye")]
        [InlineData(86401200, 2, "1 gün, 1 saniye")]
        [InlineData(86401200, 3, "1 gün, 1 saniye, 200 milisaniye")]
        [InlineData(1296000000, 1, "2 hafta")]
        [InlineData(1296000000, 2, "2 hafta, 1 gün")]
        [InlineData(1299600000, 2, "2 hafta, 1 gün")]
        [InlineData(1299600000, 3, "2 hafta, 1 gün, 1 saat")]
        [InlineData(1299630020, 3, "2 hafta, 1 gün, 1 saat")]
        [InlineData(1299630020, 4, "2 hafta, 1 gün, 1 saat, 30 saniye")]
        [InlineData(1299630020, 5, "2 hafta, 1 gün, 1 saat, 30 saniye, 20 milisaniye")]
        public void TimeSpanWithPrecesion(int milliseconds, int precesion, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precesion);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("zaman farkı yok", actual);
        }
    }
}
