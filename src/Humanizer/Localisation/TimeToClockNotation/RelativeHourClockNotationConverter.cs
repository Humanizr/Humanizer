#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Provides clock notation that combines relative hour phrases with day-period words.
/// </summary>
class RelativeHourClockNotationConverter(RelativeHourClockNotationProfile profile) : ITimeOnlyToClockNotationConverter
{
    const int Morning = 6;
    const int Noon = 12;
    const int Afternoon = 21;

    readonly RelativeHourClockNotationProfile profile = profile;

    /// <summary>
    /// Converts the given time using the relative-hour phrase profile.
    /// </summary>
    /// <returns>The localized clock-notation string.</returns>
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        // Midnight and noon are fixed phrases, not template-driven hour buckets.
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return profile.Midnight;
            case { Hour: 12, Minute: 0 }:
                return profile.Noon;
        }

        var article = GetArticle(time);
        // We keep both the current and next hour around because the "minus" phrases flip to the
        // next hour while the "plus" phrases stay anchored to the current one.
        var articleNextHour = GetArticle(time.AddHours(1));
        var hour = NormalizeHour(time).ToWords(GrammaticalGender.Feminine);
        var nextHour = NormalizeHour(time.AddHours(1)).ToWords(GrammaticalGender.Feminine);
        var dayPeriod = GetDayPeriod(time);
        var dayPeriodNextHour = GetDayPeriod(time.AddHours(1));

        // The rounded minute value drives both the phrasing and the day-period selection, so we
        // normalize it once before the bucket switch.
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        // At 35 minutes and beyond, the spoken form is already about the next hour; keep those
        // branches separate instead of trying to fold them into the "plus" family.
        return normalizedMinutes switch
        {
            0 => $"{article} {hour} {dayPeriod}",
            15 => $"{article} {hour} {profile.PlusQuarter} {dayPeriod}",
            30 => $"{article} {hour} {profile.PlusHalf} {dayPeriod}",
            35 => $"{articleNextHour} {nextHour} {profile.MinusTwentyFive} {dayPeriodNextHour}",
            40 => $"{articleNextHour} {nextHour} {profile.MinusTwenty} {dayPeriodNextHour}",
            45 => $"{articleNextHour} {nextHour} {profile.MinusQuarter} {dayPeriodNextHour}",
            50 => $"{articleNextHour} {nextHour} {profile.MinusTen} {dayPeriodNextHour}",
            55 => $"{articleNextHour} {nextHour} {profile.MinusFive} {dayPeriodNextHour}",
            60 => $"{articleNextHour} {nextHour} {dayPeriodNextHour}",
            _ => $"{article} {hour} {profile.PlusConnector} {normalizedMinutes.ToWords()} {dayPeriod}"
        };
    }

    static int NormalizeHour(TimeOnly time)
    {
        // Spoken phrases use 12 instead of 0 for the hour position.
        return time.Hour % 12 != 0 ? time.Hour % 12 : 12;
    }

    string GetArticle(TimeOnly time)
    {
        // Some supported languages use a singular article for one o'clock and a plural article
        // for every other hour.
        return time.Hour is 1 or 13 ? profile.SingularArticle : profile.PluralArticle;
    }

    string GetDayPeriod(TimeOnly time)
    {
        // These buckets match the day-part words expected by the supported relative-hour phrases.
        if (time.Hour is >= 1 and < Morning)
        {
            return profile.EarlyMorning;
        }

        if (time.Hour is >= Morning and < Noon)
        {
            return profile.Morning;
        }

        if (time.Hour is >= Noon and < Afternoon)
        {
            return profile.Afternoon;
        }

        return profile.Night;
    }
}

/// <summary>
/// Stores the phrases used by <see cref="RelativeHourClockNotationConverter"/>.
/// </summary>
sealed class RelativeHourClockNotationProfile(
    string midnight,
    string noon,
    string singularArticle,
    string pluralArticle,
    string plusConnector,
    string plusQuarter,
    string plusHalf,
    string minusTwentyFive,
    string minusTwenty,
    string minusQuarter,
    string minusTen,
    string minusFive,
    string earlyMorning,
    string morning,
    string afternoon,
    string night)
{
    /// <summary>Gets the phrase used for midnight.</summary>
    public string Midnight { get; } = midnight;
    /// <summary>Gets the phrase used for noon.</summary>
    public string Noon { get; } = noon;
    /// <summary>Gets the article used for singular hour phrases.</summary>
    public string SingularArticle { get; } = singularArticle;
    /// <summary>Gets the article used for plural hour phrases.</summary>
    public string PluralArticle { get; } = pluralArticle;
    /// <summary>Gets the connector used for "past" minute phrases.</summary>
    public string PlusConnector { get; } = plusConnector;
    /// <summary>Gets the phrase used for a quarter past the hour.</summary>
    public string PlusQuarter { get; } = plusQuarter;
    /// <summary>Gets the phrase used for half past the hour.</summary>
    public string PlusHalf { get; } = plusHalf;
    /// <summary>Gets the phrase used for twenty-five minutes to the next hour.</summary>
    public string MinusTwentyFive { get; } = minusTwentyFive;
    /// <summary>Gets the phrase used for twenty minutes to the next hour.</summary>
    public string MinusTwenty { get; } = minusTwenty;
    /// <summary>Gets the phrase used for a quarter to the next hour.</summary>
    public string MinusQuarter { get; } = minusQuarter;
    /// <summary>Gets the phrase used for ten minutes to the next hour.</summary>
    public string MinusTen { get; } = minusTen;
    /// <summary>Gets the phrase used for five minutes to the next hour.</summary>
    public string MinusFive { get; } = minusFive;
    /// <summary>Gets the day-period word used for early morning times.</summary>
    public string EarlyMorning { get; } = earlyMorning;
    /// <summary>Gets the day-period word used for morning times.</summary>
    public string Morning { get; } = morning;
    /// <summary>Gets the day-period word used for afternoon times.</summary>
    public string Afternoon { get; } = afternoon;
    /// <summary>Gets the day-period word used for night times.</summary>
    public string Night { get; } = night;
}

#endif
