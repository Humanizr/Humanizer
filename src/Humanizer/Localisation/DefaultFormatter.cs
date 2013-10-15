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

        public virtual string TimeSpanHumanize_MultipleWeeks(int weeks)
        {
            return Format(ResourceKeys.TimeSpanHumanize_MultipleWeeks, weeks);
        }

        public virtual string TimeSpanHumanize_SingleWeek()
        {
            return Format(ResourceKeys.TimeSpanHumanize_SingleWeek);
        }

        public virtual string TimeSpanHumanize_MultipleDays(int days)
        {
            return Format(ResourceKeys.TimeSpanHumanize_MultipleDays, days);
        }

        public virtual string TimeSpanHumanize_SingleDay()
        {
            return Format(ResourceKeys.TimeSpanHumanize_SingleDay);
        }

        public virtual string TimeSpanHumanize_MultipleHours(int hours)
        {
            return Format(ResourceKeys.TimeSpanHumanize_MultipleHours, hours);
        }

        public virtual string TimeSpanHumanize_SingleHour()
        {
            return Format(ResourceKeys.TimeSpanHumanize_SingleHour);
        }

        public virtual string TimeSpanHumanize_MultipleMinutes(int minutes)
        {
            return Format(ResourceKeys.TimeSpanHumanize_MultipleMinutes, minutes);
        }

        public virtual string TimeSpanHumanize_SingleMinute()
        {
            return Format(ResourceKeys.TimeSpanHumanize_SingleMinute);
        }

        public virtual string TimeSpanHumanize_MultipleSeconds(int seconds)
        {
            return Format(ResourceKeys.TimeSpanHumanize_MultipleSeconds, seconds);
        }

        public virtual string TimeSpanHumanize_SingleSecond()
        {
            return Format(ResourceKeys.TimeSpanHumanize_SingleSecond);
        }

        public virtual string TimeSpanHumanize_MultipleMilliseconds(int milliSeconds)
        {
            return Format(ResourceKeys.TimeSpanHumanize_MultipleMilliseconds, milliSeconds);
        }

        public virtual string TimeSpanHumanize_SingleMillisecond()
        {
            return Format(ResourceKeys.TimeSpanHumanize_SingleMillisecond);
        }

        public virtual string TimeSpanHumanize_Zero()
        {
            return Format(ResourceKeys.TimeSpanHumanize_Zero);
        }

        protected virtual string Format(string resourceKey)
        {
            return Resources.GetResource(GetResourceKey(resourceKey));
        }

        protected virtual string Format(string resourceKey, int number)
        {
            return string.Format(Resources.GetResource(GetResourceKey(resourceKey, number)), number);
        }

        protected virtual string GetResourceKey(string resourceKey, int number)
        {
            return resourceKey;
        }

        protected virtual string GetResourceKey(string resourceKey)
        {
            return resourceKey;
        }
    }
}