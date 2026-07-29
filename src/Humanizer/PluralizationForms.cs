namespace Humanizer;

/// <summary>
/// Provides authored singular and plural forms of one noun.
/// </summary>
/// <remarks>
/// Humanizer applies the supported culture's cardinal plural rule to select an authored form.
/// Missing selected forms are not inferred.
/// </remarks>
public sealed class PluralizationForms
{
    /// <summary>
    /// Creates a set of authored singular and plural forms.
    /// </summary>
    /// <param name="singular">The noun's singular form.</param>
    /// <param name="other">The form for the CLDR <c>other</c> category.</param>
    /// <param name="zero">The form for the CLDR <c>zero</c> category, or <see langword="null"/> when unavailable.</param>
    /// <param name="one">The form for the CLDR <c>one</c> category, or <see langword="null"/> when unavailable.</param>
    /// <param name="two">The form for the CLDR <c>two</c> category, or <see langword="null"/> when unavailable.</param>
    /// <param name="few">The form for the CLDR <c>few</c> category, or <see langword="null"/> when unavailable.</param>
    /// <param name="many">The form for the CLDR <c>many</c> category, or <see langword="null"/> when unavailable.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="singular"/> or <paramref name="other"/> is empty or whitespace, or an optional supplied form is empty or whitespace.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="singular"/> or <paramref name="other"/> is <see langword="null"/>.</exception>
    public PluralizationForms(
        string singular,
        string other,
        string? zero = null,
        string? one = null,
        string? two = null,
        string? few = null,
        string? many = null)
    {
        Singular = RequireForm(singular, nameof(singular));
        Other = RequireForm(other, nameof(other));
        Zero = ValidateOptionalForm(zero, nameof(zero));
        One = ValidateOptionalForm(one, nameof(one));
        Two = ValidateOptionalForm(two, nameof(two));
        Few = ValidateOptionalForm(few, nameof(few));
        Many = ValidateOptionalForm(many, nameof(many));
    }

    /// <summary>
    /// Gets the noun's singular form.
    /// </summary>
    public string Singular { get; }

    /// <summary>
    /// Gets the form for the CLDR <c>other</c> category.
    /// </summary>
    public string Other { get; }

    /// <summary>
    /// Gets the form for the CLDR <c>zero</c> category.
    /// </summary>
    public string? Zero { get; }

    /// <summary>
    /// Gets the form for the CLDR <c>one</c> category.
    /// </summary>
    public string? One { get; }

    /// <summary>
    /// Gets the form for the CLDR <c>two</c> category.
    /// </summary>
    public string? Two { get; }

    /// <summary>
    /// Gets the form for the CLDR <c>few</c> category.
    /// </summary>
    public string? Few { get; }

    /// <summary>
    /// Gets the form for the CLDR <c>many</c> category.
    /// </summary>
    public string? Many { get; }

    /// <summary>
    /// Creates forms for a noun that remains unchanged in every cardinal category.
    /// </summary>
    /// <param name="word">The invariant word.</param>
    /// <returns>A form set containing <paramref name="word"/> for every category.</returns>
    /// <exception cref="ArgumentException"><paramref name="word"/> is empty or whitespace.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="word"/> is <see langword="null"/>.</exception>
    public static PluralizationForms Invariant(string word) =>
        new(word, word, word, word, word, word, word);

    /// <summary>
    /// Attempts to pluralize this noun for a quantity using the supported culture's cardinal plural rule.
    /// </summary>
    /// <param name="quantity">The quantity. Its encoded decimal scale supplies CLDR visible-fraction operands.</param>
    /// <param name="culture">The supported culture whose cardinal plural rule is applied.</param>
    /// <param name="result">The selected authored form when available; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when the selected form is available; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="culture"/> is <see langword="null"/>.</exception>
    public bool TryPluralize(
        decimal quantity,
        CultureInfo culture,
        [NotNullWhen(true)] out string? result)
    {
        ArgumentNullException.ThrowIfNull(culture);

        if (LocalizedInflectionCatalog.TrySelectCategory(culture, quantity, out var category) &&
            TryGetForm(category, out result))
        {
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Attempts to resolve one of this noun's exact authored forms to its singular form.
    /// </summary>
    /// <param name="form">An exact authored form.</param>
    /// <param name="result">The singular form when <paramref name="form"/> matches; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when <paramref name="form"/> matches an authored form; otherwise, <see langword="false"/>.</returns>
    /// <remarks>Matching is Unicode NFC-normalized, ordinal, and case-sensitive.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="form"/> is <see langword="null"/>.</exception>
    public bool TrySingularize(
        string form,
        [NotNullWhen(true)] out string? result)
    {
        ArgumentNullException.ThrowIfNull(form);

        var normalized = Normalize(form);
        if (Matches(normalized, Singular) ||
            Matches(normalized, Other) ||
            Matches(normalized, Zero) ||
            Matches(normalized, One) ||
            Matches(normalized, Two) ||
            Matches(normalized, Few) ||
            Matches(normalized, Many))
        {
            result = Singular;
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Attempts to get the explicitly authored form for a cardinal category.
    /// </summary>
    /// <param name="category">The category to resolve.</param>
    /// <param name="form">The authored form when available; otherwise, <see langword="null"/>.</param>
    internal bool TryGetForm(
        CardinalPluralCategory category,
        [NotNullWhen(true)] out string? form)
    {
        form = category switch
        {
            CardinalPluralCategory.Other => Other,
            CardinalPluralCategory.Zero => Zero,
            CardinalPluralCategory.One => One,
            CardinalPluralCategory.Two => Two,
            CardinalPluralCategory.Few => Few,
            CardinalPluralCategory.Many => Many,
            _ => throw new ArgumentOutOfRangeException(nameof(category))
        };

        return form is not null;
    }

    static string RequireForm(string value, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(value, parameterName);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty or whitespace.", parameterName);
        }

        return value;
    }

    static string? ValidateOptionalForm(string? value, string parameterName)
    {
        if (value is not null && string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty or whitespace.", parameterName);
        }

        return value;
    }

    static bool Matches(string normalized, string? candidate) =>
        candidate is not null &&
        string.Equals(normalized, Normalize(candidate), StringComparison.Ordinal);

    static string Normalize(string value) =>
        value.IsNormalized(NormalizationForm.FormC)
            ? value
            : value.Normalize(NormalizationForm.FormC);
}