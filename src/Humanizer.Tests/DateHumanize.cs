using System;
using System.Globalization;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
{
    public class DateHumanize
    {
        readonly static object LockObject = new object();
        static void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow, CultureInfo culture)
        {
            var utcNow = DateTime.UtcNow;
            var localNow = DateTime.Now;

            // feels like the only way to avoid breaking tests because CPU ticks over is to inject the base date
            VerifyWithDate(expectedString, deltaFromNow, culture, localNow, utcNow);
        }

        static void VerifyWithCurrentDateAndNeverThreshold(string expectedString, TimeSpan deltaFromNow, CultureInfo culture, DateTime dateNeverThreshold, DateTime dateNeverThresholdUtc)
        {
            var utcNow = DateTime.UtcNow;
            var localNow = DateTime.Now;

            // feels like the only way to avoid breaking tests because CPU ticks over is to inject the base date
            VerifyWithDateAndNeverThreshold(expectedString, deltaFromNow, culture, localNow, utcNow, dateNeverThreshold, dateNeverThresholdUtc);
        }

        static void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow, CultureInfo culture)
        {
            var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
            var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

            VerifyWithDate(expectedString, deltaFromNow, culture, now, utcNow);
        }

        static void VerifyWithDateInjectionAndNeverThreshold(string expectedString, TimeSpan deltaFromNow, CultureInfo culture, DateTime dateNeverThreshold, DateTime dateNeverThresholdUtc)
        {
            var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
            var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

            VerifyWithDateAndNeverThreshold(expectedString, deltaFromNow, culture, now, utcNow, dateNeverThreshold, dateNeverThresholdUtc);
        }

        static void VerifyWithDate(string expectedString, TimeSpan deltaFromBase, CultureInfo culture, DateTime baseDate, DateTime baseDateUtc)
        {
            Assert.Equal(expectedString, baseDateUtc.Add(deltaFromBase).Humanize(utcDate: true, dateToCompareAgainst: baseDateUtc, culture: culture));
            Assert.Equal(expectedString, baseDate.Add(deltaFromBase).Humanize(false, baseDate, culture: culture));
        }

        static void VerifyWithDateAndNeverThreshold(string expectedString, TimeSpan deltaFromBase, CultureInfo culture, DateTime baseDate, DateTime baseDateUtc, DateTime dateNeverThreshold, DateTime dateNeverThresholdUtc)
        {
            Assert.Equal(expectedString, baseDateUtc.Add(deltaFromBase).Humanize(utcDate: true, dateToCompareAgainst: baseDateUtc, culture: culture, dateNeverThreshold: dateNeverThresholdUtc));
            Assert.Equal(expectedString, baseDate.Add(deltaFromBase).Humanize(false, baseDate, culture: culture, dateNeverThreshold: dateNeverThreshold));
        }

        public static void Verify(string expectedString, int unit, TimeUnit timeUnit, Tense tense, double? precision = null, CultureInfo culture = null, DateTime? baseDate = null, DateTime? baseDateUtc = null, DateTime? dateNeverThreshold = null, DateTime? dateNeverThresholdUtc = null)
        {
            // We lock this as these tests can be multi-threaded and we're setting a static
            lock (LockObject)
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

                if (baseDate == null)
                {
                    if (dateNeverThreshold.HasValue && dateNeverThresholdUtc.HasValue)
                    {
                        VerifyWithCurrentDateAndNeverThreshold(expectedString, deltaFromNow, culture, dateNeverThreshold.Value, dateNeverThresholdUtc.Value);
                        VerifyWithDateInjectionAndNeverThreshold(expectedString, deltaFromNow, culture, dateNeverThreshold.Value, dateNeverThresholdUtc.Value);
                    }
                    else
                    {
                        VerifyWithCurrentDate(expectedString, deltaFromNow, culture);
                        VerifyWithDateInjection(expectedString, deltaFromNow, culture);
                    }                        
                }                
                else
                {
                    if (dateNeverThreshold.HasValue && dateNeverThresholdUtc.HasValue)
                    {
                        VerifyWithDateAndNeverThreshold(expectedString, deltaFromNow, culture, baseDate.Value, baseDateUtc.Value, dateNeverThreshold.Value, dateNeverThresholdUtc.Value);
                    }
                    else
                    {
                        VerifyWithDate(expectedString, deltaFromNow, culture, baseDate.Value, baseDateUtc.Value);
                    }                    
                }                
            }
        }
    }
}