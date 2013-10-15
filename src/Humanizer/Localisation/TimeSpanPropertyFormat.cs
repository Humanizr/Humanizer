using System;

namespace Humanizer.Localisation
{
    /// <summary>
    /// Stores a single mapping of a part of the time span (Day, Hour etc.) to its associated
    /// formatter method for Zero, Single, Multiple.
    /// </summary>
    public class TimeSpanPropertyFormat
    {
        public TimeSpanPropertyFormat(
            Func<TimeSpan, int> propertySelector,
            Func<string> single,
            Func<int, string> multiple)
        {
            PropertySelector = propertySelector;
            Single = single;
            Multiple = multiple;
            Zero = () => null;
        }

        public TimeSpanPropertyFormat(Func<TimeSpan, int> propertySelector, Func<string> zeroFunc)
        {
            PropertySelector = propertySelector;
            Zero = zeroFunc;
        }

        public Func<TimeSpan, int> PropertySelector { get; private set; }
        public Func<string> Single { get; private set; }
        public Func<int, string> Multiple { get; private set; }
        public Func<string> Zero { get; private set; }
    }
}