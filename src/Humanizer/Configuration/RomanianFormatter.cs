namespace Humanizer.Configuration
{
    class RomanianFormatter : DefaultFormatter
    {
        private const string Above20PostFix = "_above_20";

        private string FormatWithAbove20Rule(string resourceKey, int number)
        {
            if (0 < number%100 && number%100 < 20)
            {
                var format = Resources.GetResource(resourceKey);
                return string.Format(format, number);
            }

            var above20 = resourceKey + Above20PostFix;
            var above20Format = Resources.GetResource(above20);

            return string.Format(above20Format, number);
        }

        public override string DateHumanize__years_ago(int numberOfYears)
        {
            return FormatWithAbove20Rule(ResourceKeys.DateHumanize__years_ago, numberOfYears);
        }

        public override string DateHumanize__days_ago(int numberOfDays)
        {
            return FormatWithAbove20Rule(ResourceKeys.DateHumanize__days_ago, numberOfDays);
        }

        public override string DateHumanize__hours_ago(int numberOfHours)
        {
            return FormatWithAbove20Rule(ResourceKeys.DateHumanize__hours_ago, numberOfHours);
        }

        public override string DateHumanize__minutes_ago(int numberOfMinutes)
        {
            return FormatWithAbove20Rule(ResourceKeys.DateHumanize__minutes_ago, numberOfMinutes);
        }

        public override string DateHumanize__seconds_ago(int numberOfSeconds)
        {
            return FormatWithAbove20Rule(ResourceKeys.DateHumanize__seconds_ago, numberOfSeconds);
        }
    }
}