namespace Humanizer;

class RussianNumberToWordsConverter : GenderedNumberToWordsConverter
{
    static readonly string[] HundredsMap = ["ноль", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот"];
    static readonly string[] TensMap = ["ноль", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто"];
    static readonly string[] UnitsMap = ["ноль", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"];
    static readonly string[] UnitsOrdinalPrefixes = [string.Empty, string.Empty, "двух", "трёх", "четырёх", "пяти", "шести", "семи", "восьми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати"];
    static readonly string[] TensOrdinalPrefixes = [string.Empty, "десяти", "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "восьмидесяти", "девяносто"];
    static readonly string[] TensOrdinal = [string.Empty, "десят", "двадцат", "тридцат", "сороков", "пятидесят", "шестидесят", "семидесят", "восьмидесят", "девяност"];
    static readonly string[] UnitsOrdinal = [string.Empty, "перв", "втор", "трет", "четверт", "пят", "шест", "седьм", "восьм", "девят", "десят", "одиннадцат", "двенадцат", "тринадцат", "четырнадцат", "пятнадцат", "шестнадцат", "семнадцат", "восемнадцат", "девятнадцат"];

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input == 0)
        {
            return "ноль";
        }

        var parts = new List<string>();

        if (input < 0)
        {
            parts.Add("минус");
        }

        CollectParts(parts, ref input, 1000000000000000000, GrammaticalGender.Masculine, "квинтиллион", "квинтиллиона", "квинтиллионов");
        CollectParts(parts, ref input, 1000000000000000, GrammaticalGender.Masculine, "квадриллион", "квадриллиона", "квадриллионов");
        CollectParts(parts, ref input, 1000000000000, GrammaticalGender.Masculine, "триллион", "триллиона", "триллионов");
        CollectParts(parts, ref input, 1000000000, GrammaticalGender.Masculine, "миллиард", "миллиарда", "миллиардов");
        CollectParts(parts, ref input, 1000000, GrammaticalGender.Masculine, "миллион", "миллиона", "миллионов");
        CollectParts(parts, ref input, 1000, GrammaticalGender.Feminine, "тысяча", "тысячи", "тысяч");

        if (input != 0)
        {
            CollectPartsUnderOneThousand(parts, Math.Abs(input), gender);
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int input, GrammaticalGender gender)
    {
        if (input == 0)
        {
            return "нулев" + GetEndingForGender(gender, input);
        }

        var parts = new List<string>();

        if (input < 0)
        {
            parts.Add("минус");
            input = -input;
        }

        var number = (long)input;
        CollectOrdinalParts(parts, ref number, 1000000000, GrammaticalGender.Masculine, "миллиардн" + GetEndingForGender(gender, number), "миллиард", "миллиарда", "миллиардов");
        CollectOrdinalParts(parts, ref number, 1000000, GrammaticalGender.Masculine, "миллионн" + GetEndingForGender(gender, number), "миллион", "миллиона", "миллионов");
        CollectOrdinalParts(parts, ref number, 1000, GrammaticalGender.Feminine, "тысячн" + GetEndingForGender(gender, number), "тысяча", "тысячи", "тысяч");

        if (number >= 100)
        {
            var ending = GetEndingForGender(gender, number);
            var hundreds = number / 100;
            number %= 100;
            if (number == 0)
            {
                parts.Add(UnitsOrdinalPrefixes[hundreds] + "сот" + ending);
            }
            else
            {
                parts.Add(HundredsMap[hundreds]);
            }
        }

        if (number >= 20)
        {
            var ending = GetEndingForGender(gender, number);
            var tens = number / 10;
            number %= 10;
            if (number == 0)
            {
                parts.Add(TensOrdinal[tens] + ending);
            }
            else
            {
                parts.Add(TensMap[tens]);
            }
        }

        if (number > 0)
        {
            parts.Add(UnitsOrdinal[number] + GetEndingForGender(gender, number));
        }

        return string.Join(" ", parts);
    }

    static void CollectPartsUnderOneThousand(List<string> parts, long number, GrammaticalGender gender)
    {
        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            parts.Add(HundredsMap[hundreds]);
        }

        if (number >= 20)
        {
            var tens = number / 10;
            parts.Add(TensMap[tens]);
            number %= 10;
        }

        if (number > 0)
        {
            if (number == 1 && gender == GrammaticalGender.Feminine)
            {
                parts.Add("одна");
            }
            else if (number == 1 && gender == GrammaticalGender.Neuter)
            {
                parts.Add("одно");
            }
            else if (number == 2 && gender == GrammaticalGender.Feminine)
            {
                parts.Add("две");
            }
            else if (number < 20)
            {
                parts.Add(UnitsMap[number]);
            }
        }
    }

    static string GetPrefix(long number)
    {
        var parts = new List<string>();

        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            if (hundreds != 1)
            {
                parts.Add(UnitsOrdinalPrefixes[hundreds] + "сот");
            }
            else
            {
                parts.Add("сто");
            }
        }

        if (number >= 20)
        {
            var tens = number / 10;
            number %= 10;
            parts.Add(TensOrdinalPrefixes[tens]);
        }

        if (number > 0)
        {
            parts.Add(number == 1 ? "одно" : UnitsOrdinalPrefixes[number]);
        }

        return string.Concat(parts);
    }

    static void CollectParts(List<string> parts, ref long number, long divisor, GrammaticalGender gender, params string[] forms)
    {
        var result = Math.Abs(number / divisor);
        if (result == 0)
        {
            return;
        }

        CollectPartsUnderOneThousand(parts, result, gender);
        parts.Add(ChooseOneForGrammaticalNumber(result, forms));
        number = Math.Abs(number % divisor);
    }

    static void CollectOrdinalParts(List<string> parts, ref long number, int divisor, GrammaticalGender gender, string prefixedForm, params string[] forms)
    {
        if (number < divisor)
        {
            return;
        }

        var result = number / divisor;
        number %= divisor;
        if (number == 0)
        {
            if (result == 1)
            {
                parts.Add(prefixedForm);
            }
            else
            {
                parts.Add(GetPrefix(result) + prefixedForm);
            }
        }
        else
        {
            CollectPartsUnderOneThousand(parts, result, gender);
            parts.Add(ChooseOneForGrammaticalNumber(result, forms));
        }
    }

    static int GetIndex(RussianGrammaticalNumber number)
    {
        if (number == RussianGrammaticalNumber.Singular)
        {
            return 0;
        }

        if (number == RussianGrammaticalNumber.Paucal)
        {
            return 1;
        }

        return 2;
    }

    static string ChooseOneForGrammaticalNumber(long number, string[] forms) =>
        forms[GetIndex(RussianGrammaticalNumberDetector.Detect(number))];

    static string GetEndingForGender(GrammaticalGender gender, long number)
    {
        switch (gender)
        {
            case GrammaticalGender.Masculine:
                if (number is 0 or 2 or 6 or 7 or 8 or 40)
                {
                    return "ой";
                }

                if (number == 3)
                {
                    return "ий";
                }

                return "ый";
            case GrammaticalGender.Feminine:
                if (number == 3)
                {
                    return "ья";
                }

                return "ая";
            case GrammaticalGender.Neuter:
                if (number == 3)
                {
                    return "ье";
                }

                return "ое";
            default:
                throw new ArgumentOutOfRangeException(nameof(gender));
        }
    }
}