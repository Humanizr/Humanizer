using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class SpanishNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", "once", "doce",
                                                        "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve", "veinte", "veintiuno",
                                                        "veintidós", "veintitrés", "veinticuatro", "veinticinco", "veintiséis", "veintisiete", "veintiocho", "veintinueve"};
        private const string Feminine1 = "una";
        private const string Feminine21 = "veintiuna";
        private const string LongMinValue = "menos nueve trillones doscientos veintitrés mil trescientos setenta y dos billones treinta y seis mil ochocientos cincuenta y cuatro millones setecientos setenta y cinco mil ochocientos ocho";
        private static readonly string[] TensMap = { "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        private static readonly string[] HundredsMap = { "cero", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };
        private static readonly string[] FeminineHundredsMap = { "cero", "ciento", "doscientas", "trescientas", "cuatrocientas", "quinientas", "seiscientas", "setecientas", "ochocientas", "novecientas" };
        private static readonly string[] TensMapOrdinal = { "", "décimo", "vigésimo", "trigésimo", "cuadragésimo", "quincuagésimo", "sexagésimo", "septuagésimo", "octogésimo", "nonagésimo" };
        private static readonly string[] HundredsMapOrdinal = { "", "centésimo", "ducentésimo", "tricentésimo", "cuadringentésimo", "quingentésimo", "sexcentésimo", "septingentésimo", "octingentésimo", "noningentésimo" };
        private static readonly string[] ThousandsMapOrdinal = { "", "milésimo", "dosmilésimo", "tresmilésimo", "cuatromilésimo", "cincomilésimo", "seismilésimo", "sietemilésimo", "ochomilésimo", "nuevemilésimo" };
        private static readonly Dictionary<int, string> Ordinals = new()
        {
            {1, "primero"},
            {2, "segundo"},
            {3, "tercero"},
            {4, "cuarto"},
            {5, "quinto"},
            {6, "sexto"},
            {7, "séptimo"},
            {8, "octavo"},
            {9, "noveno"},
        };

        public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
        {
            if (input == 0)
            {
                return "cero";
            }

            if (input == long.MinValue)
            {
                return LongMinValue;
            }

            if (input < 0)
            {
                return $"menos {Convert(-input)}";
            }

            const long oneTrillion = 1_000_000_000_000_000_000;
            const long oneBillion = 1_000_000_000_000;
            const long oneMillion = 1_000_000;

            var parts = new List<string>();

            if ((input / oneTrillion) > 0)
            {
                if (input / oneTrillion == 1)
                {
                    parts.Add("un trillón");
                }
                else
                {
                    if ((input / oneTrillion) % 10 == 1)
                    {
                        parts.Add($"{Convert(input / oneTrillion, GrammaticalGender.Neuter)} trillones");
                    }
                    else
                    {
                        parts.Add($"{Convert(input / oneTrillion)} trillones");
                    }
                }

                input %= oneTrillion;
            }

            if ((input / oneBillion) > 0)
            {
                if (input / oneBillion == 1)
                {
                    parts.Add("un billón");
                }
                else
                {
                    if ((input / oneBillion) % 10 == 1)
                    {
                        parts.Add($"{Convert(input / oneBillion, GrammaticalGender.Neuter)} billones");
                    }
                    else
                    {
                        parts.Add($"{Convert(input / oneBillion)} billones");
                    }
                }

                input %= oneBillion;
            }

            if ((input / oneMillion) > 0)
            {
                if (input / oneMillion == 1)
                {
                    parts.Add("un millón");
                }
                else
                {
                    if ((input / oneMillion) % 10 == 1)
                    {
                        parts.Add($"{Convert(input / oneMillion, GrammaticalGender.Neuter)} millones");
                    }
                    else
                    {
                        parts.Add($"{Convert(input / oneMillion)} millones");
                    }
                }

                input %= oneMillion;
            }

            if ((input / 1000) > 0)
            {
                if (input / 1000 == 1)
                {
                    parts.Add("mil");
                }
                else
                {
                    if (gender == GrammaticalGender.Feminine)
                    {
                        parts.Add($"{Convert(input / 1000, GrammaticalGender.Feminine)} mil");
                    }
                    else
                    {
                        parts.Add(string.Format($"{Convert(input / 1000, GrammaticalGender.Neuter)} mil"));
                    }
                }

                input %= 1000;
            }

            if ((input / 100) > 0)
            {
                parts.Add(input == 100
                    ? "cien"
                    : gender == GrammaticalGender.Feminine
                        ? FeminineHundredsMap[input / 100]
                        : HundredsMap[input / 100]);
                input %= 100;
            }

            if (input > 0)
            {
                if (input < 30)
                {
                    if (input == 1)
                    {
                        if (gender == GrammaticalGender.Feminine)
                        {
                            parts.Add(Feminine1);
                        }
                        else if (gender == GrammaticalGender.Neuter)
                        {
                            parts.Add("un");
                        }
                        else
                        {
                            parts.Add(UnitsMap[input]);
                        }
                    }
                    else if (input == 21)
                    {
                        if (gender == GrammaticalGender.Feminine)
                        {
                            parts.Add(Feminine21);
                        }
                        else if (gender == GrammaticalGender.Neuter)
                        {
                            parts.Add("veintiún");
                        }
                        else
                        {
                            parts.Add(UnitsMap[input]);
                        }
                    }
                    else
                    {
                        parts.Add(UnitsMap[input]);
                    }
                }
                else
                {
                    var lastPart = TensMap[input / 10];
                    var units = input % 10;
                    if (units == 1)
                    {
                        if (gender == GrammaticalGender.Feminine)
                        {
                            lastPart += " y una";
                        }
                        else if (gender == GrammaticalGender.Neuter)
                        {
                            lastPart += " y un";
                        }
                        else
                        {
                            lastPart += " y uno";
                        }
                    }
                    else if (units > 0)
                    {
                        lastPart += $" y {UnitsMap[input % 10]}";
                    }

                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts.ToArray());
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

            return string.Join(" ", parts.ToArray());
        }

        public override string ConvertToTuple(int number)
        {
            string[] map = {"cero veces", "una vez", "doble", "triple", "cuádruple", "quíntuble", "séxtuple", "séptuple", "óctuple", "nonupla", "décuplo", "undécuplo", "duodécuplo", "terciodécuplpo"};

            number = Math.Abs(number);

            if (number < map.Length)
                return map[number];

            return Convert(number) + " veces";
        }
    }
}
