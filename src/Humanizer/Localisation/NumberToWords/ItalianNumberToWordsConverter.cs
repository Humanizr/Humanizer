using System;
using System.Collections.Generic;
using Humanizer.Localisation.NumberToWords;

namespace Humanizer
{
    internal class ItalianNumberToWordsConverter : INumberToWordsConverter
    {
        private static readonly Dictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
        {
            {0, "zeresimo"},
            {1, "primo"},
            {2, "secondo"},
            {3, "terzo"},
            {4, "quarto"},
            {5, "quinto"},
            {6, "sesto"},
            {7, "settimo"},
            {8, "ottavo"},
            {9, "nono"},
            {10, "decimo"},
        };

        private static readonly string[] UnitsNames = new[] {"zero","uno","due","tre","quattro","cinque","sei","sette","otto","nove","dieci","undici","dodici",
        "tredici","quattordici","quindici","sedici","diciassette","diciotto","diciannove"};
        private static readonly string[] TensNames = new[] { "zero", "dieci", "venti", "trenta", "quaranta", "cinquanta", "sessanta", "settanta", "ottanta", "novanta" };


        public string Convert(int number)
        {
            string result = string.Empty;
            int groupLevel = 0;
            while (number >= 1)
            {
                var groupNumber = number % 1000;
                number /= 1000;

                var hundreds = groupNumber / 100;
                groupNumber %= 100;
                var tens = groupNumber / 10;
                groupNumber %= 10;
                var units = groupNumber % 10;
                string process = string.Empty;
                if (hundreds > 0)
                {
                    if (hundreds > 1)
                        process = UnitsNames[hundreds];
                    process += "cento";
                }

                if (tens > 0)
                {
                    if (units == 0)
                        process += TensNames[tens];
                    else if (tens == 1)
                        process += UnitsNames[tens * 10 + units];
                    else
                        if (units == 1 || units == 8)
                            process += TensNames[tens].Substring(0, TensNames[tens].Length - 1);
                        else
                            process += TensNames[tens];
                }

                if (units > 0 && tens != 1)
                {
                    if (groupLevel == 0 || units + tens * 10 + hundreds * 100 != 1)
                        process += UnitsNames[units];
                }

                switch (groupLevel)
                {
                    case 0:
                        break;
                    case 1:
                        if (units + tens * 10 + hundreds * 100 > 0)
                            process += units + tens * 10 + hundreds * 100 > 1 ? "mila" : "mille";
                        break;
                    case 2:
                        if (units + tens * 10 + hundreds * 100 > 0)
                            process += units + tens * 10 + hundreds * 100 > 1 ? "milioni" : "unmilione";
                        break;
                    case 3:
                        if (units + tens * 10 + hundreds * 100 > 0)
                            process += units + tens * 10 + hundreds * 100 > 1 ? "miliardi" : "unmiliardo";
                        break;
                }

                groupLevel++;
                result = process + result;
            }
            return result;
        }

        public string ConvertToOrdinal(int number)
        {
            if (number >= 0 && number <= 10)
                return OrdinalExceptions[number];
            var previousLevelHasHundreds = false;
            int groupLevel = 0;
            int previousGroup = 0;

            string result = string.Empty;
            while (number >= 1)
            {
                int groupNumber;
                groupNumber = number % 1000;

                number /= 1000;

                var hundreds = groupNumber / 100;
                previousLevelHasHundreds = hundreds > 0;
                var tens = groupNumber % 100 / 10;
                var units = groupNumber % 10;
                
                string process = string.Empty;
                if (hundreds > 0)
                {
                    if (hundreds > 1)
                        process = UnitsNames[hundreds];
                    process += "cento";
                }

                if (tens > 0)
                {
                    if (units == 0)
                        process += TensNames[tens];
                    else if (tens == 1)
                        process += UnitsNames[tens * 10 + units];
                    else
                        if (units == 1 || units == 8)
                            process += TensNames[tens].Substring(0, TensNames[tens].Length - 1);
                        else
                            process += TensNames[tens];
                }

                if (units > 0 && tens != 1)
                {
                    if (groupLevel == 0 || units + tens * 10 + hundreds * 100 != 1)
                        process += UnitsNames[units];
                }
                var thousands = units + tens * 10 + hundreds * 100;

                switch (groupLevel)
                {
                    case 0:
                        break;
                    case 1:
                        if (thousands > 0)
                        {
                            if (thousands == 1 || ((thousands == 10 || thousands == 100) && previousGroup == 0))
                                process += "mille";
                            else
                                process += "mila";
                        }
                        break;
                    case 2:
                        process += thousands > 1 ? "milioni" : "milione";
                        break;
                    case 3:
                        process += thousands > 1 ? "miliardi" : "miliardo";
                        break;
                }

                groupLevel++;
                previousGroup = groupNumber;
                result = process + result;
            }

            if (result.EndsWith("sei"))
                return result + "esimo";
            return result.Substring(0, result.Length - 1) + "esimo";
        }
    }
}