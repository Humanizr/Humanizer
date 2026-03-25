namespace Humanizer;

class BulgarianFormatter(CultureInfo culture)
    : DefaultFormatter(culture)
{
    const string SingularPostfix = "_Singular";
    const string PluralPostfix = "_Plural";

    protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        number.ToWords(GetUnitGender(unit), culture);

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

        if (count > 1 && Resources.TryGetResourceWithFallback(resourceKey + PluralPostfix, Culture, out var pluralResource))
        {
            return pluralResource;
        }

        if (Resources.TryGetResourceWithFallback(resourceKey, Culture, out var resource))
        {
            return resource;
        }

        return base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
    }

    static GrammaticalGender GetUnitGender(TimeUnit unit) =>
        unit switch
        {
            TimeUnit.Hour or TimeUnit.Day or TimeUnit.Month => GrammaticalGender.Masculine,
            _ => GrammaticalGender.Feminine
        };
}
