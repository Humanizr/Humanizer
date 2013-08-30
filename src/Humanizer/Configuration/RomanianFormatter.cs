using System.Diagnostics;

namespace Humanizer.Configuration
{
    class RomanianFormatter : DefaultFormatter
    {
        public override string DateHumanize__hours_ago(int number)
        {
            var numberOfHours = number;
            Debug.Assert(numberOfHours > 1);

            if (0 < numberOfHours%100 && numberOfHours%100 < 20)
            {
                return base.DateHumanize__hours_ago(numberOfHours);
            }

            return string.Format("acum {0} de ore", numberOfHours);
        }

        public override string DateHumanize__minutes_ago(int number)
        {
            Debug.Assert(number > 1);

            if (0 < number % 100 && number % 100 < 20)
            {
                return base.DateHumanize__minutes_ago(number);
            }

            return string.Format("acum {0} de minute", number);
        }
    }
}