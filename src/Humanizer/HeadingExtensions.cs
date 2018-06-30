using System;
using System.Globalization;
using Humanizer.Localisation;

namespace Humanizer
{
    /// <summary>
    /// Style for the cardinal direction humanization
    /// </summary>
    public enum HeadingStyle
    {
        /// <summary>
        /// Returns an abbreviated format
        /// </summary>
        Abbreviated,

        /// <summary>
        /// Returns the full format
        /// </summary>
        Full
    }

    /// <summary>
    /// Contains extensions to transform a number indicating a heading into the 
    /// textual representation of the heading.
    /// </summary>
    public static class HeadingExtensions
    {
        internal static readonly string[] headings = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
        internal static readonly char[] headingArrows = { '↑', '↗', '→', '↘', '↓', '↙', '←', '↖' };

        // https://stackoverflow.com/a/7490772/1720761
        /// <summary>
        /// Returns a textual representation of the heading.
        /// 
        /// This representation has a maximum deviation of 11.25 degrees.
        /// </summary>
        /// <returns>A textual representation of the heading</returns>
        /// <param name="culture">The culture to return the textual representation in</param>
        /// <param name="style">Whether to return a short result or not. <see cref="HeadingStyle"/></param>
        public static string ToHeading(this double heading, HeadingStyle style = HeadingStyle.Abbreviated, CultureInfo culture = null)
        {
            var val = (int)((heading / 22.5) + .5);

            var result = headings[val % 16];

            if (style == HeadingStyle.Abbreviated)
            {
                return Resources.GetResource($"{result}_Short", culture);
            }

            result = Resources.GetResource(result, culture);
            return result;
        }

        /// <summary>
        /// Returns a char arrow indicating the heading.
        /// 
        /// This representation has a maximum deviation of 22.5 degrees.
        /// </summary>
        /// <returns>The heading arrow.</returns>
        public static char ToHeadingArrow(this double heading)
        {
            var val = (int)((heading / 45) + .5);

            return headingArrows[val % 8];
        }

        /// <summary>
        /// Returns a heading based on the short textual representation of the heading.
        /// </summary>
        /// <returns>The heading. -1 if the heading could not be parsed.</returns>
        public static double FromAbbreviatedHeading(this string heading)
        {
            var index = Array.IndexOf(headings, heading.ToUpperInvariant());

            if (index == -1)
            {
                return -1;
            }

            return (index * 22.5);
        }
    }
}
