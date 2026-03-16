namespace Humanizer;

class TrimPluralSuffixFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
        base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
}
