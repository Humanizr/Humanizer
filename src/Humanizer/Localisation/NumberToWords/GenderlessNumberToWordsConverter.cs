namespace Humanizer;

/// <summary>
/// Base converter for languages whose number-to-words rendering does not depend on grammatical gender.
///
/// The default implementations provide the overload fan-out used by the public API while derived
/// types override only the cardinal and ordinal core forms.
/// </summary>
abstract class GenderlessNumberToWordsConverter : INumberToWordsConverter
{
    /// <summary>
    /// Converts the given value into cardinal words.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public abstract string Convert(long number);

    /// <summary>
    /// Converts the given value using the requested word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public string Convert(long number, WordForm wordForm) =>
        Convert(number);

    /// <summary>
    /// Converts the given value using the locale's default conjunction behavior.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="addAnd">Ignored by this overload; conjunction behavior is fixed by the concrete converter.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public virtual string Convert(long number, bool addAnd) =>
        Convert(number);

    /// <summary>
    /// Converts the given value using the requested word form and conjunction behavior.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="addAnd">Ignored by this overload; conjunction behavior is fixed by the concrete converter.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public string Convert(long number, bool addAnd, WordForm wordForm) =>
        Convert(number, wordForm);

    /// <summary>
    /// Converts the given value using the requested gender and conjunction behavior.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <param name="addAnd">Ignored by this overload; conjunction behavior is fixed by the concrete converter.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public virtual string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number);

    /// <summary>
    /// Converts the given value using the requested word form, gender, and conjunction behavior.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <param name="addAnd">Ignored by this overload; conjunction behavior is fixed by the concrete converter.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    public virtual string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, addAnd, wordForm);

    /// <summary>
    /// Converts the given value into ordinal words.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public abstract string ConvertToOrdinal(int number);

    /// <summary>
    /// Converts the given value into ordinal words using the requested gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        ConvertToOrdinal(number);

    /// <summary>
    /// Converts the given value into ordinal words using the requested word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public virtual string ConvertToOrdinal(int number, WordForm wordForm) =>
        ConvertToOrdinal(number);

    /// <summary>
    /// Converts the given value into ordinal words using the requested gender and word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public virtual string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm) =>
        ConvertToOrdinal(number, wordForm);

    /// <summary>
    /// Converts the given value to a tuple form when the locale supports one.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized tuple words for <paramref name="number"/>.</returns>
    public virtual string ConvertToTuple(int number) =>
        Convert(number);
}