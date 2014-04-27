using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class GermanNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "null", "ein", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun", "zehn", "elf", "zwölf", "dreizehn", "vierzehn", "fünfzehn", "sechzehn", "siebzehn", "achtzehn", "neunzehn" };
        private static readonly string[] TensMap = { "null", "zehn", "zwanzig", "dreißig", "vierzig", "fünfzig", "sechzig", "siebzig", "achtzig", "neunzig" };

        public override string Convert(int number)
        {
            if (number == 0)
                return "null";

            if (number < 0)
                return string.Format("minus {0}", Convert(-number));

            var parts = new List<string>();

            var billions = number / 1000000000;
            if (billions > 0)
            {
                parts.Add(Part("{0} Milliarden", "eine Milliarde", billions));
                number %= 1000000000;
                if (number > 0)
                    parts.Add(" ");
            }

            var millions = number / 1000000;
            if (millions > 0)
            {
                parts.Add(Part("{0} Millionen", "eine Million", millions));
                number %= 1000000;
                if (number > 0)
                    parts.Add(" ");
            }

            var thousands = number / 1000;
            if (thousands > 0)
            {
                parts.Add(Part("{0}tausend", "eintausend", thousands));
                number %= 1000;
            }

            var hundreds = number / 100;
            if (hundreds > 0)
            {
                parts.Add(Part("{0}hundert", "einhundert", hundreds));
                number %= 100;
            }

            if (number > 0)
            {
                if (number < 20)
                {
                    if (number > 1)
                        parts.Add(UnitsMap[number]);
                    else
                        parts.Add("eins");
                }
                else
                {
                    var units = number % 10;
                    if (units > 0)
                        parts.Add(string.Format("{0}und", UnitsMap[units]));

                    parts.Add(TensMap[number / 10]);
                }
            }

            return string.Join("", parts);
        }

        private string Part(string pluralFormat, string singular, int number)
        {
            if (number == 1)
                return singular;
            return string.Format(pluralFormat, Convert(number));
        }
    }
}