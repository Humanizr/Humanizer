namespace Humanizer
{
    /// <summary>
    /// Verbosity of time span units
    /// </summary>
    public enum TimeSpanFormat
    {
        /// <summary>
        /// Normal, eg 1 year, 2 months, 2 days, 5 hours, 56 minutes, 12 seconds
        /// </summary>
        Normal,

        /// <summary>
        /// Short, eg 1 yr, 2 mos, 2 days, 5 hrs, 56 mins, 12 secs
        /// </summary>
        Short,

        /// <summary>
        /// Single, eg 1y, 2m, 2d, 5h, 56m, 12s
        /// </summary>
        Single
    }
}
