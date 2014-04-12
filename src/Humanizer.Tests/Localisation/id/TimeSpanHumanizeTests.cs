using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.id
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("id-ID") { }

        [Theory]
        [InlineData(-7, "waktu kosong")]
        [InlineData(7, "1 minggu")]
        [InlineData(21, "3 minggu")]
        [InlineData(367, "52 minggu")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3, "3 hari")]
        [InlineData(8, "1 minggu")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(12, "12 jam")]
        [InlineData(24, "1 hari")]
        [InlineData(25, "1 hari")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 menit")]
        [InlineData(60, "1 jam")]
        [InlineData(120, "2 jam")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 detik")]
        [InlineData(60, "1 menit")]
        [InlineData(150, "2 menit")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 milidetik")]
        [InlineData(2500, "2 detik")]
        [InlineData(65000, "1 menit")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
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