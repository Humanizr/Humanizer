namespace Humanizer;

class FilipinoNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["sero", "isa", "dalawa", "tatlo", "apat", "lima", "anim", "pito", "walo", "siyam"];
    static readonly string[] TensMap = ["", "sampu", "dalawampu", "tatlumpu", "apatnapu", "limampu", "animnapu", "pitumpu", "walumpu", "siyamnapu"];

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return UnitsMap[0];
        }

        if (number < 0)
        {
            return $"minus {Convert(-number)}";
        }

        if (number < 10)
        {
            return UnitsMap[number];
        }

        if (number == 10)
        {
            return TensMap[1];
        }

        if (number < 20)
        {
            return $"labing-{UnitsMap[number - 10]}";
        }

        if (number < 100)
        {
            var tens = TensMap[number / 10];
            var units = number % 10;
            return units == 0 ? tens : $"{tens}'t {UnitsMap[units]}";
        }

        if (number < 1000)
        {
            var hundreds = Quantified(number / 100, "daan");
            var remainder = number % 100;
            return JoinMajorGroup(hundreds, remainder);
        }

        if (number < 1_000_000)
        {
            var thousands = Quantified(number / 1000, "libo");
            var remainder = number % 1000;
            return JoinMajorGroup(thousands, remainder);
        }

        if (number < 1_000_000_000)
        {
            var millions = Quantified(number / 1_000_000, "milyon");
            var remainder = number % 1_000_000;
            return JoinMajorGroup(millions, remainder);
        }

        var billions = Quantified(number / 1_000_000_000, "bilyon");
        var billionRemainder = number % 1_000_000_000;
        return JoinMajorGroup(billions, billionRemainder);
    }

    public override string ConvertToOrdinal(int number)
    {
        if (number < 0)
        {
            return $"minus {ConvertToOrdinal(-number)}";
        }

        return number switch
        {
            1 => "una",
            2 => "ikalawa",
            3 => "ikatlo",
            4 => "ikaapat",
            5 => "ikalima",
            6 => "ikaanim",
            7 => "ikapito",
            8 => "ikawalo",
            9 => "ikasiyam",
            10 => "ikasampu",
            _ => $"ika{Convert(number)}"
        };
    }

    static string Quantified(long number, string unit)
    {
        if (number == 1)
        {
            return $"isang {unit}";
        }

        if (number < 10)
        {
            return $"{Link(number)} {unit}";
        }

        return $"{LinkPhrase(ConvertStatic(number))} {unit}";
    }

    static string Link(long number) =>
        number switch
        {
            1 => "isang",
            2 => "dalawang",
            3 => "tatlong",
            4 => "apat na",
            5 => "limang",
            6 => "anim na",
            7 => "pitong",
            8 => "walong",
            9 => "siyam na",
            _ => ConvertStatic(number)
        };

    static string ConvertStatic(long number) =>
        new FilipinoNumberToWordsConverter().Convert(number);

    static string JoinMajorGroup(string prefix, long remainder)
    {
        if (remainder == 0)
        {
            return prefix;
        }

        var joiner = remainder >= 100
            ? " "
            : prefix.EndsWith("libo", StringComparison.Ordinal) ? "'t " : " at ";
        return $"{prefix}{joiner}{ConvertStatic(remainder)}";
    }

    static string LinkPhrase(string phrase)
    {
        var trimmed = phrase.TrimEnd();
        if (trimmed.EndsWith('n'))
        {
            return $"{trimmed}g";
        }

        return "aeiou".Contains(trimmed[^1]) ? $"{trimmed}ng" : $"{trimmed} na";
    }
}
