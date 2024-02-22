#nullable enable
namespace Humanizer;

public partial class ResourceKeys
{
    /// <summary>
    /// Encapsulates the logic required to get the resource keys for TimeSpan.Humanize
    /// Examples: TimeSpanHumanize_SingleMinute, TimeSpanHumanize_MultipleHours.
    /// </summary>
    public static class TimeSpanHumanize
    {
        /// <summary>
        /// Generates Resource Keys according to convention.
        /// </summary>
        /// <param name="unit">Time unit, <see cref="TimeUnit"/>.</param>
        /// <param name="count">Number of units, default is One.</param>
        /// <param name="toWords">Result to words, default is false.</param>
        /// <returns>Resource key, like TimeSpanHumanize_SingleMinute</returns>
        public static string GetResourceKey(TimeUnit unit, int count = 1, bool toWords = false)
        {
            ValidateRange(count);

            if (count == 0 && toWords)
            {
                return "TimeSpanHumanize_Zero";
            }

            if (count == 1)
            {
                return $"TimeSpanHumanize_Single{unit}";
            }

            return $"TimeSpanHumanize_Multiple{unit}s";
        }
    }
}