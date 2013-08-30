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

        private static CurrentCultureChanger RomanianCulture()
        {
            return new CurrentCultureChanger(new CultureInfo("ro-RO"));
        }
    }
}