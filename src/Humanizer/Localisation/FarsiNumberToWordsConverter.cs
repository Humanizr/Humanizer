﻿using System;
using System.Collections.Generic;

namespace Humanizer.Localisation
{
    internal class FarsiNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private static readonly string[] farsiHundredsMap = { "صفر", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
        private static readonly string[] farsiTensMap = { "صفر", "ده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
        private static readonly string[] farsiUnitsMap = { "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه", "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };

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
                {(int)Math.Pow(10, 2), n => farsiHundredsMap[n]}
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
                parts.Add(farsiTensMap[number / 10]);
                number %= 10;
            }

            if (number > 0)
                parts.Add(farsiUnitsMap[number]);

            return string.Join(" و ", parts);
        }
    }
}
