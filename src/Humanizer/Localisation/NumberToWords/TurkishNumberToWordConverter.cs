namespace Humanizer
{
    internal class TurkishNumberToWordConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "sıfır", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz" };
        private static readonly string[] TensMap = { "sıfır", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan" };

        private static readonly Dictionary<char, string> OrdinalSuffix = new Dictionary<char, string>
        {
            {'ı', "ıncı"},
            {'i', "inci"},
            {'u', "uncu"},
            {'ü', "üncü"},
            {'o', "uncu"},
            {'ö', "üncü"},
            {'e', "inci"},
            {'a', "ıncı"},
        };

        private static readonly Dictionary<char, string> TupleSuffix = new Dictionary<char, string>
        {
            {'ı', "lı"},
            {'i', "li"},
            {'u', "lu"},
            {'ü', "lü"},
            {'o', "lu"},
            {'ö', "lü"},
            {'e', "li"},
            {'a', "lı"},
        };

        public override string Convert(long input)
        {
            var number = input;
            if (number == 0)
            {
                return UnitsMap[0];
            }

            if (number < 0)
            {
                return $"eksi {Convert(-number)}";
            }

            var parts = new List<string>();

            if (number / 1000000000000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000000000000)} kentilyon");
                number %= 1000000000000000000;
            }

            if (number / 1000000000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000000000)} katrilyon");
                number %= 1000000000000000;
            }

            if (number / 1000000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000000)} trilyon");
                number %= 1000000000000;
            }

            if (number / 1000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000)} milyar");
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000)} milyon");
                number %= 1000000;
            }

            var thousand = number / 1000;
            if (thousand > 0)
            {
                parts.Add($"{(thousand > 1 ? Convert(thousand) : "")} bin".Trim());
                number %= 1000;
            }

            var hundred = number / 100;
            if (hundred > 0)
            {
                parts.Add($"{(hundred > 1 ? Convert(hundred) : "")} yüz".Trim());
                number %= 100;
            }

            if (number / 10 > 0)
            {
                parts.Add(TensMap[number / 10]);
                number %= 10;
            }

            if (number > 0)
            {
                parts.Add(UnitsMap[number]);
            }

            var toWords = string.Join(" ", parts);

            return toWords;
        }

        public override string ConvertToOrdinal(int number)
        {
            var word = Convert(number);
            var wordSuffix = string.Empty;
            var suffixFoundOnLastVowel = false;

            for (var i = word.Length - 1; i >= 0; i--)
            {
                if (OrdinalSuffix.TryGetValue(word[i], out wordSuffix))
                {
                    suffixFoundOnLastVowel = i == word.Length - 1;
                    break;
                }
            }

            if (word[word.Length - 1] == 't')
            {
                word = word.Substring(0, word.Length - 1) + 'd';
            }

            if (suffixFoundOnLastVowel)
            {
                word = word.Substring(0, word.Length - 1);
            }

            return $"{word}{wordSuffix}";
        }

        public override string ConvertToTuple(int number)
        {
            switch (number)
            {
                case 1:
                    return "tek";
                case 2:
                    return "çift";
                default:
                    var word = Convert(number);
                    var wordSuffix = string.Empty;

                    for (var i = word.Length - 1; i >= 0; i--)
                    {
                        if (TupleSuffix.TryGetValue(word[i], out wordSuffix))
                        {
                            break;
                        }
                    }

                    return $"{word}{wordSuffix}";
            }
        }
    }
}
