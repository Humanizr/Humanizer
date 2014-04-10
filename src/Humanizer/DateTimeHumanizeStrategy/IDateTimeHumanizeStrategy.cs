using System;

namespace Humanizer.DateTimeHumanizeStrategy
{
    public interface IDateTimeHumanizeStrategy
    {
        string Humanize(DateTime input, DateTime comparisonBase);
    }
}