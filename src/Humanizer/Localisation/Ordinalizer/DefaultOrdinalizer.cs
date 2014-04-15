namespace Humanizer.Localisation.Ordinalizer
{
    internal class DefaultOrdinalizer
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