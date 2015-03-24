using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer
{
    /// <summary>
    /// Gives an approximation of a timespan, given a time unit and a quantity
    /// </summary>
    public static class NotStandardTimeExtensions
    {
        public static string Humanize(this KeyValuePair<TimeUnitEnum, double> input)
        {
            string output = string.Empty;
            double value = input.Value;
            output += ((int)value).ToString() + " ";
            int intValue = (int)value;
            double remainder = value - (double)intValue;
            bool isHalf = false;
            if (remainder == 0.5)
            {
                isHalf = true;
            }
            output += input.Key.ToString().ToLower();
            output += intValue > 1 ? "s " : " ";

            output += isHalf ? "and a half" : "";
            return output;
        }
    }
}
