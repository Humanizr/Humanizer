namespace Humanizer;

/// <summary>
/// Indicates whether a relative time reference is in the past or the future.
/// </summary>
public enum Tense
{
    /// <summary>
    /// A future reference such as "in 2 days".
    /// </summary>
    Future,

    /// <summary>
    /// A past reference such as "2 days ago".
    /// </summary>
    Past
}
