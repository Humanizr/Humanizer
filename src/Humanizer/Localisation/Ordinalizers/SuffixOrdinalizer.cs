namespace Humanizer;

class SuffixOrdinalizer(string masculineSuffix, string feminineSuffix, string neuterSuffix, bool zeroAsPlainNumber = false)
    : DefaultOrdinalizer
{
    public SuffixOrdinalizer(string suffix, bool zeroAsPlainNumber = false)
        : this(suffix, suffix, suffix, zeroAsPlainNumber)
    {
    }

    public override string Convert(int number, string numberString) =>
        Convert(number, numberString, GrammaticalGender.Masculine);

    public override string Convert(int number, string numberString, GrammaticalGender gender)
    {
        if (zeroAsPlainNumber && number == 0)
        {
            return "0";
        }

        return numberString + gender switch
        {
            GrammaticalGender.Feminine => feminineSuffix,
            GrammaticalGender.Neuter => neuterSuffix,
            _ => masculineSuffix
        };
    }
}
