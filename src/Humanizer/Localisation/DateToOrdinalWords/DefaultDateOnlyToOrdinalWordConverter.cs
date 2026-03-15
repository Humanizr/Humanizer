#if NET6_0_OR_GREATER

namespace Humanizer;

class DefaultDateOnlyToOrdinalWordConverter : IDateOnlyToOrdinalWordConverter
{
    public virtual string Convert(DateOnly date)
    {
        var culture = CultureInfo.CurrentCulture;
        if (culture.TwoLetterISOLanguageName != "en")
        {
            return SanitizeNonEnglishDate(date.ToString("d", culture));
        }

        return date.Day.Ordinalize() + date.ToString(" MMMM yyyy");
    }

    public virtual string Convert(DateOnly date, GrammaticalCase grammaticalCase) =>
        Convert(date);

    static string SanitizeNonEnglishDate(string value) =>
        value.Replace("\u200e", string.Empty)
            .Replace("\u200f", string.Empty)
            .Replace("\u061c", string.Empty);
}

#endif
