using System.Globalization;

using Humanizer.Localisation;
using Humanizer.Configuration;

namespace Humanizer
{
    /// <summary>
    /// Transform a time unit into a symbol; e.g. <see cref="TimeUnit.Year"/> => "a"
    /// </summary>
    public static class TimeUnitToSymbolExtensions
    {
        /// <summary>
        /// TimeUnit.Day.ToSymbol() -> "d"
        /// </summary>
        /// <param name="unit">Unit of time to be turned to a symbol</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToSymbol(this TimeUnit unit, CultureInfo culture = null)
        {
            return Configurator.GetFormatter(culture).TimeUnitHumanize(unit);
        }
    }
}
