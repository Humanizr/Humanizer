using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.NumberToWords
{
    internal class SpanishNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] HundredsMapOrdinal = {
            "", "centésimo", "ducentésimo", "tricentésimo", "cuadringentésimo", "quingentésimo", "sexcentésimo",
            "septingentésimo", "octingentésimo", "noningentésimo" };

        private static readonly Dictionary<int, string> Ordinals = new()
        {
            { 1, "primero" },
            { 2, "segundo" },
            { 3, "tercero" },
            { 4, "cuarto" },
            { 5, "quinto" },
            { 6, "sexto" },
            { 7, "séptimo" },
            { 8, "octavo" },
            { 9, "noveno" },
        };

        private static readonly string[] TensMapOrdinal = {
            "", "décimo", "vigésimo", "trigésimo", "cuadragésimo", "quincuagésimo", "sexagésimo", "septuagésimo",
            "octogésimo", "nonagésimo" };

        private static readonly string[] ThousandsMapOrdinal = {
            "", "milésimo", "dosmilésimo", "tresmilésimo", "cuatromilésimo", "cincomilésimo", "seismilésimo",
            "sietemilésimo", "ochomilésimo", "nuevemilésimo" };

        public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
        {
            List<string> wordBuilder = new();

            if (input == 0)
            {
                return "cero";
            }

            if (input == long.MinValue)
            {
                return
                    "menos nueve trillones doscientos veintitrés mil trescientos setenta y dos billones treinta y seis mil " +
                    "ochocientos cincuenta y cuatro millones setecientos setenta y cinco mil ochocientos ocho";
            }

            if (input < 0)
            {
                return $"menos {Convert(-input)}";
            }

            wordBuilder.Add(ConvertGreaterThanMillion(input, out var remainder));
            wordBuilder.Add(ConvertThousands(remainder, out remainder, gender));
            wordBuilder.Add(ConvertHundreds(remainder, out remainder, gender));
            wordBuilder.Add(ConvertUnits(remainder, gender));

            return BuildWord(wordBuilder);
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            var parts = new List<string>();

            if (number == 0 || number == int.MinValue)
            {
                return "ceroésimo";
            }

            if (number < 0)
            {
                return ConvertToOrdinal(Math.Abs(number), gender);
            }

            if (number >= 1000_000_000 && number % 1_000_000 == 0)
            {
                var normalizedGender = gender == GrammaticalGender.Masculine ? GrammaticalGender.Neuter : GrammaticalGender.Feminine;
                var cardinalPart = Convert(number / 1_000_000, normalizedGender);
                var sep = number == 1_000_000_000 ? "" : " ";
                var ordinalPart = ConvertToOrdinal(1_000_000, gender);
                return cardinalPart + sep + ordinalPart;
            }

            if (number >= 1000000 && number % 1000000 == 0)
            {
                return ConvertToOrdinal(number / 1000, gender).Replace("milésim", "millonésim");
            }

            if ((number / 10000) > 0)
            {
                var part = Convert((number / 1000) * 1000, gender);

                if (number < 30000
                    || (number % 10000 == 0 && number < 100000)
                    || (number % 100000 == 0 && number < 1000000)
                    || (number % 1000000 == 0 && number < 10000000)
                    || (number % 10000000 == 0 && number < 100000000)
                    || (number % 100000000 == 0 && number < 1000000000)
                    || (number % 1000000000 == 0 && number < int.MaxValue)
                    )
                {
                    if (number == 21000)
                    {
                        part = part.Replace("a", "");
                    }

                    part = part.Replace("ú", "u");
                    part = part.TrimEnd(' ', 'm', 'i', 'l');
                }
                else
                {
                    part = part.TrimEnd('m', 'i', 'l');
                }

                part += "milésim" + (gender == GrammaticalGender.Masculine ? "o" : "a");

                parts.Add(part);
                number %= 1000;
            }

            if ((number / 1000) > 0)
            {
                var thousandsPart = ThousandsMapOrdinal[number / 1000];
                if (gender == GrammaticalGender.Feminine)
                {
                    thousandsPart = thousandsPart.TrimEnd('o') + "a";
                }

                parts.Add(thousandsPart);
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                var hundredsPart = HundredsMapOrdinal[number / 100];
                if (gender == GrammaticalGender.Feminine)
                {
                    hundredsPart = hundredsPart.TrimEnd('o') + "a";
                }

                parts.Add(hundredsPart);
                number %= 100;
            }

            if ((number / 10) > 0)
            {
                var tensPart = TensMapOrdinal[number / 10];
                if (gender == GrammaticalGender.Feminine)
                {
                    tensPart = tensPart.TrimEnd('o') + "a";
                }

                parts.Add(tensPart);
                number %= 10;
            }

            if (Ordinals.TryGetValue(number, out var towords))
            {
                if (gender == GrammaticalGender.Feminine)
                {
                    towords = towords.TrimEnd('o') + "a";
                }
                else if (gender == GrammaticalGender.Neuter && (number == 1 || number == 3))
                {
                    towords = towords.TrimEnd('o');
                }

                parts.Add(towords);
            }

            return BuildWord(parts);
        }

        public override string ConvertToTuple(int number)
        {
            string[] map = {
                "cero veces", "una vez", "doble", "triple", "cuádruple", "quíntuble", "séxtuple", "séptuple", "óctuple",
                "nonupla", "décuplo", "undécuplo", "duodécuplo", "terciodécuplpo" };

            number = Math.Abs(number);

            if (number < map.Length)
                return map[number];

            return Convert(number) + " veces";
        }

        private static string BuildWord(IReadOnlyList<string> wordParts)
        {
            var parts = wordParts.ToList();
            parts.RemoveAll(l => string.IsNullOrEmpty(l));
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

        private static string ConvertUnits(long inputNumber, GrammaticalGender gender)
        {
            var genderedOne = new Dictionary<GrammaticalGender, string>()
            {
                { GrammaticalGender.Feminine, "una" },
                { GrammaticalGender.Masculine, "uno" },
                { GrammaticalGender.Neuter, "un"}
            };

            var genderedtwentyOne = new Dictionary<GrammaticalGender, string>()
            {
                { GrammaticalGender.Feminine, "veintiuna" },
                { GrammaticalGender.Masculine, "veintiuno" },
                { GrammaticalGender.Neuter, "veintiún"}
            };

            string[] UnitsMap = {
                "cero", genderedOne[gender], "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", "once", "doce",
                "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve", "veinte", genderedtwentyOne[gender],
                "veintidós", "veintitrés", "veinticuatro", "veinticinco", "veintiséis", "veintisiete", "veintiocho", "veintinueve"};

            string[] TensMap = {
                "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };

            var wordPart = string.Empty;

            if (inputNumber > 0)
            {
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

            string[] HundredsRootMap = {
                "cero", "ciento", "doscient", "trescient", "cuatrocient", "quinient", "seiscient", "setecient",
                "ochocient", "novecient" };

            var map = new List<string>();
            map.AddRange(HundredsRootMap.Take(2));

            for (var i = 2; i < HundredsRootMap.Length; i++)
            {
                map.Add(HundredsRootMap[i] + genderedEnding);
            }

            return map;
        }

        private static string PluralizeWord(string singularWord)
        {
            if (singularWord.EndsWith("ón"))
            {
                return singularWord.TrimEnd('ó', 'n') + "ones";
            }

            return singularWord;
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
                            $"{Convert(remainder / numberAndWord.Value, GrammaticalGender.Neuter)} {PluralizeWord(numberAndWord.Key)}" :
                            $"{Convert(remainder / numberAndWord.Value)} {PluralizeWord(numberAndWord.Key)}");
                    }

                    remainder %= numberAndWord.Value;
                }
            }

            return BuildWord(wordBuilder);
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
                        $"{Convert(inputNumber / 1000, GrammaticalGender.Neuter)} mil";
                }

                remainder = inputNumber % 1000;
            }

            return wordPart;
        }
    }
}
