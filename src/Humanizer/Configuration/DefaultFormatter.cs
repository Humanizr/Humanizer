using Humanizer.Properties;

namespace Humanizer.Configuration
{
    class DefaultFormatter : IFormatter
    {
        public virtual string DateHumanize__days_ago(int number)
        {
            return string.Format(Resources.DateHumanize__days_ago, number);
        }

        public virtual string DateHumanize__hours_ago(int number)
        {
            return string.Format(Resources.DateHumanize__hours_ago, number);
        }

        public virtual string DateHumanize__minutes_ago(int number)
        {
            return string.Format(Resources.DateHumanize__minutes_ago, number);
        }

        public virtual string DateHumanize__months_ago(int number)
        {
            return string.Format(Resources.DateHumanize__months_ago, number);
        }

        public virtual string DateHumanize__seconds_ago(int number)
        {
            return string.Format(Resources.DateHumanize__seconds_ago, number);
        }

        public virtual string DateHumanize__years_ago(int number)
        {
            return string.Format(Resources.DateHumanize__years_ago, number);
        }

        public virtual string DateHumanize_a_minute_ago()
        {
            return Resources.DateHumanize_a_minute_ago;
        }

        public virtual string DateHumanize_an_hour_ago()
        {
            return Resources.DateHumanize_an_hour_ago;
        }

        public virtual string DateHumanize_not_yet()
        {
            return Resources.DateHumanize_not_yet;
        }

        public virtual string DateHumanize_one_month_ago()
        {
            return Resources.DateHumanize_one_month_ago;
        }

        public virtual string DateHumanize_one_second_ago()
        {
            return Resources.DateHumanize_one_second_ago;
        }

        public virtual string DateHumanize_one_year_ago()
        {
            return Resources.DateHumanize_one_year_ago;
        }

        public virtual string DateHumanize_yesterday()
        {
            return Resources.DateHumanize_yesterday;
        }
    }
}