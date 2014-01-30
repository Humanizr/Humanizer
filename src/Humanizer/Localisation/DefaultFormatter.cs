namespace Humanizer.Localisation
{
    public class DefaultFormatter : IFormatter
    {
        public virtual string DateHumanize_MultipleDaysAgo(int numberOfDays)
        {
            return Format(ResourceKeys.DateHumanize.MultipleDaysAgo, numberOfDays);
        }

        public virtual string DateHumanize_MultipleHoursAgo(int numberOfHours)
        {
            return Format(ResourceKeys.DateHumanize.MultipleHoursAgo, numberOfHours);
        }

        public virtual string DateHumanize_MultipleMinutesAgo(int numberOfMinutes)
        {
            return Format(ResourceKeys.DateHumanize.MultipleMinutesAgo, numberOfMinutes);
        }

        public virtual string DateHumanize_MultipleMonthsAgo(int numberOfMonths)
        {
            return Format(ResourceKeys.DateHumanize.MultipleMonthsAgo, numberOfMonths);
        }

        public virtual string DateHumanize_MultipleSecondsAgo(int numberOfSeconds)
        {
            return Format(ResourceKeys.DateHumanize.MultipleSecondsAgo, numberOfSeconds);
        }

        public virtual string DateHumanize_MultipleYearsAgo(int numberOfYears)
        {
            return Format(ResourceKeys.DateHumanize.MultipleYearsAgo, numberOfYears);
        }

        public virtual string DateHumanize_SingleMinuteAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleMinuteAgo);
        }

        public virtual string DateHumanize_SingleHourAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleHourAgo);
        }

        public virtual string DateHumanize_SingleMonthAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleMonthAgo);
        }

        public virtual string DateHumanize_SingleSecondAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleSecondAgo);
        }

        public virtual string DateHumanize_SingleYearAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleYearAgo);
        }

        public virtual string DateHumanize_SingleDayAgo()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleDayAgo);
        }

        public virtual string DateHumanize_MultipleDaysFromNow(int numberOfDays)
        {
            return Format(ResourceKeys.DateHumanize.MultipleDaysFromNow, numberOfDays);
        }

        public virtual string DateHumanize_MultipleHoursFromNow(int numberOfHours)
        {
            return Format(ResourceKeys.DateHumanize.MultipleHoursFromNow, numberOfHours);
        }

        public virtual string DateHumanize_MultipleMinutesFromNow(int numberOfMinutes)
        {
            return Format(ResourceKeys.DateHumanize.MultipleMinutesFromNow, numberOfMinutes);
        }

        public virtual string DateHumanize_MultipleMonthsFromNow(int numberOfMonths)
        {
            return Format(ResourceKeys.DateHumanize.MultipleMonthsFromNow, numberOfMonths);
        }

        public virtual string DateHumanize_MultipleSecondsFromNow(int numberOfSeconds)
        {
            return Format(ResourceKeys.DateHumanize.MultipleSecondsFromNow, numberOfSeconds);
        }

        public virtual string DateHumanize_MultipleYearsFromNow(int numberOfYears)
        {
            return Format(ResourceKeys.DateHumanize.MultipleYearsFromNow, numberOfYears);
        }

        public virtual string DateHumanize_SingleMinuteFromNow()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleMinuteFromNow);
        }

        public virtual string DateHumanize_SingleHourFromNow()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleHourFromNow);
        }

        public virtual string DateHumanize_Now()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.Now);
        }

        public virtual string DateHumanize_SingleMonthFromNow()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleMonthFromNow);
        }

        public virtual string DateHumanize_SingleSecondFromNow()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleSecondFromNow);
        }

        public virtual string DateHumanize_SingleYearFromNow()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleYearFromNow);
        }

        public virtual string DateHumanize_SingleDayFromNow()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.SingleDayFromNow);
        }

        public virtual string TimeSpanHumanize_MultipleWeeks(int weeks)
        {
            return Format(ResourceKeys.TimeSpanHumanize.MultipleWeeks, weeks);
        }

        public virtual string TimeSpanHumanize_SingleWeek()
        {
            return Format(ResourceKeys.TimeSpanHumanize.SingleWeek);
        }

        public virtual string TimeSpanHumanize_MultipleDays(int days)
        {
            return Format(ResourceKeys.TimeSpanHumanize.MultipleDays, days);
        }

        public virtual string TimeSpanHumanize_SingleDay()
        {
            return Format(ResourceKeys.TimeSpanHumanize.SingleDay);
        }

        public virtual string TimeSpanHumanize_MultipleHours(int hours)
        {
            return Format(ResourceKeys.TimeSpanHumanize.MultipleHours, hours);
        }

        public virtual string TimeSpanHumanize_SingleHour()
        {
            return Format(ResourceKeys.TimeSpanHumanize.SingleHour);
        }

        public virtual string TimeSpanHumanize_MultipleMinutes(int minutes)
        {
            return Format(ResourceKeys.TimeSpanHumanize.MultipleMinutes, minutes);
        }

        public virtual string TimeSpanHumanize_SingleMinute()
        {
            return Format(ResourceKeys.TimeSpanHumanize.SingleMinute);
        }

        public virtual string TimeSpanHumanize_MultipleSeconds(int seconds)
        {
            return Format(ResourceKeys.TimeSpanHumanize.MultipleSeconds, seconds);
        }

        public virtual string TimeSpanHumanize_SingleSecond()
        {
            return Format(ResourceKeys.TimeSpanHumanize.SingleSecond);
        }

        public virtual string TimeSpanHumanize_MultipleMilliseconds(int milliSeconds)
        {
            return Format(ResourceKeys.TimeSpanHumanize.MultipleMilliseconds, milliSeconds);
        }

        public virtual string TimeSpanHumanize_SingleMillisecond()
        {
            return Format(ResourceKeys.TimeSpanHumanize.SingleMillisecond);
        }

        public virtual string TimeSpanHumanize_Zero()
        {
            return Format(ResourceKeys.TimeSpanHumanize.Zero);
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