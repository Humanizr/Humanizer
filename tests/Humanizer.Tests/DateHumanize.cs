
public class DateHumanize
{
    static readonly Lock LockObject = new();

    static void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow, CultureInfo? culture)
    {
        var utcNow = DateTime.UtcNow;
        var localNow = DateTime.Now;

        // feels like the only way to avoid breaking tests because CPU ticks over is to inject the base date
        VerifyWithDate(expectedString, deltaFromNow, culture, localNow, utcNow);
    }

    static void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow, CultureInfo? culture)
    {
        var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
        var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

        VerifyWithDate(expectedString, deltaFromNow, culture, now, utcNow);
    }

    static void VerifyWithDate(string expectedString, TimeSpan deltaFromBase, CultureInfo? culture, DateTime baseDate, DateTime baseDateUtc)
    {
        Assert.Equal(expectedString, baseDateUtc
            .Add(deltaFromBase)
            .Humanize(utcDate: true, dateToCompareAgainst: baseDateUtc, culture: culture));
        Assert.Equal(expectedString, baseDate
            .Add(deltaFromBase)
            .Humanize(false, baseDate, culture: culture));

        // Compared with default utcDate
        Assert.Equal(expectedString, baseDateUtc
            .Add(deltaFromBase)
            .Humanize(utcDate: null, dateToCompareAgainst: baseDateUtc, culture: culture));
        Assert.Equal(expectedString, baseDate
            .Add(deltaFromBase)
            .Humanize(null, baseDate, culture: culture));
    }

    public static void Verify(string expectedString, int unit, TimeUnit timeUnit, Tense tense, double? precision = null, CultureInfo? culture = null, DateTime? baseDate = null, DateTime? baseDateUtc = null)
    {
        // We lock this as these tests can be multi-threaded and we're setting a static
        lock (LockObject)
        {
            if (precision.HasValue)
            {
                Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision.Value);
            }
            else
            {
                Configurator.DateTimeHumanizeStrategy = new DefaultDateTimeHumanizeStrategy();
            }

            unit = Math.Abs(unit);

            if (tense == Tense.Past)
            {
                unit = -unit;
            }

            var deltaFromNow = timeUnit switch
            {
                TimeUnit.Millisecond => TimeSpan.FromMilliseconds(unit),
                TimeUnit.Second => TimeSpan.FromSeconds(unit),
                TimeUnit.Minute => TimeSpan.FromMinutes(unit),
                TimeUnit.Hour => TimeSpan.FromHours(unit),
                TimeUnit.Day => TimeSpan.FromDays(unit),
                TimeUnit.Month => TimeSpan.FromDays(unit * 31),
                TimeUnit.Year => TimeSpan.FromDays(unit * 366),
                _ => new()
            };

            if (baseDate == null)
            {
                VerifyWithCurrentDate(expectedString, deltaFromNow, culture);
                VerifyWithDateInjection(expectedString, deltaFromNow, culture);
            }
            else
            {
                VerifyWithDate(expectedString, deltaFromNow, culture, baseDate.Value, baseDateUtc!.Value);
            }
        }
    }
}