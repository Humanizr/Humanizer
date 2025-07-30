﻿namespace Humanizer;

/// <summary>
/// Options for specifying the form of the word when different variations of the same word exists.
/// </summary>
public enum WordForm
{
    /// <summary>
    /// Indicates the normal form of a written word.
    /// </summary>
    Normal,

    /// <summary>
    /// Indicates the shortened form of a written word.
    /// </summary>
    Abbreviation,

    /// <summary>
    /// Indicates the Eifeler Rule form of a word.
    /// https://lb.wikipedia.org/wiki/Eifeler_Reegel
    /// </summary>
    Eifeler,
}