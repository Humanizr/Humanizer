namespace Humanizer;

class LatvianNumberToWordsConverter : GenderedNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["nulle", "vien", "div", "trīs", "četr", "piec", "seš", "septiņ", "astoņ", "deviņ", "desmit", "vienpadsmit", "divpadsmit", "trīspadsmit", "četrpadsmit", "piecpadsmit", "sešpadsmit", "septiņpadsmit", "astoņpadsmit", "deviņpadsmit"];
    static readonly string[] TensMap = ["nulle", "desmit", "divdesmit", "trīsdesmit", "četrdesmit", "piecdesmit", "sešdesmit", "septiņdesmit", "astoņdesmit", "deviņdesmit"];
    static readonly string[] HundredsMap = ["nulle", "simt", "divsimt", "trīssimt", "četrsimt", "piecsimt", "sešsimt", "septiņsimt", "astoņsimt", "deviņsimt"];
    static readonly string[] UnitsOrdinal = [string.Empty, "pirm", "otr", "treš", "ceturt", "piekt", "sest", "septīt", "astot", "devīt", "desmit", "vienpadsmit", "divpadsmit", "trīspadsmit", "četrpadsmit", "piecpadsmit", "sešpadsmit", "septiņpadsmit", "astoņpadsmit", "deviņpadsmit", "divdesmit"];

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var parts = new List<string>();

        if (input / 1000000 > 0)
        {
            string millionPart;
            if (input == 1000000)
            {
                millionPart = "miljons";
            }
            else
            {
                millionPart = Convert(input / 1000000, GrammaticalGender.Masculine) + " miljoni";
            }
            input %= 1000000;
            parts.Add(millionPart);
        }

        if (input / 1000 > 0)
        {
            string thousandsPart;
            if (input == 1000)
            {
                thousandsPart = "tūkstotis";
            }
            else if (input is > 1000 and < 2000)
            {
                thousandsPart = "tūkstoš";
            }
            else
            {
                thousandsPart = Convert(input / 1000, GrammaticalGender.Masculine) + " tūkstoši";
            }
            parts.Add(thousandsPart);
            input %= 1000;
        }

        if (input / 100 > 0)
        {
            string hundredsPart;
            if (input == 100)
            {
                hundredsPart = parts.Contains("tūkstoš") ? "viens simts" : "simts";
            }
            else if (input is > 100 and < 200)
            {
                hundredsPart = "simtu";
            }
            else
            {
                hundredsPart = Convert(input / 100, GrammaticalGender.Masculine) + " simti";
            }
            parts.Add(hundredsPart);
            input %= 100;
        }

        if (input > 19)
        {
            var tensPart = TensMap[input / 10];
            parts.Add(tensPart);
            input %= 10;
        }

        if (input > 0)
        {
            parts.Add(UnitsMap[input] + GetCardinalEndingForGender(gender, input));
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int input, GrammaticalGender gender)
    {
        if (input == 0)
        {
            return "nulle";
        }

        var parts = new List<string>();

        if (input < 0)
        {
            parts.Add("mīnus");
            input = -input;
        }

        if (input / 1000000 > 0)
        {
            string millionPart;
            if (input == 1000000)
            {
                millionPart = "miljon" + GetOrdinalEndingForGender(gender);
            }
            else
            {
                millionPart = Convert(input / 1000000, GrammaticalGender.Masculine) + " miljon" + GetOrdinalEndingForGender(gender);
            }
            input %= 1000000;
            parts.Add(millionPart);
        }

        if (input / 1000 > 0)
        {
            string thousandsPart;
            if (input % 1000 == 0)
            {
                if (input == 1000)
                {
                    thousandsPart = "tūkstoš" + GetOrdinalEndingForGender(gender);
                }
                else
                {
                    thousandsPart = Convert(input / 1000, GrammaticalGender.Masculine) + " tūkstoš" + GetOrdinalEndingForGender(gender);
                }
            }
            else
            {
                if (input is > 1000 and < 2000)
                {
                    thousandsPart = "tūkstoš";
                }
                else
                {
                    thousandsPart = Convert(input / 1000, GrammaticalGender.Masculine) + " tūkstoši";
                }
            }
            parts.Add(thousandsPart);
            input %= 1000;
        }

        if (input / 100 > 0)
        {
            string hundredsPart;
            if (input % 100 == 0)
            {
                hundredsPart = HundredsMap[input / 100] + GetOrdinalEndingForGender(gender);
            }
            else
            {
                if (input is > 100 and < 200)
                {
                    hundredsPart = "simtu";
                }
                else
                {
                    hundredsPart = Convert(input / 100, GrammaticalGender.Masculine) + " simti";
                }
            }
            parts.Add(hundredsPart);
            input %= 100;
        }

        if (input > 19)
        {
            var tensPart = TensMap[input / 10];
            if (input % 10 == 0)
            {
                tensPart += GetOrdinalEndingForGender(gender);
            }
            parts.Add(tensPart);
            input %= 10;
        }

        if (input > 0)
        {
            parts.Add(UnitsOrdinal[input] + GetOrdinalEndingForGender(gender));
        }

        return string.Join(" ", parts);
    }

    static string GetOrdinalEndingForGender(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => "ais",
            GrammaticalGender.Feminine => "ā",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };

    static string GetCardinalEndingForGender(GrammaticalGender gender, long number)
    {
        switch (gender)
        {
            case GrammaticalGender.Masculine:
                if (number == 1)
                {
                    return "s";
                }

                if (number != 3 && number < 10)
                {
                    return "i";
                }

                return "";
            case GrammaticalGender.Feminine:
                if (number == 1)
                {
                    return "a";
                }

                if (number != 3 && number < 10)
                {
                    return "as";
                }

                return "";
            default:
                throw new ArgumentOutOfRangeException(nameof(gender));
        }
    }
}