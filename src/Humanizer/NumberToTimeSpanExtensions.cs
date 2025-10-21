namespace Humanizer;

/// <summary>
/// Provides extension methods for converting numeric values to <see cref="TimeSpan"/> instances,
/// enabling fluent and readable time duration creation (e.g., 5.Seconds(), 3.Hours(), 2.Weeks()).
/// </summary>
/// <remarks>
/// These extensions make it easy to create TimeSpan values in a more natural, readable way:
/// - Instead of TimeSpan.FromHours(3), you can write 3.Hours()
/// - Instead of TimeSpan.FromMinutes(30), you can write 30.Minutes()
/// - Supports all numeric types: byte, sbyte, short, ushort, int, uint, long, ulong, and double
/// - Weeks are converted to days (1 week = 7 days)
/// </remarks>
public static class NumberToTimeSpanExtensions
{
    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// 500.Milliseconds() => TimeSpan representing 500 milliseconds
    /// 1000.Milliseconds() => TimeSpan representing 1 second
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this byte ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// ((sbyte)500).Milliseconds() => TimeSpan.FromMilliseconds(500)
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this sbyte ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// ((short)500).Milliseconds() => TimeSpan.FromMilliseconds(500)
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this short ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// ((ushort)500).Milliseconds() => TimeSpan.FromMilliseconds(500)
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this ushort ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// 500.Milliseconds() => TimeSpan.FromMilliseconds(500)
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this int ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// 500U.Milliseconds() => TimeSpan.FromMilliseconds(500)
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this uint ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// 500L.Milliseconds() => TimeSpan.FromMilliseconds(500)
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this long ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// 500UL.Milliseconds() => TimeSpan.FromMilliseconds(500)
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this ulong ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of milliseconds.
    /// </summary>
    /// <param name="ms">The number of milliseconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="ms"/> milliseconds.</returns>
    /// <example>
    /// <code>
    /// 500.0.Milliseconds() => TimeSpan.FromMilliseconds(500)
    /// </code>
    /// </example>
    public static TimeSpan Milliseconds(this double ms) =>
        TimeSpan.FromMilliseconds(ms);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// ((byte)30).Seconds() => TimeSpan.FromSeconds(30)
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this byte seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// ((sbyte)30).Seconds() => TimeSpan.FromSeconds(30)
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this sbyte seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// ((short)30).Seconds() => TimeSpan.FromSeconds(30)
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this short seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// ((ushort)30).Seconds() => TimeSpan.FromSeconds(30)
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this ushort seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// 30.Seconds() => TimeSpan.FromSeconds(30)
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this int seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// 30U.Seconds() => TimeSpan.FromSeconds(30)
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this uint seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// 30L.Seconds() => TimeSpan.FromSeconds(30)
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this long seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// 30UL.Seconds() => TimeSpan.FromSeconds(30)
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this ulong seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of seconds.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="seconds"/> seconds.</returns>
    /// <example>
    /// <code>
    /// 30.Seconds() => TimeSpan representing 30 seconds
    /// 90.Seconds() => TimeSpan representing 1 minute and 30 seconds
    /// </code>
    /// </example>
    public static TimeSpan Seconds(this double seconds) =>
        TimeSpan.FromSeconds(seconds);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// ((byte)30).Minutes() => TimeSpan.FromMinutes(30)
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this byte minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// ((sbyte)30).Minutes() => TimeSpan.FromMinutes(30)
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this sbyte minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// ((short)30).Minutes() => TimeSpan.FromMinutes(30)
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this short minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// ((ushort)30).Minutes() => TimeSpan.FromMinutes(30)
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this ushort minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// 30.Minutes() => TimeSpan.FromMinutes(30)
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this int minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// 30U.Minutes() => TimeSpan.FromMinutes(30)
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this uint minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// 30L.Minutes() => TimeSpan.FromMinutes(30)
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this long minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// 30UL.Minutes() => TimeSpan.FromMinutes(30)
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this ulong minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of minutes.
    /// </summary>
    /// <param name="minutes">The number of minutes.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="minutes"/> minutes.</returns>
    /// <example>
    /// <code>
    /// 30.Minutes() => TimeSpan representing 30 minutes
    /// 90.Minutes() => TimeSpan representing 1 hour and 30 minutes
    /// </code>
    /// </example>
    public static TimeSpan Minutes(this double minutes) =>
        TimeSpan.FromMinutes(minutes);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// ((byte)3).Hours() => TimeSpan.FromHours(3)
    /// </code>
    /// </example>
    public static TimeSpan Hours(this byte hours) =>
        Hours((double)hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// ((sbyte)3).Hours() => TimeSpan.FromHours(3)
    /// </code>
    /// </example>
    public static TimeSpan Hours(this sbyte hours) =>
        Hours((double)hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// ((short)3).Hours() => TimeSpan.FromHours(3)
    /// </code>
    /// </example>
    public static TimeSpan Hours(this short hours) =>
        Hours((double)hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// ((ushort)3).Hours() => TimeSpan.FromHours(3)
    /// </code>
    /// </example>
    public static TimeSpan Hours(this ushort hours) =>
        Hours((double)hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// 3.Hours() => TimeSpan.FromHours(3)
    /// </code>
    /// </example>
    public static TimeSpan Hours(this int hours) =>
        Hours((double)hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// 3U.Hours() => TimeSpan.FromHours(3)
    /// </code>
    /// </example>
    public static TimeSpan Hours(this uint hours) =>
        Hours((double)hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// 3L.Hours() => TimeSpan.FromHours(3)
    /// </code>
    /// </example>
    public static TimeSpan Hours(this long hours) =>
        Hours((double)hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// 3UL.Hours() => TimeSpan.FromHours(3)
    /// </code>
    /// </example>
    public static TimeSpan Hours(this ulong hours) =>
        Hours((double)hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of hours.
    /// </summary>
    /// <param name="hours">The number of hours.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="hours"/> hours.</returns>
    /// <example>
    /// <code>
    /// 3.Hours() => TimeSpan representing 3 hours
    /// 24.Hours() => TimeSpan representing 1 day
    /// 1.5.Hours() => TimeSpan representing 1 hour and 30 minutes
    /// </code>
    /// </example>
    public static TimeSpan Hours(this double hours) =>
        TimeSpan.FromHours(hours);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// ((byte)2).Days() => TimeSpan.FromDays(2)
    /// </code>
    /// </example>
    public static TimeSpan Days(this byte days) =>
        Days((double)days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// ((sbyte)2).Days() => TimeSpan.FromDays(2)
    /// </code>
    /// </example>
    public static TimeSpan Days(this sbyte days) =>
        Days((double)days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// ((short)2).Days() => TimeSpan.FromDays(2)
    /// </code>
    /// </example>
    public static TimeSpan Days(this short days) =>
        Days((double)days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// ((ushort)2).Days() => TimeSpan.FromDays(2)
    /// </code>
    /// </example>
    public static TimeSpan Days(this ushort days) =>
        Days((double)days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// 2.Days() => TimeSpan.FromDays(2)
    /// </code>
    /// </example>
    public static TimeSpan Days(this int days) =>
        Days((double)days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// 2U.Days() => TimeSpan.FromDays(2)
    /// </code>
    /// </example>
    public static TimeSpan Days(this uint days) =>
        Days((double)days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// 2L.Days() => TimeSpan.FromDays(2)
    /// </code>
    /// </example>
    public static TimeSpan Days(this long days) =>
        Days((double)days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// 2UL.Days() => TimeSpan.FromDays(2)
    /// </code>
    /// </example>
    public static TimeSpan Days(this ulong days) =>
        Days((double)days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of days.
    /// </summary>
    /// <param name="days">The number of days.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="days"/> days.</returns>
    /// <example>
    /// <code>
    /// 2.Days() => TimeSpan representing 2 days  
    /// 7.Days() => TimeSpan representing 1 week
    /// 1.5.Days() => TimeSpan representing 1 day and 12 hours
    /// </code>
    /// </example>
    public static TimeSpan Days(this double days) =>
        TimeSpan.FromDays(days);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <example>
    /// <code>
    /// ((byte)2).Weeks() => new TimeSpan(14, 0, 0, 0)
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this byte input) =>
        Weeks((double)input);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <example>
    /// <code>
    /// ((sbyte)2).Weeks() => new TimeSpan(14, 0, 0, 0)
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this sbyte input) =>
        Weeks((double)input);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <example>
    /// <code>
    /// ((short)2).Weeks() => new TimeSpan(14, 0, 0, 0)
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this short input) =>
        Weeks((double)input);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <example>
    /// <code>
    /// ((ushort)2).Weeks() => new TimeSpan(14, 0, 0, 0)
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this ushort input) =>
        Weeks((double)input);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <example>
    /// <code>
    /// 2.Weeks() => new TimeSpan(14, 0, 0, 0)
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this int input) =>
        Weeks((double)input);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <example>
    /// <code>
    /// 2U.Weeks() => new TimeSpan(14, 0, 0, 0)
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this uint input) =>
        Weeks((double)input);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <example>
    /// <code>
    /// 2L.Weeks() => new TimeSpan(14, 0, 0, 0)
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this long input) =>
        Weeks((double)input);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <example>
    /// <code>
    /// 2UL.Weeks() => new TimeSpan(14, 0, 0, 0)
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this ulong input) =>
        Weeks((double)input);

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> representing the specified number of weeks.
    /// </summary>
    /// <param name="input">The number of weeks.</param>
    /// <returns>A <see cref="TimeSpan"/> representing <paramref name="input"/> weeks (converted to days: 1 week = 7 days).</returns>
    /// <remarks>
    /// Since <see cref="TimeSpan"/> doesn't have a native concept of weeks, this method converts
    /// weeks to days (multiplying by 7).
    /// </remarks>
    /// <example>
    /// <code>
    /// 2.Weeks() => TimeSpan representing 14 days
    /// 1.Weeks() => TimeSpan representing 7 days
    /// 0.5.Weeks() => TimeSpan representing 3.5 days
    /// </code>
    /// </example>
    public static TimeSpan Weeks(this double input) =>
        Days(7 * input);
}