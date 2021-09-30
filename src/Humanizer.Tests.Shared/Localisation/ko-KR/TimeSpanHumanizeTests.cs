using System;
using Xunit;

namespace Humanizer.Tests.Localisation.koKR
{
    [UseCulture("ko-KR")]
    public class TimeSpanHumanizeTests
    {
        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1년")]
        [InlineData(731, "2년")]
        [InlineData(1096, "3년")]
        [InlineData(4018, "11년")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1개월")]
        [InlineData(61, "2개월")]
        [InlineData(92, "3개월")]
        [InlineData(335, "11개월")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(7, "1주")]
        [InlineData(14, "2주")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1일")]
        [InlineData(2, "2일")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1시간")]
        [InlineData(2, "2시간")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1분")]
        [InlineData(2, "2분")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1초")]
        [InlineData(2, "2초")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(1, "1밀리초")]
        [InlineData(2, "2밀리초")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        [Trait("Translation", "Google")]
        public void NoTime()
        {
            Assert.Equal("0밀리초", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("방금", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
