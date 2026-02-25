namespace Humanizer;

/// <summary>
/// Enumeration of time units used by Humanizer's time-span humanization and formatting methods.
/// </summary>
public enum TimeUnit
{
    /// <summary>One thousandth of a second (1/1,000 s).</summary>
    Millisecond,
    /// <summary>The base SI unit of time.</summary>
    Second,
    /// <summary>Sixty seconds.</summary>
    Minute,
    /// <summary>Sixty minutes (3,600 seconds).</summary>
    Hour,
    /// <summary>Twenty-four hours (86,400 seconds).</summary>
    Day,
    /// <summary>Seven days (604,800 seconds).</summary>
    Week,
    /// <summary>A calendar month (approximately 30.44 days on average).</summary>
    Month,
    /// <summary>A calendar year (approximately 365.25 days on average).</summary>
    Year
}
