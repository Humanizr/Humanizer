using System;
using System.Collections.Generic;
using System.Globalization;

namespace Humanizer
{
    /// <summary>
    /// Transform a number into words; e.g. 1 => one
    /// </summary>
    public static class NumberToWordsExtension
    {
        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <returns></returns>
        public static string ToWords(this int number)
        {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "ar":
                    return ToArabicWords(number);
                default:
                    return ToEnglishWords(number);
            }
        }

        private static string ToEnglishWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return string.Format("minus {0}", ToWords(Math.Abs(number)));

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0} billion", ToWords(number / 1000000000)));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0} million", ToWords(number / 1000000)));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(string.Format("{0} thousand", ToWords(number / 1000)));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(string.Format("{0} hundred", ToWords(number / 100)));
                number %= 100;
            }

            if (number > 0)
            {
                if (parts.Count != 0)
                    parts.Add("and");

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    parts.Add(unitsMap[number]);
                else
                {
                    var lastPart = tensMap[number / 10];
                    if ((number % 10) > 0)
                        lastPart += string.Format("-{0}", unitsMap[number % 10]);
                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts.ToArray());
        }

        private static string ToArabicWords(int number)
        {
            string[] arabicGroup = { "مئة", "ألف", "مليون", "مليار", "تريليون", "كوادريليون", "كوينتليون", "سكستيليون" };
            string[] arabicAppendedGroup = { "", "ألفاً", "مليوناً", "ملياراً", "تريليوناً", "كوادريليوناً", "كوينتليوناً", "سكستيليوناً" };
            string[] arabicPluralGroups = { "", "آلاف", "ملايين", "مليارات", "تريليونات", "كوادريليونات", "كوينتليونات", "سكستيليونات" };
            string[] onesGroup = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
            string[] tensGroup = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
            string[] hundredsGroup = { "", "مئة", "مئتان", "ثلاث مئة", "أربع مئة", "خمس مئة", "ست مئة", "سبع مئة", "ثمان مئة", "تسع مئة" };
            string[] arabicAppendedTwos = { "مئتان", "ألفان", "مليونان", "ملياران", "تريليونان", "كوادريليونان", "كوينتليونان", "سكستيليونلن" };
            string[] arabicTwos = { "مئتان", "ألفان", "مليونان", "ملياران", "تريليونان", "كوادريليونان", "كوينتليونان", "سكستيليونان" };

            if (number == 0)
                return "صفر";

            string result = String.Empty;
            int groupLevel = 0;

            while (number >= 1)
            {
                int groupNumber = number % 1000;
                number /= 1000;

                int tens = groupNumber % 100;
                int hundreds = groupNumber / 100;
                string process = String.Empty;

                if (hundreds > 0)
                {
                    if (tens == 0 && hundreds == 2)
                        process = arabicAppendedTwos[0];
                    else
                        process = hundredsGroup[hundreds];
                }

                if (tens > 0)
                {
                    if (tens < 20)
                    {
                        if (tens == 2 && hundreds == 0 && groupLevel > 0)
                        {
                            if (number == 2000 || number == 2000000 || number == 2000000000)
                                process = arabicAppendedTwos[groupLevel];
                            else
                                process = arabicTwos[groupLevel];
                        }
                        else
                        {
                            if (process != String.Empty)
                                process += " و ";

                            if (tens == 1 && groupLevel > 0 && hundreds == 0)
                                process += " ";
                            else
                                process += onesGroup[tens];
                        }
                    }
                    else
                    {
                        int ones = tens % 10;
                        tens = (tens / 10);

                        if (ones > 0)
                        {
                            if (process != String.Empty)
                                process += " و ";

                            process += onesGroup[ones];
                        }

                        if (process != String.Empty)
                            process += " و ";

                        process += tensGroup[tens];
                    }
                }

                if (process != String.Empty)
                {
                    if (groupLevel > 0)
                    {
                        if (result != String.Empty)
                            result = String.Format("{0} {1}", "و", result);

                        if (groupNumber != 2)
                        {
                            if (groupNumber % 100 != 1)
                            {
                                if (groupNumber >= 3 && groupNumber <= 10)
                                    result = String.Format("{0} {1}", arabicPluralGroups[groupLevel], result);
                                else
                                {
                                    result = String.Format("{0} {1}", result != String.Empty ? arabicAppendedGroup[groupLevel] : arabicGroup[groupLevel], result);
                                }
                            }
                            else
                            {
                                result = String.Format("{0} {1}", arabicGroup[groupLevel], result);
                            }
                        }
                    }
                    result = String.Format("{0} {1}", process, result);
                }
                groupLevel++;
            }

            return result.Trim();
        }
    }
}
