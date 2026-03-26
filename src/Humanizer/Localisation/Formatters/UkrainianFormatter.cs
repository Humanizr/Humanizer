namespace Humanizer;

class UkrainianFormatter(CultureInfo culture)
    : DefaultFormatter(culture)
{
    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        var resourceKey = DataUnitResourceKeys.GetResourceKey(dataUnit, toSymbol);

        if (toSymbol)
        {
            return Resources.GetResource(resourceKey, Culture);
        }

        var absoluteCount = Math.Abs(count);
        var suffix = absoluteCount % 1 == 0
            ? RussianGrammaticalNumberDetector.Detect((int)absoluteCount) switch
            {
                RussianGrammaticalNumber.Singular => "",
                RussianGrammaticalNumber.Paucal => "_Paucal",
                _ => "_Plural"
            }
            : "_Plural";

        return Resources.GetResource(resourceKey + suffix, Culture);
    }

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
            TimeUnit.Day or TimeUnit.Week or TimeUnit.Month or TimeUnit.Year => GrammaticalGender.Masculine,
            _ => GrammaticalGender.Feminine
        };
}
