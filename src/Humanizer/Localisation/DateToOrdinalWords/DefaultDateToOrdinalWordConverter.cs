namespace Humanizer;

/// <summary>
/// Provides the default conversion from <see cref="DateTime"/> values to ordinal date words.
/// </summary>
/// <remarks>
/// English renders the day as an ordinal word. Other cultures use the current culture's short date
/// pattern and then strip directional marks that some calendars embed in formatted output.
/// </remarks>
class DefaultDateToOrdinalWordConverter : IDateToOrdinalWordConverter
{
    const char leftToRightMark = (char)0x200E;
    const char rightToLeftMark = (char)0x200F;
    const char arabicLetterMark = (char)0x061C;

    /// <summary>
    /// Converts the given date using the default ordinal-date rules for the current culture.
    /// </summary>
    /// <returns>The localized ordinal-date string.</returns>
    public virtual string Convert(DateTime date)
    {
        var culture = CultureInfo.CurrentCulture;
        if (culture.TwoLetterISOLanguageName != "en")
        {
            // Non-English cultures already know how to render their date ordering; we only clean
            // up formatting marks that would make the embedded result harder to read.
            // Some calendars emit directional marks in short-date output; strip them so the
            // text stays readable when it is embedded into a larger ordinal phrase.
            return SanitizeNonEnglishDate(date.ToString("d", culture));
        }

        return date.Day.Ordinalize() + date.ToString(" MMMM yyyy");
    }

    /// <summary>
    /// Converts the given date using the same rules as <see cref="Convert(DateTime)"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="grammaticalCase"/> value is ignored because this fallback converter
    /// does not vary its ordinal-date wording by grammatical case.
    /// </remarks>
    /// <returns>The localized ordinal-date string.</returns>
    public virtual string Convert(DateTime date, GrammaticalCase grammaticalCase) =>
        Convert(date);

    static string SanitizeNonEnglishDate(string value) =>
        value.Replace(leftToRightMark.ToString(), string.Empty)
            .Replace(rightToLeftMark.ToString(), string.Empty)
            .Replace(arabicLetterMark.ToString(), string.Empty);
}
