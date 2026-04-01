#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Provides the default conversion from <see cref="DateOnly"/> values to ordinal date words.
/// </summary>
/// <remarks>
/// English renders the day as an ordinal word. Other cultures use the current culture's short date
/// pattern and then strip directional marks that some calendars embed in formatted output.
/// </remarks>
class DefaultDateOnlyToOrdinalWordConverter : IDateOnlyToOrdinalWordConverter
{
    /// <summary>
    /// Converts the given date using the default ordinal-date rules for the current culture.
    /// </summary>
    /// <returns>The localized ordinal-date string.</returns>
    public virtual string Convert(DateOnly date)
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
    /// Converts the given date using the same rules as <see cref="Convert(DateOnly)"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="grammaticalCase"/> value is ignored because this fallback converter
    /// does not vary its ordinal-date wording by grammatical case.
    /// </remarks>
    /// <returns>The localized ordinal-date string.</returns>
    public virtual string Convert(DateOnly date, GrammaticalCase grammaticalCase) =>
        Convert(date);

    static string SanitizeNonEnglishDate(string value) =>
        value.Replace("\u200e", string.Empty)
            .Replace("\u200f", string.Empty)
            .Replace("\u061c", string.Empty);
}

#endif
