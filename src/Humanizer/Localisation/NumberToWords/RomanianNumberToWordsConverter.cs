using System;
using Humanizer.Localisation.NumberToWords.Romanian;

namespace Humanizer.Localisation.NumberToWords
{
    internal class RomanianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        public override string Convert(int number, GrammaticalGender gender)
        {
            var converter = new RomanianCardinalNumberConverter();
            return converter.Convert(number, gender);
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            var converter = new RomanianOrdinalNumberConverter();
            return converter.Convert(number, gender);
        }
    }
}
