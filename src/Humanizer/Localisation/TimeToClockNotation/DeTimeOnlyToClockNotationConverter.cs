#if NET6_0_OR_GREATER

using System;

namespace Humanizer;

internal class DeTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
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
            return ret;

        return PrePendQuarter.Value switch
        {
            false => ret + ' ' + time.GetDayQuarterGermanGenitive(),
            true => time.GetDayQuarterGermanGenitive() + ' ' + ret
        };
    }
}

#endif
