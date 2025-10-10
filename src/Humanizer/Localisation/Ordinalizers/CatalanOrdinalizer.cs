namespace Humanizer;

class CatalanOrdinalizer() : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        Convert(number, numberString, GrammaticalGender.Masculine, WordForm.Normal);

    public override string Convert(int number, string numberString, GrammaticalGender gender) =>
        Convert(number, numberString, gender, WordForm.Normal);

    public override string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm)
    {
        if (number is 0 or int.MinValue)
        {
            return "0";
        }

        if (number < 0)
        {
            return Convert(-number, (-number).ToString(), gender);
        }

        if (gender == GrammaticalGender.Feminine)
        {
            return $"{numberString}a";
        }

        if (number % 10 == 1 || number % 10 == 3)
        {
            return $"{numberString}r";
        }

        if (number % 10 == 2)
        {
            return $"{numberString}n";
        }

        if (number % 10 == 4)
        {
            return $"{numberString}t";
        }

        return $"{numberString}è";


    }
}
