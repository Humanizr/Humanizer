using JetBrains.Annotations;

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
        [NotNull]
        [PublicAPI]
        string Convert(int number, [NotNull] string numberString);

        /// <summary>
        /// Ordinalizes the number using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberString"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        [NotNull]
        [PublicAPI]
        string Convert(int number, [NotNull] string numberString, GrammaticalGender gender);
    }
}
