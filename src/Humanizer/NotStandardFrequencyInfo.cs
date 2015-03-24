using Humanizer.Localisation;
using System.Collections.Generic;

namespace Humanizer
{
    public class NotStandardFrequencyInfo
    {
        public static readonly Dictionary<TimeUnit, Frequencies> MapFromTimeUnitToFrequencies =
            new Dictionary<TimeUnit, Frequencies>
            {
                {TimeUnit.Millisecond, Frequencies.EveryMillisecond},
                {TimeUnit.Second, Frequencies.EverySecond},
                {TimeUnit.Minute, Frequencies.EveryMinute},
                {TimeUnit.Hour, Frequencies.Hourly},
                {TimeUnit.Day, Frequencies.Daily},
                {TimeUnit.Week, Frequencies.Weekly},
                {TimeUnit.Month, Frequencies.Monthly},
                {TimeUnit.Year, Frequencies.Yearly},
            };

        private KeyValuePair<TimeUnit, double> _frequency;

        public KeyValuePair<TimeUnit, double> Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        public static bool TryGetFrequenciesFromTimeUnit(
            TimeUnit input, out Frequencies output)
        {
            return MapFromTimeUnitToFrequencies.TryGetValue(input, out output);
        }

    }
}
