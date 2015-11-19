using System;
using Xunit;

namespace Humanizer.Tests.Localisation.it
{
    [UseCulture("it")]
    public class TimeSpanHumanizeTests 
    {

        [Theory]
        [InlineData(7, "1 settimana")]
        [InlineData(14, "2 settimane")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 giorno")]
        [InlineData(2, "2 giorni")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 ora")]
        [InlineData(2, "2 ore")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minuto")]
        [InlineData(2, "2 minuti")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "1 secondo")]
        [InlineData(2, "2 secondi")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "1 millisecondo")]
        [InlineData(2, "2 millisecondi")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            // This does not make much sense in italian, anyway
            Assert.Equal("0 secondi", TimeSpan.Zero.Humanize());
        }
    }
}
