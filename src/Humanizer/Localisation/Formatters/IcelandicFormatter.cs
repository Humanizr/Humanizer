namespace Humanizer;

class IcelandicFormatter(CultureInfo culture) :
    TrimPluralSuffixFormatter(culture)
{
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
