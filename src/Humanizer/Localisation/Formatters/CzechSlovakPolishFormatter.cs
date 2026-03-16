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
        if (count == 1)
        {
            try
            {
                return Resources.GetResource(resourceKey + SingularPostfix, Culture);
            }
            catch (ArgumentException)
            {
            }
        }

        var resolvedKey = GetResourceKey(resourceKey, (int)count);
        try
        {
            return Resources.GetResource(resolvedKey, Culture);
        }
        catch (ArgumentException)
        {
        }

        return base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
    }
}
