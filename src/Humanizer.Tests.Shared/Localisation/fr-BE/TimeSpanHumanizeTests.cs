using System;
using Humanizer.Localisation;

using Xunit;

namespace Humanizer.Tests.Localisation.frBE
{
    [UseCulture("fr-BE")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 an")]
        [InlineData(731, "2 ans")]
        [InlineData(1096, "3 ans")]
        [InlineData(4018, "11 ans")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 mois")]
        [InlineData(61, "2 mois")]
        [InlineData(92, "3 mois")]
        [InlineData(335, "11 mois")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));
        }

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

        [Theory]
        [InlineData(TimeUnit.Year, "0 an")]
        [InlineData(TimeUnit.Month, "0 mois")]
        [InlineData(TimeUnit.Week, "0 semaine")]
        [InlineData(TimeUnit.Day, "0 jour")]
        [InlineData(TimeUnit.Hour, "0 heure")]
        [InlineData(TimeUnit.Minute, "0 minute")]
        [InlineData(TimeUnit.Second, "0 seconde")]
        [InlineData(TimeUnit.Millisecond, "0 milliseconde")]
        public void NoTime(TimeUnit minUnit, string expected)
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize(minUnit: minUnit);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTimeToWords()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize(toWords: true);
            Assert.Equal("temps nul", actual);
        }
    }
}
