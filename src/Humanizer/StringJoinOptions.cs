using System;

namespace Humanizer
{
    /// <summary>
    /// Options for joining a collection of strings
    /// </summary>
    [Flags]
    public enum StringJoinOptions
    {
        /// <summary>
        /// Leaves items whitespace and leaves blank items.
        /// </summary>
        None = 0,
        /// <summary>
        /// Trims item whitespace Prevents "A,   B   , and C" for { "A", "  B  ", "C" }; outputs "A, B, and C".
        /// </summary>
        TrimEntries = 1,
        /// <summary>
        /// Removes blank items. Prevents "A, , and C" for { "A", " ", "C" }; outputs "A and C".
        /// </summary>
        RemoveBlankEntries = 2,
        /// <summary>
        /// Trims item whitespace and removes blank items. Prevents "A ,  , and C" for { "A ", " ", "C" }; outputs "A and C".
        /// </summary>
        Default = TrimEntries | RemoveBlankEntries,
    }
}