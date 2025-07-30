namespace Humanizer;

/// <summary>
/// Number to TimeSpan extensions
/// </summary>
public static class NumberToTimeSpanExtensions
{
    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this byte ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this sbyte ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this short ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this ushort ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this int ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this uint ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this long ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this ulong ms) =>
        Milliseconds((double)ms);

    /// <summary>
    /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5)
    /// </summary>
    public static TimeSpan Milliseconds(this double ms) =>
        TimeSpan.FromMilliseconds(ms);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this byte seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this sbyte seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this short seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this ushort seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this int seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this uint seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this long seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this ulong seconds) =>
        Seconds((double)seconds);

    /// <summary>
    /// 5.Seconds() == TimeSpan.FromSeconds(5)
    /// </summary>
    public static TimeSpan Seconds(this double seconds) =>
        TimeSpan.FromSeconds(seconds);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this byte minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this sbyte minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this short minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this ushort minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this int minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this uint minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this long minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this ulong minutes) =>
        Minutes((double)minutes);

    /// <summary>
    /// 4.Minutes() == TimeSpan.FromMinutes(4)
    /// </summary>
    public static TimeSpan Minutes(this double minutes) =>
        TimeSpan.FromMinutes(minutes);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this byte hours) =>
        Hours((double)hours);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this sbyte hours) =>
        Hours((double)hours);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this short hours) =>
        Hours((double)hours);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this ushort hours) =>
        Hours((double)hours);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this int hours) =>
        Hours((double)hours);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this uint hours) =>
        Hours((double)hours);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this long hours) =>
        Hours((double)hours);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this ulong hours) =>
        Hours((double)hours);

    /// <summary>
    /// 3.Hours() == TimeSpan.FromHours(3)
    /// </summary>
    public static TimeSpan Hours(this double hours) =>
        TimeSpan.FromHours(hours);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this byte days) =>
        Days((double)days);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this sbyte days) =>
        Days((double)days);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this short days) =>
        Days((double)days);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this ushort days) =>
        Days((double)days);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this int days) =>
        Days((double)days);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this uint days) =>
        Days((double)days);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this long days) =>
        Days((double)days);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this ulong days) =>
        Days((double)days);

    /// <summary>
    /// 2.Days() == TimeSpan.FromDays(2)
    /// </summary>
    public static TimeSpan Days(this double days) =>
        TimeSpan.FromDays(days);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this byte input) =>
        Weeks((double)input);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this sbyte input) =>
        Weeks((double)input);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this short input) =>
        Weeks((double)input);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this ushort input) =>
        Weeks((double)input);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this int input) =>
        Weeks((double)input);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this uint input) =>
        Weeks((double)input);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this long input) =>
        Weeks((double)input);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this ulong input) =>
        Weeks((double)input);

    /// <summary>
    /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
    /// </summary>
    public static TimeSpan Weeks(this double input) =>
        Days(7 * input);
}