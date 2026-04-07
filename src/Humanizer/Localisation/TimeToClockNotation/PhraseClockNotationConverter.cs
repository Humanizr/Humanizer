#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Provides clock notation using a unified phrase-based profile that covers all locale patterns
/// through YAML-driven configuration: bucket phrases, hour modes, day periods, and zero-filler words.
/// </summary>
class PhraseClockNotationConverter(PhraseClockNotationProfile profile) : ITimeOnlyToClockNotationConverter
{
    readonly PhraseClockNotationProfile profile = profile;

    /// <summary>
    /// Converts the given time using the phrase-clock profile.
    /// </summary>
    /// <returns>The localized clock-notation string.</returns>
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        // When rounding pushes minutes to 60, advance to the next hour so the bucket switch
        // does not need a second normalization pass.
        int hour;
        if (normalizedMinutes == 60)
        {
            hour = time.Hour + 1;
            normalizedMinutes = 0;
        }
        else
        {
            hour = time.Hour;
        }

        // Midnight and noon are fixed phrases that bypass all template machinery.
        if (hour % 24 == 0 && normalizedMinutes == 0 && profile.Midnight.Length > 0)
        {
            return profile.Midnight;
        }

        if (hour % 24 == 12 && normalizedMinutes == 0 && profile.Midday.Length > 0)
        {
            return profile.Midday;
        }

        var hourWords = ResolveHourWords(hour);
        var nextHourWords = ResolveHourWords(hour + 1);
        var rawMinuteWords = ResolveMinuteWords(normalizedMinutes);

        // When a zero-filler is configured and minutes are 1-9, prepend the filler word
        // so templates like "{hour} {minutes}" produce "et nul fem" instead of "et fem".
        var minuteWords = normalizedMinutes is > 0 and < 10 && profile.ZeroFiller.Length > 0
            ? profile.ZeroFiller + " " + rawMinuteWords
            : rawMinuteWords;

        var reverseMinuteWords = normalizedMinutes > 0 ? ResolveMinuteWords(60 - normalizedMinutes) : "";

        string halfMinuteWords;
        if (normalizedMinutes < 30)
        {
            halfMinuteWords = ResolveMinuteWords(30 - normalizedMinutes);
        }
        else if (normalizedMinutes > 30)
        {
            halfMinuteWords = ResolveMinuteWords(normalizedMinutes - 30);
        }
        else
        {
            halfMinuteWords = "";
        }

        // Check minute-bucket template first (exact 5-minute intervals).
        var template = GetBucketTemplate(normalizedMinutes);
        if (template.Length > 0)
        {
            var result = ExpandTemplate(template, hourWords, nextHourWords, minuteWords, reverseMinuteWords, halfMinuteWords);
            return ApplyDayPeriod(result, hour);
        }

        // Fall to default template.
        if (profile.DefaultTemplate.Length > 0)
        {
            var result = ExpandTemplate(profile.DefaultTemplate, hourWords, nextHourWords, minuteWords, reverseMinuteWords, halfMinuteWords);
            return ApplyDayPeriod(result, hour);
        }

