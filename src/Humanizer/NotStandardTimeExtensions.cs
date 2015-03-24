using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Humanizer.Localisation;
using Humanizer.Configuration;
using Humanizer.Localisation.Formatters;

namespace Humanizer
{
    /// <summary>
    /// Gives an approximation of a timespan, given a time unit and a quantity
    /// </summary>
    public static class NotStandardTimeExtensions
    {
        public static string Humanize(this KeyValuePair<TimeUnit, double> input, CultureInfo culture = null)
        {
            const bool isStandard = false;
            var formatter = Configurator.GetFormatter(culture);
            string output = string.Empty;
            double value = input.Value;
            int intValue = (int)value;
            double remainder = value - (double)intValue;
            bool isHalf = remainder == 0.5;
            Frequencies freq;
            if (NotStandardFrequencyInfo.TryGetFrequenciesFromTimeUnit(input.Key, out freq))
	        {
                output += string.Format(
                    ((DefaultFormatter)formatter).FrequencyHumanize(freq, intValue, isStandard),
                    intValue > 1 ?intValue.ToString() : string.Empty);
	        }

            // If there is a remainder (i.e. if there is a half part), then putting it (which is lesser than 1)
            // in FrequencyHumanize will return the string for Half
            output += isHalf ? " " + ((DefaultFormatter)formatter).FrequencyHumanize(freq, (int)remainder, isStandard) : string.Empty;
            return output;
        }
    }
}
