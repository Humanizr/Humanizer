using System;
using Humanizer.Configuration;
using Humanizer.DistanceOfTimeCalculators;
using Xunit;

namespace Humanizer.Tests
{
    public abstract class DateHumanizeTestsBase : IDisposable
    {
        private readonly IDistanceOfTimeInWords previousCalculator;

        protected DateHumanizeTestsBase()
        {
            previousCalculator = Configurator.DistanceOfTimeInWords;
        }

        public void Dispose()
        {
            Configurator.DistanceOfTimeInWords = previousCalculator;
        }

        protected static void Verify(string expectedString, TimeSpan deltaFromNow)
        {
            VerifyWithCurrentDate(expectedString, deltaFromNow);
            VerifyWithDateInjection(expectedString, deltaFromNow);
        }

        private static void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow)
        {
            Assert.Equal(expectedString, DateTime.UtcNow.Add(deltaFromNow).Humanize());
            Assert.Equal(expectedString, DateTime.Now.Add(deltaFromNow).Humanize(false));
        }

        private static void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow)
        {
            var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
            var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

            Assert.Equal(expectedString, utcNow.Add(deltaFromNow).Humanize(dateToCompareAgainst: utcNow));
            Assert.Equal(expectedString, now.Add(deltaFromNow).Humanize(false, now));
        }
    }
}