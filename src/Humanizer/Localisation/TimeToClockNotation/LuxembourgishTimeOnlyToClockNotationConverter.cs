#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Provides Luxembourgish clock notation with half-hour phrases and inflected number forms.
/// </summary>
class LuxembourgishTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    /// <summary>
    /// Converts the given time to Luxembourgish clock notation.
    /// </summary>
    /// <returns>The localized clock-notation string.</returns>
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        // Luxembourgish shares the same broad half-hour family as German, but it also needs
        // morphology-aware number rendering ("eng"/"zwou", Eifeler handling for seven, singular
        // vs plural minute nouns). That additional inflection logic is why it remains a dedicated
        // leaf for now instead of being forced into a weaker generic schema.
        // Round first so every downstream branch can reason over a single time value and the
        // minute buckets do not need to special-case both rounded and raw inputs.
        var roundedTime = roundToNearestFive is ClockNotationRounding.NearestFiveMinutes
            ? GetRoundedTime(time)
            : time;

        // The nested switch mirrors the spoken forms directly: exact hour, quarter, half-hour, and
        // the handful of minute-word patterns that require special grammar.
        return roundedTime switch
        {
            { Hour: 0, Minute: 0 } => "Mëtternuecht",
            { Hour: 12, Minute: 0 } => "Mëtteg",
            _ => roundedTime.Minute switch
            {
                00 => GetHourExpression(roundedTime.Hour, "Auer"),
                15 => $"Véirel op {GetHourExpression(roundedTime.Hour)}",
                25 => $"{GetMinuteExpression(30 - roundedTime.Minute, "vir")} hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                30 => $"hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                35 => $"{GetMinuteExpression(roundedTime.Minute - 30, "op")} hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                45 => $"Véirel vir {GetHourExpression(roundedTime.Hour + 1)}",
                01 => $"{GetMinuteExpression(roundedTime.Minute, "Minutt")} op {GetHourExpression(roundedTime.Hour)}",
                59 => $"{GetMinuteExpression(60 - roundedTime.Minute, "Minutt")} vir {GetHourExpression(roundedTime.Hour + 1)}",
                05 or 10 or 20 => $"{GetMinuteExpression(roundedTime.Minute, "op")} {GetHourExpression(roundedTime.Hour)}",
                40 or 50 or 55 => $"{GetMinuteExpression(60 - roundedTime.Minute, "vir")} {GetHourExpression(roundedTime.Hour + 1)}",
                > 00 and < 25 => $"{GetMinuteExpression(roundedTime.Minute, "Minutten")} op {GetHourExpression(roundedTime.Hour)}",
                > 25 and < 30 => $"{GetMinuteExpression(30 - roundedTime.Minute, "Minutten")} vir hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                > 30 and < 35 => $"{GetMinuteExpression(roundedTime.Minute - 30, "Minutten")} op hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                > 35 and < 60 => $"{GetMinuteExpression(60 - roundedTime.Minute, "Minutten")} vir {GetHourExpression(roundedTime.Hour + 1)}",
                _ => $"{GetHourExpression(time.Hour, "Auer")} {GetMinuteExpression(time.Minute)}"
            }
        };
    }

    // Rounding is performed before phrase selection so the rest of the converter can reason over
    // the spoken-minute buckets instead of carrying dual rounded/unrounded branches.
    private static TimeOnly GetRoundedTime(TimeOnly time)
    {
        // Rounding can push the minute component to 60; convert that to the next hour so the rest
        // of the converter can keep matching against ordinary clock values.
        var tempRoundedMinutes = (int)(5 * Math.Round(time.Minute / 5.0));
        var roundedHours = tempRoundedMinutes == 60 ? time.Hour + 1 : time.Hour;
        var roundedMinutes = tempRoundedMinutes == 60 ? 0 : tempRoundedMinutes;
        return new(roundedHours, roundedMinutes);
    }

    private static string GetMinuteExpression(int minute, string nextWord = "")
        => GetFormattedExpression(minute, nextWord);

    private static string GetHourExpression(int hour, string nextWord = "")
    {
        var normalizedHour = hour % 12;
        var hourExpression = normalizedHour == 0 ? 12 : normalizedHour;

        return GetFormattedExpression(hourExpression, nextWord);
    }

    // 1, 2, and 7 trigger special feminine or Eifeler forms; everything else uses the normal
    // cardinal rendering plus an optional suffix.
    private static string GetFormattedExpression(int number, string nextWord) =>
        (number switch
        {
            // The forms for 1, 2, and 7 are morphologically special; the fallback would be wrong
            // even if it looks simpler.
            1 or 2 => $"{number.ToWords(GrammaticalGender.Feminine)} {nextWord}",
            7 => $"{number.ToWords(EifelerRule.DoesApply(nextWord) ? WordForm.Eifeler : WordForm.Normal)} {nextWord}",
            _ => $"{number.ToWords()} {nextWord}"
        }).TrimEnd();
}

#endif