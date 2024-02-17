namespace Humanizer;

class IcelandicFormatter() :
    DefaultFormatter(LocaleCode)
{
    const string LocaleCode = "is";
    readonly CultureInfo localCulture = new(LocaleCode);

    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
        base.DataUnitHumanize(dataUnit, count, toSymbol)?.TrimEnd('s');

    protected override string Format(string resourceKey, int number, bool toWords = false)
    {
        var resourceString = Resources.GetResource(GetResourceKey(resourceKey, number), localCulture);

        if (string.IsNullOrEmpty(resourceString))
        {
            throw new ArgumentException($@"The resource object with key '{resourceKey}' was not found", nameof(resourceKey));
        }

        if (toWords)
        {
            var unitGender = GetGrammaticalGender(resourceString);
            return string.Format(resourceString, number.ToWords(unitGender, localCulture));
        }

        return string.Format(resourceString, number);
    }

    static GrammaticalGender GetGrammaticalGender(string resource)
    {
        if (resource.Contains(" mán") ||
            resource.Contains(" dag"))
        {
            return GrammaticalGender.Masculine;
        }

        if (resource.Contains(" ár"))
        {
            return GrammaticalGender.Neuter;
        }

        return GrammaticalGender.Feminine;
    }
}