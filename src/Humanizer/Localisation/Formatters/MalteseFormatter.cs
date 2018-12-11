using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.Formatters
{
    internal class MalteseFormatter : DefaultFormatter
    {
        private const string DualPostfix = "_Dual";

        private static readonly string[] DualResourceKeys = new[]
        {
            "DateHumanize_MultipleDaysAgo", "DateHumanize_MultipleDaysFromNow", "DateHumanize_MultipleHoursAgo", "DateHumanize_MultipleHoursFromNow" ,
            "DateHumanize_MultipleMonthsAgo", "DateHumanize_MultipleMonthsFromNow", "DateHumanize_MultipleYearsAgo", "DateHumanize_MultipleYearsFromNow",
            "TimeSpanHumanize_MultipleDays", "TimeSpanHumanize_MultipleYears", "TimeSpanHumanize_MultipleMonths", "TimeSpanHumanize_MultipleHours",
            "TimeSpanHumanize_MultipleWeeks"
        };

        public MalteseFormatter(string localeCode)
            : base(localeCode)
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number == 2 && DualResourceKeys.Contains(resourceKey))
            {
                return resourceKey + DualPostfix;
            }

            return resourceKey;
        }
    }
}