        // Absolute fallback: "{hour} {minutes}".
        var fallback = minuteWords.Length > 0 ? hourWords + " " + minuteWords : hourWords;
        return ApplyDayPeriod(fallback, hour);
    }

    int ResolveHourValue(int hour)
    {
        return profile.HourMode switch
        {
            PhraseClockHourMode.H12 => hour % 12 != 0 ? hour % 12 : 12,
            PhraseClockHourMode.H24 => hour % 24,
            PhraseClockHourMode.Numeric => hour % 24,
            _ => hour % 12
        };
    }

    string ResolveHourWords(int rawHour)
    {
        var hourValue = ResolveHourValue(rawHour);

        if (profile.HourMode == PhraseClockHourMode.Numeric)
        {
            return hourValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        return profile.HourGender switch
        {
            GrammaticalGender.Feminine => hourValue.ToWords(GrammaticalGender.Feminine),
            GrammaticalGender.Neuter => hourValue.ToWords(GrammaticalGender.Neuter),
            _ => hourValue.ToWords()
        };
    }

    string ResolveMinuteWords(int minutes)
    {
        if (minutes == 0)
        {
            return "";
        }

        if (profile.HourMode == PhraseClockHourMode.Numeric)
        {
            return minutes.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        return profile.MinuteGender switch
        {
            GrammaticalGender.Feminine => minutes.ToWords(GrammaticalGender.Feminine),
            GrammaticalGender.Neuter => minutes.ToWords(GrammaticalGender.Neuter),
            _ => minutes.ToWords()
        };
    }

    string GetBucketTemplate(int minutes)
    {
        return minutes switch
        {
            0 => profile.Min0,
            5 => profile.Min5,
            10 => profile.Min10,
            15 => profile.Min15,
            20 => profile.Min20,
            25 => profile.Min25,
            30 => profile.Min30,
            35 => profile.Min35,
            40 => profile.Min40,
            45 => profile.Min45,
            50 => profile.Min50,
            55 => profile.Min55,
            _ => ""
        };
    }

    static string ExpandTemplate(
        string template, string hour, string nextHour,
        string minutes, string minutesReverse, string minutesFromHalf)
    {
        var result = template;
        if (result.Contains("{hour}"))
        {
            result = result.Replace("{hour}", hour);
        }

        if (result.Contains("{nextHour}"))
        {
            result = result.Replace("{nextHour}", nextHour);
        }

        if (result.Contains("{minutes}"))
        {
            result = result.Replace("{minutes}", minutes);
        }

        if (result.Contains("{minutesReverse}"))
        {
            result = result.Replace("{minutesReverse}", minutesReverse);
        }

        if (result.Contains("{minutesFromHalf}"))
        {
            result = result.Replace("{minutesFromHalf}", minutesFromHalf);
        }

        return result;
    }

    string ApplyDayPeriod(string basePhrase, int hour)
    {
        if (profile.EarlyMorning.Length == 0 && profile.Morning.Length == 0 &&
            profile.Afternoon.Length == 0 && profile.Night.Length == 0)
        {
            return basePhrase;
        }

        var period = GetDayPeriod(hour);
        if (period.Length == 0)
        {
            return basePhrase;
        }

        return profile.DayPeriodPosition == PhraseClockDayPeriodPosition.Prefix
            ? period + " " + basePhrase
            : basePhrase + " " + period;
    }

    string GetDayPeriod(int hour)
    {
        var normalized = hour % 24;
        if (normalized is >= 1 and < 6)
        {
            return profile.EarlyMorning;
        }

        if (normalized is >= 6 and < 12)
        {
            return profile.Morning;
        }

        if (normalized is >= 12 and < 21)
        {
            return profile.Afternoon;
        }

        return profile.Night;
    }
}

/// <summary>
/// Defines the hour rendering mode for <see cref="PhraseClockNotationConverter"/>.
/// </summary>
enum PhraseClockHourMode
{
    /// <summary>12-hour clock (1-12).</summary>
    H12,
    /// <summary>24-hour clock (0-23).</summary>
    H24,
    /// <summary>Numeric digits instead of words.</summary>
    Numeric
}

/// <summary>
/// Defines day-period attachment position for <see cref="PhraseClockNotationConverter"/>.
/// </summary>
enum PhraseClockDayPeriodPosition
{
    /// <summary>Day period appears after the time phrase.</summary>
    Suffix,
    /// <summary>Day period appears before the time phrase.</summary>
    Prefix
}

/// <summary>
/// Stores the YAML-driven configuration for <see cref="PhraseClockNotationConverter"/>.
/// All fields are source-generated from locale YAML and cached as static instances.
/// </summary>
sealed class PhraseClockNotationProfile(
    PhraseClockHourMode hourMode,
    GrammaticalGender hourGender,
    GrammaticalGender minuteGender,
    string midnight,
    string midday,
    string min0,
    string min5,
    string min10,
    string min15,
    string min20,
    string min25,
    string min30,
    string min35,
    string min40,
    string min45,
    string min50,
    string min55,
    string defaultTemplate,
    string zeroFiller,
    string earlyMorning,
    string morning,
    string afternoon,
    string night,
    PhraseClockDayPeriodPosition dayPeriodPosition)
{
    /// <summary>Gets the hour rendering mode.</summary>
    public PhraseClockHourMode HourMode { get; } = hourMode;
    /// <summary>Gets the grammatical gender for hour number words.</summary>
    public GrammaticalGender HourGender { get; } = hourGender;
    /// <summary>Gets the grammatical gender for minute number words.</summary>
    public GrammaticalGender MinuteGender { get; } = minuteGender;
    /// <summary>Gets the fixed phrase for midnight.</summary>
    public string Midnight { get; } = midnight;
    /// <summary>Gets the fixed phrase for midday.</summary>
    public string Midday { get; } = midday;
    /// <summary>Gets the template for on-the-hour (0 minutes).</summary>
    public string Min0 { get; } = min0;
    /// <summary>Gets the template for 5 minutes past.</summary>
    public string Min5 { get; } = min5;
    /// <summary>Gets the template for 10 minutes past.</summary>
    public string Min10 { get; } = min10;
    /// <summary>Gets the template for 15 minutes past.</summary>
    public string Min15 { get; } = min15;
    /// <summary>Gets the template for 20 minutes past.</summary>
    public string Min20 { get; } = min20;
    /// <summary>Gets the template for 25 minutes past.</summary>
    public string Min25 { get; } = min25;
    /// <summary>Gets the template for 30 minutes past.</summary>
    public string Min30 { get; } = min30;
    /// <summary>Gets the template for 35 minutes past.</summary>
    public string Min35 { get; } = min35;
    /// <summary>Gets the template for 40 minutes past.</summary>
    public string Min40 { get; } = min40;
    /// <summary>Gets the template for 45 minutes past.</summary>
    public string Min45 { get; } = min45;
    /// <summary>Gets the template for 50 minutes past.</summary>
    public string Min50 { get; } = min50;
    /// <summary>Gets the template for 55 minutes past.</summary>
    public string Min55 { get; } = min55;
    /// <summary>Gets the fallback template for non-bucketed minutes.</summary>
    public string DefaultTemplate { get; } = defaultTemplate;
    /// <summary>Gets the zero-filler word inserted when minutes are 1-9 (e.g., "noll", "nul").</summary>
    public string ZeroFiller { get; } = zeroFiller;
    /// <summary>Gets the day-period word for early morning (1:00-5:59).</summary>
    public string EarlyMorning { get; } = earlyMorning;
    /// <summary>Gets the day-period word for morning (6:00-11:59).</summary>
    public string Morning { get; } = morning;
    /// <summary>Gets the day-period word for afternoon (12:00-20:59).</summary>
    public string Afternoon { get; } = afternoon;
    /// <summary>Gets the day-period word for night (21:00-0:59).</summary>
    public string Night { get; } = night;
    /// <summary>Gets the position of day-period words relative to the time phrase.</summary>
    public PhraseClockDayPeriodPosition DayPeriodPosition { get; } = dayPeriodPosition;
}

#endif