namespace Humanizer.Localisation.Ordinalizers
{
    /// <summary>
    /// The interface used to localise the Ordinalize method
    /// </summary>
    public interface IOrdinalizer
    {
        /// <summary>
        /// Ordinalizes the number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberString"></param>
        /// <returns></returns>
        string Convert(int number, string numberString);

        /// <summary>
        /// Ordinalizes the number to a locale's specific form.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberString"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        string Convert(int number, string numberString, WordForm wordForm);

        /// <summary>
        /// Ordinalizes the number using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberString"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        string Convert(int number, string numberString, GrammaticalGender gender);

        /// <summary>
        /// Ordinalizes the number to a locale's specific form using the provided grammatical gender.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberString"></param>
        /// <param name="gender"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm);
    }
}
