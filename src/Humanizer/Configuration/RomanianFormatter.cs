using System.Diagnostics;

namespace Humanizer.Configuration
{
    class RomanianFormatter : DefaultFormatter
    {
        public override string DateHumanize__hours_ago(int numberOfHours)
        {
            Debug.Assert(numberOfHours > 1);

            if (0 < numberOfHours%100 && numberOfHours%100 < 20)
            {
                return base.DateHumanize__hours_ago(numberOfHours);
            }

            return string.Format("acum {0} de ore", numberOfHours);
        }

        public override string DateHumanize__minutes_ago(int numberOfMinutes)
        {
            Debug.Assert(numberOfMinutes > 1);

            if (0 < numberOfMinutes % 100 && numberOfMinutes % 100 < 20)
            {
                return base.DateHumanize__minutes_ago(numberOfMinutes);
            }

            return string.Format("acum {0} de minute", numberOfMinutes);
        }
    }
}