using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.id
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("id-ID") { }

        [Theory]
        [InlineData(14, "2 minggu")]
        [InlineData(7, "1 minggu")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 hari")]
        [InlineData(2, "2 hari")]
        [InlineData(1, "1 hari")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 jam")]
        [InlineData(1, "1 jam")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 menit")]
        [InlineData(1, "1 menit")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(135, "2 menit")]
        [InlineData(60, "1 menit")]
        [InlineData(2, "2 detik")]
        [InlineData(1, "1 detik")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 detik")]
        [InlineData(1400, "1 detik")]
        [InlineData(2, "2 milidetik")]
        [InlineData(1, "1 milidetik")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "waktu kosong")]
        [InlineData(0, 2, "waktu kosong")]
        [InlineData(10, 2, "10 milidetik")]
        [InlineData(1400, 2, "1 detik, 400 milidetik")]
        [InlineData(2500, 2, "2 detik, 500 milidetik")]
        [InlineData(120000, 2, "2 menit")]
        [InlineData(62000, 2, "1 menit, 2 detik")]
        [InlineData(62020, 2, "1 menit, 2 detik")]
        [InlineData(62020, 3, "1 menit, 2 detik, 20 milidetik")]
        [InlineData(3600020, 4, "1 jam, 20 milidetik")]
        [InlineData(3600020, 3, "1 jam, 20 milidetik")]
        [InlineData(3600020, 2, "1 jam, 20 milidetik")]
        [InlineData(3600020, 1, "1 jam")]
        [InlineData(3603001, 2, "1 jam, 3 detik")]
        [InlineData(3603001, 3, "1 jam, 3 detik, 1 milidetik")]
        [InlineData(86400000, 3, "1 hari")]
        [InlineData(86400000, 2, "1 hari")]
        [InlineData(86400000, 1, "1 hari")]
        [InlineData(86401000, 1, "1 hari")]
        [InlineData(86401000, 2, "1 hari, 1 detik")]
        [InlineData(86401200, 2, "1 hari, 1 detik")]
        [InlineData(86401200, 3, "1 hari, 1 detik, 200 milidetik")]
        [InlineData(1296000000, 1, "2 minggu")]
        [InlineData(1296000000, 2, "2 minggu, 1 hari")]
        [InlineData(1299600000, 2, "2 minggu, 1 hari")]
        [InlineData(1299600000, 3, "2 minggu, 1 hari, 1 jam")]
        [InlineData(1299630020, 3, "2 minggu, 1 hari, 1 jam")]
        [InlineData(1299630020, 4, "2 minggu, 1 hari, 1 jam, 30 detik")]
        [InlineData(1299630020, 5, "2 minggu, 1 hari, 1 jam, 30 detik, 20 milidetik")]
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
            Assert.Equal("waktu kosong", actual);
        }
    }
}