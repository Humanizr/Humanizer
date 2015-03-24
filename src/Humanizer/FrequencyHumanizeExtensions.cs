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
    public static class FrequencyHumanizeExtensions
    {
        public static string Humanize(this Frequency input, CultureInfo culture = null)
        {
            var dateTimeFormatInfo = new DateTimeFormatInfo();
            var formatter = Configurator.GetFormatter(culture);
            string output = string.Empty;
            switch (input.OccurrenceFrequency)
            {
                case Frequencies.Never:
                case Frequencies.Hourly:
                case Frequencies.Daily:
                case Frequencies.Weekly:
                case Frequencies.Monthly:
                case Frequencies.Quarterly:
                    output = ((DefaultFormatter)formatter).FrequencyHumanize(input.OccurrenceFrequency);
                    break;
                case Frequencies.OnceOff:
                    return string.Format(
                        ((DefaultFormatter)formatter).FrequencyHumanize(input.OccurrenceFrequency),
                        ((DateTime)input.ListOfOccurrences[0]).Humanize());
                case Frequencies.Yearly:
                    if (input.ListOfOccurrences != null && input.ListOfOccurrences.Count > 0)
                    {
                        output = ((DefaultFormatter)formatter).FrequencyHumanize(input.OccurrenceFrequency) + " ";
                        output += string.Format(
                            ((DefaultFormatter)formatter).FrequencyHumanize(input.OccurrenceFrequency, 1, true, false, false, true),
                            input.ListOfOccurrences[0].Day.Ordinalize(),
                            dateTimeFormatInfo.GetMonthName(input.ListOfOccurrences[0].Month));
                    }
                    break;
                case Frequencies.NotStandard:
                    output = input.NotStandardFrequencyInfo.Frequency.Humanize();
                    break;
                default:
                    break;
            }
            if (DateTime.Compare(input.StartDate, DateTime.MinValue) > 0)
            {
                output += " ";
                output += string.Format(
                     ((DefaultFormatter)formatter).FrequencyHumanize(input.OccurrenceFrequency, 1, true, true),
                     input.StartDate.Day.Ordinalize(),
                     dateTimeFormatInfo.GetMonthName(input.ListOfOccurrences[0].Month),
                     input.StartDate.Year);
            }
            if (DateTime.Compare(input.EndDate, DateTime.MinValue) > 0)
            {
                output += " ";
                output += string.Format(
                    ((DefaultFormatter)formatter).FrequencyHumanize(input.OccurrenceFrequency, 1, true, false, true),
                    input.EndDate.Day.Ordinalize(),
                    dateTimeFormatInfo.GetMonthName(input.ListOfOccurrences[0].Month),
                    input.EndDate.Year);
            }

            return output;
        }
    }
}
