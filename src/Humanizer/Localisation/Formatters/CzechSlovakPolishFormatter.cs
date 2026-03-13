namespace Humanizer;

class CzechSlovakPolishFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string PaucalPostfix = "_Paucal";
    const string SingularPostfix = "_Singular";

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (number is > 1 and < 5)
        {
            return resourceKey + PaucalPostfix;
        }

        return resourceKey;
    }

    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        if (toSymbol)
        {
            return base.DataUnitHumanize(dataUnit, count, toSymbol);
        }

        var resourceKey = $"DataUnit_{dataUnit}";
        if (count == 1 && Resources.TryGetResource(resourceKey + SingularPostfix, Culture, out var singular))
        {
            return singular;
        }

        var resolvedKey = GetResourceKey(resourceKey, (int)count);
        if (Resources.TryGetResource(resolvedKey, Culture, out var localized))
        {
            return localized;
        }

        return base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
    }
}
