namespace Humanizer;

class IcelandicFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
        base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');

    protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        number.ToWords(GetUnitGender(unit), culture);

    static GrammaticalGender GetUnitGender(TimeUnit unit) =>
        unit switch
        {
            TimeUnit.Day or TimeUnit.Month => GrammaticalGender.Masculine,
            TimeUnit.Year => GrammaticalGender.Neuter,
            _ => GrammaticalGender.Feminine
        };
}