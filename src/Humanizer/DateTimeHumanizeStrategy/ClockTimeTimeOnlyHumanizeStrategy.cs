#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Humanizer.DateTimeHumanizeStrategy
{
    public class ClockTimeTimeOnlyHumanizeStrategy : ITimeOnlyHumanizeStrategy
    {
        public string Humanize(TimeOnly input, TimeOnly comparisonBase, CultureInfo culture)
        {
            return DateTimeHumanizeAlgorithms.ClockTimeHumanize(input, culture);
        }
    }
}

#endif
