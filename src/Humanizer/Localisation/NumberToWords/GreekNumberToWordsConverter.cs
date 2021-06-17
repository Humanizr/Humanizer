using System;
using System.Collections.Generic;
using System.Text;

namespace Humanizer.Localisation.NumberToWords
{
    internal class GreekNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private readonly string[] UnitMap = { "μηδέν", "ένα", "δύο", "τρία", "τέσσερα", "πέντε", "έξι", "επτά", "οκτώ", "εννέα", "δέκα", "έντεκα", "δώδεκα" };

        private readonly string[] UnitsMap = { "μηδέν", "ένα", "δύο", "τρείς", "τέσσερις", "πέντε", "έξι", "επτά", "οκτώ", "εννέα", "δέκα", "έντεκα", "δώδεκα" };

        private readonly string[] TensMap = { "", "δέκα", "είκοσι", "τριάντα", "σαράντα", "πενήντα", "εξήντα", "εβδομήντα", "ογδόντα", "ενενήντα" };

        private readonly string[] TensNoDiacriticsMap = { "", "δεκα", "εικοσι", "τριαντα", "σαραντα", "πενηντα", "εξηντα", "εβδομηντα", "ογδοντα", "ενενηντα" };

        private readonly string[] HundredMap = { "", "εκατό", "διακόσια", "τριακόσια", "τετρακόσια", "πεντακόσια", "εξακόσια", "επτακόσια", "οκτακόσια", "εννιακόσια" };

        private readonly string[] HundredsMap = { "", "εκατόν", "διακόσιες", "τριακόσιες", "τετρακόσιες", "πεντακόσιες", "εξακόσιες", "επτακόσιες", "οκτακόσιες", "Εενιακόσιες" };

        private static readonly Dictionary<long, string> ΟrdinalMap = new()
        {
            { 0, string.Empty },
            { 1, "πρώτος" },
            { 2, "δεύτερος" },
            { 3, "τρίτος" },
            { 4, "τέταρτος" },
            { 5, "πέμπτος" },
            { 6, "έκτος" },
            { 7, "έβδομος" },
            { 8, "όγδοος" },
            { 9, "ένατος" },
            { 10, "δέκατος" },
            { 20, "εικοστός" },
            { 30, "τριακοστός" },
            { 40, "τεσσαρακοστός" },
            { 50, "πεντηκοστός" },
            { 60, "εξηκοστός" },
            { 70, "εβδομηκοστός" },
            { 80, "ογδοηκοστός" },
            { 90, "ενενηκοστός" },
            { 100, "εκατοστός" },
            { 200, "διακοσιοστός" },
            { 300, "τριακοσιοστός" },
            { 400, "τετρακοσιστός" },
            { 500, "πεντακοσιοστός" },
            { 600, "εξακοσιοστός" },
            { 700, "εφτακοσιοστός" },
            { 800, "οχτακοσιοστός" },
            { 900, "εννιακοσιοστός" },
            { 1000, "χιλιοστός" }
        };


        public override string Convert(long number)
        {
            return ConvertImpl(number, false);
        }

        public override string ConvertToOrdinal(int number)
        {
            if (number / 10 == 0)
            {
                return GetOneDigitOrdinal(number);
            }

            if (number / 10 > 0 && number / 10 < 10)
            {
                return GetTwoDigigOrdinal(number);

            }

            if (number / 100 > 0 && number / 100 < 10)
            {
                return GetThreeDigitOrdinal(number);
            }

            if (number / 1000 > 0 && number / 1000 < 10)
            {
                return GetFourDigitOrdinal(number);
            }

            return string.Empty;
        }

       
        private string GetOneDigitOrdinal(int number)
        {
            if (!ΟrdinalMap.TryGetValue(number, out var output)) return string.Empty;

            return output;
        }

        private string GetTwoDigigOrdinal(int number)
        {
            if (number == 11) return "ενδέκατος";
            if (number == 12) return "δωδέκατος";

            var decades = number / 10;

            if (!ΟrdinalMap.TryGetValue(decades*10, out var decadesString)) return string.Empty;

            if(number -decades*10 > 0)
            {
                return decadesString + " " + GetOneDigitOrdinal(number - decades * 10);
            }

            return decadesString;
        }

