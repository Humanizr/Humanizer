namespace Humanizer;

/// <summary>
/// Localizes the ordinal form of a number.
/// </summary>
public interface IOrdinalizer
{
    /// <summary>
    /// Ordinalizes the number using the default grammatical form.
    /// </summary>
    /// <param name="number">The numeric value being ordinalized.</param>
    /// <param name="numberString">The cardinal representation of the number.</param>
    /// <returns>The ordinalized text.</returns>
    string Convert(int number, string numberString);

    /// <summary>
    /// Ordinalizes the number using a locale-specific word form.
    /// </summary>
    /// <param name="number">The numeric value being ordinalized.</param>
    /// <param name="numberString">The cardinal representation of the number.</param>
    /// <param name="wordForm">The word form to use when the locale distinguishes abbreviations from full words.</param>
    /// <returns>The ordinalized text.</returns>
    string Convert(int number, string numberString, WordForm wordForm);

    /// <summary>
    /// Ordinalizes the number using the provided grammatical gender.
    /// </summary>
    /// <param name="number">The numeric value being ordinalized.</param>
    /// <param name="numberString">The cardinal representation of the number.</param>
    /// <param name="gender">The grammatical gender to use when the locale requires one.</param>
    /// <returns>The ordinalized text.</returns>
    string Convert(int number, string numberString, GrammaticalGender gender);

    /// <summary>
    /// Ordinalizes the number using the provided grammatical gender and word form.
    /// </summary>
    /// <param name="number">The numeric value being ordinalized.</param>
    /// <param name="numberString">The cardinal representation of the number.</param>
    /// <param name="gender">The grammatical gender to use when the locale requires one.</param>
    /// <param name="wordForm">The word form to use when the locale distinguishes abbreviations from full words.</param>
    /// <returns>The ordinalized text.</returns>
    string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm);
}

/// <summary>
/// Localizes the ordinal form of a 64-bit integer.
/// </summary>
/// <remarks>
/// Implement this interface when registering a custom ordinalizer that supports values outside the
/// <see cref="int"/> range. Existing <see cref="IOrdinalizer"/> implementations remain supported for
/// 32-bit values.
/// </remarks>
public interface ILongOrdinalizer : IOrdinalizer
{
    /// <summary>
    /// Ordinalizes the number using the default grammatical form.
    /// </summary>
    /// <param name="number">The numeric value being ordinalized.</param>
    /// <param name="numberString">The cardinal representation of the number.</param>
    /// <returns>The ordinalized text.</returns>
    string Convert(long number, string numberString);

    /// <summary>
    /// Ordinalizes the number using a locale-specific word form.
    /// </summary>
    /// <param name="number">The numeric value being ordinalized.</param>
    /// <param name="numberString">The cardinal representation of the number.</param>
    /// <param name="wordForm">The word form to use when the locale distinguishes abbreviations from full words.</param>
    /// <returns>The ordinalized text.</returns>
    string Convert(long number, string numberString, WordForm wordForm);

    /// <summary>
    /// Ordinalizes the number using the provided grammatical gender.
    /// </summary>
    /// <param name="number">The numeric value being ordinalized.</param>
    /// <param name="numberString">The cardinal representation of the number.</param>
    /// <param name="gender">The grammatical gender to use when the locale requires one.</param>
    /// <returns>The ordinalized text.</returns>
    string Convert(long number, string numberString, GrammaticalGender gender);

    /// <summary>
    /// Ordinalizes the number using the provided grammatical gender and word form.
    /// </summary>
    /// <param name="number">The numeric value being ordinalized.</param>
    /// <param name="numberString">The cardinal representation of the number.</param>
    /// <param name="gender">The grammatical gender to use when the locale requires one.</param>
    /// <param name="wordForm">The word form to use when the locale distinguishes abbreviations from full words.</param>
    /// <returns>The ordinalized text.</returns>
    string Convert(long number, string numberString, GrammaticalGender gender, WordForm wordForm);
}