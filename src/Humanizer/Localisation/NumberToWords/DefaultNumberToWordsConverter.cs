namespace Humanizer.Localisation.NumberToWords
{
    internal class DefaultNumberToWordsConverter : INumberToWordsConverter
    {
        public virtual string Convert(int number, GrammaticalGender gender)
        {
            return Convert(number);
        }

        public virtual string Convert(int number)
        {
            return number.ToString();
        }

        public virtual string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return ConvertToOrdinal(number);
        }

        public virtual string ConvertToOrdinal(int number)
        {
            return number.ToString();
        }
    }
}