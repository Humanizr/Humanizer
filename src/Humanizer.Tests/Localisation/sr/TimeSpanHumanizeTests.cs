using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.srCyrl
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("sr") { }

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
        [InlineData(135, "2 минута")]
        [InlineData(60, "1 минут")]
        [InlineData(5, "5 секунди")]
        [InlineData(3, "3 секунде")]
        [InlineData(2, "2 секунде")]
        [InlineData(1, "1 секунда")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 секунде")]
        [InlineData(1400, "1 секунда")]
        [InlineData(2, "2 милисекунде")]
        [InlineData(1, "1 милисекунда")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "без протеклог времена")]
        [InlineData(0, 2, "без протеклог времена")]
        [InlineData(10, 2, "10 милисекунди")]
        [InlineData(1400, 2, "1 секунда, 400 милисекунди")]
        [InlineData(2500, 2, "2 секунде, 500 милисекунди")]
        [InlineData(120000, 2, "2 минута")]
        [InlineData(62000, 2, "1 минут, 2 секунде")]
        [InlineData(62020, 2, "1 минут, 2 секунде")]
        [InlineData(62020, 3, "1 минут, 2 секунде, 20 милисекунди")]
        [InlineData(3600020, 4, "1 сат, 20 милисекунди")]
        [InlineData(3600020, 3, "1 сат, 20 милисекунди")]
        [InlineData(3600020, 2, "1 сат, 20 милисекунди")]
        [InlineData(3600020, 1, "1 сат")]
        [InlineData(3603001, 2, "1 сат, 3 секунде")]
        [InlineData(3603001, 3, "1 сат, 3 секунде, 1 милисекунда")]
        [InlineData(86400000, 3, "1 дан")]
        [InlineData(86400000, 2, "1 дан")]
        [InlineData(86400000, 1, "1 дан")]
        [InlineData(86401000, 1, "1 дан")]
        [InlineData(86401000, 2, "1 дан, 1 секунда")]
        [InlineData(86401200, 2, "1 дан, 1 секунда")]
        [InlineData(86401200, 3, "1 дан, 1 секунда, 200 милисекунди")]
        [InlineData(1296000000, 1, "2 недеље")]
        [InlineData(1296000000, 2, "2 недеље, 1 дан")]
        [InlineData(1299600000, 2, "2 недеље, 1 дан")]
        [InlineData(1299600000, 3, "2 недеље, 1 дан, 1 сат")]
        [InlineData(1299630020, 3, "2 недеље, 1 дан, 1 сат")]
        [InlineData(1299630020, 4, "2 недеље, 1 дан, 1 сат, 30 секунди")]
        [InlineData(1299630020, 5, "2 недеље, 1 дан, 1 сат, 30 секунди, 20 милисекунди")]
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
            Assert.Equal("без протеклог времена", actual);
        }
    }
}
