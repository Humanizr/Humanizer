namespace Humanizer;

class RomanianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const int PrepositionIndicatingDecimals = 2;
    const int MaxNumeralWithNoPreposition = 19;
    const int MinNumeralWithNoPreposition = 1;
    const string UnitPreposition = " de";
    const string SingularPostfix = "_Singular";

    static readonly double Divider = Math.Pow(10, PrepositionIndicatingDecimals);

    protected override string Format(TimeUnit unit, string resourceKey, int number, bool toWords = false)
    {
        var format = Resources.GetResource(GetResourceKey(resourceKey, number), Culture);
        var preposition = ShouldUsePreposition(number)
            ? UnitPreposition
            : string.Empty;

        return string.Format(format, number, preposition);
    }

    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        if (toSymbol)
        {
            return base.DataUnitHumanize(dataUnit, count, toSymbol);
        }

        var resourceKey = DataUnitResourceKeys.GetResourceKey(dataUnit, false);
        if (count == 1 && Resources.TryGetResourceWithFallback(resourceKey + SingularPostfix, Culture, out var singularResource))
        {
            return singularResource;
        }

        if (Resources.TryGetResourceWithFallback(resourceKey, Culture, out var resource))
        {
            return resource;
        }

        return base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
    }

    static bool ShouldUsePreposition(int number)
    {
        var prepositionIndicatingNumeral = Math.Abs(number % Divider);
        return prepositionIndicatingNumeral is < MinNumeralWithNoPreposition or > MaxNumeralWithNoPreposition;
    }
}
