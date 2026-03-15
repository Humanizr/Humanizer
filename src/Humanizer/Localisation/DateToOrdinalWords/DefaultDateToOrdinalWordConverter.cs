namespace Humanizer;

class DefaultDateToOrdinalWordConverter : IDateToOrdinalWordConverter
{
    public virtual string Convert(DateTime date)
    {
        var culture = CultureInfo.CurrentCulture;
        if (culture.TwoLetterISOLanguageName != "en")
        {
            return SanitizeNonEnglishDate(date.ToString("d", culture));
        }

        return date.Day.Ordinalize() + date.ToString(" MMMM yyyy");
    }

    public virtual string Convert(DateTime date, GrammaticalCase grammaticalCase) =>
        Convert(date);

    static string SanitizeNonEnglishDate(string value) =>
        value.Replace("\u200e", string.Empty)
            .Replace("\u200f", string.Empty)
            .Replace("\u061c", string.Empty);
}
