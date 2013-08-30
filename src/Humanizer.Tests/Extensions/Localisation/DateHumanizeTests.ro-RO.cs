using System;
using Xunit;

namespace Humanizer.Tests.Extensions.Localisation
{
    /// <summary>
    /// Test that for values bigger than 19 "de" is added between the numeral
    /// and the time unit: http://ebooks.unibuc.ro/filologie/NForascu-DGLR/numerale.htm.
    /// There is no test for months since there are only 12 of them in a year.
    /// </summary>
    public class RomanianDateHumanizeTests : AmbientCulture
    {
        public RomanianDateHumanizeTests() : base("ro-RO")
        {
        }

        [Fact]
        public void RomanianTranslationIsCorrectForThreeHoursAgo()
        {
            var threeHoursAgo = DateTime.UtcNow.AddHours(-3).Humanize();

            Assert.Equal("acum 3 ore", threeHoursAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor20HoursAgo()
        {
            var threeHoursAgo = DateTime.UtcNow.AddHours(-20).Humanize();

            Assert.Equal("acum 20 de ore", threeHoursAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor19MinutesAgo()
        {
            var nineteenMinutesAgo = DateTime.UtcNow.AddMinutes(-19).Humanize();

            Assert.Equal("acum 19 minute", nineteenMinutesAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor44MinutesAgo()
        {
            var fourtyFourMinutesAgo = DateTime.UtcNow.AddMinutes(-44).Humanize();

            Assert.Equal("acum 44 de minute", fourtyFourMinutesAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor2SecondsAgo()
        {
            var twoSecondsAgo = DateTime.UtcNow.AddSeconds(-2).Humanize();

            Assert.Equal("acum 2 secunde", twoSecondsAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor59SecondsAgo()
        {
            var fiftyNineSecondsAgo = DateTime.UtcNow.AddSeconds(-59).Humanize();

            Assert.Equal("acum 59 de secunde", fiftyNineSecondsAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor10DaysAgo()
        {
            var tenDaysAgo = DateTime.UtcNow.AddDays(-10).Humanize();

            Assert.Equal("acum 10 zile", tenDaysAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor23DaysAgo()
        {
            var twentyThreeDaysAgo = DateTime.UtcNow.AddDays(-23).Humanize();

            Assert.Equal("acum 23 de zile", twentyThreeDaysAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor119YearsAgo()
        {
            var oneHundredNineteenYearsAgo = DateTime.UtcNow.AddYears(-119).Humanize();

            Assert.Equal("acum 119 ani", oneHundredNineteenYearsAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor100YearsAgo()
        {
            var hunderedYearsAgo = DateTime.UtcNow.AddYears(-100).Humanize();

            Assert.Equal("acum 100 de ani", hunderedYearsAgo);
        }
    }
}