namespace Humanizer;

public static partial class InflectorExtensions
{
    /// <summary>
    /// Attempts to inflect an authored set of cardinal noun forms for a quantity and culture.
    /// </summary>
    /// <param name="forms">The authored cardinal forms.</param>
    /// <param name="quantity">The cardinal quantity. Its encoded decimal scale supplies CLDR visible-fraction operands.</param>
    /// <param name="culture">The culture whose cardinal plural rules are applied.</param>
    /// <param name="result">The selected authored form when available; otherwise, <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> when Humanizer supports <paramref name="culture"/> and the selected
    /// category has an explicitly authored form; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Missing category forms are not inferred. Use <see cref="CardinalInflectionForms.Invariant"/>
    /// for an invariant noun, or explicitly supply equal forms when categories share spelling.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="forms"/> or <paramref name="culture"/> is <see langword="null"/>.</exception>
    public static bool TryInflect(
        this CardinalInflectionForms forms,
        decimal quantity,
        CultureInfo culture,
        [NotNullWhen(true)] out string? result)
    {
        ArgumentNullException.ThrowIfNull(forms);
        ArgumentNullException.ThrowIfNull(culture);

        if (LocalizedInflectionCatalog.TrySelectCategory(culture, quantity, out var category) &&
            forms.TryGetForm(category, out result))
        {
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Attempts to inflect a localized citation-form noun from Humanizer's built-in exact lexicon.
    /// </summary>
    /// <param name="lemma">A single common noun in its localized citation form.</param>
    /// <param name="quantity">The cardinal quantity. Its encoded decimal scale supplies CLDR visible-fraction operands.</param>
    /// <param name="culture">The culture whose exact lexicon and cardinal rules are applied.</param>
    /// <param name="result">The selected authored form when available; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> for a known exact lexeme and form; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Matching is Unicode NFC-normalized, ordinal, and case-sensitive. Output preserves authored
    /// casing. This method never falls back to English and never guesses unknown noun morphology.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="lemma"/> or <paramref name="culture"/> is <see langword="null"/>.</exception>
    public static bool TryInflect(
        this string lemma,
        decimal quantity,
        CultureInfo culture,
        [NotNullWhen(true)] out string? result)
    {
        ArgumentNullException.ThrowIfNull(lemma);
        ArgumentNullException.ThrowIfNull(culture);

        return LocalizedInflectionCatalog.TryInflect(culture, lemma, quantity, out result);
    }

    /// <summary>
    /// Attempts to resolve a localized cardinal noun form to its citation form.
    /// </summary>
    /// <param name="form">An exact form from Humanizer's built-in localized lexicon.</param>
    /// <param name="culture">The culture whose exact lexicon is searched.</param>
    /// <param name="lemma">The unique citation form in the selected lexicon; otherwise, <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> when the selected culture's authored reverse index identifies exactly
    /// one citation form; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Matching is Unicode NFC-normalized, ordinal, and case-sensitive. The returned lemma preserves
    /// authored casing. This method is catalog-bounded: it never reverses spelling rules or claims
    /// uniqueness across the complete natural language.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="form"/> or <paramref name="culture"/> is <see langword="null"/>.</exception>
    public static bool TryLemmatize(
        this string form,
        CultureInfo culture,
        [NotNullWhen(true)] out string? lemma)
    {
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(culture);

        return LocalizedInflectionCatalog.TryLemmatize(culture, form, out lemma);
    }
}