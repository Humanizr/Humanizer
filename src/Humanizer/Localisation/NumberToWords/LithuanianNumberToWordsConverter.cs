using System;
using System.Collections.Generic;

using Humanizer.Localisation.GrammaticalNumber;

namespace Humanizer.Localisation.NumberToWords
{
    internal class LithuanianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "nulis", "vienas", "du", "trys", "keturi", "penki", "šeši", "septyni", "aštuoni", "devyni", "dešimt", "vienuolika", "dvylika", "trylika", "keturiolika", "penkiolika", "šešiolika", "septyniolika", "aštuoniolika", "devyniolika" };
        private static readonly string[] TensMap = { string.Empty, "dešimt", "dvidešimt", "trisdešimt", "keturiasdešimt", "penkiasdešimt", "šešiasdešimt", "septyniasdešimt", "aštuoniasdešimt", "devyniasdešimt" };
        private static readonly string[] HundredsMap = { string.Empty, "šimtas", "du šimtai", "trys šimtai", "keturi šimtai", "penki šimtai", "šeši šimtai", "septyni šimtai", "aštuoni šimtai", "devyni šimtai" };

        private static readonly string[] OrdinalUnitsMap = { string.Empty, "pirm", "antr", "treči", "ketvirt", "penkt", "šešt", "septint", "aštunt", "devint", "dešimt", "vienuolikt", "dvylikt", "trylikt", "keturiolikt", "penkiolikt", "šešiolikt", "septyniolikt", "aštuoniolikt", "devyniolikt", "dvidešimt" };
        private static readonly string[] OrdinalTensMap = { string.Empty, "dešimt", "dvidešimt", "trisdešimt", "keturiasdešimt", "penkiasdešimt", "šešiasdešimt", "septyniasdešimt", "aštuoniasdešimt", "devyniasdešimt" };
        private static readonly string[] OrdinalHundredsMap = { string.Empty, "šimt", "du šimt", "trys šimt", "keturi šimt", "penki šimt", "šeši šimt", "septyni šimt", "aštuoni šimt", "devyni šimt" };

        public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
        {
            if (gender == GrammaticalGender.Neuter)
            {
                throw new NotSupportedException();
            }

            var parts = new List<string>();
            var number = input;

            HandleNegative(parts, ref number);
            CollectParts(parts, ref number, 1000000000000000000, GrammaticalGender.Masculine, "kvintilijonas", "kvintilijonai", "kvintilijonų");
            CollectParts(parts, ref number, 1000000000000000, GrammaticalGender.Masculine, "kvadrilijonas", "kvadrilijonai", "kvadrilijonų");
            CollectParts(parts, ref number, 1000000000000, GrammaticalGender.Masculine, "trilijonas", "trilijonai", "trilijonų");
            CollectParts(parts, ref number, 1000000000, GrammaticalGender.Masculine, "milijardas", "milijardai", "milijardų");
            CollectParts(parts, ref number, 1000000, GrammaticalGender.Masculine, "milijonas", "milijonai", "milijonų");
            CollectParts(parts, ref number, 1000, GrammaticalGender.Masculine, "tūkstantis", "tūkstančiai", "tūkstančių");
            CollectPartsUnderOneThousand(parts, number, gender);

            return string.Join(" ", parts);
        }

        public override string ConvertToOrdinal(int input, GrammaticalGender gender)
        {
            if (gender == GrammaticalGender.Neuter)
            {
                throw new NotSupportedException();
            }

            var parts = new List<string>();
            var number = (long)input;

            HandleNegative(parts, ref number);
            CollectOrdinalParts(parts, ref number, 1000000000, GrammaticalGender.Masculine, "milijard" + GetOrdinalEndingForGender(gender), "milijardas", "milijardai", "milijardų");
            CollectOrdinalParts(parts, ref number, 1000000, GrammaticalGender.Masculine, "milijon" + GetOrdinalEndingForGender(gender), "milijonas", "milijonai", "milijonų");
            CollectOrdinalParts(parts, ref number, 1000, GrammaticalGender.Masculine, "tūkstant" + GetOrdinalEndingForGender(gender), "tūkstantis", "tūkstančiai", "tūkstančių");
            CollectOrdinalPartsUnderOneThousand(parts, number, gender, true);

            return string.Join(" ", parts);
        }

        private static void HandleNegative(List<string> parts, ref long number)
        {
            if (number < 0)
            {
                parts.Add("minus");
                number = -number;
            }
        }

        private static void CollectParts(ICollection<string> parts, ref long number, long divisor,
            GrammaticalGender gender, params string[] forms)
        {
            var result = number / divisor;
            if (result == 0)
            {
                return;
            }

            number %= divisor;

            if (result > 1)
            {
                CollectPartsUnderOneThousand(parts, result, gender);
            }

            parts.Add(ChooseForm(result, forms));
        }

        private static void CollectOrdinalParts(ICollection<string> parts, ref long number, long divisor,
            GrammaticalGender gender, string ordinalForm, params string[] forms)
        {
            var result = number / divisor;
            if (result == 0)
            {
                return;
            }

            number %= divisor;

            if (result > 1)
            {
                CollectOrdinalPartsUnderOneThousand(parts, result, gender);
            }

            parts.Add(ChooseCardinalOrOrdinalForm(result, ordinalForm, forms, useOrdinalForm: number == 0));
        }

        private static void CollectPartsUnderOneThousand(ICollection<string> parts, long number, GrammaticalGender gender)
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

            if (number > 0 || parts.Count == 0)
            {
                parts.Add(GetCardinalNumberForGender(UnitsMap[number], gender));
            }
        }

        private static void CollectOrdinalPartsUnderOneThousand(ICollection<string> parts, long number,
            GrammaticalGender gender, bool lastNumber = false)
        {
            if (number >= 100)
            {
                var hundreds = number / 100;
                number %= 100;

                parts.Add(!lastNumber || number > 0
                    ? HundredsMap[hundreds]
                    : OrdinalHundredsMap[hundreds] + GetOrdinalEndingForGender(gender));
            }

            if (number >= 20)
            {
                var tens = number / 10;
                number %= 10;

                parts.Add(!lastNumber || number > 0
                    ? TensMap[tens]
                    : OrdinalTensMap[tens] + GetOrdinalEndingForGender(gender));
            }

            if (number > 0)
            {
                parts.Add(!lastNumber
                    ? UnitsMap[number]
                    : OrdinalUnitsMap[number] + GetOrdinalEndingForGender(gender));
            }
            else if (number == 0 && parts.Count == 0)
            {
                parts.Add(gender == GrammaticalGender.Masculine ? "nulinis" : "nulinė");
            }
        }

        private static string ChooseForm(long number, string[] forms)
        {
            return forms[GetFormIndex(number)];
        }

        private static string ChooseCardinalOrOrdinalForm(long number, string ordinalForm, string[] cardinalForms,
            bool useOrdinalForm = false)
        {
            if (useOrdinalForm)
            {
                return ordinalForm;
            }

            return ChooseForm(number, cardinalForms);
        }

        private static int GetFormIndex(long number)
        {
            var form = LithuanianNumberFormDetector.Detect(number);

            switch (form)
            {
                case LithuanianNumberForm.Singular:
                    {
                        return 0;
                    }
                case LithuanianNumberForm.Plural:
                    {
                        return 1;
                    }
                case LithuanianNumberForm.GenitivePlural:
                    {
                        return 2;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(form));
            }
        }

        private static string GetCardinalNumberForGender(string number, GrammaticalGender gender)
        {
            if (gender == GrammaticalGender.Masculine)
            {
                return number;
            }

            if (gender == GrammaticalGender.Feminine)
            {
                if (number == "du")
                {
                    return "dvi";
                }

                if (number.EndsWith("as"))
                {
                    return number.Substring(0, number.Length - 1);
                }

                if (number.EndsWith("i"))
                {
                    return number + "os";
                }

                return number;
            }

            throw new ArgumentOutOfRangeException(nameof(gender));
        }

        private static string GetOrdinalEndingForGender(GrammaticalGender gender)
        {
            switch (gender)
            {
                case GrammaticalGender.Masculine:
                    {
                        return "as";
                    }
                case GrammaticalGender.Feminine:
                    {
                        return "a";
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender));
            }
        }
    }
}
