using System;
using Xunit;

namespace Humanizer.Tests.Localisation.sr
{
    [UseCulture("sr")]
    public class TimeSpanHumanizeTests 
    {

        [Theory]
        [InlineData(35, "5 недеља")]
        [InlineData(14, "2 недеље")]
        [InlineData(7, "1 недеља")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 дана")]
        [InlineData(2, "2 дана")]
        [InlineData(1, "1 дан")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5, "5 сати")]
        [InlineData(2, "2 сата")]
        [InlineData(1, "1 сат")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 минута")]
        [InlineData(1, "1 минут")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 секунде")]
        [InlineData(1, "1 секунда")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 милисекунде")]
        [InlineData(1, "1 милисекунда")]
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
            Assert.Equal("без протеклог времена", actual);
        }
    }
}
