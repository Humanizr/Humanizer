namespace Humanizer.Localisation.Ordinalizers
{
    internal class DutchOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return Convert(number, numberString, GrammaticalGender.Masculine);
        }

        public override string Convert(int number, string numberString, GrammaticalGender gender)
        {
            // N/A in Dutch
            if (number == 0)
            {
                return "0";
            }

            return numberString + "e";
        }
    }
}