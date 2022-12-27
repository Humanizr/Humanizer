#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Humanizer.Localisation.TimeToClockNotation;

public class DeTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    /// <summary> Switch to output Digits as Words </summary>
    public bool AsWords;

    /// <summary> Used to pre-pend, append or omit the Quarter of the Day </summary>
    public bool? PrePendQuarter = false;

    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        var ret = time.AsClockDe(roundToNearestFive, AsWords);
        if (!PrePendQuarter.HasValue
            || ret == German.MidNight
            || ret == German.MidDay)
        {
            return ret;
        }
        return PrePendQuarter switch
        {
            false => ret + ' ' + time.GetDayQuarterGermanGenitive(),
            true => time.GetDayQuarterGermanGenitive() + ' ' + ret, _ => ret
        };
    }
}

public static class German
{
    public const string MidNight = "Mitternacht";
    public const string MidDay = "Mittag";

    public static readonly IReadOnlyList<string> QuarterDay = new[] {"Nacht", "Morgen", "Nachmittag", "Abend"};

    public static string GetDayQuarterGerman(this TimeOnly time) => QuarterDay[time.Hour / 6];
    public static string GetDayQuarterGermanGenitive(this TimeOnly time) => GetDayQuarterGerman(time).ToLower() + "s";

    /// <summary> Use 12 Hours but avoid 0 </summary>
    private static int NormalizeHour(TimeOnly time) => time.Hour % 12 != 0 ? (time.Hour % 12) : 12;

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public static string AsClockDe(this TimeOnly time, ClockNotationRounding round, bool asWords = false)
    {
        if (round == ClockNotationRounding.NearestFiveMinutes)
        {
            var ticks = 5 * TimeSpan.TicksPerMinute;
            var quotient = (time.Ticks + ticks / 2) / ticks;
            var total = quotient * ticks;
            if (total >= TimeSpan.TicksPerDay)
            {
                total -= TimeSpan.TicksPerDay;
            }
            time = new TimeOnly(total);
        }
        switch (time)
        {
            case { Hour: 0, Minute: 0 }: return MidNight;
            case { Hour: 12, Minute: 0 }: return MidDay;
        }

        var addHour = time.AddHours(1);
        var hours = NormalizeHour(time);
        var hour = asWords ? hours.ToWords() : hours.ToString();
        var nextHours = NormalizeHour(addHour);
        var nextHour = asWords ? nextHours.ToWords() : nextHours.ToString();

        return time.Minute switch
        {
            00 => $"{hour} Uhr",
            05 => $"fünf nach {hour}",
            10 => $"zehn nach {hour}",
            15 => $"Viertel nach {hour}",
            20 => $"zwanzig nach {hour}",
            25 => $"fünf vor halb {nextHour}",
            30 => $"halb {nextHour}",
            35 => $"fünf nach halb {nextHour}",
            40 => $"zwanzig vor {nextHour}",
            45 => $"Viertel vor {nextHour}",
            50 => $"zehn vor {nextHour}",
            55 => $"fünf vor {nextHour}",
            60 => $"{nextHour} Uhr",
            _ => $"{hour} Uhr {time.Minute}" //.AsWords()
        };
    }

}

#endif
