namespace Humanizer;

/// <summary>
/// Ordinalizer that appends fixed suffixes based on grammatical gender.
/// </summary>
class SuffixOrdinalizer(string masculineSuffix, string feminineSuffix, string neuterSuffix, bool zeroAsPlainNumber = false)
    : DefaultOrdinalizer
{
    /// <summary>
    /// Initializes a suffix ordinalizer that uses the same suffix for every grammatical gender.
    /// </summary>
    /// <param name="suffix">The suffix to use for every gender.</param>
    /// <param name="zeroAsPlainNumber">Whether zero should be returned as plain <c>0</c>.</param>
    public SuffixOrdinalizer(string suffix, bool zeroAsPlainNumber = false)
        : this(suffix, suffix, suffix, zeroAsPlainNumber)
    {
    }

    /// <summary>
    /// Ordinalizes the number using the masculine suffix by default.
    /// </summary>
    public override string Convert(int number, string numberString) =>
        Convert(number, numberString, GrammaticalGender.Masculine);

    /// <summary>
    /// Ordinalizes the number using the suffix for the requested grammatical gender.
    /// </summary>
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
