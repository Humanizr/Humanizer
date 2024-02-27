namespace Humanizer;

class BulgarianFormatter(CultureInfo culture)
    : DefaultFormatter(culture)
{
    protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        number.ToWords(GetUnitGender(unit), culture);

    static GrammaticalGender GetUnitGender(TimeUnit unit) =>
        unit switch
        {
            TimeUnit.Hour or TimeUnit.Day or TimeUnit.Month => GrammaticalGender.Masculine,
            _ => GrammaticalGender.Feminine
        };
}