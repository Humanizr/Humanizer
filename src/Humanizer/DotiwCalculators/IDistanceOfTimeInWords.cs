using System;

namespace Humanizer.DotiwCalculators
{
    /// <summary>
    /// Distance of time in works calculator
    /// </summary>
    public interface IDistanceOfTimeInWords
    {
        string Calculate(DateTime date1, DateTime date2);
    }
}