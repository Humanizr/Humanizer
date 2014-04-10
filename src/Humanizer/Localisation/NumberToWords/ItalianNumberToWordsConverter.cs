using System;
using System.Collections.Generic;
using Humanizer.Localisation.NumberToWords;

namespace Humanizer
{
    internal class ItalianNumberToWordsConverter : INumberToWordsConverter
    {
        private static readonly Dictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
        {
            {1, "primo"},
            {2, "secondo"},
            {3, "terzo"},
            {4, "quarto"},
            {5, "quinto"},
            {8, "sesto"},
            {9, "nono"},
            {10, "decimo"},
        };

        private static readonly string[] Unita = new[] {"zero","uno","due","tre","quattro","cinque","sei","sette","otto","nove","dieci","undici","dodici",
        "tredici","quattordici","quindici","sedici","diciassette","diciotto","diciannove"};
        private static readonly string[] Decine = new[] { "zero", "dieci", "venti", "trenta", "quaranta", "cinquanta", "sessanta", "settanta", "ottanta", "novanta" };

        public string Convert(int number)
        {
            string result = string.Empty;
            int groupLevel = 0;
            while (number >= 1)
            {
                var groupNumber = number % 1000;
                number /= 1000;

                var centinaia = groupNumber / 100;
                groupNumber %= 100;
                var decine = groupNumber / 10;
                groupNumber %= 10;
                var unita = groupNumber % 10;
                string process = string.Empty;
                if (centinaia > 0)
                {
                    if (centinaia > 1)
                        process = Unita[centinaia];
                    process += "cento";
                }

                if (decine > 0)
                {
                    if (unita == 0)
                        process += Decine[decine];
                    else if (decine == 1)
                        process += Unita[decine*10 + unita];
                    else
                        if (unita == 1 || unita == 8)
                            process += Decine[decine].Substring(0, Decine[decine].Length - 1);
                        else
                            process += Decine[decine];
                }

                if (unita > 0 && decine != 1)
                {
                    if (groupLevel == 0 || unita + decine * 10 + centinaia * 100 != 1)
                        process += Unita[unita];
                }

                switch (groupLevel)
                {
                    case 0:
                        break;
                    case 1:
                        if (unita + decine * 10 + centinaia * 100> 0)
                        process += unita + decine * 10 + centinaia * 100 > 1 ? "mila" : "mille";
                        break;
                    case 2:
                        if(unita + decine * 10 + centinaia * 100>0)
                        process += unita + decine * 10 + centinaia * 100 > 1 ? "milioni" : "unmilione";
                        break;
                    case 3:
                        if (unita + decine * 10 + centinaia * 100>0)
                        process += unita + decine * 10 + centinaia * 100 > 1 ? "miliardi" : "unmiliardo";
                        break;
                }

                groupLevel++;
                result = process + result;
            }
            return result;
        }

        public string ConvertToOrdinal(int number)
        {
            return "";
        }
    }
}