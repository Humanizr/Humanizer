using System;
using Humanizer.Configuration;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Extensions
{
    public class DateHumanizeTests
    {
        void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow)
        {
            Assert.Equal(expectedString, DateTime.UtcNow.Add(deltaFromNow).Humanize());
            Assert.Equal(expectedString, DateTime.Now.Add(deltaFromNow).Humanize(false));
        }

        void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow)
        {
            var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
            var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

            Assert.Equal(expectedString, utcNow.Add(deltaFromNow).Humanize(dateToCompareAgainst: utcNow));
            Assert.Equal(expectedString, now.Add(deltaFromNow).Humanize(false, now));
        }

        public void Verify(string expectedString, TimeSpan deltaFromNow)
        {
            VerifyWithCurrentDate(expectedString, deltaFromNow);
            VerifyWithDateInjection(expectedString, deltaFromNow);
        }

        [Fact]
        public void FutureDates()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize_not_yet), new TimeSpan(0, 0, 1, 0));
        }

        [Fact]
        public void JustNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize_one_second_ago), new TimeSpan(0, 0, 0, -1));
        }

        [Fact]
        public void SecondsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize__seconds_ago), 10), new TimeSpan(0, 0, 0, -10));
        }

        [Fact]
        public void OneMinuteAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize_a_minute_ago), new TimeSpan(0, 0, -1, 0));
        }

        [Fact]
        public void AFewMinutesAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize__minutes_ago), 10), new TimeSpan(0, 0, -10, 0));
        }

        [Fact]
        public void AnHourAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize_an_hour_ago), new TimeSpan(0, -1, -10, 0));
        }

        [Fact]
        public void HoursAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize__hours_ago), 10), new TimeSpan(0, -10, 0, 0));
        }

        [Fact]
        public void Yesterday()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize_yesterday), new TimeSpan(-1, -10, 0, 0));
        }

        [Fact]
        public void AFewDaysAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize__days_ago), 10), new TimeSpan(-10, 0, 0, 0));
        }

        [Fact]
        public void OneMonthAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize_one_month_ago), new TimeSpan(-30, 0, 0, 0));
        }

        [Fact]
        public void AFewMonthsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize__months_ago), 2), new TimeSpan(-60, 0, 0, 0));
        }

        [Fact]
        public void OneYearAgoIsNotAccureate()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize_one_year_ago), new TimeSpan(-360, 0, 0, 0));
        }

        [Fact]
        public void OneYearAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize_one_year_ago), new TimeSpan(-400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize__years_ago), 2), new TimeSpan(-900, 0, 0, 0));
        }
    }
}