using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer
{
    public enum FrequencyEnum
    {
        /// <summary>
        /// Never happens
        /// </summary>
        Never,
        /// <summary>
        /// Happens only once
        /// </summary>
        OnceOff,
        EveryMinute,
        /// <summary>
        /// Happens every hour
        /// </summary>
        Hourly,
        /// <summary>
        /// Happens every day
        /// </summary>
        Daily,
        /// <summary>
        /// Happens every week
        /// </summary>
        Weekly,
        /// <summary>
        /// Happens every month
        /// </summary>
        Monthly,
        /// <summary>
        /// Happens every querter
        /// </summary>
        Quarterly,
        /// <summary>
        /// Happens every year
        /// </summary>
        Yearly,
        /// <summary>
        /// Happens without a standard frequency. (e.g. every 120 days and a half)
        /// </summary>
        NotStandard
    }
}
