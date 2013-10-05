namespace Humanizer.Localisation
{
    public class DefaultFormatter : IFormatter
    {
        public virtual string DateHumanize_MultipleDaysAgo(int numberOfDays)
        {
            return Format(ResourceKeys.DateHumanize_MultipleDaysAgo, numberOfDays);
        }

        public virtual string DateHumanize_MultipleHoursAgo(int numberOfHours)
        {
            return Format(ResourceKeys.DateHumanize_MultipleHoursAgo, numberOfHours);
        }

        public virtual string DateHumanize_MultipleMinutesAgo(int numberOfMinutes)
        {
            return Format(ResourceKeys.DateHumanize_MultipleMinutesAgo, numberOfMinutes);
        }

        public virtual string DateHumanize_MultipleMonthsAgo(int numberOfMonths)
        {
            return Format(ResourceKeys.DateHumanize_MultipleMonthsAgo, numberOfMonths);
        }

        public virtual string DateHumanize_MultipleSecondsAgo(int numberOfSeconds)
        {
            return Format(ResourceKeys.DateHumanize_MultipleSecondsAgo, numberOfSeconds);
        }

        public virtual string DateHumanize_MultipleYearsAgo(int numberOfYears)
        {
            return Format(ResourceKeys.DateHumanize_MultipleYearsAgo, numberOfYears);
        }

        public virtual string DateHumanize_SingleMinuteAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_SingleMinuteAgo);
        }

        public virtual string DateHumanize_SingleHourAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_SingleHourAgo);
        }

        public virtual string DateHumanize_NotYet()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_NotYet);
        }

        public virtual string DateHumanize_SingleMonthAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_SingleMonthAgo);
        }

        public virtual string DateHumanize_SingleSecondAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_SingleSecondAgo);
        }

        public virtual string DateHumanize_SingleYearAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_SingleYearAgo);
        }

        public virtual string DateHumanize_SingleDayAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize_SingleDayAgo);
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