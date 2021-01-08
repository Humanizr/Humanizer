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


        public override string Convert(long number)
        {
            return Convert(number, false);
        }

        public override string ConvertToOrdinal(int number)
        {
            return null;
        }

        private string Convert(long number, bool returnPluralized)
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

                result += Convert(number % 10, returnPluralized).ToLower();
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
                result += $" {Convert(number % 100, returnPluralized).ToLower()}";
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

                return $"χίλια {Convert(number % 1000, false).ToLower()}";
            }

            var result = $"{Convert(number / 1000, true)} χιλιάδες";

            if (number % 1000 != 0)
            {
                result += $" {Convert(number % 1000, false).ToLower()}";
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

                return $"ένα εκατομμύριο {Convert(number % 1000000, true).ToLower()}";
            }

            var result = $"{Convert(number / 1000000, false)} εκατομμύρια";

            if (number % 1000000 != 0)
            {
                result += $" {Convert(number % 1000000, false).ToLower()}";
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

                return $"ένα δισεκατομμύριο {Convert(number % 1000000000, true).ToLower()}";
            }

            var result = $"{Convert(number / 1000000000, false)} δισεκατομμύρια";

            if (number % 1000000000 != 0)
            {
                result += $" {Convert(number % 1000000000, false).ToLower()}";
            }

            return result;
        }
    }
}
