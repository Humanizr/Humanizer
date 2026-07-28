namespace Humanizer;

/// <summary>
/// Base ordinalizer that leaves the input unchanged unless a derived type overrides it.
/// </summary>
class DefaultOrdinalizer : IOrdinalizer
{
    /// <summary>
    /// Ordinalizes the number using the default grammatical form.
    /// </summary>
    public virtual string Convert(int number, string numberString, GrammaticalGender gender) =>
        Convert(number, numberString);

    /// <summary>
    /// Returns the original numeric text unchanged.
    /// </summary>
    public virtual string Convert(int number, string numberString) =>
        numberString;

    /// <summary>
    /// Ordinalizes the number using a locale-specific word form.
    /// </summary>
    public virtual string Convert(int number, string numberString, WordForm wordForm) =>
        Convert(number, numberString, default, wordForm);

    /// <summary>
    /// Ordinalizes the number using the default implementation for the provided gender and word form.
    /// </summary>
    public virtual string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm) =>
        Convert(number, numberString, gender);
}