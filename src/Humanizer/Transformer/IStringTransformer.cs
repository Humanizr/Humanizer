using JetBrains.Annotations;

namespace Humanizer
{
    /// <summary>
    /// Can tranform a string
    /// </summary>
    public interface IStringTransformer
    {
        /// <summary>
        /// Transform the input
        /// </summary>
        /// <param name="input">String to be transformed</param>
        /// <returns></returns>
        [Pure]
        [NotNull]
        [PublicAPI]
        string Transform([NotNull] string input);
    }
}