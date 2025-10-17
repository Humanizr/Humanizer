namespace Humanizer;

class TamilNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["சுழியம்", "ஒன்று", "இரண்டு", "மூன்று", "நான்கு", "ஐந்து", "ஆறு", "ஏழு", "எட்டு", "ஒன்பது", "பத்து", "பதினொன்று", "பனிரெண்டு", "பதிமூன்று", "பதினான்கு", "பதினைந்து", "பதினாறு", "பதினேழு", "பதினெட்டு", "பத்தொன்பது"];
    static readonly string[] TensMap = ["சுழியம்", "பத்து", "இருப", "முப்ப", "நாற்ப", "ஐம்ப", "அறுப", "எழுப", "எண்ப", "தொண்ணூ"];
    static readonly string[] HundredsMap = ["நூ", "இருநூ", "முன்னூ", "நானூ", "ஐந்நூ", "அறுநூ", "எழுநூ", "எண்ணூ", "தொள்ளாயிர"];
    static readonly string[] ThousandsMap = ["ஆ", "இரண்டா", "மூன்றா", "நான்கா", "ஐந்தா", "ஆறா", "ஏழா", "எட்டா", "ஒன்பதா", "பத்தா", "பதினொன்றா", "பனிரெண்டா", "பதிமூன்றா", "பதினான்கா", "பதினைந்தா", "பதினாறா", "பதினேழா", "பதினெட்டா", "பத்தொன்பதா"];

    static readonly string[] LakhsMap = ["இலட்ச"];

    static readonly FrozenDictionary<long, string> OrdinalExceptions = new Dictionary<long, string>
    {
        {
            1, "முதலாவது"
        },
        {
            2, "இரண்டாவது"
        },
        {
            3, "மூன்றாவது"
        },
        {
            4, "நான்காவது"
        },
        {
            5, "ஐந்தாவது"
        },
        {
            8, "எட்டாவது"
        },
        {
            9, "ஒன்பதாவது"
        },
        {
            12, "பனிரெண்டாவது"
        },
    }.ToFrozenDictionary();

    public override string Convert(long number) =>
        ConvertImpl(number, false);

    public override string ConvertToOrdinal(int number) =>
        ConvertImpl(number, true);

    string ConvertImpl(long number, bool isOrdinal)
    {
        if (number == 0)
        {
            return GetUnitValue(0, isOrdinal);
        }

        if (number < 0)
        {
            return $"கழித்தல் {Convert(-number)}";
        }

        var parts = new List<string>();

        if (number / 1000000000000000000 > 0)
        {
            parts.Add($"{Convert(number / 1000000000000000000)} quintillion");
            number %= 1000000000000000000;
        }

        if (number / 1000000000000000 > 0)
        {
            parts.Add($"{Convert(number / 1000000000000000)} quadrillion");
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

        if (number / 10000000 > 0)
        {
            parts.Add(GetCroresValue(ref number));
        }

        if (number / 100000 > 0)
        {
            parts.Add(GetLakhsValue(ref number, isOrdinal));
        }

        if (number / 1000 > 0)
        {
            parts.Add(GetThousandsValue(ref number));
        }

        if (number / 100 > 0)
        {
            parts.Add(GetHundredsValue(ref number));
        }

        if (number > 0)
        {
            parts.Add(GetTensValue(number, isOrdinal));
        }
        else if (isOrdinal)
        {
            parts[^1] += "வது";
        }

        var toWords = string.Join(" ", parts);

        if (isOrdinal)
        {
            toWords = RemoveOnePrefix(toWords);
        }

        return toWords;
    }

    static string GetUnitValue(long number, bool isOrdinal)
    {
        if (isOrdinal)
        {
            if (ExceptionNumbersToWords(number, out var exceptionString))
            {
                return exceptionString;
            }

            return UnitsMap[number] + "வது";
        }

        return UnitsMap[number];
    }

    static string GetTensValue(long number, bool isOrdinal, bool isThousand = false)
    {
        var local_word = "";
        if (number < 20)
        {
            local_word = GetUnitValue(number, isOrdinal);
        }
        else if (number is >= 20 and <= 99)
        {
            var lastPart = TensMap[number / 10];
            var quot = number / 10;
            if (number % 10 > 0)
            {
                if (quot == 9)
                {
                    lastPart += "ற்றி ";
                }
                else if (quot is 7 or 8 or 4)
                {
                    lastPart += "த்தி ";
                }
                else
                {
                    lastPart += "த்து ";
                }

                if (!isThousand)
                {
                    lastPart += $"{GetUnitValue(number % 10, isOrdinal)}";
                }
            }
            else if (number % 10 == 0)
            {
                if (isThousand)
                {
                    if (quot == 9)
                    {
                        lastPart += "றா";
                    }
                    else
                    {
                        lastPart += "தா";
                    }
                }
                else
                {
                    if (quot == 9)
                    {
                        lastPart += "று";
                    }
                    else
                    {
                        lastPart += "து";
                    }
                }
            }
            else if (isOrdinal)
            {
                lastPart = lastPart.TrimEnd('y') + "ieth";
            }

            local_word = lastPart;
        }

        return local_word;
    }

    static string GetLakhsValue(ref long number, bool isOrdinal)
    {
        var num_above_10 = number / 100000;
        var local_word = "";
        if (num_above_10 >= 20)
        {
            local_word = GetTensValue(num_above_10, false, false);
            local_word += " " + LakhsMap[0];
        }
        else if (num_above_10 == 1)
        {
            local_word = "ஒரு " + LakhsMap[0];
        }
        else
        {
            local_word += GetTensValue(number / 100000, isOrdinal) + " " + LakhsMap[0];
        }

        if (number % 1000000 == 0 || number % 100000 == 0)
        {
            local_word += "ம்";
        }
        else
        {
            local_word += "த்து";
        }

        number %= 100000;
        return local_word;
    }

    static string GetCroresValue(ref long number)
    {
        var local_word = "";
        var num_above_10 = number / 10000000;
        var str_crore = "கோடி";

        if (num_above_10 is > 99999 and <= 9999999)
        {
            local_word = GetLakhsValue(ref num_above_10, false);
            local_word += " ";
        }

        if (num_above_10 is > 999 and <= 99999)
        {
            local_word += GetThousandsValue(ref num_above_10);
            local_word += " ";
        }

        if (num_above_10 is > 99 and <= 999)
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
        {
            local_word = "ஒரு ";
        }
        else if (num_above_10 > 0)
        {
            local_word += GetTensValue(num_above_10, false) + " ";
        }

        local_word = local_word.TrimEnd() + " " + str_crore;
        if (number % 10000000 == 0 || number % 100000000 == 0)
        {
            local_word += "";
        }
        else
        {
            local_word += "யே";
        }

        number %= 10000000;
        return local_word;
    }

    static string GetThousandsValue(ref long number)
    {
        var num_above_10 = number / 1000;
        var local_word = "";
        if (num_above_10 >= 20)
        {
            local_word = GetTensValue(num_above_10, false, true);

            if (num_above_10 % 10 == 1)
            {
                local_word += "ஓரா";
            }
            else if (num_above_10 % 10 > 1)
            {
                local_word += ThousandsMap[num_above_10 % 10 - 1];
            }
        }
        else
        {
            local_word += ThousandsMap[number / 1000 - 1];
        }

        number %= 1000;

        if (number > 0)
        {
            local_word += "யிரத்து";
        }
        else
        {
            local_word += "யிரம்";
        }

        return local_word;
    }

    static string GetHundredsValue(ref long number)
    {
        var local_word = HundredsMap[number / 100 - 1];
        if (number / 100 == 9)
        {
            if (number % 100 == 0)
            {
                local_word += "ம்";
            }
            else
            {
                local_word += "த்து";
            }
        }
        else if (number % 100 >= 1)
        {
            local_word += "ற்று";
        }
        else
        {
            local_word += "று";
        }

        number %= 100;

        return local_word;
    }

    static string RemoveOnePrefix(string toWords)
    {
        // one hundred => hundredth
        if (toWords.StartsWith("one", StringComparison.Ordinal))
        {
            toWords = toWords[4..];
        }

        return toWords;
    }

    static bool ExceptionNumbersToWords(long number, [NotNullWhen(true)] out string? words) =>
        OrdinalExceptions.TryGetValue(number, out words);
}
