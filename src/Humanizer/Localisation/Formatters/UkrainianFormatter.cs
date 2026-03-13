namespace Humanizer;

class UkrainianFormatter(CultureInfo culture)
    : DefaultFormatter(culture)
{
    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        var resourceKey = (toSymbol, dataUnit) switch
        {
            (true, DataUnit.Bit) => "DataUnit_BitSymbol",
            (true, DataUnit.Byte) => "DataUnit_ByteSymbol",
            (true, DataUnit.Kilobyte) => "DataUnit_KilobyteSymbol",
            (true, DataUnit.Megabyte) => "DataUnit_MegabyteSymbol",
            (true, DataUnit.Gigabyte) => "DataUnit_GigabyteSymbol",
            (true, DataUnit.Terabyte) => "DataUnit_TerabyteSymbol",
            (true, _) => $"DataUnit_{dataUnit}Symbol",
            (false, DataUnit.Bit) => "DataUnit_Bit",
            (false, DataUnit.Byte) => "DataUnit_Byte",
            (false, DataUnit.Kilobyte) => "DataUnit_Kilobyte",
            (false, DataUnit.Megabyte) => "DataUnit_Megabyte",
            (false, DataUnit.Gigabyte) => "DataUnit_Gigabyte",
            (false, DataUnit.Terabyte) => "DataUnit_Terabyte",
            (false, _) => $"DataUnit_{dataUnit}"
        };

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
