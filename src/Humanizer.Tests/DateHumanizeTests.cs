using System;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
{
    public class DateHumanizeTests
    {
        static void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow)
        {
            var utcNow = DateTime.UtcNow;
            var localNow = DateTime.Now;

            // feels like the only way to avoid breaking tests because CPU ticks over is to inject the base date
            Assert.Equal(expectedString, utcNow.Add(deltaFromNow).Humanize(utcDate: true, dateToCompareAgainst: utcNow));
            Assert.Equal(expectedString, localNow.Add(deltaFromNow).Humanize(utcDate: false, dateToCompareAgainst: localNow));
        }

        static void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow)
        {
            var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
            var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

            Assert.Equal(expectedString, utcNow.Add(deltaFromNow).Humanize(dateToCompareAgainst: utcNow));
            Assert.Equal(expectedString, now.Add(deltaFromNow).Humanize(false, now));
        }

        static void Verify(string expectedString, TimeSpan deltaFromNow)
        {
            VerifyWithCurrentDate(expectedString, deltaFromNow);
            VerifyWithDateInjection(expectedString, deltaFromNow);
        }

        [Fact]
        public void OneSecondFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleSecondFromNow), new TimeSpan(0, 0, 0, 1));
        }

        [Fact]
        public void SecondsFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleSecondsFromNow), 10), new TimeSpan(0, 0, 0, 10));
        }

        [Fact]
        public void OneMinuteFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleMinuteFromNow), new TimeSpan(0, 0, 1, 1));
        }

        [Fact]
        public void AFewMinutesFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleMinutesFromNow), 10), new TimeSpan(0, 0, 10, 0));
        }

        [Fact]
        public void AnHourFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleHourFromNow), new TimeSpan(0, 1, 10, 0));
        }

        [Fact]
        public void HoursFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleHoursFromNow), 10), new TimeSpan(0, 10, 0, 0));
        }

        [Fact]
        public void Tomorrow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleDayFromNow), new TimeSpan(1, 10, 0, 0));
        }

        [Fact]
        public void AFewDaysFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleDaysFromNow), 10), new TimeSpan(10, 1, 0, 0));
        }

        [Fact]
        public void OneMonthFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleMonthFromNow), new TimeSpan(31, 1, 0, 0));
        }

        [Fact]
        public void AFewMonthsFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleMonthsFromNow), 2), new TimeSpan(62, 1, 0, 0));
        }

        [Fact]
        public void OneYearFromNowIsNotAccureate()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleYearFromNow), new TimeSpan(360, 0, 0, 0));
        }

        [Fact]
        public void OneYearFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleYearFromNow), new TimeSpan(400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleYearsFromNow), 2), new TimeSpan(900, 0, 0, 0));
        }

        [Fact]
        public void JustNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.Now), new TimeSpan(0, 0, 0, 0));
        }

        [Fact]
        public void OneSecondAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleSecondAgo), new TimeSpan(0, 0, 0, -1));
        }

        [Fact]
        public void SecondsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleSecondsAgo), 10), new TimeSpan(0, 0, 0, -10));
        }

        [Fact]
        public void OneMinuteAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleMinuteAgo), new TimeSpan(0, 0, -1, -10));
        }

        [Fact]
        public void AFewMinutesAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleMinutesAgo), 10), new TimeSpan(0, 0, -10, 0));
        }

        [Fact]
        public void AnHourAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleHourAgo), new TimeSpan(0, -1, -10, 0));
        }

        [Fact]
        public void HoursAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleHoursAgo), 10), new TimeSpan(0, -10, 0, 0));
        }

        [Fact]
        public void Yesterday()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleDayAgo), new TimeSpan(-1, -10, 0, 0));
        }

        [Fact]
        public void AFewDaysAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleDaysAgo), 10), new TimeSpan(-10, -1, 0, 0));
        }

        [Fact]
        public void OneMonthAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleMonthAgo), new TimeSpan(-31, -1, 0, 0));
        }

        [Fact]
        public void AFewMonthsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleMonthsAgo), 2), new TimeSpan(-62, -1, 0, 0));
        }

        [Fact]
        public void OneYearAgoIsNotAccureate()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleYearAgo), new TimeSpan(-360, 0, 0, 0));
        }

        [Fact]
        public void OneYearAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleYearAgo), new TimeSpan(-400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleYearsAgo), 2), new TimeSpan(-900, 0, 0, 0));
        }
    }
}