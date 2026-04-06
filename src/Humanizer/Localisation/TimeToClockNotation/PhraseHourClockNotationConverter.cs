#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Provides clock notation that is built from fixed phrase templates around each hour.
/// </summary>
class PhraseHourClockNotationConverter(PhraseHourClockNotationProfile profile) : ITimeOnlyToClockNotationConverter
{
    readonly PhraseHourClockNotationProfile profile = profile;

    /// <summary>
    /// Converts the given time using the phrase template profile.
    /// </summary>
    /// <returns>The localized clock-notation string.</returns>
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        // Midnight and noon are fixed tokens, so they bypass the template machinery entirely.
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return profile.Midnight;
            case { Hour: 12, Minute: 0 }:
                return profile.Noon;
        }

        var normalizedHour = NormalizeHour(time);
        // The next-hour rendering is computed once because the "minus" templates need that value
        // and recomputing it inside each branch would obscure the minute-bucket mapping.
        var nextHour = NormalizeHour(time.AddHours(1));
        var hourWords = normalizedHour.ToWords(GrammaticalGender.Feminine);
        var nextHourWords = nextHour.ToWords(GrammaticalGender.Feminine);

        // Template selection is intentionally driven by the rounded minute bucket rather than by
        // ad-hoc string concatenation so locales can swap phrasing without changing control flow.
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        return normalizedMinutes switch
        {
            0 => string.Format(profile.OnHourTemplate, hourWords),
            15 => string.Format(profile.QuarterPastTemplate, hourWords),
            30 => string.Format(profile.HalfPastTemplate, hourWords),
            40 => string.Format(profile.MinusTwentyTemplate, nextHourWords),
            45 => string.Format(profile.MinusQuarterTemplate, nextHourWords),
            50 => string.Format(profile.MinusTenTemplate, nextHourWords),
            55 => string.Format(profile.MinusFiveTemplate, nextHourWords),
            60 => string.Format(profile.NextHourTemplate, nextHourWords),
            _ => string.Format(profile.DefaultTemplate, hourWords, normalizedMinutes.ToWords())
        };
    }

    static int NormalizeHour(TimeOnly time)
    {
        // Spoken phrases use 12 instead of 0 for the hour position.
        return time.Hour % 12 != 0 ? time.Hour % 12 : 12;
    }
}

/// <summary>
/// Stores the fixed phrases used by <see cref="PhraseHourClockNotationConverter"/>.
/// </summary>
sealed class PhraseHourClockNotationProfile(
    string midnight,
    string noon,
    string onHourTemplate,
    string quarterPastTemplate,
    string halfPastTemplate,
    string minusTwentyTemplate,
    string minusQuarterTemplate,
    string minusTenTemplate,
    string minusFiveTemplate,
    string nextHourTemplate,
    string defaultTemplate)
{
    /// <summary>Gets the phrase used for midnight.</summary>
    public string Midnight { get; } = midnight;
    /// <summary>Gets the phrase used for noon.</summary>
    public string Noon { get; } = noon;
    /// <summary>Gets the template used for exact hours.</summary>
    public string OnHourTemplate { get; } = onHourTemplate;
    /// <summary>Gets the template used for quarter past the hour.</summary>
    public string QuarterPastTemplate { get; } = quarterPastTemplate;
    /// <summary>Gets the template used for half past the hour.</summary>
    public string HalfPastTemplate { get; } = halfPastTemplate;
    /// <summary>Gets the template used for twenty minutes to the next hour.</summary>
    public string MinusTwentyTemplate { get; } = minusTwentyTemplate;
    /// <summary>Gets the template used for a quarter to the next hour.</summary>
    public string MinusQuarterTemplate { get; } = minusQuarterTemplate;
    /// <summary>Gets the template used for ten minutes to the next hour.</summary>
    public string MinusTenTemplate { get; } = minusTenTemplate;
    /// <summary>Gets the template used for five minutes to the next hour.</summary>
    public string MinusFiveTemplate { get; } = minusFiveTemplate;
    /// <summary>Gets the template used when the next hour is rendered directly.</summary>
    public string NextHourTemplate { get; } = nextHourTemplate;
    /// <summary>Gets the fallback template for non-bucketed minute values.</summary>
    public string DefaultTemplate { get; } = defaultTemplate;
}

#endif