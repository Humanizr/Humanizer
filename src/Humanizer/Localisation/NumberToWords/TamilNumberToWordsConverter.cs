using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class TamilNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "சுழியம்", "ஒன்று", "இரண்டு", "மூன்று", "நான்கு", "ஐந்து", "ஆறு", "ஏழு", "எட்டு", "ஒன்பது", "பத்து", "பதினொன்று", "பனிரெண்டு", "பதிமூன்று", "பதினான்கு", "பதினைந்து", "பதினாறு", "பதினேழு", "பதினெட்டு", "பத்தொன்பது" };
        private static readonly string[] TensMap = { "சுழியம்", "பத்து", "இருப", "முப்ப", "நாற்ப", "ஐம்ப", "அறுப", "எழுப", "எண்ப", "தொண்ணூ" };
        private static readonly string[] HundredsMap = { "நூ", "இருநூ", "முன்னூ", "நானூ", "ஐந்நூ", "அறுநூ", "எழுநூ", "எண்ணூ", "தொள்ளாயிர", };
        private static readonly string[] ThousandsMap = { "ஆ", "இரண்டா", "மூன்றா", "நான்கா", "ஐந்தா", "ஆறா", "ஏழா", "எட்டா", "ஒன்பதா", "பத்தா", "பதினொன்றா", "பனிரெண்டா", "பதிமூன்றா", "பதினான்கா", "பதினைந்தா", "பதினாறா", "பதினேழா", "பதினெட்டா", "பத்தொன்பதா" };

        private static readonly string[] LakhsMap = { "இலட்ச" };

        private static readonly Dictionary<long, string> OrdinalExceptions = new Dictionary<long, string>
        {
            {1, "முதலாவது"},
            {2, "இரண்டாவது"},
            {3, "மூன்றாவது"},
            {4, "நான்காவது"},
            {5, "ஐந்தாவது"},
            {8, "எட்டாவது"},
            {9, "ஒன்பதாவது"},
            {12, "பனிரெண்டாவது"},
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
                return string.Format("கழித்தல் {0}", Convert(-number));

            var parts = new List<string>();

            if ((number / 1000000000000000000) > 0)
            {
                parts.Add(string.Format("{0} quintillion", Convert(number / 1000000000000000000)));
                number %= 1000000000000000000;
            }

            if ((number / 1000000000000000) > 0)
            {
                parts.Add(string.Format("{0} quadrillion", Convert(number / 1000000000000000)));
                number %= 1000000000000000;
            }

            //if ((number / 1000000000000) > 0)
            //{
            //    parts.Add(string.Format("{0} trillion", Convert(number / 1000000000000)));
            //    number %= 1000000000000;
            //}

            //if ((number / 1000000000) > 0)
            //{
            //    parts.Add(string.Format("{0} பில்லியன்", Convert(number / 1000000000)));
            //    number %= 1000000000;
            //}

            //if ((number / 1000000) > 0)
            //{
            //    parts.Add(string.Format("{0} மில்லியன்", Convert(number / 1000000)));
            //    number %= 1000000;
            //}

            if ((number / 10000000) > 0) parts.Add(GetCroresValue(ref number));

            if ((number / 100000) > 0) parts.Add(GetLakhsValue(ref number, isOrdinal));

            if ((number / 1000) > 0) parts.Add(GetThousandsValue(ref number));

            if ((number / 100) > 0) parts.Add(GetHundredsValue(ref number));

            if (number > 0)
                parts.Add(GetTensValue(number, isOrdinal));
            else if (isOrdinal)
                parts[parts.Count - 1] += "வது";

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
                    return UnitsMap[number] + "வது";
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
                        lastPart += "ற்றி ";
                    else if (quot == 7 || quot == 8 || quot == 4)
                        lastPart += "த்தி ";
                    else
                        lastPart += "த்து ";
                    
                    if (!isThousand) lastPart += string.Format("{0}", GetUnitValue(number % 10, isOrdinal));
                }
                else if (number % 10 == 0)
                {
                    if (isThousand)
                    {
                        if (quot == 9)
                            lastPart += "றா";
                        else
                            lastPart += "தா";
                    }
                    else
                    {
                        if (quot == 9)
                            lastPart += "று";
                        else
                            lastPart += "து";
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
                local_word = "ஒரு " + LakhsMap[0];
            else local_word += GetTensValue((number / 100000), isOrdinal) + " " + LakhsMap[0];

            if (number % 1000000 == 0 || number % 100000 == 0)
                local_word += "ம்";
            else
                local_word += "த்து";

            number %= 100000;
            return local_word;
        }
        private static string GetCroresValue(ref long number)
        {
            var local_word = "";
            var num_above_10 = number / 10000000;
            var str_crore = "கோடி";

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
                local_word = "ஒரு ";
            else if (num_above_10 > 0) local_word += GetTensValue(num_above_10, false) + " ";

            local_word = local_word.TrimEnd() + " " + str_crore;
            if (number % 10000000 == 0 || number % 100000000 == 0)
                local_word += "";
            else
                local_word += "யே";

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
                    local_word += "ஓரா";
                else if (num_above_10 % 10 > 1)
                    local_word += ThousandsMap[(num_above_10 % 10) - 1];

            }
            else
                local_word += ThousandsMap[(number / 1000) - 1];

            number %= 1000;
            
            if (number > 0)
                local_word = local_word + "யிரத்து";
            else
                local_word = local_word + "யிரம்";

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
                        local_word += "ம்";
                    else
                        local_word += "த்து";
                }
                else if (number % 100 >= 1)
                    local_word += "ற்று";
                else
                    local_word += "று";

                number %= 100;

                return local_word;
            }
        }

        private static string RemoveOnePrefix(string toWords)
        {
            // one hundred => hundredth
            if (toWords.StartsWith("one", StringComparison.Ordinal))
                toWords = toWords.Remove(0, 4);

            return toWords;
        }

        private static bool ExceptionNumbersToWords(long number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }
    }
}
