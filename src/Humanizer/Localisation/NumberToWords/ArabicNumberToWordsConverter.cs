using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.NumberToWords
{
    internal class ArabicNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] Groups = { "مئة", "ألف", "مليون", "مليار", "تريليون", "كوادريليون", "كوينتليون", "سكستيليون" };
        private static readonly string[] AppendedGroups = { "", "ألفاً", "مليوناً", "ملياراً", "تريليوناً", "كوادريليوناً", "كوينتليوناً", "سكستيليوناً" };
        private static readonly string[] PluralGroups = { "", "آلاف", "ملايين", "مليارات", "تريليونات", "كوادريليونات", "كوينتليونات", "سكستيليونات" };
        private static readonly string[] OnesGroup = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
        private static readonly string[] TensGroup = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
        private static readonly string[] HundredsGroup = { "", "مئة", "مئتان", "ثلاث مئة", "أربع مئة", "خمس مئة", "ست مئة", "سبع مئة", "ثمان مئة", "تسع مئة" };
        private static readonly string[] AppendedTwos = { "مئتان", "ألفان", "مليونان", "ملياران", "تريليونان", "كوادريليونان", "كوينتليونان", "سكستيليونلن" };
        private static readonly string[] Twos = { "مئتان", "ألفان", "مليونان", "ملياران", "تريليونان", "كوادريليونان", "كوينتليونان", "سكستيليونان" };

        public override string Convert(int number)
        {
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
                        process = AppendedTwos[0];
                    else
                        process = HundredsGroup[hundreds];
                }

                if (tens > 0)
                {
                    if (tens < 20)
                    {
                        if (tens == 2 && hundreds == 0 && groupLevel > 0)
                        {
                            if (number == 2000 || number == 2000000 || number == 2000000000)
                                process = AppendedTwos[groupLevel];
                            else
                                process = Twos[groupLevel];
                        }
                        else
                        {
                            if (process != String.Empty)
                                process += " و ";

                            if (tens == 1 && groupLevel > 0 && hundreds == 0)
                                process += " ";
                            else
                                process += OnesGroup[tens];
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

                            process += OnesGroup[ones];
                        }

                        if (process != String.Empty)
                            process += " و ";

                        process += TensGroup[tens];
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
                                    result = String.Format("{0} {1}", PluralGroups[groupLevel], result);
                                else
                                    result = String.Format("{0} {1}", result != String.Empty ? AppendedGroups[groupLevel] : Groups[groupLevel], result);
                            }
                            else
                                result = String.Format("{0} {1}", Groups[groupLevel], result);
                        }
                    }

                    result = String.Format("{0} {1}", process, result);
                }

                groupLevel++;
            }

            return result.Trim();
        }

        private static readonly Dictionary<string, string> OrdinalExceptions = new Dictionary<string, string>
        {
            {"واحد", "الحادي"},
            {"أحد", "الحادي"},
            {"اثنان", "الثاني"},
            {"اثنا", "الثاني"},
            {"ثلاثة", "الثالث"},
            {"أربعة", "الرابع"},
            {"خمسة", "الخامس"},
            {"ستة", "السادس"},
            {"سبعة", "السابع"},
            {"ثمانية", "الثامن"},
            {"تسعة", "التاسع"},
            {"عشرة", "العاشر"},
        };

        public override string ConvertToOrdinal(int number)
        {
            if (number == 0) return "الصفر";
            var beforeOneHundredNumber = number%100;
            var overTensPart = number/100*100;
            var beforeOneHundredWord = string.Empty;
            var overTensWord = string.Empty;
            
            if (beforeOneHundredNumber > 0)
            {
                beforeOneHundredWord = Convert(beforeOneHundredNumber);
                beforeOneHundredWord = ParseNumber(beforeOneHundredWord, beforeOneHundredNumber);
            }
            
            if (overTensPart > 0)
            {
                overTensWord = Convert(overTensPart);
                overTensWord = ParseNumber(overTensWord, overTensPart);
            }

            var word = beforeOneHundredWord + 
                (overTensPart > 0
                    ? (string.IsNullOrWhiteSpace(beforeOneHundredWord) ? string.Empty : " بعد ") + overTensWord
                    : string.Empty);
            return word.Trim();
        }

        private static string ParseNumber(string word, int number)
        {
            if (number == 1) 
                return "الأول";
            
            if (number <= 10)
            {
                foreach (var kv in OrdinalExceptions.Where(kv => word.EndsWith(kv.Key)))
                {
                    // replace word with exception
                    return word.Substring(0, word.Length - kv.Key.Length) + kv.Value;
                }
            }
            else if (number > 10 && number < 100)
            {
                var parts = word.Split(' ');
                var newParts = new string[parts.Length];
                int count = 0;

                foreach (var part in parts)
                {
                    var newPart = part;
                    var oldPart = part;

                    foreach (var kv in OrdinalExceptions.Where(kv => oldPart.EndsWith(kv.Key)))
                    {
                        // replace word with exception
                        newPart = oldPart.Substring(0, oldPart.Length - kv.Key.Length) + kv.Value;
                    }
                    
                    if (number > 19 && newPart == oldPart && oldPart.Length > 1)
                        newPart = "ال" + oldPart;
                    
                    newParts[count++] = newPart;
                }

                word = string.Join(" ", newParts);
            }
            else
                word = "ال"+word;

            return word;
        }
    }
}