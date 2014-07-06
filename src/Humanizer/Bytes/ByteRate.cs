using System;
using Humanizer.Localisation;

namespace Humanizer.Bytes
{

    /// <summary>
    /// Class to hold a ByteSize and a measurement interval, for the purpose of calculating the rate of transfer
    /// </summary>
    public class ByteRate
    {
        /// <summary>
        /// Quantity of bytes
        /// </summary>
        /// <returns></returns>
        public ByteSize Size { get; private set;}

        /// <summary>
        /// Interval that bytes were transferred in
        /// </summary>
        /// <returns></returns>
        public TimeSpan Interval { get; private set; }

        /// <summary>
        /// Create a ByteRate with given quantity of bytes across an interval
        /// </summary>
        /// <param name="size"></param>
        /// <param name="interval"></param>
        public ByteRate(ByteSize size, TimeSpan interval)
        {
            this.Size = size;
            this.Interval = interval;
        }

        /// <summary>
        /// Calculate rate for the quantity of bytes and interval defined by this instance
        /// </summary>
        /// <param name="timeUnit">Unit of time to calculate rate for (defaults is per second)</param>
        /// <returns></returns>
        public string Humanize(TimeUnit timeUnit = TimeUnit.Second)
        {
            TimeSpan displayInterval;
            string displayUnit;

            if (timeUnit == TimeUnit.Second)
            {
                displayInterval = TimeSpan.FromSeconds(1);
                displayUnit = "s";
            }
            else if (timeUnit == TimeUnit.Minute)
            {
                displayInterval = TimeSpan.FromMinutes(1);
                displayUnit = "min";
            }
            else if (timeUnit == TimeUnit.Hour)
            {
                displayInterval = TimeSpan.FromHours(1);
                displayUnit = "hour";
            }
            else
                throw new NotSupportedException("timeUnit must be Second, Minute, or Hour");

            return (new ByteSize(Size.Bytes / Interval.TotalSeconds * displayInterval.TotalSeconds)).Humanize() + '/' + displayUnit;
        }
    }
}