        private string GetThreeDigitOrdinal(int number)
        {

            var hundrends = number / 100;

            if (!ΟrdinalMap.TryGetValue(hundrends*100, out var hundrentsString)) return string.Empty;

            if (number - hundrends*100> 10)
            {
                return hundrentsString + " " + GetTwoDigigOrdinal(number - hundrends*100);
            }

            if(number - hundrends * 100 > 0)
            {
                return hundrentsString + " " + GetOneDigitOrdinal(number - hundrends*100);
            }

            return hundrentsString;
        }

        private string GetFourDigitOrdinal(int number)
        {

            var thousands = number / 1000;

            if (!ΟrdinalMap.TryGetValue(thousands*1000, out var thousandsString)) return string.Empty;

            if (number - thousands * 1000 > 100)
            {
                return thousandsString + " " + GetThreeDigitOrdinal(number - thousands*1000);
            }

            if (number - thousands * 1000 > 10)
            {
                return thousandsString + " " + GetTwoDigigOrdinal(number - thousands * 1000);
            }

            if (number - thousands * 1000 > 0)
            {
                return thousandsString + " " + GetOneDigitOrdinal(number - thousands * 1000);
            }

            return thousandsString;

        }

        private string ConvertImpl(long number, bool returnPluralized)
        {
            if (number < 13)
            {
                return ConvertIntΒ13(number, returnPluralized);
            }
            else if (number < 100)
            {
                return ConvertIntBH(number, returnPluralized);
            }
            else if (number < 1000)
            {
                return ConvertIntBT(number, returnPluralized);
            }
            else if (number < 1000000)
            {
                return ConvertIntBM(number);
            }
            else if (number < 1000000000)
            {
                return ConvertIntBB(number);
            }
            else if (number < 1000000000000)
            {
                return ConvertIntBTR(number);
            }

            return "";
        }

        private string ConvertIntΒ13(long number, bool returnPluralized)
        {
            return returnPluralized ? UnitsMap[number] : UnitMap[number];
        }

        private string ConvertIntBH(long number, bool returnPluralized)
        {
            var result = (number / 10 == 1) ? TensNoDiacriticsMap[number / 10] : TensMap[number / 10];

            if (number % 10 != 0)
            {
                if (number / 10 != 1)
                {
                    result += " ";
                }

                result += ConvertImpl(number % 10, returnPluralized).ToLower();
            }

            return result;
        }

        private string ConvertIntBT(long number, bool returnPluralized)
        {
            var result = "";

            if (number / 100 == 1)
            {
                if (number % 100 == 0)
                {
                    return HundredMap[number / 100];
                }

                result = HundredsMap[number / 100];
            }
            else
            {
                result = returnPluralized ? HundredsMap[number / 100] : HundredMap[number / 100];
            }

            if (number % 100 != 0)
            {
                result += $" {ConvertImpl(number % 100, returnPluralized).ToLower()}";
            }

            return result;
        }

        private string ConvertIntBM(long number)
        {
            if (number / 1000 == 1)
            {
                if (number % 1000 == 0)
                {
                    return "χίλια";
                }

                return $"χίλια {ConvertImpl(number % 1000, false).ToLower()}";
            }

            var result = $"{ConvertImpl(number / 1000, true)} χιλιάδες";

            if (number % 1000 != 0)
            {
                result += $" {ConvertImpl(number % 1000, false).ToLower()}";
            }

            return result;
        }

        private string ConvertIntBB(long number)
        {
            if (number / 1000000 == 1)
            {
                if (number % 1000000 == 0)
                {
                    return "ένα εκατομμύριο";
                }

                return $"ένα εκατομμύριο {ConvertImpl(number % 1000000, true).ToLower()}";
            }

            var result = $"{ConvertImpl(number / 1000000, false)} εκατομμύρια";

            if (number % 1000000 != 0)
            {
                result += $" {ConvertImpl(number % 1000000, false).ToLower()}";
            }

            return result;
        }

        private string ConvertIntBTR(long number)
        {
            if (number / 1000000000 == 1)
            {
                if (number % 1000000000 == 0)
                {
                    return "ένα δισεκατομμύριο";
                }

                return $"ένα δισεκατομμύριο {ConvertImpl(number % 1000000000, true).ToLower()}";
            }

            var result = $"{ConvertImpl(number / 1000000000, false)} δισεκατομμύρια";

            if (number % 1000000000 != 0)
            {
                result += $" {ConvertImpl(number % 1000000000, false).ToLower()}";
            }

            return result;
        }
    }
}
