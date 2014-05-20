using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.vi
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("vi") { }

        [Theory]
        [InlineData(14, "2 tuần")]
        [InlineData(7, "1 tuần")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 ngày")]
        [InlineData(2, "2 ngày")]
        [InlineData(1, "1 ngày")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 giờ")]
        [InlineData(1, "1 giờ")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 phút")]
        [InlineData(1, "1 phút")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(135, "2 phút")]
        [InlineData(60, "1 phút")]
        [InlineData(2, "2 giây")]
        [InlineData(1, "1 giây")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 giây")]
        [InlineData(1400, "1 giây")]
        [InlineData(2, "2 phần ngàn giây")]
        [InlineData(1, "1 phần ngàn giây")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "không giờ")]
        [InlineData(0, 2, "không giờ")]
        [InlineData(10, 2, "10 phần ngàn giây")]
        [InlineData(1400, 2, "1 giây, 400 phần ngàn giây")]
        [InlineData(2500, 2, "2 giây, 500 phần ngàn giây")]
        [InlineData(120000, 2, "2 phút")]
        [InlineData(62000, 2, "1 phút, 2 giây")]
        [InlineData(62020, 2, "1 phút, 2 giây")]
        [InlineData(62020, 3, "1 phút, 2 giây, 20 phần ngàn giây")]
        [InlineData(3600020, 4, "1 giờ, 20 phần ngàn giây")]
        [InlineData(3600020, 3, "1 giờ, 20 phần ngàn giây")]
        [InlineData(3600020, 2, "1 giờ, 20 phần ngàn giây")]
        [InlineData(3600020, 1, "1 giờ")]
        [InlineData(3603001, 2, "1 giờ, 3 giây")]
        [InlineData(3603001, 3, "1 giờ, 3 giây, 1 phần ngàn giây")]
        [InlineData(86400000, 3, "1 ngày")]
        [InlineData(86400000, 2, "1 ngày")]
        [InlineData(86400000, 1, "1 ngày")]
        [InlineData(86401000, 1, "1 ngày")]
        [InlineData(86401000, 2, "1 ngày, 1 giây")]
        [InlineData(86401200, 2, "1 ngày, 1 giây")]
        [InlineData(86401200, 3, "1 ngày, 1 giây, 200 phần ngàn giây")]
        [InlineData(1296000000, 1, "2 tuần")]
        [InlineData(1296000000, 2, "2 tuần, 1 ngày")]
        [InlineData(1299600000, 2, "2 tuần, 1 ngày")]
        [InlineData(1299600000, 3, "2 tuần, 1 ngày, 1 giờ")]
        [InlineData(1299630020, 3, "2 tuần, 1 ngày, 1 giờ")]
        [InlineData(1299630020, 4, "2 tuần, 1 ngày, 1 giờ, 30 giây")]
        [InlineData(1299630020, 5, "2 tuần, 1 ngày, 1 giờ, 30 giây, 20 phần ngàn giây")]
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
            Assert.Equal("không giờ", actual);
        }
    }
}
