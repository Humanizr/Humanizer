using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class LatvianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "nulle", "vien", "div", "trīs", "četr", "piec", "seš", "septiņ", "astoņ", "deviņ", "desmit", "vienpadsmit", "divpadsmit", "trīspadsmit", "četrpadsmit", "piecpadsmit", "sešpadsmit", "septiņpadsmit", "astoņpadsmit", "deviņpadsmit" };
        private static readonly string[] TensMap = { "nulle", "desmit", "divdesmit", "trīsdesmit", "četrdesmit", "piecdesmit", "sešdesmit", "septiņdesmit", "astoņdesmit", "deviņdesmit" };
        private static readonly string[] HundredsMap = { "nulle", "simt", "divsimt", "trīssimt", "četrsimt", "piecsimt", "sešsimt", "septiņsimt", "astoņsimt", "deviņsimt" };
        private static readonly string[] UnitsOrdinal = { string.Empty, "pirm", "otr", "treš", "ceturt", "piekt", "sest", "septīt", "astot", "devīt", "desmit", "vienpadsmit", "divpadsmit", "trīspadsmit", "četrpadsmit", "piecpadsmit", "sešpadsmit", "septiņpadsmit", "astoņpadsmit", "deviņpadsmit", "divdesmit" };

        public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
        {
            if (input > int.MaxValue || input < int.MinValue)
            {
                throw new NotImplementedException();
            }

            var parts = new List<string>();
            var number = (long)input;

            if ((number / 1000000) > 0)
            {
                var millionPart = "";
                if (number == 1000000)
                {
                    millionPart = "miljons";
                }
                else
                {
                    millionPart = Convert(number / 1000000, GrammaticalGender.Masculine) + " miljoni";
                }
                number %= 1000000;
                parts.Add(millionPart);
            }

            if ((number / 1000) > 0)
            {
                var thousandsPart = "";
                if (number == 1000)
                {
                    thousandsPart = "tūkstotis";
                }
                else if (number > 1000 && number < 2000)
                {
                    thousandsPart = "tūkstoš";
                }
                else
                {
                    thousandsPart = Convert(number / 1000, GrammaticalGender.Masculine) + " tūkstoši";
                }
                parts.Add(thousandsPart);
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                var hundredsPart = "";
                if (number == 100)
                {
                    hundredsPart = parts.Contains("tūkstoš") ? "viens simts" : "simts";
                }
                else if (number > 100 && number < 200)
                {
                    hundredsPart = "simtu";
                }
                else
                {
                    hundredsPart = Convert(number / 100, GrammaticalGender.Masculine) + " simti";
                }
                parts.Add(hundredsPart);
                number %= 100;
            }

            if (number > 19)
            {
                var tensPart = TensMap[(number / 10)];
                parts.Add(tensPart);
                number %= 10;
            }

            if (number > 0)
            {
                parts.Add(UnitsMap[number] + GetCardinalEndingForGender(gender, number));
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

            var number = (long)input;

            if ((number / 1000000) > 0)
            {
                var millionPart = "";
                if (number == 1000000)
                {
                    millionPart = "miljon" + GetOrdinalEndingForGender(gender);
                }
                else
                {
                    millionPart = Convert(number / 1000000, GrammaticalGender.Masculine) + " miljon" + GetOrdinalEndingForGender(gender);
                }
                number %= 1000000;
                parts.Add(millionPart);
            }

            if ((number / 1000) > 0)
            {
                var thousandsPart = "";
                if ((number % 1000) == 0)
                {
                    if (number == 1000)
                    {
                        thousandsPart = "tūkstoš" + GetOrdinalEndingForGender(gender);
                    }
                    else
                    {
                        thousandsPart = Convert(number / 1000, GrammaticalGender.Masculine) + " tūkstoš" + GetOrdinalEndingForGender(gender);
                    }
                }
                else
                {
                    if (number > 1000 && number < 2000)
                    {
                        thousandsPart = "tūkstoš";
                    }
                    else
                    {
                        thousandsPart = Convert(number / 1000, GrammaticalGender.Masculine) + " tūkstoši";
                    }
                }
                parts.Add(thousandsPart);
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                var hundredsPart = "";
                if ((number % 100) == 0)
                {
                    hundredsPart = HundredsMap[(number / 100)] + GetOrdinalEndingForGender(gender);
                }
                else
                {
                    if (number > 100 && number < 200)
                    {
                        hundredsPart = "simtu";
                    }
                    else
                    {
                        hundredsPart = Convert(number / 100, GrammaticalGender.Masculine) + " simti";
                    }
                }
                parts.Add(hundredsPart);
                number %= 100;
            }

            if (number > 19)
            {
                var tensPart = TensMap[(number / 10)];
                if ((number % 10) == 0)
                {
                    tensPart += GetOrdinalEndingForGender(gender);
                }
                parts.Add(tensPart);
                number %= 10;
            }

            if (number > 0)
            {
                parts.Add(UnitsOrdinal[number] + GetOrdinalEndingForGender(gender));
            }

            return string.Join(" ", parts);
        }

        private static string GetOrdinalEndingForGender(GrammaticalGender gender)
        {
            switch (gender)
            {
                case GrammaticalGender.Masculine:
                    {
                        return "ais";
                    }
                case GrammaticalGender.Feminine:
                    {
                        return "ā";
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender));
            }
        }

        private static string GetCardinalEndingForGender(GrammaticalGender gender, long number)
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
}
