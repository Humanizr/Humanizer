using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class MalteseNumberToWordsConvertor : GenderedNumberToWordsConverter
    {
        private static readonly string[] OrdinalOverrideMap =
        {
            "0", "l-ewwel", "it-tieni", "it-tielet", "ir-raba'", "il-ħames", "is-sitt", "is-seba'", "it-tmien", "id-disa'",
            "l-għaxar", "il-ħdax", "it-tnax", "it-tlettax", "l-erbatax", "il-ħmistax", "is-sittax", "is-sbatax",
            "it-tmintax", "id-dsatax", "l-għoxrin"
        };

        private static readonly string[] UnitsMap =
        {
            "żero", "wieħed", "tnejn", "tlieta", "erbgħa", "ħamsa", "sitta", "sebgħa", "tmienja", "disgħa", "għaxra",
            "ħdax", "tnax", "tlettax", "erbatax", "ħmistax", "sittax", "sbatax", "tmintax", "dsatax"
        };

        private static readonly string[] TensMap =
            {"zero", "għaxra", "għoxrin", "tletin", "erbgħin", "ħamsin", "sittin", "sebgħin", "tmenin", "disgħin"};

        private static readonly string[] HundredsMap =
        {
            string.Empty, string.Empty, string.Empty, "tlett", "erbgħa", "ħames", "sitt", "sebgħa", "tminn", "disgħa",
            "għaxar"
        };

        private static readonly string[] PrefixMap =
        {
            string.Empty, string.Empty, string.Empty, "tlett", "erbat", "ħamest", "sitt", "sebat", "tmint", "disat",
            "għaxart", "ħdax-il", "tnax-il", "tletax-il", "erbatax-il", "ħmistax-il", "sittax-il", "sbatax-il",
            "tmintax-il", "dsatax-il"
        };

        public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
        {
            bool negativeNumber = false;

            if (input < 0)
            {
                negativeNumber = true;
                input = input * -1;
            }

            if (input < 1000000000)
            {
                return GetMillions(input, gender) + (negativeNumber ? " inqas minn żero" : String.Empty);
            }

            var billions = input / 1000000000;
            var tensInBillions = billions % 100;
            var millions = input % 1000000000;

            var billionsText = GetPrefixText(billions, tensInBillions, "biljun", "żewġ biljuni", "biljuni", false, gender);
            var millionsText = GetMillions(millions, gender);

            if (millions == 0)
            {
                return billionsText;
            }

            return $"{billionsText} u {millionsText}" + (negativeNumber ? " inqas minn żero" : String.Empty);
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            if (number <= 20)
            {
                return OrdinalOverrideMap[number];
            }

            var ordinal = Convert(number, gender);

            if (ordinal.StartsWith("d"))
            {
                return $"id-{Convert(number, gender)}";
            }
            if (ordinal.StartsWith("s"))
            {
                return $"is-{Convert(number, gender)}";
            }
            if (ordinal.StartsWith("t"))
            {
                return $"it-{Convert(number, gender)}";
            }
            if (ordinal.StartsWith("e"))
            {
                return $"l-{Convert(number, gender)}";
            }
            return $"il-{Convert(number, gender)}";
        }
        
        private static string GetTens(long value, bool usePrefixMap, bool usePrefixMapForLowerDigits, GrammaticalGender gender)
        {
            if (value == 1 && gender == GrammaticalGender.Feminine)
            {
                return "waħda";
            }

            if (value < 11 && usePrefixMap && usePrefixMapForLowerDigits)
            {
                return PrefixMap[value];
            }

            if (value < 11 && usePrefixMap && !usePrefixMapForLowerDigits)
            {
                return HundredsMap[value];
            }

            if (value > 10 && value < 20 && usePrefixMap)
            {
                return PrefixMap[value];
            }

            if (value < 20)
            {
                return UnitsMap[value];
            }

            var single = value % 10;
            var numberOfTens = value / 10;
            if (single == 0)
            {

                return TensMap[numberOfTens];
            }

            return $"{UnitsMap[single]} u {TensMap[numberOfTens]}";
        }

        private static string GetHundreds(long value, bool usePrefixMap, bool usePrefixMapForLowerValueDigits, GrammaticalGender gender)
        {
            if (value < 100)
            {
                return GetTens(value, usePrefixMap, usePrefixMapForLowerValueDigits, gender);
            }

            var tens = value % 100;
            var numberOfHundreds = value / 100;

            var hundredsText = string.Empty;
            if (numberOfHundreds == 1)
            {
                hundredsText = "mija";
            }
            else if (numberOfHundreds == 2)
            {
                hundredsText = "mitejn";
            }
            else
            {
                hundredsText = HundredsMap[numberOfHundreds] + " mija";
            }

            if (tens == 0)
            {
                return hundredsText;
            }

            return $"{hundredsText} u {GetTens(tens, usePrefixMap, usePrefixMapForLowerValueDigits, gender)}";
        }

        private static string GetThousands(long value, GrammaticalGender gender)
        {
            if (value < 1000)
            {
                return GetHundreds(value, false, false, gender);
            }

            var thousands = value / 1000;
            var tensInThousands = thousands % 100;
            var hundreds = value % 1000;

            var thousandsInText = GetPrefixText(thousands, tensInThousands, "elf", "elfejn", "elef", true, gender);

            var hundredsInText = GetHundreds(hundreds, false, false, gender);

            if (hundreds == 0)
            {
                return thousandsInText;
            }

            return $"{thousandsInText} u {hundredsInText}";
        }

        private static string GetMillions(long value, GrammaticalGender gender)
        {
            if (value < 1000000)
            {
                return GetThousands(value, gender);
            }

            var millions = value / 1000000;
            var tensInMillions = millions % 100;
            var thousands = value % 1000000;

            var millionsText = GetPrefixText(millions, tensInMillions, "miljun", "żewġ miljuni", "miljuni", false, gender);
            var thousandsText = GetThousands(thousands, gender);

            if (thousands == 0)
            {
                return millionsText;
            }

            return $"{millionsText} u {thousandsText}";
        }

        private static string GetPrefixText(long thousands, long tensInThousands, string singular, string dual, string plural, bool usePrefixMapForLowerValueDigits, GrammaticalGender gender)
        {
            if (thousands == 1)
            {
                return singular;
            }

            if (thousands == 2)
            {
                return dual;
            }

            if (tensInThousands > 10)
            {
                return $"{GetHundreds(thousands, true, usePrefixMapForLowerValueDigits, gender)} {singular}";
            }

            if (thousands == 100)
            {
                return $"mitt {singular}";
            }

            if (thousands == 101)
            {
                return $"mija u {singular}";
            }

            return $"{GetHundreds(thousands, true, usePrefixMapForLowerValueDigits, gender)} {plural}";

        }
    }
}
