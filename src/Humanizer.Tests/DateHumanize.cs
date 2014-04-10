using System;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
{
    public class DateHumanize
    {
        static void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow)
        {
            var utcNow = DateTime.UtcNow;
            var localNow = DateTime.Now;

            // feels like the only way to avoid breaking tests because CPU ticks over is to inject the base date
            Assert.Equal(expectedString, utcNow.Add(deltaFromNow).Humanize(utcDate: true, dateToCompareAgainst: utcNow));
            Assert.Equal(expectedString, localNow.Add(deltaFromNow).Humanize(utcDate: false, dateToCompareAgainst: localNow));
        }

        static void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow)
        {
            var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
            var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

            Assert.Equal(expectedString, utcNow.Add(deltaFromNow).Humanize(utcDate: true, dateToCompareAgainst: utcNow));
            Assert.Equal(expectedString, now.Add(deltaFromNow).Humanize(false, now));
        }

        public static void Verify(string expectedString, int unit, TimeUnit timeUnit, Tense tense, double? precision = null)
        {
            if (precision.HasValue)
                Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision.Value);
            else
                Configurator.DateTimeHumanizeStrategy = new DefaultDateTimeHumanizeStrategy();

            var deltaFromNow = new TimeSpan();
            unit = Math.Abs(unit);

            if (tense == Tense.Past)
                unit = -unit;

            switch (timeUnit)
            {
                case TimeUnit.Millisecond:
                    deltaFromNow = TimeSpan.FromMilliseconds(unit);
                    break;
                case TimeUnit.Second:
                    deltaFromNow = TimeSpan.FromSeconds(unit);
                    break;
                case TimeUnit.Minute:
                    deltaFromNow = TimeSpan.FromMinutes(unit);
                    break;
                case TimeUnit.Hour:
                    deltaFromNow = TimeSpan.FromHours(unit);
                    break;
                case TimeUnit.Day:
                    deltaFromNow = TimeSpan.FromDays(unit);
                    break;
                case TimeUnit.Month:
                    deltaFromNow = TimeSpan.FromDays(unit*31);
                    break;
                case TimeUnit.Year:
                    deltaFromNow = TimeSpan.FromDays(unit*366);
                    break;
            }

            VerifyWithCurrentDate(expectedString, deltaFromNow);
            VerifyWithDateInjection(expectedString, deltaFromNow);
        }
    }
}