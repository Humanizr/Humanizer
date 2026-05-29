namespace Humanizer;

/// <summary>
/// Controls which grammatical genders contribute ordinal spellings to the generated map.
/// </summary>
enum TokenMapOrdinalGenderVariant
{
    /// <summary>
    /// Uses only the default ordinal form.
    /// </summary>
    None,
    /// <summary>
    /// Uses masculine and feminine ordinal forms.
    /// </summary>
    MasculineAndFeminine,
    /// <summary>
    /// Uses the default, feminine, and neuter ordinal forms.
    /// </summary>
    All
}

/// <summary>
/// Builds normalized ordinal lookup maps for token-map converters.
/// </summary>
static class TokenMapWordsToNumberOrdinalMapBuilder
{
    /// <summary>
    /// Builds a normalized ordinal lookup map for the named number-to-words kind.
    /// </summary>
    /// <param name="numberToWordsKind">The locale-specific number-to-words kind to resolve.</param>
    /// <param name="normalizationProfile">The normalization profile used to canonicalize ordinals.</param>
    /// <param name="genderVariant">Which grammatical gender forms should be included.</param>
    /// <returns>A frozen dictionary that maps normalized ordinal text to numeric values.</returns>
    public static FrozenDictionary<string, long> Build(
        string numberToWordsKind,
        TokenMapNormalizationProfile normalizationProfile,
        TokenMapOrdinalGenderVariant genderVariant) =>
        Build(
            NumberToWordsProfileCatalog.Resolve(numberToWordsKind, CultureInfo.InvariantCulture),
            normalizationProfile,
            genderVariant);

    /// <summary>
    /// Builds a normalized ordinal lookup map from an existing number-to-words converter.
    /// </summary>
    /// <param name="converter">The number-to-words converter used to render ordinals.</param>
    /// <param name="normalizationProfile">The normalization profile used to canonicalize ordinals.</param>
    /// <param name="genderVariant">Which grammatical gender forms should be included.</param>
    /// <returns>A frozen dictionary that maps normalized ordinal text to numeric values.</returns>
    public static FrozenDictionary<string, long> Build(
        INumberToWordsConverter converter,
        TokenMapNormalizationProfile normalizationProfile,
        TokenMapOrdinalGenderVariant genderVariant)
    {
        var ordinals = new Dictionary<string, long>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            switch (genderVariant)
            {
                case TokenMapOrdinalGenderVariant.None:
                    ordinals[Normalize(converter.ConvertToOrdinal(number), normalizationProfile)] = number;
                    break;
                case TokenMapOrdinalGenderVariant.MasculineAndFeminine:
                    ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Masculine), normalizationProfile)] = number;
                    ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine), normalizationProfile)] = number;
                    break;
                case TokenMapOrdinalGenderVariant.All:
                    ordinals[Normalize(converter.ConvertToOrdinal(number), normalizationProfile)] = number;
                    ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine), normalizationProfile)] = number;
                    ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Neuter), normalizationProfile)] = number;
                    break;
            }
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }

    /// <summary>
    /// Normalizes ordinal text using the supplied profile.
    /// </summary>
    /// <param name="words">The ordinal text to normalize.</param>
    /// <param name="normalizationProfile">The normalization profile to apply.</param>
    /// <returns>The normalized ordinal text.</returns>
    static string Normalize(string words, TokenMapNormalizationProfile normalizationProfile) =>
        TokenMapWordsToNumberNormalizer.Normalize(words, normalizationProfile);
}