using System;
using Humanizer.Localisation.NumberToWords.Italian;
using JetBrains.Annotations;

namespace Humanizer.Localisation.NumberToWords
{
    internal class ItalianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        public override string Convert(long input, GrammaticalGender gender)
        {
            if (input > Int32.MaxValue || input < Int32.MinValue)
            {
                throw new NotImplementedException();
            }
            var number = (int)input;

            if (number < 0)
                return "meno " + Convert(Math.Abs(number), gender);
        
            var cruncher = new ItalianCardinalNumberCruncher(number, gender);
            
            return cruncher.Convert();
        }

        [NotNull]
        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            var cruncher = new ItalianOrdinalNumberCruncher(number, gender);
            
            return cruncher.Convert();
        }
    }
}