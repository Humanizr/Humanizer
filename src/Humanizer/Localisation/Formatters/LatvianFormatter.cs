namespace Humanizer;

class LatvianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        var resourceValue = base.DataUnitHumanize(dataUnit, count, toSymbol);

        if (toSymbol)
        {
            return resourceValue;
        }

        if (count == 1)
        {
            return resourceValue.TrimEnd('i') + 's';
        }

        return resourceValue.TrimEnd('s');
    }
}
