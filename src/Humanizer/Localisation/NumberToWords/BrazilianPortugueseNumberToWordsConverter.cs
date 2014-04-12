using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class BrazilianPortugueseNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private static readonly string[] PortugueseUnitsMap = { "zero", "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove" };
        private static readonly string[] PortugueseTensMap = { "zero", "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa" };
        private static readonly string[] PortugueseHundredsMap = { "zero", "cento", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos" };

        public override string Convert(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return string.Format("menos {0}", Convert(Math.Abs(number)));

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(number/1000000000 > 2
                    ? string.Format("{0} bilhões", Convert(number/1000000000))
                    : string.Format("{0} bilhão", Convert(number/1000000000)));

                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(number/1000000 > 2
                    ? string.Format("{0} milhões", Convert(number/1000000))
                    : string.Format("{0} milhão", Convert(number/1000000)));

                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(number/1000 == 1 ? "mil" : string.Format("{0} mil", Convert(number/1000)));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                if (number == 100)
                    parts.Add(parts.Count > 0 ? "e cem" : "cem");
                else
                    parts.Add(PortugueseHundredsMap[(number / 100)]);

                number %= 100;
            }

            if (number > 0)
            {
                if (parts.Count != 0)
                    parts.Add("e");

                if (number < 20)
                    parts.Add(PortugueseUnitsMap[number]);
                else
                {
                    var lastPart = PortugueseTensMap[number / 10];
                    if ((number % 10) > 0)
                        lastPart += string.Format(" e {0}", PortugueseUnitsMap[number % 10]);

                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts.ToArray());
        }
    }
}