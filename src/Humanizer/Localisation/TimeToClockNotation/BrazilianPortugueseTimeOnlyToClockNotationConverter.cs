#if NET6_0_OR_GREATER

namespace Humanizer;

class BrazilianPortugueseTimeOnlyToClockNotationConverter :
    ITimeOnlyToClockNotationConverter
{
    static CultureInfo culture = new("pt-BR");
    static INumberToWordsConverter numberToWordsConverter = Configurator.GetNumberToWordsConverter(culture);

    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return "meia-noite";
            case { Hour: 12, Minute: 0 }:
                return "meio-dia";
        }

        var normalizedHour = time.Hour % 12;
        var normalizedMinutes = NormalizedMinutes(time, roundToNearestFive);

        return normalizedMinutes switch
        {
            00 => $"{ToFeminineWords(normalizedHour)} em ponto",
            30 => $"{ToFeminineWords(normalizedHour)} e meia",
            40 => $"vinte para as {ToFeminineWords(normalizedHour + 1)}",
            45 => $"quinze para as {ToFeminineWords(normalizedHour + 1)}",
            50 => $"dez para as {ToFeminineWords(normalizedHour + 1)}",
            55 => $"cinco para as {ToFeminineWords(normalizedHour + 1)}",
            60 => $"{ToFeminineWords(normalizedHour + 1)} em ponto",
            _ => $"{ToFeminineWords(normalizedHour)} e {ToMasculineWords(normalizedMinutes)}"
        };
    }

    static string ToFeminineWords(int normalizedHour) =>
        numberToWordsConverter.Convert(normalizedHour, GrammaticalGender.Feminine);

    static string ToMasculineWords(int normalizedHour) =>
        numberToWordsConverter.Convert(normalizedHour, GrammaticalGender.Masculine);

    static int NormalizedMinutes(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        if (roundToNearestFive == ClockNotationRounding.NearestFiveMinutes)
        {
            return (int) (5 * Math.Round(time.Minute / 5.0));
        }

        return time.Minute;
    }
}

#endif