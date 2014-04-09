using Humanizer.Localisation.NumberToWords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer {
    public class SpanishNumberToWordsConverter : INumberToWordsConverter {
        private static readonly string[] HundredsMap = { "cero", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };
        private static readonly string[] UnitsMap = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
        private static readonly string[] TensMap = { "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };

        public string Convert(int number) 
        {
            if (number == 0)
                return "cero";

            if (number < 0)
                return string.Format("menos {0}", Convert(Math.Abs(number)));

            var parts = new List<string>();

            if ((number / 1000000000) > 0) 
            {
                if (number / 1000000000 == 1)
                    parts.Add(string.Format("mil millones", Convert(number / 1000000000)));
                else
                    parts.Add(string.Format("{0} mil millones", Convert(number / 1000000000)));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0) 
            {
                if (number / 1000000 == 1)
                    parts.Add(string.Format("millón", Convert(number / 1000000)));
                else
                    parts.Add(string.Format("{0} millones", Convert(number / 1000000)));
                number %= 1000000;
            }

            if ((number / 1000) > 0) 
            {
                if (number / 1000 == 1)
                    parts.Add(string.Format("mil", Convert(number / 1000)));
                else
                    parts.Add(string.Format("{0} mil", Convert(number / 1000)));
                number %= 1000;
            }

            if ((number / 100) > 0) 
            {
                if (number == 100)
                    parts.Add(string.Format("cien", Convert(number / 100)));
                else
                    parts.Add(HundredsMap[(number / 100)]);

                number %= 100;
            }

            if (number > 0) 
            {
                if (number < 20)
                    parts.Add(UnitsMap[number]);
                else if (number > 20 && number < 30) {
                    var lastPart = TensMap[number / 10];
                    if ((number % 10) > 0)
                        lastPart += string.Format(" {0}", UnitsMap[number % 10]);

                    parts.Add(lastPart);
                } 
                else 
                {
                    var lastPart = TensMap[number / 10];
                    if ((number % 10) > 0)
                        lastPart += string.Format(" y {0}", UnitsMap[number % 10]);

                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts.ToArray());
        }

        public string ConvertToOrdinal(int number) {
            throw new NotImplementedException();
        }
    }
}