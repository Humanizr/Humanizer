namespace Humanizer;

class FrenchOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        Convert(number, numberString, GrammaticalGender.Masculine);

    public override string Convert(int number, string numberString, GrammaticalGender gender)
    {
        if (number == 1)
        {
            if (gender == GrammaticalGender.Feminine)
            {
                return numberString + "ère";
            }

            return numberString + "er";
        }

        return numberString + "ème";
    }
}