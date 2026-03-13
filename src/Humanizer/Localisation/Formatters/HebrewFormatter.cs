namespace Humanizer;

class HebrewFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string DualPostfix = "_Dual";
    const string PluralPostfix = "_Plural";
    const string SingularPostfix = "_Singular";

    protected override string GetResourceKey(string resourceKey, int number)
    {
        //In Hebrew pluralization 2 entities gets a different word.
        if (number == 2)
        {
            return resourceKey + DualPostfix;
        }

        //In Hebrew pluralization entities where the count is between 3 and 10 gets a different word.
        //See http://lib.cet.ac.il/pages/item.asp?item=21585 for explanation
        if (number is >= 3 and <= 10)
        {
            return resourceKey + PluralPostfix;
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
