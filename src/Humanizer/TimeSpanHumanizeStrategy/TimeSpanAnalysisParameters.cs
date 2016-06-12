using System;
using System.Globalization;
using Humanizer.Localisation;

namespace Humanizer
{
    public struct TimeSpanAnalysisParameters
    {
        /// <summary>
        /// The duration to be interpreted.
        /// </summary>
        public TimeSpan TimeSpan { get; private set; }

        /// <summary>
        /// The maximum number of time units to return.
        /// </summary>
        public int Precision { get; private set; }

        /// <summary>
        /// Culture to use. If null, current thread's UI culture is used.
        /// </summary>
        public CultureInfo CultureInfo { get; private set; }

        /// <summary>
        /// The maximum unit of time to output.
        /// </summary>
        public TimeUnit MaximumTimeUnit { get; private set; }

        /// <summary>
        /// The minimum unit of time to output.
        /// </summary>
        public TimeUnit MinimumTimeUnit { get; private set; }

        /// <summary>
        /// Defines a point in time which will be used to accurately determine the <see cref="TimeSpan"/>
        /// when working with <see cref="TimeUnit"/>s larger than a week.
        /// </summary>
        public DateTime TimeReference { get; private set; }

        /// <summary>
        /// Controls whether empty time units should be counted towards maximum number of time units.
        /// Leading empty time units never count.
        /// </summary>
        public bool CountEmptyUnits { get; private set; }

        /// <summary>
        /// Denotes whether the <see cref="TimeSpan"/> given happens before or after the <see cref="TimeReference"/> provided.
        /// </summary>
        public bool IsBeforeReference { get; private set; }

        /// <summary>
        /// The separator to use when combining humanized time parts. If null, the default collection 
        /// formatter for the current culture is used.
        /// </summary>
        public string CollectionSeparator { get; private set; }

        /// <summary>
        /// Stores all relevant data when humanizing TimeSpans. 
        /// </summary>
        /// <param name="timeSpan">The duration to be interpreted.</param>
        /// <param name="precision">The maximum number of time units to return.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maximumTimeUnit">The maximum unit of time to output.</param>
        /// <param name="minimumTimeUnit">The minimum unit of time to output.</param>
        /// <param name="timeReference"> Defines a point in time which will be used to accurately determine the <paramref name="timeSpan"/>
        /// when working with <see cref="TimeUnit"/>s larger than a week.</param>
        /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units.
        /// Leading empty time units never count.</param>
        /// <param name="isBeforeReference">Denotes whether the <paramref name="timeSpan"/> given happens before or after the <paramref name="timeReference"/> provided.</param>
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection 
        /// formatter for the current culture is used.</param>
        public TimeSpanAnalysisParameters(TimeSpan timeSpan, int precision = 1, CultureInfo culture = null, TimeUnit maximumTimeUnit = TimeUnit.Year, TimeUnit minimumTimeUnit = TimeUnit.Millisecond, DateTime timeReference = default(DateTime), bool isBeforeReference = false, bool countEmptyUnits = false,  string collectionSeparator = ", ")
        {
            TimeSpan = timeSpan;
            Precision = precision;
            CultureInfo = culture;
            MaximumTimeUnit = maximumTimeUnit;
            MinimumTimeUnit = minimumTimeUnit;
            TimeReference = timeReference;
            CountEmptyUnits = countEmptyUnits;
            IsBeforeReference = isBeforeReference;
            CollectionSeparator = collectionSeparator;
        }
    }
}
