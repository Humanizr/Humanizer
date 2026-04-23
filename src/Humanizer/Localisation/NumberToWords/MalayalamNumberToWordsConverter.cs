using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class MalayalamNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "പൂജ്യം", "ഒന്ന്", "രണ്ട്", "മൂന്ന്", "നാല്", "അഞ്ച്", "ആറ്", "ഏഴ്", "എട്ട്", "ഒമ്പത്", "പത്ത്", "പതിനൊന്ന്", "പന്ത്രണ്ട്" ,"പതിമൂന്ന്","പതിനാല്","പതിനഞ്ച്","പതിനാറ്","പതിനേഴ്","പതിനെട്ട്","പത്തൊമ്പത്" };
        private static readonly string[] TensMap = { "സീറോ", "പ", "ഇരുപ", "മുപ്പ", "നാൽപ്പ", "അമ്പ", "അറുപ", "എഴുപ", "എൺപ", "തൊണ്ണൂ" };
        private static readonly string[] HundredsMap = { "നൂ", "ഇരുന്നു", "മുന്നൂ", "നാനൂ", "അഞ്ഞൂ", "അറുന്നൂ", "എഴുന്നൂ", "എണ്ണൂ", "തൊള്ളായിര", };
        private static readonly string[] ThousandsMap = { "ആ", "രണ്ടാ", "മൂവാ", "നാലാ", "അയ്യാ", "ആറാ", "ഏഴാ", "എട്ടാ", "ഒന്‍പതാ", "പത്താ", "പതിനൊന്നാ", "പന്ത്രണ്ടാ", "പതിമൂന്നാ", "പതിനാലാ", "പതിനഞ്ചാ", "പതിനാറാ", "പതിനേഴാ", "പതിനെട്ടാ", "പത്തൊമ്പതാ" };

        private static readonly string[] LakhsMap = { "ലക്ഷ" };

        private static readonly Dictionary<long, string> OrdinalExceptions = new Dictionary<long, string>
        {
            {1, "ഒന്നാമത്"},
            {2, "രണ്ടാമത്"},
            {3, "മൂന്നാമത്"},
            {4, "നാലാമത്"},
            {5, "അഞ്ചാമത്"},
            {8, "എട്ടാമത്"},
            {9, "ഒമ്പതാമത്"},
            {12, "പന്ത്രണ്ടാമത്"},
        };

        public override string Convert(long number)
        {
            return ConvertImpl(number, false);
        }

        public override string ConvertToOrdinal(int number)
        {
            return ConvertImpl(number, true);
        }

        private string ConvertImpl(long number, bool isOrdinal)
        {
            if (number == 0)
                return GetUnitValue(0, isOrdinal);

            if (number < 0)
                return string.Format("മൈനസ് {0}", Convert(-number));

            var parts = new List<string>();

            if ((number / 1000000000000000000) > 0)
            {
                parts.Add(string.Format("{0} ക്വിന്റില്യൺ", Convert(number / 1000000000000000000)));
                number %= 1000000000000000000;
            }

            if ((number / 1000000000000000) > 0)
            {
                parts.Add(string.Format("{0} ക്വാഡ്രില്യൺ", Convert(number / 1000000000000000)));
                number %= 1000000000000000;
            }

            //if ((number / 1000000000000) > 0)
            //{
            //    parts.Add(string.Format("{0} ട്രില്യണ്‍", Convert(number / 1000000000000)));
            //    number %= 1000000000000;
            //}

            //if ((number / 1000000000) > 0)
            //{
            //    parts.Add(string.Format("{0} ബില്യണ്‍", Convert(number / 1000000000)));
            //    number %= 1000000000;
            //}

            //if ((number / 1000000) > 0)
            //{
            //    parts.Add(string.Format("{0} മില്യണ്‍", Convert(number / 1000000)));
            //    number %= 1000000;
            //}

            if ((number / 10000000) > 0) parts.Add(GetCroresValue(ref number));

            if ((number / 100000) > 0) parts.Add(GetLakhsValue(ref number, isOrdinal));

            if ((number / 1000) > 0) parts.Add(GetThousandsValue(ref number));

            if ((number / 100) > 0) parts.Add(GetHundredsValue(ref number));

            if (number > 0)
                parts.Add(GetTensValue(number, isOrdinal));
            else if (isOrdinal)
                parts[parts.Count - 1] += "മത്";

            var toWords = string.Join(" ", parts.ToArray());

            if (isOrdinal)
                toWords = RemoveOnePrefix(toWords);

            return toWords;
        }

        private static string GetUnitValue(long number, bool isOrdinal)
        {
            if (isOrdinal)
            {
                if (ExceptionNumbersToWords(number, out var exceptionString))
                    return exceptionString;
                else
                    return UnitsMap[number] + "മത്";
            }
            else
                return UnitsMap[number];
        }

        private static string GetTensValue(long number, bool isOrdinal, bool isThousand = false)
        {
            var local_word = "";
            if (number < 20) 
                local_word = GetUnitValue(number, isOrdinal);
            else if ((number >= 20) && (number <= 99))
            {
                var lastPart = TensMap[number / 10];
                var quot = number / 10;
                if ((number % 10) > 0)
                {
                    if (quot == 9) 
                        lastPart += "റ്റി ";
                    else if (quot == 7 || quot == 8 || quot == 4)
                        lastPart += "ത്തി ";
                    else
                        lastPart += "ത്തു ";
                    
                    if (!isThousand) lastPart += string.Format("{0}", GetUnitValue(number % 10, isOrdinal));
                }
                else if (number % 10 == 0)
                {
                    if (isThousand)
                    {
                        if (quot == 9)
                            lastPart += "രാ";
                        else
                            lastPart += "താ";
                    }
                    else
                    {
                        if (quot == 9)
                            lastPart += "റ്";
                        else
                            lastPart += "ത്";
                    }
                }
                else if (isOrdinal)
                    lastPart = lastPart.TrimEnd('y') + "ieth";

                local_word = lastPart;
            }
            return local_word;
        }
        private static string GetLakhsValue(ref long number, bool isOrdinal)
        {
            var num_above_10 = number / 100000;
            var local_word = "";
            if (num_above_10 >= 20)
            {
                local_word = GetTensValue(num_above_10, false, false);
                local_word += " " + LakhsMap[0];
            }
            else if (num_above_10 == 1)
                local_word = "ഒരു " + LakhsMap[0];
            else local_word += GetTensValue((number / 100000), isOrdinal) + " " + LakhsMap[0];

            if (number % 1000000 == 0 || number % 100000 == 0)
                local_word += "๐";
            else
                local_word += "ത്തി";

            number %= 100000;
            return local_word;
        }
        private static string GetCroresValue(ref long number)
        {
            var local_word = "";
            var num_above_10 = number / 10000000;
            var str_crore = "കോടി";

            if (num_above_10 > 99999 && num_above_10 <= 9999999)
            {
                local_word = GetLakhsValue(ref num_above_10, false);
                local_word += " ";
            }

            if (num_above_10 > 999 && num_above_10 <= 99999)
            {
                local_word += GetThousandsValue(ref num_above_10);
                local_word += " ";
            }
            if (num_above_10 > 99 && num_above_10 <= 999)
            {
                local_word += GetHundredsValue(ref num_above_10);
                local_word += " ";
            }

            if (num_above_10 >= 20)
            {
                local_word += GetTensValue(num_above_10, false, false);
                local_word += " ";
            }
            else if (num_above_10 == 1)
                local_word = "ഒരു ";
            else if (num_above_10 > 0) local_word += GetTensValue(num_above_10, false) + " ";

            local_word = local_word.TrimEnd() + " " + str_crore;
            if (number % 10000000 == 0 || number % 100000000 == 0)
                local_word += "";
            else
                local_word += "";

            number %= 10000000;
            return local_word;

        }
        private static string GetThousandsValue(ref long number)
        {

            var num_above_10 = number / 1000;
            var local_word = "";
            if (num_above_10 >= 20)
            {
                local_word = GetTensValue(num_above_10, false, true);

                if (num_above_10 % 10 == 1)
                    local_word += "ഒരാ";
                else if (num_above_10 % 10 > 1)
                    local_word += ThousandsMap[(num_above_10 % 10) - 1];

            }
            else
                local_word += ThousandsMap[(number / 1000) - 1];

            number %= 1000;
            
            if (number > 0)
                local_word = local_word + "യിരത്തി";
            else
                local_word = local_word + "യിരം";

            return local_word;
        }
        private static string GetHundredsValue(ref long number)
        {
            string local_word = "";

            {
                local_word = HundredsMap[(number / 100) - 1];
                if (number / 100 == 9)
                {
                    if (number % 100 == 0)
                        local_word += "๐";
                    else
                        local_word += "ത്തി";
                }
                else if (number % 100 >= 1)
                    local_word += "റ്റി";
                else
                    local_word += "റ്";

                number %= 100;

                return local_word;
            }
        }

        private static string RemoveOnePrefix(string toWords)
        {
            // one hundred => hundredth
            if (toWords.StartsWith("ഒന്ന്", StringComparison.Ordinal))
                toWords = toWords.Remove(0, 4);

            return toWords;
        }

        private static bool ExceptionNumbersToWords(long number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }
    }
}
