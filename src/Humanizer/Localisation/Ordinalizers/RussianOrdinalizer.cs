namespace Humanizer;

class RussianOrdinalizer : DefaultOrdinalizer
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
            return numberString + "-я";
        }

        return numberString + "-е";
    }
}