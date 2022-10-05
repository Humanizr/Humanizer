using System;
using System.Collections.Generic;

using Humanizer.Localisation.GrammaticalNumber;

namespace Humanizer.Localisation.NumberToWords
{
    internal class LithuanianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "nulis", "vien", "d", "tr", "ketur", "penk", "šeš", "septyn", "aštuon", "devyn", "dešimt", "vienuolika", "dvylika", "trylika", "keturiolika", "penkiolika", "šešiolika", "septyniolika", "aštuoniolika", "devyniolika" };
        private static readonly string[] TensMap = { "", "dešmit", "dvidešimt", "trisdešimt", "keturiasdešimt", "penkiasdešimt", "šešiasdešimt", "septyniasdešimt", "aštuoniasdešimt", "devyniasdešimt" };
        private static readonly string[] UnitsOrdinal = { "nulin", "pirm", "antr", "treči", "ketvirt", "penkt", "šešt", "septint", "aštunt", "devint", "dešimt", "vienuolikt", "dvylikt", "trylikt", "keturiolikt", "penkiolikt", "šešiolikt", "septyniolikt", "aštuoniolikt", "devyniolikt", "dvidešimt" };
        private static readonly Operation[] Operations = new Operation[]
        {
            new Operation { Power = 18, Singular = "kvintilijonas", Plural = "kvintilijonai", SpecialCase = "kvintilijonų", Part = "kvintilijon" },
            new Operation { Power = 15, Singular = "kvadrilijonas", Plural = "kvadrilijonai", SpecialCase = "kvadrilijonų", Part = "kvadrilijon" },
            new Operation { Power = 12, Singular = "trilijonas", Plural = "trilijonai", SpecialCase = "trilijonų", Part = "trilijon" },
            new Operation { Power = 9, Singular = "milijardas", Plural = "milijardai", SpecialCase = "milijardų", Part = "milijard" },
            new Operation { Power = 6, Singular = "milijonas", Plural = "milijonai", SpecialCase = "milijonų", Part = "milijon" },
            new Operation { Power = 3, Singular = "tūkstantis", Plural = "tūkstančiai", SpecialCase = "tūkstančių", Part = "tūkstant" },
            new Operation { Power = 2, Singular = "šimtas", Plural = "šimtai", SpecialCase = "šimtų", Part = "šimt" },
        };

        public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
        {
            if (input == 0)
                return UnitsMap[0];
            
            var parts = new List<string>();
            
            var number = input;
            
            if (number < 0)
            {
                parts.Add("minus");
                number = -number;
            }

            foreach (var op in Operations)
            {
                var pow = (long) Math.Pow(10, op.Power);
                var units = number / pow;

                if (units > 0)
                {
                    parts.Add(GetNumber(units, op.Singular, op.Plural, op.SpecialCase));
                
                    number %= pow;                    
                }
            }

            if (number > 19)
            {
                parts.Add(TensMap[number / 10]);
                number %= 10;
            }

            if (number > 0)
                parts.Add(UnitsMap[number] + GetCardinalEndingForGender(gender, number));

            return string.Join(" ", parts);
        }

        public override string ConvertToOrdinal(int input, GrammaticalGender gender)
        {
            return ConvertToOrdinal((long) input, gender);
        }

        private string ConvertToOrdinal(long input, GrammaticalGender gender)
        {
            if (input == 0)
                return UnitsOrdinal[0] + GetOrdinalEndingForGender(gender, input);
            
            var parts = new List<string>();

            var number = input;

            if (input < 0)
            {
                parts.Add("minus");
                input = -input;
            }

            foreach (var op in Operations)
            {
                var pow = (long)Math.Pow(10, op.Power);
                var units = number / pow;

                if (units > 0)
                {
                    var part = "";
                    if ((number % pow) == 0)
                    {
                        var text = op.Part + GetOrdinalEndingForGender(gender, input);

                        part = GetNumberOrdinal(number / pow, text, text, text);
                    }
                    else
                        part = GetNumberOrdinal(number / pow, op.Singular, op.Plural, op.SpecialCase);
                    number %= pow;
                    parts.Add(part);
                }
            }

            if (number > 19)
            {
                var tensPart = TensMap[(number / 10)];
                if ((number % 10) == 0)
                    tensPart += GetOrdinalEndingForGender(gender, number);
                parts.Add(tensPart);
                number %= 10;
            }

            if (number > 0)
                parts.Add(UnitsOrdinal[number] + GetOrdinalEndingForGender(gender, number));

            return string.Join(" ", parts);
        }
        
        private string GetNumber(long number, string singular, string plural, string specialCase)
        {
            if (number == 1)
                return singular;

            var text = Convert(number, GrammaticalGender.Masculine);
            
            return LithuanianGrammaticalNumberDetector.Detect(number) switch
            {
                LithuanianGrammaticalNumber.Singular => $"{text} {singular}",
                LithuanianGrammaticalNumber.Plural => $"{text} {plural}",
                LithuanianGrammaticalNumber.SpecialCase or _ => $"{text} {specialCase}",
            };
        }

        private string GetNumberOrdinal(long number, string singular, string plural, string specialCase)
        {
            if (number == 1)
                return singular;

            var text = Convert(number, GrammaticalGender.Masculine);

            return LithuanianGrammaticalNumberDetector.Detect(number) switch
            {
                LithuanianGrammaticalNumber.Singular => $"{text} {singular}",
                LithuanianGrammaticalNumber.Plural => $"{text} {plural}",
                LithuanianGrammaticalNumber.SpecialCase or _ => $"{text} {specialCase}",
            };
        }

        private static string GetOrdinalEndingForGender(GrammaticalGender gender, long number)
        {
            if (number == 0)
            {
                return gender switch
                {
                    GrammaticalGender.Masculine => "is",
                    GrammaticalGender.Feminine => "ė",
                    _ => throw new ArgumentOutOfRangeException(nameof(gender))
                };
            }

            return gender switch
            {
                GrammaticalGender.Masculine => (number % 10) switch
                {
                    0 => "asis",
                    _ => "as"
                },
                GrammaticalGender.Feminine => (number % 10) switch
                {
                    0 => "oji",
                    _ => "a"
                },
                _ => throw new ArgumentOutOfRangeException(nameof(gender))
            };
        }

        private static string GetCardinalEndingForGender(GrammaticalGender gender, long number)
        {
            return gender switch
            {
                GrammaticalGender.Masculine => number switch
                {
                    1 => "as",
                    2 => "u",
                    3 => "ys",
                    >0 and < 10 => "i",
                    _ => ""
                },
                GrammaticalGender.Feminine => number switch
                {
                    1 => "a",
                    2 => "vi",
                    3 => "ejos",
                    >0 and < 10 => "ios",
                    _ => ""
                },
                _ => throw new ArgumentOutOfRangeException(nameof(gender))
            };
        }

        private class Operation
        {
            public int Power { get; set; }
            public string Singular { get; set; }
            public string Plural { get; set; }
            public string SpecialCase { get; set; }
            public string Part { get; set; }
        }
    }
}
