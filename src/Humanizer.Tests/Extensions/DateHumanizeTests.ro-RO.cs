using System;
using System.Globalization;
using System.Threading;

using Xunit;

namespace Humanizer.Tests.Extensions
{
    public class RomanianDateHumanizeTests
    {
        [Fact]
        public void RomanianTranslationIsCorrectForThreeHoursAgo()
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ro-RO");

            var threeHoursAgo = DateTime.UtcNow.AddHours(-3).Humanize();

            Thread.CurrentThread.CurrentUICulture = currentCulture;

            Assert.Equal("acum 3 ore", threeHoursAgo);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor20HoursAgo()
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ro-RO");

            var threeHoursAgo = DateTime.UtcNow.AddHours(-20).Humanize();

            Thread.CurrentThread.CurrentUICulture = currentCulture;

            Assert.Equal("acum 20 de ore", threeHoursAgo);
        }
    }
}