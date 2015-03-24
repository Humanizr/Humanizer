using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer
{
    public enum Frequencies
    {
        /// <summary>
        /// Never happens
        /// </summary>
        [StringValue("Never")]
        Never,
        /// <summary>
        /// Happens only once
        /// </summary>
        [StringValue("OnceOff")]
        OnceOff,
        /// <summary>
        /// Happens every millisecond
        /// </summary>
        [StringValue("Millisecond")]
        EveryMillisecond,
        /// <summary>
        /// Happens every second
        /// </summary>
        [StringValue("Second")]
        EverySecond,
        /// <summary>
        /// Happens every minute
        /// </summary>
        [StringValue("Minute")]
        EveryMinute,
        /// <summary>
        /// Happens every hour
        /// </summary>
        [StringValue("Hour")]
        Hourly,
        /// <summary>
        /// Happens every day
        /// </summary>
        [StringValue("Day")]
        Daily,
        /// <summary>
        /// Happens every week
        /// </summary>
        [StringValue("Week")]
        Weekly,
        /// <summary>
        /// Happens every month
        /// </summary>
        [StringValue("Month")]
        Monthly,
        /// <summary>
        /// Happens every querter
        /// </summary>
        [StringValue("Quarter")]
        Quarterly,
        /// <summary>
        /// Happens every year
        /// </summary>
        [StringValue("Year")]
        Yearly,
        /// <summary>
        /// Happens without a standard frequency. (e.g. every 120 days and a half)
        /// </summary>
        [StringValue("NotStandard")]
        NotStandard
    }
}
