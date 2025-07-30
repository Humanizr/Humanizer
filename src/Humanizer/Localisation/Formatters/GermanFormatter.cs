namespace Humanizer;

class GermanFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    /// <inheritdoc />
    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
        base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
}