#if NET6_0_OR_GREATER

namespace Humanizer;

class PhraseHourClockNotationConverter(PhraseHourClockNotationProfile profile) : ITimeOnlyToClockNotationConverter
{
    readonly PhraseHourClockNotationProfile profile = profile;

    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return profile.Midnight;
            case { Hour: 12, Minute: 0 }:
                return profile.Noon;
        }

        var normalizedHour = NormalizeHour(time);
        var nextHour = NormalizeHour(time.AddHours(1));
        var hourWords = normalizedHour.ToWords(GrammaticalGender.Feminine);
        var nextHourWords = nextHour.ToWords(GrammaticalGender.Feminine);

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

    static int NormalizeHour(TimeOnly time) =>
        time.Hour % 12 != 0 ? time.Hour % 12 : 12;
}

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
    public string Midnight { get; } = midnight;
    public string Noon { get; } = noon;
    public string OnHourTemplate { get; } = onHourTemplate;
    public string QuarterPastTemplate { get; } = quarterPastTemplate;
    public string HalfPastTemplate { get; } = halfPastTemplate;
    public string MinusTwentyTemplate { get; } = minusTwentyTemplate;
    public string MinusQuarterTemplate { get; } = minusQuarterTemplate;
    public string MinusTenTemplate { get; } = minusTenTemplate;
    public string MinusFiveTemplate { get; } = minusFiveTemplate;
    public string NextHourTemplate { get; } = nextHourTemplate;
    public string DefaultTemplate { get; } = defaultTemplate;
}

#endif
