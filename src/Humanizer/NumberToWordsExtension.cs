using System;
using System.Collections.Generic;

namespace Humanizer
{
    public static class NumberToWordsExtension
    {
        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <returns></returns>
        public static string ToWords(this int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return string.Format("minus {0}", ToWords(Math.Abs(number)));

            var parts = new List<string>();

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

        private static string ProcessArabicGroup(int groupNumber, int groupLevel, int number)
        {
            string[] Ones = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
            string[] Tens = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
            string[] Hundreds = { "", "مائة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };
            string[] arabicAppendedTwos = { "مئتا", "ألفا", "مليونا", "مليارا", "تريليونا", "كوادريليونا", "كوينتليونا", "سكستيليونا" };
            string[] arabicTwos = { "مئتان", "ألفان", "مليونان", "ملياران", "تريليونان", "كوادريليونان", "كوينتليونان", "سكستيليونان" };

            int tens = groupNumber % 100;
            int hundreds = groupNumber / 100;
            string result = String.Empty;

            if (hundreds > 0)
            {
                if (tens == 0 && hundreds == 2)
                    result = arabicAppendedTwos[0];
                else
                    result = Hundreds[hundreds];
            }

            if (tens > 0)
            {
                if (tens < 20)
                {
                    if (tens == 2 && hundreds == 0 && groupLevel > 0)
                    {
                        if (number == 2000 || number == 2000000 || number == 2000000000)
                            result = arabicAppendedTwos[groupLevel];
                        else
                            result = arabicTwos[groupLevel];
                    }
                    else
                    {
                        if (result != String.Empty)
                            result += " و ";

                        if (tens == 1 && groupLevel > 0 && hundreds == 0)
                            result += " ";
                        else
                            result += Ones[tens];
                    }
                }
                else
                {
                    int ones = tens % 10;
                    tens = (tens / 10);

                    if (ones > 0)
                    {
                        if (result != String.Empty)
                            result += " و ";

                        result += Ones[ones];
                    }

                    if (result != String.Empty)
                        result += " و ";

                    result += Tens[tens];
                }
            }

            return result;
        }

        /// <summary>
        /// 3501.ToWords() -> "ثلاث آلاف و خمس مائة و واحد"
        /// </summary>
        /// <param name="number">Number to be turned to words in Arabic</param>
        /// <returns></returns>
        public static string ToArabicWords(this int number)
        {
            string[] arabicGroup = { "مائة", "ألف", "مليون", "مليار", "تريليون", "كوادريليون", "كوينتليون", "سكستيليون" };
            string[] arabicAppendedGroup = { "", "ألفاً", "مليوناً", "ملياراً", "تريليوناً", "كوادريليوناً", "كوينتليوناً", "سكستيليوناً" };
            string[] arabicPluralGroups = { "", "آلاف", "ملايين", "مليارات", "تريليونات", "كوادريليونات", "كوينتليونات", "سكستيليونات" };

            if (number == 0)
                return "صفر";

            string result = String.Empty;
            int group = 0;

            while (number >= 1)
            {
                int numberToProcess = number % 1000;
                number /= 1000;

                string groupDescription = ProcessArabicGroup(numberToProcess, group, number);

                if (groupDescription != String.Empty)
                {
                    if (group > 0)
                    {
                        if (result != String.Empty)
                            result = String.Format("{0} {1}", "و", result);

                        if (numberToProcess != 2)
                        {
                            if (numberToProcess % 100 != 1)
                            {
                                if (numberToProcess >= 3 && numberToProcess <= 10)
                                    result = String.Format("{0} {1}", arabicPluralGroups[group], result);
                                else
                                {
                                    if (result != String.Empty)
                                        result = String.Format("{0} {1}", arabicAppendedGroup[group], result);
                                    else
                                        result = String.Format("{0} {1}", arabicGroup[group], result);
                                }
                            }
                            else
                            {
                                result = String.Format("{0} {1}", arabicGroup[group], result);
                            }
                        }
                    }
                    result = String.Format("{0} {1}", groupDescription, result);
                }
                group++;
            }

            return result.TrimEnd();
        }
    }
}
