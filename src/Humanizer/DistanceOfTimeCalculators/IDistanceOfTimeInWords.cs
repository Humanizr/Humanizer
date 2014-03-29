using System;

namespace Humanizer.DistanceOfTimeCalculators
{
    /// <summary>
    /// Distance of time in works calculator
    /// </summary>
    public interface IDistanceOfTimeInWords
    {
        string Calculate(DateTime date1, DateTime date2);
    }
}