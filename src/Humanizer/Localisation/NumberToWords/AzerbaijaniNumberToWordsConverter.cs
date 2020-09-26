using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class AzerbaijaniNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "sıfır", "bir", "iki", "üç", "dörd", "beş", "altı", "yeddi", "səkkiz", "doqquz" };
        private static readonly string[] TensMap = { "sıfır", "on", "iyirmi", "otuz", "qırx", "əlli", "altmış", "yetmiş", "səksən", "doxsan" };

        private static readonly Dictionary<char, string> OrdinalSuffix = new Dictionary<char, string>
        {
            {'ı', "ıncı"},
            {'i', "inci"},
            {'u', "uncu"},
            {'ü', "üncü"},
            {'o', "uncu"},
            {'ö', "üncü"},
            {'e', "inci"},
            {'a', "ıncı"},
            {'ə', "inci"},
        };

        public override string Convert(long input)
        {
            if (input > Int32.MaxValue || input < Int32.MinValue)
            {
                throw new NotImplementedException();
            }
            var number = (int)input;
            if (number == 0)
            {
                return UnitsMap[0];
            }

            if (number < 0)
            {
                return string.Format("mənfi {0}", Convert(-number));
            }

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0} milyard", Convert(number / 1000000000)));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0} milyon", Convert(number / 1000000)));
                number %= 1000000;
            }

            var thousand = (number / 1000);
            if (thousand > 0)
            {
                parts.Add(string.Format("{0} min", thousand > 1 ? Convert(thousand) : "").Trim());
                number %= 1000;
            }

            var hundred = (number / 100);
            if (hundred > 0)
            {
                parts.Add(string.Format("{0} yüz", hundred > 1 ? Convert(hundred) : "").Trim());
                number %= 100;
            }

            if ((number / 10) > 0)
            {
                parts.Add(TensMap[number / 10]);
                number %= 10;
            }

            if (number > 0)
            {
                parts.Add(UnitsMap[number]);
            }

            var toWords = string.Join(" ", parts.ToArray());

            return toWords;
        }

        public override string ConvertToOrdinal(int number)
        {
            var word = Convert(number);
            var wordSuffix = string.Empty;
            var suffixFoundOnLastVowel = false;

            for (var i = word.Length - 1; i >= 0; i--)
            {
                if (OrdinalSuffix.TryGetValue(word[i], out wordSuffix))
                {
                    suffixFoundOnLastVowel = i == word.Length - 1;
                    break;
                }
            }

            if (word[word.Length - 1] == 't')
            {
                word = word.Substring(0, word.Length - 1) + 'd';
            }

            if (suffixFoundOnLastVowel)
            {
                word = word.Substring(0, word.Length - 1);
            }

            return string.Format("{0}{1}", word, wordSuffix);
        }
    }
}
