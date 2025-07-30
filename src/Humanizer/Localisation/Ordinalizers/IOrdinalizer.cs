namespace Humanizer;

/// <summary>
/// The interface used to localise the Ordinalize method
/// </summary>
public interface IOrdinalizer
{
    /// <summary>
    /// Ordinalizes the number
    /// </summary>
    string Convert(int number, string numberString);

    /// <summary>
    /// Ordinalizes the number to a locale's specific form.
    /// </summary>
    string Convert(int number, string numberString, WordForm wordForm);

    /// <summary>
    /// Ordinalizes the number using the provided grammatical gender
    /// </summary>
    string Convert(int number, string numberString, GrammaticalGender gender);

    /// <summary>
    /// Ordinalizes the number to a locale's specific form using the provided grammatical gender.
    /// </summary>
    string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm);
}