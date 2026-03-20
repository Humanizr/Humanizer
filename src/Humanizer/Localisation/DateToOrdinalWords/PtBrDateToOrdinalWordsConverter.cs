namespace Humanizer;

class PtBrDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
{
    public override string Convert(DateTime date)
    {
        var day = date.Day > 1
            ? date.Day.ToString(CultureInfo.CurrentCulture)
            : date.Day.Ordinalize(GrammaticalGender.Masculine);
        return $"{day} de {date:MMMM} de {date:yyyy}";
    }
}
