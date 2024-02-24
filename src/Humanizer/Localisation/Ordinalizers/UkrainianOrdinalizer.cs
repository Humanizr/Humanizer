namespace Humanizer;

class UkrainianOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        Convert(number, numberString, GrammaticalGender.Masculine);

    public override string Convert(int number, string numberString, GrammaticalGender gender)
    {
        if (gender == GrammaticalGender.Masculine)
        {
            return numberString + "-й";
        }

        if (gender == GrammaticalGender.Feminine)
        {
            if (number % 10 == 3)
            {
                return numberString + "-я";
            }

            return numberString + "-а";
        }

        if (gender == GrammaticalGender.Neuter)
        {
            if (number % 10 == 3)
            {
                return numberString + "-є";
            }
        }

        return numberString + "-е";
    }
}