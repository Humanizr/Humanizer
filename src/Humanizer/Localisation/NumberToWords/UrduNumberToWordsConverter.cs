namespace Humanizer;

/// <summary>
/// Urdu number to words converter.
/// Supports cardinal and ordinal numbers in Urdu script.
/// 
/// Urdu numbering follows a similar pattern to Hindi/Hindustani
/// with unique words for numbers 1-100 and multiplicative grouping
/// for larger numbers (ہزار, لاکھ, کروڑ, ارب).
/// 
/// Note: Urdu uses the South Asian numbering system (lakh/crore)
/// rather than the Western million/billion system.
/// </summary>
class UrduNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap =
    [
        "صفر", "ایک", "دو", "تین", "چار", "پانچ", "چھ", "سات", "آٹھ", "نو", "دس",
        "گیارہ", "بارہ", "تیرہ", "چودہ", "پندرہ", "سولہ", "سترہ", "اٹھارہ", "انیس",
        "بیس", "اکیس", "بائیس", "تئیس", "چوبیس", "پچیس", "چھبیس", "ستائیس", "اٹھائیس", "انتیس",
        "تیس", "اکتیس", "بتیس", "تینتیس", "چونتیس", "پینتیس", "چھتیس", "سینتیس", "اڑتیس", "انتالیس",
        "چالیس", "اکتالیس", "بیالیس", "تینتالیس", "چوالیس", "پینتالیس", "چھیالیس", "سینتالیس", "اڑتالیس", "انچاس",
        "پچاس", "اکیاون", "باون", "تریپن", "چون", "پچپن", "چھپن", "ستاون", "اٹھاون", "انسٹھ",
        "ساٹھ", "اکسٹھ", "باسٹھ", "تریسٹھ", "چونسٹھ", "پینسٹھ", "چھیاسٹھ", "سڑسٹھ", "اڑسٹھ", "انہتر",
        "ستر", "اکہتر", "بہتر", "تہتر", "چوہتر", "پچہتر", "چھہتر", "ستتر", "اٹھہتر", "انیاسی",
        "اسی", "اکیاسی", "بیاسی", "تراسی", "چوراسی", "پچاسی", "چھیاسی", "ستاسی", "اٹھاسی", "نواسی",
        "نوے", "اکیانوے", "بانوے", "ترانوے", "چورانوے", "پچانوے", "چھیانوے", "ستانوے", "اٹھانوے", "ننانوے"
    ];

    static readonly string[] HundredsMap =
    [
        "", "ایک سو", "دو سو", "تین سو", "چار سو", "پانچ سو",
        "چھ سو", "سات سو", "آٹھ سو", "نو سو"
    ];

    // South Asian numbering: ہزار (1,000), لاکھ (100,000), کروڑ (10,000,000), ارب (1,000,000,000)
    static readonly (long Divisor, string Name)[] Groups =
    [
        (1_000_000_000_000_000, "نیل"),
        (1_000_000_000_000, "کھرب"),
        (1_00_00_00_000, "ارب"),
        (1_00_00_000, "کروڑ"),
        (1_00_000, "لاکھ"),
        (1_000, "ہزار")
    ];

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return "صفر";
        }

        if (number == long.MinValue)
        {
            return "منفی نو نیل دو کھرب تئیس ارب سینتیس کروڑ چھتیس لاکھ پچاسی ہزار چار سو پچہتر ارب سات سو آٹھ";
        }

        if (number < 0)
        {
            return $"منفی {Convert(-number)}";
        }

        var parts = new List<string>();

        foreach (var (divisor, name) in Groups)
        {
            if (number / divisor > 0)
            {
                var groupValue = number / divisor;
                parts.Add($"{Convert(groupValue)} {name}");
                number %= divisor;
            }
        }

        if (number >= 100)
        {
            parts.Add(HundredsMap[number / 100]);
            number %= 100;
        }

        if (number > 0)
        {
            parts.Add(UnitsMap[number]);
        }

        return string.Join(" ", parts);
    }

    static string ConvertUpTo99(long number)
    {
        if (number < 100)
        {
            return UnitsMap[number];
        }

        var result = HundredsMap[number / 100];
        number %= 100;

        if (number > 0)
        {
            result += " " + UnitsMap[number];
        }

        return result;
    }

    static readonly string[] OrdinalSuffixes =
    [
        "واں", "پہلا", "دوسرا", "تیسرا", "چوتھا", "پانچواں",
        "چھٹا", "ساتواں", "آٹھواں", "نواں", "دسواں"
    ];

    public override string ConvertToOrdinal(int number)
    {
        if (number <= 0)
        {
            return Convert(number);
        }

        // Special cases for 1-10
        if (number <= 10)
        {
            return OrdinalSuffixes[number];
        }

        // For numbers > 10, append واں
        return Convert(number) + "واں";
    }
}
