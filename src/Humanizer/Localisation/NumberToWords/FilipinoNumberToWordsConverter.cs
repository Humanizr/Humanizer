using System.Collections.Generic;

namespace Humanizer;

class FilipinoNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] Units =
    {
        "sero", "isa", "dalawa", "tatlo", "apat", "lima", "anim", "pito", "walo", "siyam", "sampu",
        "labing-isa", "labing-dalawa", "labing-tatlo", "labing-apat", "labing-lima", "labing-anim", "labing-pito",
        "labing-walo", "labing-siyam"
    };

    static readonly string[] Tens =
    {
        "", "", "dalawampu", "tatlumpu", "apatnapu", "limampu", "animnapu", "pitumpu", "walumpu", "siyamnapu"
    };

    static readonly Dictionary<long, string> HundredWords = new()
    {
        [1] = "isang daan",
        [2] = "dalawang daan",
        [3] = "tatlong daan",
        [4] = "apat na daan",
        [5] = "limang daan",
        [6] = "anim na daan",
        [7] = "pitong daan",
        [8] = "walong daan",
        [9] = "siyam na daan"
    };

    static readonly Dictionary<long, string> ScalePrefixes = new()
    {
        [1] = "isang",
        [2] = "dalawang",
        [3] = "tatlong",
        [4] = "apat na",
        [5] = "limang",
        [6] = "anim na",
        [7] = "pitong",
        [8] = "walong",
        [9] = "siyam na"
    };

    static readonly (long Value, string Name)[] Scales =
    {
        (1_000_000_000_000_000_000, "kwintilyon"),
        (1_000_000_000_000_000, "kwadrilyon"),
        (1_000_000_000_000, "trilyon"),
        (1_000_000_000, "bilyon"),
        (1_000_000, "milyon"),
        (1_000, "libo")
    };

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return Units[0];
        }

        if (number < 0)
        {
            return $"minus {Convert(-number)}";
        }

        var parts = new List<string>();
        var remainder = number;

        foreach (var scale in Scales)
        {
            var part = remainder / scale.Value;
            if (part > 0)
            {
                remainder %= scale.Value;
                parts.Add(BuildScalePart(part, scale.Name));
            }
        }

        if (remainder > 0)
        {
            parts.Add(ConvertUnderThousand(remainder));
        }

        return string.Join(" ", parts);
    }

    static string BuildScalePart(long part, string scale)
    {
        if (ScalePrefixes.TryGetValue(part, out var prefix))
        {
            return $"{prefix} {scale}";
        }

        return $"{ApplyScaleLinking(ConvertUnderThousand(part))} {scale}";
    }

    static string ConvertUnderThousand(long number)
    {
        if (number < 100)
        {
            return ConvertUnderHundred(number);
        }

        var hundreds = number / 100;
        var remainder = number % 100;
        var parts = new List<string> { HundredWords[hundreds] };

        if (remainder > 0)
        {
            parts.Add($"at {ConvertUnderHundred(remainder)}");
        }

        return string.Join(" ", parts);
    }

    static string ConvertUnderHundred(long number)
    {
        if (number < Units.Length)
        {
            return Units[number];
        }

        var tens = (int)(number / 10);
        var remainder = number % 10;
        var tensWord = Tens[tens];

        return remainder == 0
            ? tensWord
            : $"{tensWord}'t {Units[remainder]}";
    }

    static string ApplyScaleLinking(string words)
    {
        if (words.EndsWith(" isa", StringComparison.Ordinal))
        {
            return $"{words[..^4]} isang";
        }

        if (words.EndsWith(" dalawa", StringComparison.Ordinal))
        {
            return $"{words[..^7]} dalawang";
        }

        if (words.EndsWith(" tatlo", StringComparison.Ordinal))
        {
            return $"{words[..^6]} tatlong";
        }

        if (words.EndsWith(" apat", StringComparison.Ordinal))
        {
            return $"{words[..^5]} apat na";
        }

        if (words.EndsWith(" lima", StringComparison.Ordinal))
        {
            return $"{words[..^5]} limang";
        }

        if (words.EndsWith(" anim", StringComparison.Ordinal))
        {
            return $"{words[..^5]} anim na";
        }

        if (words.EndsWith(" pito", StringComparison.Ordinal))
        {
            return $"{words[..^5]} pitong";
        }

        if (words.EndsWith(" walo", StringComparison.Ordinal))
        {
            return $"{words[..^5]} walong";
        }

        if (words.EndsWith(" siyam", StringComparison.Ordinal))
        {
            return $"{words[..^6]} siyam na";
        }

        return words;
    }

    public override string ConvertToOrdinal(int number) => Convert(number);
}
