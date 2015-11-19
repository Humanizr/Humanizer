using System;
using Xunit;

namespace Humanizer.Tests.Localisation.frBE
{
    [UseCulture("fr-BE")]
    public class TimeSpanHumanizeTests 
    {
        [Theory]
        [InlineData(14, "2 semaines")]
        [InlineData(7, "1 semaine")]
        public void Weeks(int days, string expected) 
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 jours")]
        [InlineData(1, "1 jour")]
        public void Days(int days, string expected) 
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 heures")]
        [InlineData(1, "1 heure")]
        public void Hours(int hours, string expected) 
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 minutes")]
        [InlineData(1, "1 minute")]
        public void Minutes(int minutes, string expected) 
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 secondes")]
        [InlineData(1, "1 seconde")]
        public void Seconds(int seconds, string expected) 
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 millisecondes")]
        [InlineData(1, "1 milliseconde")]
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
            Assert.Equal("pas de temps", actual);
        }
    }
}
