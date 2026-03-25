namespace Humanizer;

class CroatianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string PaucalPostfix = "_Paucal";
    const string SingularPostfix = "_Singular";

    protected override string GetResourceKey(string resourceKey, int number)
    {
        var mod10 = number % 10;
        var mod100 = number % 100;
        if (mod10 is > 1 and < 5 && mod100 is not 12 and not 13 and not 14)
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
        if (count == 1 && Resources.TryGetResourceWithFallback(resourceKey + SingularPostfix, Culture, out var singularResource))
        {
            return singularResource;
        }

        var resolvedKey = GetResourceKey(resourceKey, (int)count);
        if (Resources.TryGetResourceWithFallback(resolvedKey, Culture, out var resource))
        {
            return resource;
        }

        return base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
    }
}
