namespace Humanizer.Localisation.Ordinalizer
{
    internal class SpanishOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return Convert(number, numberString, GrammaticalGender.Masculine);
        }

        public override string Convert(int number, string numberString, GrammaticalGender gender)
        {
            // N/A in Spanish
            if (number == 0)
                return "0";

            if (gender == GrammaticalGender.Feminine)
                return numberString + "ª";

            return numberString + "º";
        }
    }
}