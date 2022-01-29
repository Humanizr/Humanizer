using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.NumberToWords
{
    internal class SpanishNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] HundredsRootMap = {
            "cero", "ciento", "doscient", "trescient", "cuatrocient", "quinient", "seiscient", "setecient",
            "ochocient", "novecient" };

        private static readonly string[] HundredthsRootMap = {
            "", "centésim", "ducentésim", "tricentésim", "cuadringentésim", "quingentésim", "sexcentésim",
            "septingentésim", "octingentésim", "noningentésim" };

        private static readonly string[] OrdinalsRootMap = { "" , "primer", "segund", "tercer", "cuart", "quint", "sext",
            "séptim", "octav", "noven"};

        private static readonly string[] TensMap = {
            "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };

        private static readonly string[] TenthsRootMap = {
            "", "décim", "vigésim", "trigésim", "cuadragésim", "quincuagésim", "sexagésim", "septuagésim",
            "octogésim", "nonagésim" };

        private static readonly string[] ThousandthsRootMap = {
            "", "milésim", "dosmilésim", "tresmilésim", "cuatromilésim", "cincomilésim", "seismilésim",
            "sietemilésim", "ochomilésim", "nuevemilésim" };

        private static readonly string[] TupleMap = {
            "cero veces", "una vez", "doble", "triple", "cuádruple", "quíntuple", "séxtuple", "séptuple", "óctuple",
            "nonuplo", "décuplo", "undécuplo", "duodécuplo", "terciodécuplo" };

        private static readonly string[] UnitsMap = {
            "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", "once", "doce",
            "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve", "veinte", "veintiuno",
            "veintidós", "veintitrés", "veinticuatro", "veinticinco", "veintiséis", "veintisiete", "veintiocho", "veintinueve"};

        public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
        {
            return Convert(input, WordForm.Normal, gender, addAnd);
        }

        public override string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
        {
            List<string> wordBuilder = new();

            if (number == 0)
            {
                return "cero";
            }

            if (number == long.MinValue)
            {
                return
                    "menos nueve trillones doscientos veintitrés mil trescientos setenta y dos billones treinta y seis mil " +
                    "ochocientos cincuenta y cuatro millones setecientos setenta y cinco mil ochocientos ocho";
            }

            if (number < 0)
            {
                return $"menos {Convert(-number)}";
            }

            wordBuilder.Add(ConvertGreaterThanMillion(number, out var remainder));
            wordBuilder.Add(ConvertThousands(remainder, out remainder, gender));
            wordBuilder.Add(ConvertHundreds(remainder, out remainder, gender));
            wordBuilder.Add(ConvertUnits(remainder, gender, wordForm));

            return BuildWord(wordBuilder);
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return ConvertToOrdinal(number, gender, WordForm.Normal);
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
        {
            List<string> wordBuilder = new();

            if (number == 0 || number == int.MinValue)
            {
                return "cero";
            }

            if (number < 0)
            {
                return ConvertToOrdinal(Math.Abs(number), gender);
            }

            if (IsRoundBillion(number))
            {
                return ConvertRoundBillionths(number, gender);
            }

            if (IsRoundMillion(number))
            {
                return ConvertToOrdinal(number / 1000, gender).Replace("milésim", "millonésim");
            }

            wordBuilder.Add(ConvertTensAndHunderdsOfThousandths(number, out var remainder, gender));
            wordBuilder.Add(ConvertThousandths(remainder, out remainder, gender));
            wordBuilder.Add(ConvertHundredths(remainder, out remainder, gender));
            wordBuilder.Add(ConvertTenths(remainder, out remainder, gender));
            wordBuilder.Add(ConvertOrdinalUnits(remainder, gender, wordForm));

            return BuildWord(wordBuilder);
        }

        public override string ConvertToTuple(int number)
        {
            number = Math.Abs(number);

            if (number < TupleMap.Length)
                return TupleMap[number];

            return Convert(number) + " veces";
        }

        private static string BuildWord(IReadOnlyList<string> wordParts)
        {
            var parts = wordParts.ToList();
            parts.RemoveAll(string.IsNullOrEmpty);
            return string.Join(" ", parts);
        }

        private static string ConvertHundreds(in long inputNumber, out long remainder, GrammaticalGender gender)
        {
            var wordPart = string.Empty;
            remainder = inputNumber;

            if ((inputNumber / 100) > 0)
            {
                wordPart = inputNumber == 100 ?
                    "cien" :
                    GetGenderedHundredsMap(gender)[(int)(inputNumber / 100)];

                remainder = inputNumber % 100;
            }

            return wordPart;
        }

        private static string ConvertHundredths(in int number, out int remainder, GrammaticalGender gender)
        {
            return ConvertMappedOrdinalNumber(number, 100, HundredthsRootMap, out remainder, gender);
        }

        private static string ConvertMappedOrdinalNumber(
            in int number,
            in int divisor,
            IReadOnlyList<string> map,
            out int remainder,
            GrammaticalGender gender)
        {
            var wordPart = string.Empty;
            remainder = number;

            if ((number / divisor) > 0)
            {
                var genderedEnding = gender == GrammaticalGender.Feminine ? "a" : "o";
                wordPart = map[number / divisor] + genderedEnding;
                remainder = number % divisor;
            }

            return wordPart;
        }

        private static string ConvertOrdinalUnits(in int number, GrammaticalGender gender, WordForm wordForm)
        {
            if (number is > 0 and < 10)
            {
                Dictionary<GrammaticalGender, string> genderedEndingDict = new()
                {
                    { GrammaticalGender.Feminine, "a" },
                    { GrammaticalGender.Masculine, HasOrdinalAbbreviation(number, wordForm) ? string.Empty : "o" },
                };

                genderedEndingDict.Add(GrammaticalGender.Neuter, genderedEndingDict[GrammaticalGender.Masculine]);

                return OrdinalsRootMap[number] + genderedEndingDict[gender];
            }

            return string.Empty;
        }

        private static string ConvertTenths(in int number, out int remainder, GrammaticalGender gender)
        {
            return ConvertMappedOrdinalNumber(number, 10, TenthsRootMap, out remainder, gender);
        }

        private static string ConvertThousandths(in int number, out int remainder, GrammaticalGender gender)
        {
            return ConvertMappedOrdinalNumber(number, 1000, ThousandthsRootMap, out remainder, gender);
        }

        private static string ConvertUnits(long inputNumber, GrammaticalGender gender, WordForm wordForm = WordForm.Normal)
        {
            var wordPart = string.Empty;

            if (inputNumber > 0)
            {
                UnitsMap[1] = GetGenderedOne(gender, wordForm);
                UnitsMap[21] = GetGenderedTwentyOne(gender, wordForm);

                if (inputNumber < 30)
                {
                    wordPart = UnitsMap[inputNumber];
                }
                else
                {
                    wordPart = TensMap[inputNumber / 10];
                    if (inputNumber % 10 > 0)
                    {
                        wordPart += $" y {UnitsMap[inputNumber % 10]}";
                    }
                }
            }

            return wordPart;
        }

        private static IReadOnlyList<string> GetGenderedHundredsMap(GrammaticalGender gender)
        {
            var genderedEnding = gender == GrammaticalGender.Feminine ? "as" : "os";
            var map = new List<string>();
            map.AddRange(HundredsRootMap.Take(2));

            for (var i = 2; i < HundredsRootMap.Length; i++)
            {
                map.Add(HundredsRootMap[i] + genderedEnding);
            }

            return map;
        }

        private static string GetGenderedOne(GrammaticalGender gender, WordForm wordForm = WordForm.Normal)
        {
            var genderedOne = new Dictionary<GrammaticalGender, string>()
            {
                { GrammaticalGender.Feminine, "una" },
                { GrammaticalGender.Masculine, wordForm == WordForm.Abbreviation ? "un" : "uno" }
            };

            genderedOne.Add(GrammaticalGender.Neuter, genderedOne[GrammaticalGender.Masculine]);
            return genderedOne[gender];
        }

        private static string GetGenderedTwentyOne(GrammaticalGender gender, WordForm wordForm = WordForm.Normal)
        {
            var genderedtwentyOne = new Dictionary<GrammaticalGender, string>()
            {
                { GrammaticalGender.Feminine, "veintiuna" },
                { GrammaticalGender.Masculine, wordForm == WordForm.Abbreviation ? "veintiún" : "veintiuno" }
            };

            genderedtwentyOne.Add(GrammaticalGender.Neuter, genderedtwentyOne[GrammaticalGender.Masculine]);
            return genderedtwentyOne[gender];
        }

        private static bool HasOrdinalAbbreviation(int number, WordForm wordForm)
        {
            return (number == 1 || number == 3) && wordForm == WordForm.Abbreviation;
        }

        private static bool IsRoundBillion(int number)
        {
            return number >= 1000_000_000 && number % 1_000_000 == 0;
        }

        private static bool IsRoundMillion(int number)
        {
            return number >= 1000000 && number % 1000000 == 0;
        }

        private static string PluralizeGreaterThanMillion(string singularWord)
        {
            return singularWord.TrimEnd('ó', 'n') + "ones";
        }

        private string ConvertGreaterThanMillion(in long inputNumber, out long remainder)
        {
            List<string> wordBuilder = new();

            const long oneTrillion = 1_000_000_000_000_000_000;
            const long oneBillion = 1_000_000_000_000;
            const long oneMillion = 1_000_000;

            remainder = inputNumber;

            var numbersAndWordsDict = new Dictionary<string, long>()
            {
                { "trillón", oneTrillion },
                { "billón", oneBillion },
                { "millón", oneMillion }
            };

            foreach (var numberAndWord in numbersAndWordsDict)
            {
                if ((remainder / numberAndWord.Value) > 0)
                {
                    if (remainder / numberAndWord.Value == 1)
                    {
                        wordBuilder.Add($"un {numberAndWord.Key}");
                    }
                    else
                    {
                        wordBuilder.Add((remainder / numberAndWord.Value % 10 == 1) ?
                            $"{Convert(remainder / numberAndWord.Value, WordForm.Abbreviation, GrammaticalGender.Masculine)} {PluralizeGreaterThanMillion(numberAndWord.Key)}" :
                            $"{Convert(remainder / numberAndWord.Value)} {PluralizeGreaterThanMillion(numberAndWord.Key)}");
                    }

                    remainder %= numberAndWord.Value;
                }
            }

            return BuildWord(wordBuilder);
        }

        private string ConvertRoundBillionths(int number, GrammaticalGender gender)
        {
            var cardinalPart = Convert(number / 1_000_000, WordForm.Abbreviation, gender);
            var sep = number == 1_000_000_000 ? "" : " ";
            var ordinalPart = ConvertToOrdinal(1_000_000, gender);
            return cardinalPart + sep + ordinalPart;
        }

        private string ConvertTensAndHunderdsOfThousandths(in int number, out int remainder, GrammaticalGender gender)
        {
            var wordPart = string.Empty;
            remainder = number;

            if ((number / 10000) > 0)
            {
                wordPart = Convert((number / 1000) * 1000, gender);

                if (number < 30000 || IsRoundNumber(number))
                {
                    if (number == 21000)
                    {
                        wordPart = wordPart
                            .Replace("a", "")
                            .Replace("ú", "u");
                    }

                    wordPart = wordPart.Remove(wordPart.LastIndexOf(' '), 1);
                }

                wordPart += "ésim" + (gender == GrammaticalGender.Masculine ? "o" : "a");

                remainder = number % 1000;
            }

            return wordPart;

            static bool IsRoundNumber(int number)
            {
                return (number % 10000 == 0 && number < 100000)
                       || (number % 100000 == 0 && number < 1000000)
                       || (number % 1000000 == 0 && number < 10000000)
                       || (number % 10000000 == 0 && number < 100000000)
                       || (number % 100000000 == 0 && number < 1000000000)
                       || (number % 1000000000 == 0 && number < int.MaxValue);
            }
        }

        private string ConvertThousands(in long inputNumber, out long remainder, GrammaticalGender gender)
        {
            var wordPart = string.Empty;
            remainder = inputNumber;

            if ((inputNumber / 1000) > 0)
            {
                if (inputNumber / 1000 == 1)
                {
                    wordPart = "mil";
                }
                else
                {
                    wordPart = (gender == GrammaticalGender.Feminine) ?
                        $"{Convert(inputNumber / 1000, GrammaticalGender.Feminine)} mil" :
                        $"{Convert(inputNumber / 1000, WordForm.Abbreviation, gender)} mil";
                }

                remainder = inputNumber % 1000;
            }

            return wordPart;
        }
    }
}
