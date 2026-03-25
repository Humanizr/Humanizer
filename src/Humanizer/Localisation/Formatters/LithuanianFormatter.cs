namespace Humanizer;

class LithuanianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    private static readonly HashSet<string> KeysWithoutNumberForms = ["TimeSpanHumanize_Zero", "DateHumanize_Now"];

    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        if (toSymbol)
        {
            return base.DataUnitHumanize(dataUnit, count, toSymbol);
        }

        var resourceKey = $"DataUnit_{dataUnit}";
        if (count == 1 && Resources.TryGetResourceWithFallback(resourceKey + "_Singular", Culture, out var singularResource))
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

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (KeysWithoutNumberForms.Contains(resourceKey))
        {
            return resourceKey;
        }

        var grammaticalNumber = LithuanianNumberFormDetector.Detect(number);
        var suffix = GetSuffix(grammaticalNumber);
        return resourceKey + suffix;
    }

    static string GetSuffix(LithuanianNumberForm form)
    {
        if (form == LithuanianNumberForm.Singular)
        {
            return "_Singular";
        }

        if (form == LithuanianNumberForm.GenitivePlural)
        {
            return "_Plural";
        }

        return "";
    }
}
