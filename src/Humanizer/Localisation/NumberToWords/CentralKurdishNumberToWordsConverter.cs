namespace Humanizer;

class CentralKurdishNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] KurdishHundredsMap = ["سفر", "سەد", "دوو سەد", "سێ سەد", "چوار سەد", "پێنج سەد", "شەش سەد", "حەوت سەد", "هەشت سەد", "نۆ سەد"];
    static readonly string[] KurdishTensMap = ["سفر", "دە", "بیست", "سی", "چل", "پەنجا", "شەست", "حەفتا", "هەشتا", "نەوەد"];
    static readonly string[] KurdishUnitsMap = ["سفر", "یەک", "دوو", "سێ", "چوار", "پێنج", "شەش", "حەوت", "هەشت", "نۆ", "دە", "یازدە", "دوازدە", "سێزدە", "چواردە", "پازدە", "شازدە", "حەڤدە", "هەژدە", "نۆزدە"];

    public override string Convert(long number)
    {
        var largestNumber = Math.Pow(10, 15) * 1000 - 1;
        if (number > largestNumber || number < -largestNumber)
        {
            throw new NotImplementedException();
        }

        if (number < 0)
        {
            return $"نێگەتیڤ {Convert(-number)}";
        }

        if (number == 0)
        {
            return "سفر";
        }

        var kurdishGroupsMap = new Dictionary<long, Func<long, string>>
        {
            {
                (long) Math.Pow(10, 15), n => $"{Convert(n)} کوادریلیۆن"
            },
            {
                (long) Math.Pow(10, 12), n => $"{Convert(n)} تریلیۆن"
            },
            {
                (long) Math.Pow(10, 9), n => $"{Convert(n)} میلیارد"
            },
            {
                (long) Math.Pow(10, 6), n => $"{Convert(n)} میلیۆن"
            },
            {
                (long) Math.Pow(10, 3), n => $"{Convert(n)} هەزار"
            },
            {
                (long) Math.Pow(10, 2), n => KurdishHundredsMap[n]
            }
        };

        var parts = new List<string>();
        foreach (var group in kurdishGroupsMap.Keys)
        {
            if (number / group > 0)
            {
                parts.Add(kurdishGroupsMap[group](number / group));
                number %= group;
            }
        }

        if (number >= 20)
        {
            parts.Add(KurdishTensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(KurdishUnitsMap[number]);
        }

        var sentence = string.Join(" و ", parts);
        if (sentence.StartsWith("یەک هەزار"))
        {
            return sentence[" یەک".Length..];
        }

        return sentence;
    }

    public override string ConvertToOrdinal(int number)
    {
        var word = Convert(number);
        return $"{word}{(IsVowel(word[^1]) ? "یەم" : "ەم")}";
    }

    static bool IsVowel(char c) =>
        c is 'ا' or 'ێ' or 'ۆ' or 'ە' or 'ی';
}