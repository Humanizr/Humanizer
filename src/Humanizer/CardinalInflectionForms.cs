namespace Humanizer;

/// <summary>
/// Provides authored forms of one common noun for cardinal-count inflection.
/// </summary>
/// <remarks>
/// These forms represent a bare noun governed by a cardinal count. They do not model articles,
/// adjectives, classifiers, independently selectable grammatical case or gender, or complete noun
/// phrases. An authored form can include the case governed by its cardinal quantity.
/// </remarks>
public sealed class CardinalInflectionForms
{
    /// <summary>
    /// Creates a set of authored cardinal forms.
    /// </summary>
    /// <param name="lemma">The noun's citation form.</param>
    /// <param name="other">The form for the CLDR <c>other</c> category.</param>
    /// <param name="zero">The form for the CLDR <c>zero</c> category, or <see langword="null"/> when unavailable.</param>
    /// <param name="one">The form for the CLDR <c>one</c> category, or <see langword="null"/> when unavailable.</param>
    /// <param name="two">The form for the CLDR <c>two</c> category, or <see langword="null"/> when unavailable.</param>
    /// <param name="few">The form for the CLDR <c>few</c> category, or <see langword="null"/> when unavailable.</param>
    /// <param name="many">The form for the CLDR <c>many</c> category, or <see langword="null"/> when unavailable.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="lemma"/> or <paramref name="other"/> is empty or whitespace, or an optional supplied form is empty or whitespace.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="lemma"/> or <paramref name="other"/> is <see langword="null"/>.</exception>
    public CardinalInflectionForms(
        string lemma,
        string other,
        string? zero = null,
        string? one = null,
        string? two = null,
        string? few = null,
        string? many = null)
    {
        Lemma = RequireForm(lemma, nameof(lemma));
        Other = RequireForm(other, nameof(other));
        Zero = ValidateOptionalForm(zero, nameof(zero));
        One = ValidateOptionalForm(one, nameof(one));
        Two = ValidateOptionalForm(two, nameof(two));
        Few = ValidateOptionalForm(few, nameof(few));
        Many = ValidateOptionalForm(many, nameof(many));
    }

    /// <summary>
    /// Gets the noun's citation form.
    /// </summary>
    public string Lemma { get; }

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
    /// <param name="lemma">The invariant citation form.</param>
    /// <returns>A form set containing <paramref name="lemma"/> for every category.</returns>
    /// <exception cref="ArgumentException"><paramref name="lemma"/> is empty or whitespace.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="lemma"/> is <see langword="null"/>.</exception>
    public static CardinalInflectionForms Invariant(string lemma) =>
        new(lemma, lemma, lemma, lemma, lemma, lemma, lemma);

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
}