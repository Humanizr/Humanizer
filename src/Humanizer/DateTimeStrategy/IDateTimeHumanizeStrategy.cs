using System;

namespace Humanizer.DateTimeStrategy
{
    public interface IDateTimeHumanizeStrategy
    {
        string Humanize(DateTime input, DateTime comparisonBase);
    }
}