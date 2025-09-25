using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Humanizer;

namespace Humanizer;

internal class EnglishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    private static readonly Dictionary<string, int> NumbersMap = new()
    {
        {"zero",0}, {"one",1}, {"two",2}, {"three",3}, {"four",4}, {"five",5},
        {"six",6}, {"seven",7}, {"eight",8}, {"nine",9}, {"ten",10},
        {"eleven",11}, {"twelve",12}, {"thirteen",13}, {"fourteen",14},
        {"fifteen",15}, {"sixteen",16}, {"seventeen",17}, {"eighteen",18},
        {"nineteen",19}, {"twenty", 20}, {"thirty", 30}, {"forty", 40},
        {"fifty", 50}, {"sixty", 60}, {"seventy", 70}, {"eighty", 80},
        {"ninety", 90}, {"hundred", 100}, {"thousand", 1000},
        {"million", 1_000_000}, {"billion", 1_000_000_000}
    };

    private static readonly Dictionary<string, int> OrdinalsMap = new()
    {
        {"first",1}, {"second",2}, {"third",3}, {"fourth",4}, {"fifth",5},
        {"sixth",6}, {"seventh",7}, {"eighth",8}, {"ninth",9}, {"tenth",10},
        {"eleventh",11}, {"twelfth",12}, {"thirteenth",13}, {"fourteenth",14},
        {"fifteenth",15}, {"sixteenth",16}, {"seventeenth",17}, {"eighteenth",18},
        {"nineteenth",19}, {"twentieth",20}, {"thirtieth",30},
        {"fortieth",40}, {"fiftieth",50}, {"sixtieth",60}, {"seventieth",70},
        {"eightieth",80}, {"ninetieth",90}, {"hundredth",100}, {"thousandth",1000}
    };

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var result, out var unrecognizedword))
            throw new ArgumentException($"Unrecognized number word: {unrecognizedword}");

        return result;
    }

    public override bool TryConvert(string words, out int result) => TryConvert(words, out result, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
            throw new ArgumentException("Input words cannot be empty.");

        unrecognizedWord = null;

        words = words.Replace(",", "")
                     .Replace(" and ", " ")
                     .ToLowerInvariant()
                     .Trim();

        var isNegative = words.StartsWith("minus ") || words.StartsWith("negative ");
        if (isNegative)
            words = words.Replace("minus ", "").Replace("negative ", "").Trim();

        // Remove ordinal suffixes (st, nd, rd, th)
        words = Regex.Replace(words, @"\b(\d+)(st|nd|rd|th)\b", "$1");
        words = words.Replace("-", " ");

        if (int.TryParse(words, out var numericValue))
        {
            parsedValue = isNegative ? -numericValue : numericValue;
            return true;
        }

        if (OrdinalsMap.TryGetValue(words, out var ordinalValue))
        {
            parsedValue = isNegative ? -ordinalValue : ordinalValue;
            return true;
        }
        if (TryConvertWordsToNumber(words, out var numberValue, out var unrecognizedNumberWord))
        {
            parsedValue = isNegative ? -numberValue : numberValue;
            return true;
        }

        unrecognizedWord = unrecognizedNumberWord;
        parsedValue = default;
        return false;
    }

    private bool TryConvertWordsToNumber(string words, out int result, out string? unrecognizedWord)
    {
        var wordsArray = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        result = 0;
        unrecognizedWord = null;
        var current = 0;
        var hasOrdinal = false;

        foreach (var word in wordsArray)
        {
            if (OrdinalsMap.TryGetValue(word, out var ordinalValue))
            {
                result += current + ordinalValue;
                hasOrdinal = true;
                break; // Stop processing after an ordinal
            }

            if (!NumbersMap.TryGetValue(word, out var value))
            {
                unrecognizedWord = word;
                return false;
            }

            if (value == 100)
                current = (current == 0 ? 1 : current) * 100;

            else if (value >= 1000)
            {
                result += (current == 0 ? 1 : current) * value;
                current = 0;
            }
            else
                current += value;
        }

        if (!hasOrdinal)
            result += current;

        return true;
    }
}
