#if NET6_0_OR_GREATER
namespace Humanizer;

class PtBrDateOnlyToOrdinalWordsConverter : DefaultDateOnlyToOrdinalWordConverter
{
    public override string Convert(DateOnly date)
    {
        var day = date.Day > 1
            ? date.Day.ToString(CultureInfo.CurrentCulture)
            : date.Day.Ordinalize(GrammaticalGender.Masculine);
        return $"{day} de {date:MMMM} de {date:yyyy}";
    }
}
#endif
