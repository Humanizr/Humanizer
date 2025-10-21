namespace Humanizer;

class SpanishOrdinalizer(CultureInfo culture) : DefaultOrdinalizer
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

        return gender switch
        {
            GrammaticalGender.Masculine or GrammaticalGender.Neuter => numberString + GetWordForm(number, wordForm),
            GrammaticalGender.Feminine => numberString + ".ª",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null),
        };
    }

    string GetNumberString(int number) =>
        number.ToString(culture);

    static string GetWordForm(int number, WordForm wordForm) =>
        (number % 10 == 1 || number % 10 == 3) && wordForm == WordForm.Abbreviation ? ".er" : ".º";
}