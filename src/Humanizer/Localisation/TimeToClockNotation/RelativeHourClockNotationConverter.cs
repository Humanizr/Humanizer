#if NET6_0_OR_GREATER

namespace Humanizer;

class RelativeHourClockNotationConverter(RelativeHourClockNotationProfile profile) : ITimeOnlyToClockNotationConverter
{
    const int Morning = 6;
    const int Noon = 12;
    const int Afternoon = 21;

    readonly RelativeHourClockNotationProfile profile = profile;

    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return profile.Midnight;
            case { Hour: 12, Minute: 0 }:
                return profile.Noon;
        }

        var article = GetArticle(time);
        var articleNextHour = GetArticle(time.AddHours(1));
        var hour = NormalizeHour(time).ToWords(GrammaticalGender.Feminine);
        var nextHour = NormalizeHour(time.AddHours(1)).ToWords(GrammaticalGender.Feminine);
        var dayPeriod = GetDayPeriod(time);
        var dayPeriodNextHour = GetDayPeriod(time.AddHours(1));

        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

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

    static int NormalizeHour(TimeOnly time) =>
        time.Hour % 12 != 0 ? time.Hour % 12 : 12;

    string GetArticle(TimeOnly time) =>
        time.Hour is 1 or 13 ? profile.SingularArticle : profile.PluralArticle;

    string GetDayPeriod(TimeOnly time)
    {
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
    public string Midnight { get; } = midnight;
    public string Noon { get; } = noon;
    public string SingularArticle { get; } = singularArticle;
    public string PluralArticle { get; } = pluralArticle;
    public string PlusConnector { get; } = plusConnector;
    public string PlusQuarter { get; } = plusQuarter;
    public string PlusHalf { get; } = plusHalf;
    public string MinusTwentyFive { get; } = minusTwentyFive;
    public string MinusTwenty { get; } = minusTwenty;
    public string MinusQuarter { get; } = minusQuarter;
    public string MinusTen { get; } = minusTen;
    public string MinusFive { get; } = minusFive;
    public string EarlyMorning { get; } = earlyMorning;
    public string Morning { get; } = morning;
    public string Afternoon { get; } = afternoon;
    public string Night { get; } = night;
}

#endif
