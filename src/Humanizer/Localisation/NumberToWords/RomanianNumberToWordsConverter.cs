using System;
using Humanizer.Localisation.NumberToWords.Romanian;

namespace Humanizer.Localisation.NumberToWords
{
    internal class RomanianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
        {
            if (number > Int32.MaxValue || number < Int32.MinValue)
            {
                throw new NotImplementedException();
            }
            var converter = new RomanianCardinalNumberConverter();
            return converter.Convert((int)number, gender);
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            var converter = new RomanianOrdinalNumberConverter();
            return converter.Convert(number, gender);
        }
    }
}
