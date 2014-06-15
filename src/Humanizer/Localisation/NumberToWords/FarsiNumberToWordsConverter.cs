using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class FarsiNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] FarsiHundredsMap = { "صفر", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
        private static readonly string[] FarsiTensMap = { "صفر", "ده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
        private static readonly string[] FarsiUnitsMap = { "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه", "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };

        public override string Convert(int number)
        {
            if (number < 0)
                return string.Format("منفی {0}", Convert(-number));

            if (number == 0)
                return "صفر";

            var farsiGroupsMap = new Dictionary<int, Func<int, string>> 
            { 
                {(int)Math.Pow(10, 9), n => string.Format("{0} میلیارد", Convert(n)) },
                {(int)Math.Pow(10, 6), n => string.Format("{0} میلیون", Convert(n)) },
                {(int)Math.Pow(10, 3), n => string.Format("{0} هزار", Convert(n)) },
                {(int)Math.Pow(10, 2), n => FarsiHundredsMap[n]}
            };

            var parts = new List<string>();
            foreach (var group in farsiGroupsMap.Keys)
            {
                if (number / group > 0)
                {
                    parts.Add(farsiGroupsMap[group](number / group));
                    number %= group;
                }
            }

            if (number >= 20)
            {
                parts.Add(FarsiTensMap[number / 10]);
                number %= 10;
            }

            if (number > 0)
                parts.Add(FarsiUnitsMap[number]);

            return string.Join(" و ", parts);
        }

        public override string ConvertToOrdinal(int number)
        {
            if (number == 1)
                return "اول";

            if (number == 3)
                return "سوم";

            if (number % 10 == 3 && number != 13)
                return Convert((number / 10) * 10) + " و سوم";

            var word = Convert(number);
            return string.Format("{0}{1}", word, word.EndsWith("ی") ? " ام" : "م");
        }
    }
}
