using System;
using System.Collections.Generic;
using System.Globalization;

namespace Humanizer.Localisation.NumberToWords
{
    internal class SlovenianNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "nič", "ena", "dva", "tri", "štiri", "pet", "šest", "sedem", "osem", "devet", "deset", "enajst", "dvanajst", "trinajst", "štirinajst", "petnajst", "šestnajst", "sedemnajst", "osemnajst", "devetnajst" };
        private static readonly string[] TensMap = { "nič", "deset", "dvajset", "trideset", "štirideset", "petdeset", "šestdeset", "sedemdeset", "osemdeset", "devetdeset" };

        private readonly CultureInfo _culture;

        public SlovenianNumberToWordsConverter(CultureInfo culture)
        {
            _culture = culture;
        }

        public override string Convert(long input)
        {
            if (input > Int32.MaxValue || input < Int32.MinValue)
            {
                throw new NotImplementedException();
            }
            var number = (int)input;
            if (number == 0)
            {
                return "nič";
            }

            if (number < 0)
            {
                return string.Format("minus {0}", Convert(-number));
            }

            var parts = new List<string>();

            var billions = number / 1000000000;
            if (billions > 0)
            {
                parts.Add(Part("milijarda", "dve milijardi", "{0} milijarde", "{0} milijard", billions));
                number %= 1000000000;
                if (number > 0)
                {
                    parts.Add(" ");
                }
            }

            var millions = number / 1000000;
            if (millions > 0)
            {
                parts.Add(Part("milijon", "dva milijona", "{0} milijone", "{0} milijonov", millions));
                number %= 1000000;
                if (number > 0)
                {
                    parts.Add(" ");
                }
            }

            var thousands = number / 1000;
            if (thousands > 0)
            {
                parts.Add(Part("tisoč", "dva tisoč", "{0} tisoč", "{0} tisoč", thousands));
                number %= 1000;
                if (number > 0)
                {
                    parts.Add(" ");
                }
            }

            var hundreds = number / 100;
            if (hundreds > 0)
            {
                parts.Add(Part("sto", "dvesto", "{0}sto", "{0}sto", hundreds));
                number %= 100;
                if (number > 0)
                {
                    parts.Add(" ");
                }
            }

            if (number > 0)
            {
                if (number < 20)
                {
                    if (number > 1)
                    {
                        parts.Add(UnitsMap[number]);
                    }
                    else
                    {
                        parts.Add("ena");
                    }
                }
                else
                {
                    var units = number % 10;
                    if (units > 0)
                    {
                        parts.Add(string.Format("{0}in", UnitsMap[units]));
                    }

                    parts.Add(TensMap[number / 10]);
                }
            }

            return string.Join("", parts);
        }

        public override string ConvertToOrdinal(int number)
        {
            return number.ToString(_culture);
        }

        private string Part(string singular, string dual, string trialQuadral, string plural, int number)
        {
            if (number == 1)
            {
                return singular;
            }

            if (number == 2)
            {
                return dual;
            }

            if (number == 3 || number == 4)
            {
                return string.Format(trialQuadral, Convert(number));
            }

            return string.Format(plural, Convert(number));
        }
    }
}