using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class BulgarianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap =
        {
            "нула", "един", "две", "три", "четири", "пет", "шест", "седем", "осем", "девет", "десет", "единадесет",
            "дванадесет", "тринадесет", "четиринадесет", "петнадесет", "шестнадесет", "седемнадесет", "осемнадесет",
            "деветнадесет"
        };

        private static readonly string[] TensMap =
        {
            "нула", "десет", "двадесет", "тридесет", "четиридесет", "петдесет", "шестдесет", "седемдесет",
            "осемдесет", "деветдесет"
        };

        private static readonly string[] HundredsMap =
        {
            "нула", "сто", "двеста", "триста", "четиристотин", "петстотин", "шестстотин", "седемстотин",
            "осемстотин", "деветстотин"
        };


        private static readonly string[] HundredsOrdinalMap =
        {
            String.Empty, "стот", "двест", "трист", "четиристот", "петстот", "шестстот", "седемстот", "осемстот",
            "деветстот"
        };


        private static readonly string[] UnitsOrdinal =
        {
            string.Empty, "първ", "втор", "трет", "четвърт", "пет", "шест", "седм", "осм", "девeт", "десeт",
            "единадесет", "дванадесет", "тринадесет", "четиринадесет", "петнадесет", "шестнадесет", "седемнадесет",
            "осемнадесет", "деветнадесет"
        };

        public override string Convert(long input, GrammaticalGender gender)
        {
            return Convert(input, gender, false);
        }

        private string Convert(long input, GrammaticalGender gender, bool isOrdinal)
        {
            if (input > int.MaxValue || input < int.MinValue)
            {
                throw new NotImplementedException();
            }

            if (input == 0)
            {
                return isOrdinal ? "нулев" + GetEndingForGender(gender, input) : "нула";
            }


            var parts = new List<string>();

            if (input < 0)
            {
                parts.Add("минус");
                input = -input;
            }

            string lastOrdinalSubstitution = "";

            if ((input / 1000000000000) > 0)
            {
                parts.Add(Convert(input / 1000000000000, gender, false));
                parts.Add($"{(input < 2000000000000 ? "трилион" : "трилиона")}");
                if (isOrdinal)
                    lastOrdinalSubstitution = "трилион" + GetEndingForGender(gender, input);
                input %= 1000000000000;
            }

            if ((input / 1000000000) > 0)
            {
                parts.Add(Convert(input / 1000000000, gender, false));
                parts.Add($"{(input < 2000000000 ? "милиард" : "милиарда")}");
                if (isOrdinal)
                    lastOrdinalSubstitution = "милиард" + GetEndingForGender(gender, input);
                input %= 1000000000;
            }

            if ((input / 1000000) > 0)
            {
                parts.Add(Convert(input / 1000000, gender, false));
                parts.Add($"{(input < 2000000 ? "милион" : "милиона")}");
                if (isOrdinal)
                    lastOrdinalSubstitution = "милион" + GetEndingForGender(gender, input);

                input %= 1000000;
            }

            if ((input / 1000) > 0)
            {
                if (input < 2000)
                    parts.Add("хиляда");
                else
                {
                    parts.Add(Convert(input / 1000, gender, false));
                    parts.Add("хиляди");
                }

                if (isOrdinal)
                    lastOrdinalSubstitution = "хиляд" + GetEndingForGender(gender, input);
                
                input %= 1000;
            }

            if (input / 100 > 0)
            {
                parts.Add(HundredsMap[(int)input / 100]);
                
                if (isOrdinal)
                    lastOrdinalSubstitution = HundredsOrdinalMap[(int)input / 100] + GetEndingForGender(gender, input);
                
                input %= 100;
            }


            if (input > 19)
            {
                parts.Add(TensMap[input / 10]);
                
                if (isOrdinal)
                    lastOrdinalSubstitution = TensMap[(int)input / 10] + GetEndingForGender(gender, input);
                
                input %= 10;
            }

            if (input > 0)
            {
                if(parts.Count > 0)
                    parts.Add("и");
                
                parts.Add(UnitsMap[input]);
                
                if (isOrdinal)
                    lastOrdinalSubstitution = UnitsOrdinal[input] + GetEndingForGender(gender, input);
            }

            if (isOrdinal && !string.IsNullOrWhiteSpace(lastOrdinalSubstitution))
                parts[parts.Count - 1] = lastOrdinalSubstitution;

            return string.Join(" ", parts);
        }

        public override string ConvertToOrdinal(int input, GrammaticalGender gender)
        {
            return Convert(input, gender, true);
        }

        private static string GetEndingForGender(GrammaticalGender gender, long input)
        {
            if (input == 0)
            {
                return gender switch
                {
                    GrammaticalGender.Masculine => "",
                    GrammaticalGender.Feminine => "а",
                    GrammaticalGender.Neuter => "о",
                    _ => throw new ArgumentOutOfRangeException(nameof(gender))
                };
            }

            if (input < 99)
            {
                return gender switch
                {
                    GrammaticalGender.Masculine => "и",
                    GrammaticalGender.Feminine => "а",
                    GrammaticalGender.Neuter => "о",
                    _ => throw new ArgumentOutOfRangeException(nameof(gender))
                };
            }
            else
            {
                return gender switch
                {
                    GrammaticalGender.Masculine => "ен",
                    GrammaticalGender.Feminine => "на",
                    GrammaticalGender.Neuter => "но",
                    _ => throw new ArgumentOutOfRangeException(nameof(gender))
                };
            }
        }
    }
}
