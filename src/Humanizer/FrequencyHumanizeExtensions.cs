using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer
{
    public static class FrequencyHumanizeExtensions
    {
        public static string Humanize(this Frequency input)
        {
            string output = "every";
            switch (input.OccurrenceFrequency)
            {
                case FrequencyEnum.Never:
                    output = "never";
                    return output;
                case FrequencyEnum.OnceOff:
                    output = "once on " + ((DateTime)input.ListOfOccurrences[0]).Humanize();
                    return output;
                case FrequencyEnum.Hourly:
                    output += " hour";
                    break;
                case FrequencyEnum.Daily:
                    output += " day";
                    break;
                case FrequencyEnum.Weekly:
                    output += " week";
                    break;
                case FrequencyEnum.Monthly:
                    output += " month";
                    break;
                case FrequencyEnum.Quarterly:
                    output += " quarter";
                    break;
                case FrequencyEnum.Yearly:
                    output += " year";
                    if (input.ListOfOccurrences != null && input.ListOfOccurrences.Count > 0)
                    {
                        output += string.Format(" on {0} of {1}", input.ListOfOccurrences[0].ToDay(), input.ListOfOccurrences[0].ToMonth());
                    }
                    break;
                case FrequencyEnum.NotStandard:
                    output = string.Format(
                        "roughly " + output + " {0}",
                        input.NotStandardFrequencyInfo.Humanize());
                    break;
                default:
                    break;
            }
            if (DateTime.Compare(input.StartDate, DateTime.MinValue) > 0)
            {
                output += string.Format(" from {0} of {1} {2}", input.StartDate.ToDay(), input.StartDate.ToMonth(), input.StartDate.Year);
            }
            if (DateTime.Compare(input.EndDate, DateTime.MinValue) > 0)
            {
                output += string.Format(" till {0} of {1} {2}", input.EndDate.ToDay(), input.EndDate.ToMonth(), input.EndDate.Year);
            }

            return output;
        }
    }
}
