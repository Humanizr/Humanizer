namespace Humanizer;

class RomanianFormatter() :
    DefaultFormatter(RomanianCultureCode)
{
    const int PrepositionIndicatingDecimals = 2;
    const int MaxNumeralWithNoPreposition = 19;
    const int MinNumeralWithNoPreposition = 1;
    const string UnitPreposition = " de";
    const string RomanianCultureCode = "ro";

    static readonly double Divider = Math.Pow(10, PrepositionIndicatingDecimals);

    readonly CultureInfo _romanianCulture = new(RomanianCultureCode);

    protected override string Format(string resourceKey, int number, bool toWords = false)
    {
        var format = Resources.GetResource(GetResourceKey(resourceKey, number), _romanianCulture);
        var preposition = ShouldUsePreposition(number)
            ? UnitPreposition
            : string.Empty;

        return string.Format(format, number, preposition);
    }

    static bool ShouldUsePreposition(int number)
    {
        var prepositionIndicatingNumeral = Math.Abs(number % Divider);
        return prepositionIndicatingNumeral is < MinNumeralWithNoPreposition or > MaxNumeralWithNoPreposition;
    }
}