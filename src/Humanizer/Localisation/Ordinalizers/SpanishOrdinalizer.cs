#nullable enable
namespace Humanizer
{
    class SpanishOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString) =>
            Convert(number, numberString, GrammaticalGender.Masculine, WordForm.Normal);

        public override string Convert(int number, string numberString, GrammaticalGender gender) =>
            Convert(number, numberString, gender, WordForm.Normal);

        public override string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm)
        {
            // N/A in Spanish
            if (number is 0 or int.MinValue)
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

        static string GetNumberString(int number) =>
            number.ToString(_spanishCulture);

        static string GetWordForm(int number, WordForm wordForm) =>
            (number % 10 == 1 || number % 10 == 3) && wordForm == WordForm.Abbreviation ? ".er" : ".º";
    }
}
