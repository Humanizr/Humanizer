using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class CentralKurdishNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] KurdishHundredsMap = { "سفر", "سەد", "دوو سەد", "سێ سەد", "چوار سەد", "پێنج سەد", "شەش سەد", "حەوت سەد", "هەشت سەد", "نۆ سەد" };
        private static readonly string[] KurdishTensMap = { "سفر", "دە", "بیست", "سی", "چل", "پەنجا", "شەست", "حەفتا", "هەشتا", "نەوەد" };
        private static readonly string[] KurdishUnitsMap = { "سفر", "یەک", "دوو", "سێ", "چوار", "پێنج", "شەش", "حەوت", "هەشت", "نۆ", "دە", "یازدە", "دوازدە", "سێزدە", "چواردە", "پازدە", "شازدە", "حەڤدە", "هەژدە", "نۆزدە" };

        public override string Convert(long number)
        {
            var largestNumber = (Math.Pow(10, 15) * 1000) - 1;
            if (number > largestNumber || number < -largestNumber)
            {
                throw new NotImplementedException();
            }

            if (number < 0)
            {
                return string.Format("نێگەتیڤ {0}", Convert(-number));
            }

            if (number == 0)
            {
                return "سفر";
            }

            var kurdishGroupsMap = new Dictionary<long, Func<long, string>>
            {
                {(long)Math.Pow(10, 15), n => string.Format("{0} کوادریلیۆن", Convert(n)) },
                {(long)Math.Pow(10, 12), n => string.Format("{0} تریلیۆن", Convert(n)) },
                {(long)Math.Pow(10, 9), n => string.Format("{0} میلیارد", Convert(n)) },
                {(long)Math.Pow(10, 6), n => string.Format("{0} میلیۆن", Convert(n)) },
                {(long)Math.Pow(10, 3), n => string.Format("{0} هەزار", Convert(n)) },
                {(long)Math.Pow(10, 2), n => KurdishHundredsMap[n]}
            };

            var parts = new List<string>();
            foreach (var group in kurdishGroupsMap.Keys)
            {
                if (number / group > 0)
                {
                    parts.Add(kurdishGroupsMap[group](number / group));
                    number %= group;
                }
            }

            if (number >= 20)
            {
                parts.Add(KurdishTensMap[number / 10]);
                number %= 10;
            }

            if (number > 0)
            {
                parts.Add(KurdishUnitsMap[number]);
            }

            var sentence = string.Join(" و ", parts);
            if (sentence.StartsWith("یەک هەزار"))
                return sentence.Substring(" یەک".Length);
            else
                return sentence;
        }

        public override string ConvertToOrdinal(int number)
        {
            var word = Convert(number);
            return string.Format("{0}{1}", word, IsVowel(word[word.Length - 1]) ? "یەم" : "ەم");
        }

        private bool IsVowel(char c)
        {
            return c == 'ا' ||
                c == 'ێ' ||
                c == 'ۆ' ||
                c == 'ە' ||
                c == 'ی';
        }
    }
}
