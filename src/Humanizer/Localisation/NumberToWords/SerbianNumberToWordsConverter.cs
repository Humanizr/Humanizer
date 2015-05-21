using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.NumberToWords
{
    internal class SerbianNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string SR_NAME = "sr";
        private static readonly string SR_LATN_NAME = "sr-Latn";

        private static string[] UnitsMap = new string[1];
        private static string[] TensMap = new string[1];

        private static readonly string[] UnitsMapSr = { "нула", "један", "два", "три", "четири", "пет", "шест", "седам", "осам", "девет", "десет", "једанест", "дванаест", "тринаест", "четрнаест", "петнаест", "шестнаест", "седамнаест", "осамнаест", "деветнаест" };
        private static readonly string[] TensMapSr = { "нула", "десет", "двадесет", "тридесет", "четрдесет", "петдесет", "шестдесет", "седамдесет", "осамдесет", "деветдесет" };

        private static readonly string[] UnitsMapLatn = { "nula", "jedan", "dva", "tri", "četiri", "pet", "šest", "sedam", "osam", "devet", "deset", "jedanaest", "dvanaest", "trinaest", "četrnaestt", "petnaest", "šestnaest", "sedemnaest", "osemnaest", "devetnaest" };
        private static readonly string[] TensMapLatn = { "nula", "deset", "dvadeset", "trideset", "četrdeset", "petdeset", "šestdeset", "sedamdeset", "osamdeset", "devetdeset" };

        private readonly CultureInfo _culture;

        public SerbianNumberToWordsConverter(CultureInfo culture)
        {
            _culture = culture;

            if (_culture != null && _culture.Name.Equals(SR_LATN_NAME)) 
            {
                UnitsMap = UnitsMapLatn;
                TensMap = TensMapLatn;
            }
            else 
            {
                UnitsMap = UnitsMapSr;
                TensMap = TensMapSr;
            }
        }

        public override string Convert(int number)
        {
            bool isLatn = (_culture != null &&_culture.Name == SR_LATN_NAME);

            if (number == 0)
            {
                return (isLatn) ? "nula" : "нула";
            }

            if (number < 0)
            {
                return string.Format("- {0}", Convert(-number));
            }

            var parts = new List<string>();
            var billions = number / 1000000000;

            if (billions > 0)
            {
                if (isLatn)
                {
                    parts.Add(Part("milijarda", "dve milijarde", "{0} milijarde", "{0} milijarda", billions));
                }
                else
                {
                    parts.Add(Part("милијарда", "две милијарде", "{0} милијарде", "{0} милијарда", billions));
                }

                number %= 1000000000;
                
                if (number > 0)
                {
                    parts.Add(" ");
                }
            }

            var millions = number / 1000000;

            if (millions > 0)
            {
                if (isLatn)
                {
                    parts.Add(Part("milion", "dva miliona", "{0} miliona", "{0} miliona", millions));
                }
                else 
                {
                    parts.Add(Part("милион", "два милиона", "{0} милиона", "{0} милиона", millions));
                }

                number %= 1000000;

                if (number > 0)
                {
                    parts.Add(" ");
                }
            }

            var thousands = number / 1000;
            
            if (thousands > 0)
            {
                if (isLatn)
                {
                    parts.Add(Part("hiljadu", "dve hiljade", "{0} hiljade", "{0} hiljada", thousands));
                }
                else
                {
                    parts.Add(Part("хиљаду", "две хиљаде", "{0} хиљаде", "{0} хиљада", thousands));
                }

                number %= 1000;

                if (number > 0)
                {
                    parts.Add(" ");
                }
            }

            var hundreds = number / 100;

            if (hundreds > 0)
            {
                if (isLatn)
                {
                    parts.Add(Part("sto", "dvesto", "{0}sto", "{0}sto", hundreds));
                }
                else
                {
                    parts.Add(Part("сто", "двесто", "{0}сто", "{0}сто", hundreds));
                }
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
                    parts.Add(UnitsMap[number]);
                }
                else
                {                    
                    parts.Add(TensMap[number / 10]);

                    var units = number % 10;

                    if (units > 0)
                    {
                        parts.Add(string.Format(" {0}", UnitsMap[units]));
                    }
                }
            }

            return string.Join("", parts);
        }

        public override string ConvertToOrdinal(int number)
        {
            //TODO: In progress
            return number.ToString(_culture);
        }

        private string Part(string singular, string dual, string trialQuadral, string plural, int number)
        {
            switch (number)
            {
                case 1:
                    return singular;
                case 2:
                    return dual;
                case 3:
                case 4:
                    return string.Format(trialQuadral, Convert(number));
                default:
                    return string.Format(plural, Convert(number));
            }
        }
    }
    
}
