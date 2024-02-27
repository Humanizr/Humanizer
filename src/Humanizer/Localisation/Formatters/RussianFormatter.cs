namespace Humanizer;

class RussianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    protected override string GetResourceKey(string resourceKey, int number)
    {
        var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(number);
        var suffix = GetSuffix(grammaticalNumber);
        return resourceKey + suffix;
    }

    protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        number.ToWords(GetUnitGender(unit), culture);

    static string GetSuffix(RussianGrammaticalNumber grammaticalNumber) =>
        grammaticalNumber switch
        {
            RussianGrammaticalNumber.Singular => "_Singular",
            RussianGrammaticalNumber.Paucal => "_Paucal",
            _ => ""
        };

    static GrammaticalGender GetUnitGender(TimeUnit unit) =>
        unit switch
        {
            TimeUnit.Hour or TimeUnit.Day or TimeUnit.Month or TimeUnit.Year => GrammaticalGender.Masculine,
            _ => GrammaticalGender.Feminine
        };
}