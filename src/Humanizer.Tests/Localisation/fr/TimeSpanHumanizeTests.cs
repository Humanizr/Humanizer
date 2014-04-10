using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.fr
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("fr") { }

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
        [InlineData(135, "2 minutes")]
        [InlineData(60, "1 minute")]
        [InlineData(2, "2 secondes")]
        [InlineData(1, "1 seconde")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 secondes")]
        [InlineData(1400, "1 seconde")]
        [InlineData(2, "2 millisecondes")]
        [InlineData(1, "1 milliseconde")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "pas de temps")]
        [InlineData(10, 2, "10 millisecondes")]
        [InlineData(1400, 2, "1 seconde, 400 millisecondes")]
        [InlineData(2500, 2, "2 secondes, 500 millisecondes")]
        [InlineData(120000, 2, "2 minutes")]
        [InlineData(62000, 2, "1 minute, 2 secondes")]
        [InlineData(62020, 3, "1 minute, 2 secondes, 20 millisecondes")]
        [InlineData(3600020, 4, "1 heure, 20 millisecondes")]
        [InlineData(3600020, 1, "1 heure")]
        [InlineData(3603001, 2, "1 heure, 3 secondes")]
        [InlineData(3603001, 3, "1 heure, 3 secondes, 1 milliseconde")]
        [InlineData(86400000, 3, "1 jour")]
        [InlineData(86401000, 2, "1 jour, 1 seconde")]
        [InlineData(86401200, 3, "1 jour, 1 seconde, 200 millisecondes")]
        [InlineData(1296000000, 1, "2 semaines")]
        [InlineData(1299600000, 2, "2 semaines, 1 jour")]
        [InlineData(1299630020, 3, "2 semaines, 1 jour, 1 heure")]
        [InlineData(1299630020, 4, "2 semaines, 1 jour, 1 heure, 30 secondes")]
        [InlineData(1299630020, 5, "2 semaines, 1 jour, 1 heure, 30 secondes, 20 millisecondes")]
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
            Assert.Equal("pas de temps", actual);
        }
    }
}
