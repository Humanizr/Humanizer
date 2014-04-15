using System.Globalization;

namespace Humanizer.Localisation.Ordinalize
{
    internal class DefaultOrdinalizeConverter
    {
        public virtual string Convert(int number, string numberString, GrammaticalGender gender)
        {
            return Convert(number, numberString);
        }

        public virtual string Convert(int number, string numberString)
        {
            return numberString;
        }
    }
}