namespace Humanizer.Localisation.Ordinalizers
{

    internal class RomanianOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return Convert(number, numberString, GrammaticalGender.Masculine);
        }

        public override string Convert(int number, string numberString, GrammaticalGender gender)
        {
            // No ordinal for 0 (zero) in Romanian.
            if (number == 0)
            {
                return "0";
            }

            // Exception from the rule.
            if (number == 1)
            {
                if (gender == GrammaticalGender.Feminine)
                {
                    return "prima"; // întâia
                }

                return "primul"; // întâiul
            }

            if (gender == GrammaticalGender.Feminine)
            {
                return string.Format("a {0}-a", numberString);
            }

            return string.Format("al {0}-lea", numberString);

        }
    }
}