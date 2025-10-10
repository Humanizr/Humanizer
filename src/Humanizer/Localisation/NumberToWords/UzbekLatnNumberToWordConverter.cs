namespace Humanizer;

class UzbekLatnNumberToWordConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["nol", "bir", "ikki", "uch", "to`rt", "besh", "olti", "yetti", "sakkiz", "to`qqiz"];
    static readonly string[] TensMap = ["nol", "o`n", "yigirma", "o`ttiz", "qirq", "ellik", "oltmish", "yetmish", "sakson", "to`qson"];

    static readonly string[] OrdinalSuffixes = ["inchi", "nchi"];

    public override string Convert(long input)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;
        if (number < 0)
        {
            return $"minus {Convert(-number, true)}";
        }

        return Convert(number, true);
    }

    static string Convert(int number, bool checkForHundredRule)
    {
        if (number == 0)
        {
            return UnitsMap[0];
        }

        if (checkForHundredRule && number == 100)
        {
            return "yuz";
        }

        var sb = new StringBuilder();

        if (number / 1000000000 > 0)
        {
            sb.AppendFormat("{0} milliard ", Convert(number / 1000000000, false));
            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            sb.AppendFormat("{0} million ", Convert(number / 1000000, true));
            number %= 1000000;
        }

        var thousand = number / 1000;
        if (thousand > 0)
        {
            sb.AppendFormat("{0} ming ", Convert(thousand, true));
            number %= 1000;
        }

        var hundred = number / 100;
        if (hundred > 0)
        {
            sb.AppendFormat("{0} yuz ", Convert(hundred, false));
            number %= 100;
        }

        if (number / 10 > 0)
        {
            sb.AppendFormat("{0} ", TensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            sb.AppendFormat("{0} ", UnitsMap[number]);
        }

        return sb
            .ToString()
            .Trim();
    }

    public override string ConvertToOrdinal(int number)
    {
        var word = Convert(number);
        var i = 0;
        if (string.IsNullOrEmpty(word))
        {
            return string.Empty;
        }

        var lastChar = word[^1];
        if (lastChar is 'i' or 'a')
        {
            i = 1;
        }

        return $"{word}{OrdinalSuffixes[i]}";
    }
}