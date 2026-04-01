namespace Humanizer;

/// <summary>
/// Base converter for languages whose number-to-words rendering depends on grammatical gender.
///
/// The default implementations route the caller through the converter's configured default gender
/// so derived types only need to override the gender-aware members that actually vary by locale.
/// </summary>
abstract class GenderedNumberToWordsConverter(GrammaticalGender defaultGender = GrammaticalGender.Masculine) : INumberToWordsConverter
{
    readonly GrammaticalGender defaultGender = defaultGender;

    /// <summary>
    /// Converts the given value using the converter's default grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public string Convert(long number) =>
        Convert(number, defaultGender);

    /// <summary>
    /// Converts the given value using the converter's default grammatical gender and the requested word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public string Convert(long number, WordForm wordForm) =>
        Convert(number, wordForm, defaultGender);

    /// <summary>
    /// Converts the given value using the converter's default grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="addAnd">Ignored by this overload; use the gender-aware overload when conjunction control matters.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public string Convert(long number, bool addAnd) =>
        Convert(number, defaultGender);

    /// <summary>
    /// Converts the given value using the converter's default grammatical gender and the requested word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="addAnd">Whether the locale should insert a conjunction where applicable.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public string Convert(long number, bool addAnd, WordForm wordForm) =>
        Convert(number, wordForm, defaultGender, addAnd);

    /// <summary>
    /// Converts the given value using the supplied grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <param name="addAnd">Whether the locale should insert a conjunction where applicable.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public abstract string Convert(long number, GrammaticalGender gender, bool addAnd = true);

    /// <summary>
    /// Converts the given value using the supplied grammatical gender and word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <param name="addAnd">Whether the locale should insert a conjunction where applicable.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public virtual string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, gender, addAnd);

    /// <summary>
    /// Converts the given value to ordinal words using the converter's default grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public string ConvertToOrdinal(int number) =>
        ConvertToOrdinal(number, defaultGender);

    /// <summary>
    /// Converts the given value to ordinal words using the supplied grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public abstract string ConvertToOrdinal(int number, GrammaticalGender gender);

    /// <summary>
    /// Converts the given value to ordinal words using the converter's default grammatical gender and the requested word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public string ConvertToOrdinal(int number, WordForm wordForm) =>
        ConvertToOrdinal(number, defaultGender, wordForm);

    /// <summary>
    /// Converts the given value to ordinal words using the supplied grammatical gender and word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public virtual string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm) =>
        ConvertToOrdinal(number, gender);

    /// <summary>
    /// Converts the given value to a tuple form when the locale supports one.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized tuple words for <paramref name="number"/>.</returns>
    public virtual string ConvertToTuple(int number) =>
        Convert(number);
}
