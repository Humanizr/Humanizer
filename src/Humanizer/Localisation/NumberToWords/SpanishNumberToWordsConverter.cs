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
        private static readonly string[] TensMap = { "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        private static readonly string[] HundredsMap = { "cero", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };
        private static readonly string[] FeminineHundredsMap = { "cero", "ciento", "doscientas", "trescientas", "cuatrocientas", "quinientas", "seiscientas", "setecientas", "ochocientas", "novecientas" };

        private static readonly Dictionary<int, string> Ordinals = new Dictionary<int, string>
        {
            {1, "primero"},
            {2, "segundo"},
            {3, "tercero"},
            {4, "quarto"},
            {5, "quinto"},
            {6, "sexto"},
            {7, "séptimo"},
            {8, "octavo"},
            {9, "noveno"},
            {10, "décimo"}
        };

        public override string Convert(long input, GrammaticalGender gender)
        {
            if (input > Int32.MaxValue || input < Int32.MinValue)
            {
                throw new NotImplementedException();
            }
            var number = (int)input;
            if (number == 0)
                return "cero";

            if (number < 0)
                return string.Format("menos {0}", Convert(Math.Abs(number)));

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(number / 1000000000 == 1
                    ? "mil millones"
                    : string.Format("{0} mil millones", Convert(number / 1000000000)));

                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(number / 1000000 == 1
                    ? "un millón"
                    : string.Format("{0} millones", Convert(number / 1000000)));

                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(number / 1000 == 1
                    ? "mil"
                    : string.Format("{0} mil", Convert(number / 1000, gender)));

                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(number == 100 
                    ? "cien"
                    : gender == GrammaticalGender.Feminine 
                        ? FeminineHundredsMap[(number / 100)]
                        : HundredsMap[(number / 100)]);
                number %= 100;
            }

            if (number > 0)
            {
                if (number < 30)
                {
                    if (gender == GrammaticalGender.Feminine && (number == 1 || number == 21))
                    {
                        parts.Add(number == 1 ? Feminine1 : Feminine21);
                    }
                    else
                    {
                        parts.Add(UnitsMap[number]);
                    }
                }
                else
                {
                    var lastPart = TensMap[number/10];
                    int units = number%10;
                    if (units == 1 && gender == GrammaticalGender.Feminine)
                    {
                        lastPart += " y una";
                    }
                    else if (units > 0)
                    {
                        lastPart += string.Format(" y {0}", UnitsMap[number % 10]);
                    }

                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts.ToArray());
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            string towords;
            if (!Ordinals.TryGetValue(number, out towords))
                towords = Convert(number);

            if (gender == GrammaticalGender.Feminine)
                towords = towords.TrimEnd('o') + "a";
            else if(number % 10 == 1 || number % 10 == 3)
                towords = towords.TrimEnd('o');

            return towords;
        }
    }
}
