using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.NumberToWords
{
    internal class UzbekCyrlNumberToWordConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "нол", "бир", "икки", "уч", "тўрт", "беш", "олти", "етти", "саккиз", "тўққиз" };
        private static readonly string[] TensMap = { "нол", "ўн", "йигирма", "ўттиз", "қирқ", "эллик", "олтмиш", "етмиш", "саксон", "тўқсон" };

        private static readonly string[] OrdinalSuffixes = new string[] { "инчи", "нчи" };

        public override string Convert(int number)
        {
            if (number < 0)
                return string.Format("минус {0}", Convert(-number, true));
            return Convert(number, true);
        }

        private string Convert(int number, bool checkForHoundredRule)
        {
            if (number == 0)
                return UnitsMap[0];

            if (checkForHoundredRule && number == 100)
                return "юз";

            var sb = new StringBuilder();

            if ((number / 1000000000) > 0)
            {
                sb.AppendFormat("{0} миллиард ", Convert(number / 1000000000, false));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                sb.AppendFormat("{0} миллион ", Convert(number / 1000000, true));
                number %= 1000000;
            }

            var thousand = (number / 1000);
            if (thousand > 0)
            {
                sb.AppendFormat("{0} минг ", Convert(thousand, true));
                number %= 1000;
            }

            var hundred = (number / 100);
            if (hundred > 0)
            {
                sb.AppendFormat("{0} юз ", Convert(hundred, false));
                number %= 100;
            }

            if ((number / 10) > 0)
            {
                sb.AppendFormat("{0} ", TensMap[number / 10]);
                number %= 10;
            }

            if (number > 0)
            {
                sb.AppendFormat("{0} ", UnitsMap[number]);
            }

            return sb.ToString().Trim();
        }

        public override string ConvertToOrdinal(int number)
        {
            var word = Convert(number);
            var i = 0;
            if (string.IsNullOrEmpty(word))
                return string.Empty;

            var lastChar = word[word.Length - 1];
            if (lastChar == 'и' || lastChar == 'а')
                i = 1;

            return String.Format("{0}{1}", word, OrdinalSuffixes[i]);
        }
    }
}
