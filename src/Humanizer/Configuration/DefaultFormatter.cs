namespace Humanizer.Configuration
{
    public class DefaultFormatter : IFormatter
    {
        public virtual string DateHumanize__days_ago(int numberOfDays)
        {
            return string.Format(Resources.GetResource(ResourceKeys.DateHumanize__days_ago), numberOfDays);
        }

        public virtual string DateHumanize__hours_ago(int numberOfHours)
        {
            return string.Format(Resources.GetResource(ResourceKeys.DateHumanize__hours_ago), numberOfHours);
        }

        public virtual string DateHumanize__minutes_ago(int numberOfMinutes)
        {
            return string.Format(Resources.GetResource(ResourceKeys.DateHumanize__minutes_ago), numberOfMinutes);
        }

        public virtual string DateHumanize__months_ago(int numberOfMonths)
        {
            return string.Format(Resources.GetResource(ResourceKeys.DateHumanize__months_ago), numberOfMonths);
        }

        public virtual string DateHumanize__seconds_ago(int numberOfSeconds)
        {
            return string.Format(Resources.GetResource(ResourceKeys.DateHumanize__seconds_ago), numberOfSeconds);
        }

        public virtual string DateHumanize__years_ago(int numberOfYears)
        {
            return string.Format(Resources.GetResource(ResourceKeys.DateHumanize__years_ago), numberOfYears);
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
    }
}