using System;

namespace Humanizer.DotiwCalculators
{
    /// <summary>
    /// Precision-based distance of time in works calculator
    /// </summary>
    public class PercisionBasedDotiwCalculator : IDistanceOfTimeInWords
    {
        private const double DEFAULT_PRECISION = 0.75;

        public string Calculate(DateTime date1, DateTime date2, double precision)
        {
            throw new NotImplementedException();
        }

        public string Calculate(DateTime date1, DateTime date2)
        {
            return Calculate(date1, date2, DEFAULT_PRECISION);
        }
    }
}