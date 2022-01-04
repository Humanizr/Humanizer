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
        private static readonly Dictionary<int, string> Ordinals = new Dictionary<int, string>
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

            const long ONE_TRILLION = 1_000_000_000_000_000_000;
            const long ONE_BILLION = 1_000_000_000_000;
            const long ONE_MILLION = 1_000_000;
            
            var parts = new List<string>();

            if ((input / ONE_TRILLION) > 0)
            {
                if (input / ONE_TRILLION == 1)
                {
                    parts.Add("un trillón");
                }
                else
                {
                    if ((input / ONE_TRILLION) % 10 == 1)
                    {
                        parts.Add($"{Convert(input / ONE_TRILLION, GrammaticalGender.Neuter)} trillones");
                    }
                    else
                    {
                        parts.Add($"{Convert(input / ONE_TRILLION)} trillones");
                    }
                }

                input %= ONE_TRILLION;
            }

            if ((input / ONE_BILLION) > 0)
            {
                if (input / ONE_BILLION == 1)
                {
                    parts.Add("un billón");
                }
                else
                {
                    if ((input / ONE_BILLION) % 10 == 1)
                    {
                        parts.Add($"{Convert(input / ONE_BILLION, GrammaticalGender.Neuter)} billones");
                    }
                    else
                    {
                        parts.Add($"{Convert(input / ONE_BILLION)} billones");
                    }
                }

                input %= ONE_BILLION;
            }

            if ((input / ONE_MILLION) > 0)
            {
                if (input / ONE_MILLION == 1)
                {
                    parts.Add("un millón");
                }
                else
                {
                    if ((input / ONE_MILLION) % 10 == 1)
                    {
                        parts.Add($"{Convert(input / ONE_MILLION, GrammaticalGender.Neuter)} millones");
                    }
                    else
                    {
                        parts.Add($"{Convert(input / ONE_MILLION)} millones");
                    }
                }

                input %= ONE_MILLION;
            }

            if ((input / 1000) > 0)
            {
                if (input / 1000 == 1)
                {
                    parts.Add("mil");
                }
                else
                {                    
                    if ((input % 10 == 1) && (gender == GrammaticalGender.Feminine))
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
                parts.Add((string)(input == 100
                    ? "cien"
                    : gender == GrammaticalGender.Feminine
                        ? FeminineHundredsMap[input / 100]
                        : HundredsMap[input / 100]));
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
                        lastPart += string.Format(" y {0}", UnitsMap[input % 10]);
                    }

                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts.ToArray());
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            var parts = new List<string>();

            if (number > 9999) // @mihemihe: Implemented only up to 9999 - Dec-2017
            {
                return Convert(number, gender);
            }

            if (number < 0)
            {
                return string.Format("menos {0}", Convert(Math.Abs(number)));
            }

            if (number == 0)
            {
                return string.Format("cero");
            }

            if ((number / 1000) > 0)
            {
                var thousandsPart = ThousandsMapOrdinal[(number / 1000)];
                if (gender == GrammaticalGender.Feminine)
                {
                    thousandsPart = thousandsPart.TrimEnd('o') + "a";
                }

                parts.Add(thousandsPart);
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                var hundredsPart = HundredsMapOrdinal[(number / 100)];
                if (gender == GrammaticalGender.Feminine)
                {
                    hundredsPart = hundredsPart.TrimEnd('o') + "a";
                }

                parts.Add(hundredsPart);
                number %= 100;
            }

            if ((number / 10) > 0)
            {
                var tensPart = TensMapOrdinal[(number / 10)];
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
    }
}
