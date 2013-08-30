using System;
using System.Globalization;

using Xunit;

namespace Humanizer.Tests.Extensions
{
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


        private static CurrentCultureChanger RomanianCulture()
        {
            return new CurrentCultureChanger(new CultureInfo("ro-RO"));
        }
    }
}