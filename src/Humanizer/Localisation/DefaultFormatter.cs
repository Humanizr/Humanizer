namespace Humanizer.Localisation
{
    public class DefaultFormatter : IFormatter
    {
        public virtual string DateHumanize__days_ago(int numberOfDays)
        {
            return Format(ResourceKeys.DateHumanize__days_ago, numberOfDays);
        }

        public virtual string DateHumanize__hours_ago(int numberOfHours)
        {
            return Format(ResourceKeys.DateHumanize__hours_ago, numberOfHours);
        }

        public virtual string DateHumanize__minutes_ago(int numberOfMinutes)
        {
            return Format(ResourceKeys.DateHumanize__minutes_ago, numberOfMinutes);
        }

        public virtual string DateHumanize__months_ago(int numberOfMonths)
        {
            return Format(ResourceKeys.DateHumanize__months_ago, numberOfMonths);
        }

        public virtual string DateHumanize__seconds_ago(int numberOfSeconds)
        {
            return Format(ResourceKeys.DateHumanize__seconds_ago, numberOfSeconds);
        }

        public virtual string DateHumanize__years_ago(int numberOfYears)
        {
            return Format(ResourceKeys.DateHumanize__years_ago, numberOfYears);
        }

        public virtual string DateHumanize_a_minute_ago()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_a_minute_ago);
        }

        public virtual string DateHumanize_an_hour_ago()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_an_hour_ago);
        }

        public virtual string DateHumanize_not_yet()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_not_yet);
        }

        public virtual string DateHumanize_one_month_ago()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_one_month_ago);
        }

        public virtual string DateHumanize_one_second_ago()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_one_second_ago);
        }

        public virtual string DateHumanize_one_year_ago()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_one_year_ago);
        }

        public virtual string DateHumanize_yesterday()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_yesterday);
        }

        protected string Format(string resourceKey, int number)
        {
            return string.Format(Resources.GetResource(GetResourceKey(resourceKey, number)), number);
        }

        protected virtual string GetResourceKey(string resourceKey, int number)
        {
            return resourceKey;
        }
    }
}