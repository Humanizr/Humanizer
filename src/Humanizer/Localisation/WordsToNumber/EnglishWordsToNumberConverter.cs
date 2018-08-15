using System;
using System.Collections.Generic;
using Humanizer.Configuration;

namespace Humanizer.Localisation.WordsToNumber
{
    internal class EnglishWordsToNumberConverter : GenderlessWordsToNumberConverter
    {
        private static readonly Dictionary<string, int> NumbersMap = new Dictionary<string, int>
        {
            {"zero",0},
            {"one",1},
            {"two",2},
            {"three",3},
            {"four",4},
            {"five",5},
            {"six",6},
            {"seven",7},
            {"eight",8},
            {"nine",9},
            {"ten",10},
            {"eleven",11},
            {"twelve",12},
            {"thirteen",13},
            {"fourteen",14},
            {"fifteen",15},
            {"sixteen",16},
            {"seventeen",17},
            {"eighteen",18},
            {"nineteen",19},
            {"twenty", 20 },
            {"thirty", 30 },
            {"forty", 40 },
            {"fifty", 50 },
            {"sixty", 60 },
            {"seventy", 70 },
            {"eighty", 80 },
            {"ninety", 90 },
            {"hundred", 100 },
            {"thousand", 1000 },
            {"million", 1000000 },
            {"billion", 1000000000 }
        };

        public override int Convert(string words)
        {
            bool isNegative = false;
            if (words.StartsWith("minus"))
            {
                isNegative = true;
                words = words.Remove(0, 6);
            }

            string[] wordsArray = words.Split(' ');
            
            int response = NumbersMap[wordsArray[0]];

            for (int i = 1; i < wordsArray.Length; i++)
            {
                if(response < NumbersMap[wordsArray[i]])
                {
                    response *= NumbersMap[wordsArray[i]];
                }
                else
                {
                    response += NumbersMap[wordsArray[i]];
                }
                
            }

            if(isNegative)
            {
                return response*-1;
            }
            else
            {
                return response;
            }
            

        }
    }
}
