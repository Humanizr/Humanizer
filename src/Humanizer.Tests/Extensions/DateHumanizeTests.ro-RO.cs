using System;
using System.Globalization;

using Xunit;

namespace Humanizer.Tests.Extensions
{
    /// <summary>
    /// Test that for values bigger than 19 "de" is added between the numeral
    /// and the time unit: http://ebooks.unibuc.ro/filologie/NForascu-DGLR/numerale.htm.
    /// There is no test for months since there are only 12 of them in a year.
    /// </summary>
    public class RomanianDateHumanizeTests
    {
        [Fact]
        public void RomanianTranslationIsCorrectForThreeHoursAgo()
        {
            using (RomanianCulture())
            {
                var threeHoursAgo = DateTime.UtcNow.AddHours(-3).Humanize();

                Assert.Equal("acum 3 ore", threeHoursAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor20HoursAgo()
        {
            using (RomanianCulture())
            {
                var threeHoursAgo = DateTime.UtcNow.AddHours(-20).Humanize();

                Assert.Equal("acum 20 de ore", threeHoursAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor19MinutesAgo()
        {
            using (RomanianCulture())
            {
                var nineteenMinutesAgo = DateTime.UtcNow.AddMinutes(-19).Humanize();

                Assert.Equal("acum 19 minute", nineteenMinutesAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor44MinutesAgo()
        {
            using (RomanianCulture())
            {
                var fourtyFourMinutesAgo = DateTime.UtcNow.AddMinutes(-44).Humanize();

                Assert.Equal("acum 44 de minute", fourtyFourMinutesAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor2SecondsAgo()
        {
            using (RomanianCulture())
            {
                var twoSecondsAgo = DateTime.UtcNow.AddSeconds(-2).Humanize();

                Assert.Equal("acum 2 secunde", twoSecondsAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor59SecondsAgo()
        {
            using (RomanianCulture())
            {
                var fiftyNineSecondsAgo = DateTime.UtcNow.AddSeconds(-59).Humanize();

                Assert.Equal("acum 59 de secunde", fiftyNineSecondsAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor10DaysAgo()
        {
            using (RomanianCulture())
            {
                var tenDaysAgo = DateTime.UtcNow.AddDays(-10).Humanize();

                Assert.Equal("acum 10 zile", tenDaysAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor23DaysAgo()
        {
            using (RomanianCulture())
            {
                var twentyThreeDaysAgo = DateTime.UtcNow.AddDays(-23).Humanize();

                Assert.Equal("acum 23 de zile", twentyThreeDaysAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor119YearsAgo()
        {
            using (RomanianCulture())
            {
                var oneHundredNineteenYearsAgo = DateTime.UtcNow.AddYears(-119).Humanize();

                Assert.Equal("acum 119 ani", oneHundredNineteenYearsAgo);
            }
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor100YearsAgo()
        {
            using (RomanianCulture())
            {
                var hunderedYearsAgo = DateTime.UtcNow.AddYears(-100).Humanize();

                Assert.Equal("acum 100 de ani", hunderedYearsAgo);
            }
        }

        private static CurrentCultureChanger RomanianCulture()
        {
            return new CurrentCultureChanger(new CultureInfo("ro-RO"));
        }
    }
}