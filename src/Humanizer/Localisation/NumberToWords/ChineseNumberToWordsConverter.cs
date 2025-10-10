namespace Humanizer;

class ChineseNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"];

    public override string Convert(long number) =>
        Convert(number, false, IsSpecial(number));

    public override string ConvertToOrdinal(int number) =>
        Convert(number, true, IsSpecial(number));

    static bool IsSpecial(long number) => number is > 10 and < 20;

    static string Convert(long number, bool isOrdinal, bool isSpecial)
    {
        if (number == 0)
        {
            return UnitsMap[0];
        }

        if (number < 0)
        {
            return $"负 {Convert(-number, false, false)}";
        }

        var parts = new List<string>();

        if (number / 1000000000000 > 0)
        {
            var format = "{0}兆";
            if (number % 1000000000000 < 100000000000 && number % 1000000000000 > 0)
            {
                format = "{0}兆零";
            }

            parts.Add(string.Format(format, Convert(number / 1000000000000, false, false)));
            number %= 1000000000000;
        }

        if (number / 100000000 > 0)
        {
            var format = "{0}亿";
            if (number % 100000000 < 10000000 && number % 100000000 > 0)
            {
                format = "{0}亿零";
            }

            parts.Add(string.Format(format, Convert(number / 100000000, false, false)));
            number %= 100000000;
        }

        if (number / 10000 > 0)
        {
            var format = "{0}万";
            if (number % 10000 < 1000 && number % 10000 > 0)
            {
                format = "{0}万零";
            }

            parts.Add(string.Format(format, Convert(number / 10000, false, false)));
            number %= 10000;
        }

        if (number / 1000 > 0)
        {
            var format = "{0}千";
            if (number % 1000 < 100 && number % 1000 > 0)
            {
                format = "{0}千零";
            }

            parts.Add(string.Format(format, Convert(number / 1000, false, false)));
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            var format = "{0}百";
            if (number % 100 < 10 && number % 100 > 0)
            {
                format = "{0}百零";
            }

            parts.Add(string.Format(format, Convert(number / 100, false, false)));
            number %= 100;
        }

        if (number > 0)
        {
            if (number <= 10)
            {
                parts.Add(UnitsMap[number]);
            }
            else
            {
                var lastPart = $"{UnitsMap[number / 10]}十";
                if (number % 10 > 0)
                {
                    lastPart += $"{UnitsMap[number % 10]}";
                }

                parts.Add(lastPart);
            }
        }

        var toWords = string.Concat(parts);

        if (isSpecial)
        {
            toWords = toWords[1..];
        }

        if (isOrdinal)
        {
            toWords = $"第 {toWords}";
        }

        return toWords;
    }
}