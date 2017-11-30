using JetBrains.Annotations;

namespace Humanizer
{
    /// <summary>
    /// Can truncate a string.
    /// </summary>
    public interface ITruncator
    {
        /// <summary>
        /// Truncate a string
        /// </summary>
        /// <param name="value">The string to truncate</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncationString">The string used to truncate with</param>
        /// <param name="truncateFrom">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        [CanBeNull]
        [PublicAPI]
        [ContractAnnotation("value:null => null")]
        string Truncate([CanBeNull] string value, int length, [CanBeNull] string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right);
    }
}
