using System.Globalization;

namespace Humanizer.Localisation.Ordinalizers
{
    internal class SpanishOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return Convert(number, numberString, GrammaticalGender.Masculine, WordForm.Normal);
        }

        public override string Convert(int number, string numberString, GrammaticalGender gender)
        {
            return Convert(number, numberString, gender, WordForm.Normal);
        }

        public override string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm)
        {
            // N/A in Spanish
            if (number == 0 || number == int.MinValue)
            {
                return "0";
            }

            if (number < 0)
            {
                return Convert(-number, GetNumberString(-number), gender);
            }

            switch (gender)
            {
                case GrammaticalGender.Masculine:
                case GrammaticalGender.Neuter:
                    return numberString + GetWordForm(number, wordForm);
                case GrammaticalGender.Feminine:
                    return numberString + ".ª";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        static CultureInfo _spanishCulture = new("es-ES");

        private static string GetNumberString(int number)
        {
            return number.ToString(_spanishCulture);
        }

        private static string GetWordForm(int number, WordForm wordForm)
        {
            return (number % 10 == 1 || number % 10 == 3) && wordForm == WordForm.Abbreviation ? ".er" : ".º";
        }
    }
}
