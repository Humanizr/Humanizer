namespace Humanizer;

/// <summary>
/// Base ordinalizer that leaves the input unchanged unless a derived type overrides it.
/// </summary>
class DefaultOrdinalizer : ILongOrdinalizer
{
    /// <summary>
    /// Ordinalizes the number using the default grammatical form.
    /// </summary>
    public virtual string Convert(int number, string numberString, GrammaticalGender gender) =>
        Convert((long)number, numberString, gender);

    /// <summary>
    /// Ordinalizes the number using the default grammatical form.
    /// </summary>
    public virtual string Convert(long number, string numberString, GrammaticalGender gender) =>
        Convert(number, numberString);

    /// <summary>
    /// Returns the original numeric text unchanged.
    /// </summary>
    public virtual string Convert(int number, string numberString) =>
        Convert((long)number, numberString);

    /// <summary>
    /// Returns the original numeric text unchanged.
    /// </summary>
    public virtual string Convert(long number, string numberString) =>
        numberString;

    /// <summary>
    /// Ordinalizes the number using a locale-specific word form.
    /// </summary>
    public virtual string Convert(int number, string numberString, WordForm wordForm) =>
        Convert((long)number, numberString, wordForm);

    /// <summary>
    /// Ordinalizes the number using a locale-specific word form.
    /// </summary>
    public virtual string Convert(long number, string numberString, WordForm wordForm) =>
        Convert(number, numberString, default, wordForm);

    /// <summary>
    /// Ordinalizes the number using the default implementation for the provided gender and word form.
    /// </summary>
    public virtual string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm) =>
        Convert((long)number, numberString, gender, wordForm);

    /// <summary>
    /// Ordinalizes the number using the default implementation for the provided gender and word form.
    /// </summary>
    public virtual string Convert(long number, string numberString, GrammaticalGender gender, WordForm wordForm) =>
        Convert(number, numberString, gender);

    protected static ulong GetAbsoluteValue(long number) =>
        number < 0
            ? (ulong)(-(number + 1)) + 1
            : (ulong)number;

    protected static bool TryGetInt32Value(long number, out int value)
    {
        if (number is >= int.MinValue and <= int.MaxValue)
        {
            value = (int)number;
            return true;
        }

        value = default;
        return false;
    }
}