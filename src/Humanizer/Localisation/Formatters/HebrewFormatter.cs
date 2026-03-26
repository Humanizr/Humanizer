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

        var resourceKey = DataUnitResourceKeys.GetResourceKey(dataUnit, false);
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
