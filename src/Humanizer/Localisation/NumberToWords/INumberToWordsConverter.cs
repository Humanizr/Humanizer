namespace Humanizer;

/// <summary>
/// Converts numbers into locale-specific words, ordinals, and tuple names.
/// </summary>
public interface INumberToWordsConverter
{
    /// <summary>
    /// Converts the number using the locale's default grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    string Convert(long number);

    /// <summary>
    /// Converts the number using the locale's default grammatical gender and the specified word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The grammatical or morphological word form to use.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    string Convert(long number, WordForm wordForm);

    /// <summary>
    /// Converts the number using the locale's default grammatical gender and optionally inserts the locale-specific conjunction.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="addAnd">
    /// <c>true</c> to insert the locale-specific conjunction in compound numbers; otherwise, <c>false</c>.
    /// </param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    string Convert(long number, bool addAnd);

    /// <summary>
    /// Converts the number using the locale's default grammatical gender, the specified word form, and optional conjunction handling.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="addAnd">
    /// <c>true</c> to insert the locale-specific conjunction in compound numbers; otherwise, <c>false</c>.
    /// </param>
    /// <param name="wordForm">The grammatical or morphological word form to use.</param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    string Convert(long number, bool addAnd, WordForm wordForm);

    /// <summary>
    /// Converts the number using the specified grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes gendered forms.</param>
    /// <param name="addAnd">
    /// <c>true</c> to insert the locale-specific conjunction in compound numbers; otherwise, <c>false</c>.
    /// </param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    string Convert(long number, GrammaticalGender gender, bool addAnd = true);

    /// <summary>
    /// Converts the number using the specified grammatical gender and word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The grammatical or morphological word form to use.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes gendered forms.</param>
    /// <param name="addAnd">
    /// <c>true</c> to insert the locale-specific conjunction in compound numbers; otherwise, <c>false</c>.
    /// </param>
    /// <returns>The localized words for <paramref name="number"/>.</returns>
    string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true);

    /// <summary>
    /// Converts the number to an ordinal string using the locale's default grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    string ConvertToOrdinal(int number);

    /// <summary>
    /// Converts the number to an ordinal string using the locale's default grammatical gender and the specified word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="wordForm">The grammatical or morphological word form to use.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    string ConvertToOrdinal(int number, WordForm wordForm);

    /// <summary>
    /// Converts the number to an ordinal string using the specified grammatical gender.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes gendered forms.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    string ConvertToOrdinal(int number, GrammaticalGender gender);

    /// <summary>
    /// Converts the number to an ordinal string using the specified grammatical gender and word form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes gendered forms.</param>
    /// <param name="wordForm">The grammatical or morphological word form to use.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm);

    /// <summary>
    /// Converts the integer to a locale-specific named tuple such as <c>single</c> or <c>double</c>.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized tuple name for <paramref name="number"/> when the locale defines one; otherwise, a numeric fallback.</returns>
    string ConvertToTuple(int number);
}
