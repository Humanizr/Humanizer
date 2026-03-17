namespace Humanizer;

using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

internal partial class GermanWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    private const string CompoundNumberPattern = @"(?<unit>ein|eins|eine|einen|zwei|drei|vier|fünf|sechs|sieben|acht|neun|zehn|elf|zwölf|dreizehn|vierzehn|fünfzehn|sechzehn|siebzehn|achtzehn|neunzehn)und(?<ten>zwanzig|dreißig|dreissig|vierzig|fünfzig|sechzig|siebzig|achtzig|neunzig)";
    private const string ScalePattern = @"(hundert|tausend|million(en)?|milliarde(n)?)";

    private static readonly FrozenDictionary<string, int> NumbersMap = new Dictionary<string, int>
    {
        {"null",0}, {"ein",1}, {"eins",1}, {"eine",1}, {"einen",1}, {"zwei",2}, {"drei",3}, {"vier",4}, {"fünf",5},
        {"sechs",6}, {"sieben",7}, {"acht",8}, {"neun",9}, {"zehn",10}, {"elf",11}, {"zwölf",12}, {"dreizehn",13},
        {"vierzehn",14}, {"fünfzehn",15}, {"sechzehn",16}, {"siebzehn",17}, {"achtzehn",18}, {"neunzehn",19},
        {"zwanzig",20}, {"dreißig",30}, {"dreissig",30}, {"vierzig",40}, {"fünfzig",50}, {"sechzig",60}, {"siebzig",70}, {"achtzig",80},
        {"neunzig",90}, {"hundert",100}, {"tausend",1000}, {"million",1_000_000}, {"millionen",1_000_000}, {"milliarde",1_000_000_000},
        {"milliarden",1_000_000_000}
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<string, int> OrdinalsMap = new Dictionary<string, int>
    {
        {"erste",1}, {"erster",1}, {"erstes",1}, {"zweite",2}, {"zweiter",2}, {"zweites",2},
        {"dritte",3}, {"dritter",3}, {"drittes",3}, {"vierte",4}, {"vierter",4}, {"viertes",4},
        {"fünfte",5}, {"fünfter",5}, {"fünftes",5}, {"sechste",6}, {"sechster",6}, {"sechstes",6},
        {"siebte",7}, {"siebter",7}, {"siebtes",7}, {"achte",8}, {"achter",8}, {"achtes",8},
        {"neunte",9}, {"neunter",9}, {"neuntes",9}, {"zehnte",10}, {"zehnter",10}, {"zehntes",10},
        {"elfte",11}, {"zwölfte",12}, {"dreizehnte",13}, {"vierzehnte",14}, {"fünfzehnte",15}, {"sechzehnte",16},
        {"siebzehnte",17}, {"achtzehnte",18}, {"neunzehnte",19}, {"zwanzigste",20}, {"einundzwanzigste",21},
        {"einundzwanzigster",21}, {"einundzwanzigstes",21}, {"einundzwanzigsten",21}, {"dreißigste",30},
        {"dreissigste",30}, {"dreissigster",30}, {"dreissigstes",30}, {"dreißigsten",30}
    }.ToFrozenDictionary();

#if NET7_0_OR_GREATER
    [GeneratedRegex(CompoundNumberPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CompoundNumberRegexGenerated();

    [GeneratedRegex(ScalePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex ScaleRegexGenerated();

    private static Regex CompoundNumberRegex() => CompoundNumberRegexGenerated();
    private static Regex ScaleRegex() => ScaleRegexGenerated();
#else
    private static readonly Regex CompoundNumberRegexField = new(CompoundNumberPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
    private static readonly Regex ScaleRegexField = new(ScalePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static Regex CompoundNumberRegex() => CompoundNumberRegexField;
    private static Regex ScaleRegex() => ScaleRegexField;
#endif

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var result, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return result;
    }

    public override bool TryConvert(string words, out int parsedValue) => TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        unrecognizedWord = null;

        var normalized = Normalize(words);
        var isNegative = normalized.StartsWith("minus ", StringComparison.Ordinal) || normalized.StartsWith("negativ ", StringComparison.Ordinal);
        if (isNegative)
        {
            normalized = normalized.Replace("minus ", "", StringComparison.Ordinal);
            normalized = normalized.Replace("negativ ", "", StringComparison.Ordinal);
            normalized = normalized.Trim();
        }

        if (int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out var numericValue))
        {
            parsedValue = isNegative ? -numericValue : numericValue;
            return true;
        }

        if (OrdinalsMap.TryGetValue(normalized, out var ordinalValue))
        {
            parsedValue = isNegative ? -ordinalValue : ordinalValue;
            return true;
        }

        if (TryConvertWordsToNumber(normalized, out var numberValue, out var unrecognizedNumberWord))
        {
            parsedValue = isNegative ? -numberValue : numberValue;
            return true;
        }

        unrecognizedWord = unrecognizedNumberWord;
        parsedValue = default;
        return false;
    }

    private static string Normalize(string words) =>
        Regex.Replace(
            words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    private static bool TryConvertWordsToNumber(string words, out int result, out string? unrecognizedWord)
    {
        var wordsArray = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        result = 0;
        unrecognizedWord = null;
        var current = 0;
        var hasOrdinal = false;

        foreach (var word in wordsArray)
        {
            if (OrdinalsMap.TryGetValue(word, out var ordinalValue))
            {
                result += current + ordinalValue;
                hasOrdinal = true;
                break;
            }

            if (!TryParseGermanNumberWord(word, out var value))
            {
                unrecognizedWord = word;
                return false;
            }

            if (value == 100)
            {
                current = (current == 0 ? 1 : current) * 100;
            }
            else if (value >= 1000)
            {
                result += (current == 0 ? 1 : current) * value;
                current = 0;
            }
            else
            {
                current += value;
            }
        }

        if (!hasOrdinal)
        {
            result += current;
        }

        return true;
    }

    private static bool TryParseGermanNumberWord(string word, out int value)
    {
        if (NumbersMap.TryGetValue(word, out value) || OrdinalsMap.TryGetValue(word, out value))
        {
            return true;
        }

        if (TryParseOrdinalStem(word, out value))
        {
            return true;
        }

        var thousandIndex = word.IndexOf("tausend", StringComparison.Ordinal);
        if (thousandIndex > 0)
        {
            var thousandsPart = word[..thousandIndex];
            var remainderPart = word[(thousandIndex + "tausend".Length)..];

            if (TryParseGermanNumberWord(thousandsPart, out var thousands) &&
                TryParseOptionalRemainder(remainderPart, out var remainder))
            {
                value = thousands * 1000 + remainder;
                return true;
            }
        }

        var hundredIndex = word.IndexOf("hundert", StringComparison.Ordinal);
        if (hundredIndex > 0)
        {
            var hundredsPart = word[..hundredIndex];
            var remainderPart = word[(hundredIndex + "hundert".Length)..];

            if (TryParseGermanNumberWord(hundredsPart, out var hundreds) &&
                TryParseOptionalRemainder(remainderPart, out var remainder))
            {
                value = hundreds * 100 + remainder;
                return true;
            }
        }

        var match = CompoundNumberRegex().Match(word);
        if (match.Success && match.Value == word &&
            TryParseGermanNumberWord(match.Groups["unit"].Value, out var unit) &&
            NumbersMap.TryGetValue(match.Groups["ten"].Value, out var tens))
        {
            value = unit + tens;
            return true;
        }

        value = default;
        return false;
    }

    private static bool TryParseOrdinalStem(string word, out int value)
    {
        foreach (var suffix in new[] { "ste", "sten", "ster", "stes", "te", "ten", "ter", "tes" })
        {
            if (word.EndsWith(suffix, StringComparison.Ordinal) && word.Length > suffix.Length)
            {
                return TryParseGermanNumberWord(word[..^suffix.Length], out value);
            }
        }

        value = default;
        return false;
    }

    private static bool TryParseOptionalRemainder(string word, out int value)
    {
        if (string.IsNullOrEmpty(word))
        {
            value = 0;
            return true;
        }

        return TryParseGermanNumberWord(word, out value);
    }
}
