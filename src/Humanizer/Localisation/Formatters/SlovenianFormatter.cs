namespace Humanizer;

class SlovenianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string DualPostfix = "_Dual";
    const string SingularPostfix = "_Singular";
    const string TrialQuadralPostfix = "_Paucal";

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

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (number == 2)
        {
            return resourceKey + DualPostfix;
        }

        // When the count is three or four some words have a different form when counting in Slovenian language
        if (number is 3 or 4)
        {
            return resourceKey + TrialQuadralPostfix;
        }

        return resourceKey;
    }
}
