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

        if (count > 1)
        {
            try
            {
                return Resources.GetResource(resourceKey + PluralPostfix, Culture);
            }
            catch (ArgumentException)
            {
            }
        }

        try
        {
            return Resources.GetResource(resourceKey, Culture);
        }
        catch (ArgumentException)
        {
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
