using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class KoreanNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap1 = { "", "", "이", "삼", "사", "오", "육", "칠", "팔", "구" };
        private static readonly string[] UnitsMap2 = { "", "십", "백", "천" };
        private static readonly string[] UnitsMap3 = { "", "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극", "항하사", "아승기", "나유타", "불가사의", "무량대수"};
        
        private static readonly Dictionary<long, string> OrdinalExceptions = new Dictionary<long, string>
        {
            {0, "영번째"},
            {1, "첫번째"},
            {2, "두번째"},
            {3, "세번째"},
            {4, "네번째"},
            {5, "다섯번째"},
            {6, "여섯번째"},
            {7, "일곱번째"},
            {8, "여덟번째"},
            {9, "아홉번째"},
            {10, "열번째"},
            {11, "열한번째"},
            {12, "열두번째"},
            {13, "열세번째"},
            {14, "열네번째"},
            {15, "열다섯번째"},
            {16, "열여섯번째"},
            {17, "열일곱번째"},
            {18, "열여덟번째"},
            {19, "열아홉째"},
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
            if (isOrdinal && number < 20)
            {
                if (OrdinalExceptions.TryGetValue(number, out var words))
                    return words;
            }

            if (number == 0)
            {
                return "영";
            }

            if (number < 0)
            {
                return string.Format("마이너스 {0}", ConvertImpl(-number, false));
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
                    + (n0 == 1 ? "일" : UnitsMap1[n0])
                    + (groupNumber == 0 ? "" : UnitsMap3[groupLevel])
                );

                groupLevel++;
            }

            parts.Reverse();
            var toWords = string.Join("", parts.ToArray());

            if (isOrdinal)
            {
                toWords = string.Format("{0}번째", toWords);
            }

            return toWords;
        }
    }
}
