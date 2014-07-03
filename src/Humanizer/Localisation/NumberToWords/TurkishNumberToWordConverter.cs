using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.NumberToWords
{
    internal class TurkishNumberToWordConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "sıfır", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz" };
        private static readonly string[] TensMap = { "sıfır", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan" };

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
        };

        public override string Convert(int number)
        {
            if (number == 0)
                return UnitsMap[0];

            if (number < 0)
                return string.Format("eksi {0}", Convert(-number));

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0} milyar", Convert(number / 1000000000)));
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
                parts.Add(string.Format("{0} bin", thousand > 1 ? Convert(thousand) : "").Trim());
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

            string toWords = string.Join(" ", parts.ToArray());

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
                word = word.Substring(0, word.Length - 1) + 'd';

            if (suffixFoundOnLastVowel)
                word = word.Substring(0, word.Length - 1);

            return String.Format("{0}{1}", word, wordSuffix);
        }
    }
}
