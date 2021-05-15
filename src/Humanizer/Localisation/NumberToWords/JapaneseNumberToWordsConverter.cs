using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class JapaneseNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap1 = { "", "", "二", "三", "四", "五", "六", "七", "八", "九" };
        private static readonly string[] UnitsMap2 = { "", "十", "百", "千" };
        private static readonly string[] UnitsMap3 = { "", "万", "億", "兆", "京", "垓", "𥝱", "穣", "溝", "澗", "正", "載", "極",
                                                       "恒河沙", "阿僧祇", "那由他", "不可思議", "無量大数"};

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
            {
                return isOrdinal ? "〇番目" : "〇";
            }

            if (number < 0)
            {
                return string.Format("マイナス {0}", ConvertImpl(-number, false));
            }

            var parts = new List<string>();
            var groupLevel = 0;
            while (number > 0)
            {
                var groupNumber = number % 10000;
                number /= 10000;

                var n0 = groupNumber % 10;
                var n1 = (groupNumber % 100 - groupNumber % 10) / 10;
                var n2 = (groupNumber % 1000 - groupNumber % 100) / 100;
                var n3 = (groupNumber - groupNumber % 1000) / 1000;

                parts.Add(
                    UnitsMap1[n3] + (n3 == 0 ? "" : UnitsMap2[3])
                    + UnitsMap1[n2] + (n2 == 0 ? "" : UnitsMap2[2])
                    + UnitsMap1[n1] + (n1 == 0 ? "" : UnitsMap2[1])
                    + (n0 == 1 ? "一" : UnitsMap1[n0])
                    + (groupNumber == 0 ? "" : UnitsMap3[groupLevel])
                );

                groupLevel++;
            }

            parts.Reverse();
            var toWords = string.Join("", parts.ToArray());

            if (isOrdinal)
            {
                toWords = string.Format("{0}番目", toWords);
            }

            return toWords;
        }
    }
}
