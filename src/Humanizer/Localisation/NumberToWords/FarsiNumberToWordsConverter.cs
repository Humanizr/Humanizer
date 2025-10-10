namespace Humanizer;

class FarsiNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] FarsiHundredsMap = ["صفر", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد"];
    static readonly string[] FarsiTensMap = ["صفر", "ده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود"];
    static readonly string[] FarsiUnitsMap = ["صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه", "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده"];

    public override string Convert(long number)
    {
        if (number < 0)
        {
            return $"منفی {Convert(-number)}";
        }

        if (number == 0)
        {
            return "صفر";
        }

        var farsiGroupsMap = new Dictionary<long, Func<long, string>>
        {
            {
                (long) Math.Pow(10, 18), n => $"{Convert(n)} تریلیون"
            },
            {
                (long) Math.Pow(10, 15), n => $"{Convert(n)} بیلیارد"
            },
            {
                (long) Math.Pow(10, 12), n => $"{Convert(n)} بیلیون"
            },
            {
                (long) Math.Pow(10, 9), n => $"{Convert(n)} میلیارد"
            },
            {
                (long) Math.Pow(10, 6), n => $"{Convert(n)} میلیون"
            },
            {
                (long) Math.Pow(10, 3), n => $"{Convert(n)} هزار"
            },
            {
                (long) Math.Pow(10, 2), n => FarsiHundredsMap[n]
            }
        };

        var parts = new List<string>();
        foreach (var group in farsiGroupsMap.Keys)
        {
            if (number / group > 0)
            {
                parts.Add(farsiGroupsMap[group](number / group));
                number %= group;
            }
        }

        if (number >= 20)
        {
            parts.Add(FarsiTensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(FarsiUnitsMap[number]);
        }

        return string.Join(" و ", parts);
    }

    public override string ConvertToOrdinal(int number)
    {
        if (number == 1)
        {
            return "اول";
        }

        if (number == 3)
        {
            return "سوم";
        }

        if (number % 10 == 3 && number != 13)
        {
            return Convert(number / 10 * 10) + " و سوم";
        }

        var word = Convert(number);
        return $"{word}{(word.EndsWith('ی') ? " ام" : "م")}";
    }
}