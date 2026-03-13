namespace Humanizer;

class FinnishFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string SingularPostfix = "_Singular";

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

        try
        {
            return Resources.GetResource(resourceKey, Culture);
        }
        catch (ArgumentException)
        {
        }

        return base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
    }
}
