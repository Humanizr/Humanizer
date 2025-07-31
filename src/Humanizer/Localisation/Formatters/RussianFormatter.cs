namespace Humanizer;

class RussianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    /// <inheritdoc />
    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
        base.DataUnitHumanize(dataUnit, count, toSymbol).TrimEnd('s');
    protected override string GetResourceKey(string resourceKey, int number)
    {
        var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(number);
        return grammaticalNumber switch
        {
            RussianGrammaticalNumber.Singular => resourceKey + "_Singular",
            RussianGrammaticalNumber.Paucal => resourceKey + "_Paucal",
            _ => resourceKey
        };
    }

    protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        number.ToWords(GetUnitGender(unit), culture);


    static GrammaticalGender GetUnitGender(TimeUnit unit) =>
        unit switch
        {
            TimeUnit.Hour or TimeUnit.Day or TimeUnit.Month or TimeUnit.Year => GrammaticalGender.Masculine,
            _ => GrammaticalGender.Feminine
        };
}