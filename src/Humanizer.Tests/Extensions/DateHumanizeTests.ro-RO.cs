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

        /// <summary>
        /// Temporarily change the CurrentUICulture and restore it back
        /// </summary>
        private class CurrentCultureChanger: IDisposable
        {
            private readonly CultureInfo initialCulture;
            private readonly Thread initialThread;

            public CurrentCultureChanger(CultureInfo cultureInfo)
            {
                if (cultureInfo == null)
                    throw new ArgumentNullException("cultureInfo");

                initialThread = Thread.CurrentThread;
                initialCulture = initialThread.CurrentUICulture;

                initialThread.CurrentUICulture = cultureInfo;
            }

            public void Dispose()
            {
                initialThread.CurrentUICulture = initialCulture;
            }
        }
    }
}