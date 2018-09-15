using System;
using Xunit;

namespace Humanizer.Tests.Localisation.vi
{
    [UseCulture("vi")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 năm")]
        [InlineData(731, "2 năm")]
        [InlineData(1096, "3 năm")]
        [InlineData(4018, "11 năm")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 tháng")]
        [InlineData(61, "2 tháng")]
        [InlineData(92, "3 tháng")]
        [InlineData(335, "11 tháng")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(14, "2 tuần")]
        [InlineData(7, "1 tuần")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(2, "2 giây")]
        [InlineData(1, "1 giây")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 phần ngàn giây")]
        [InlineData(1, "1 phần ngàn giây")]
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
            Assert.Equal("0 phần ngàn giây", actual);
        }

        [Fact]
        public void NoTimeToWords()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize(toWords: true);
            Assert.Equal("không giờ", actual);
        }
    }
}
