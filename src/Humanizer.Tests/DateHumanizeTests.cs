﻿using System;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
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

        void Verify(string expectedString, TimeSpan deltaFromNow)
        {
            VerifyWithCurrentDate(expectedString, deltaFromNow);
            VerifyWithDateInjection(expectedString, deltaFromNow);
        }

        [Fact]
        public void FutureDates()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.NotYet), new TimeSpan(0, 0, 1, 0));
        }

        [Fact]
        public void JustNow()
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
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleMinuteAgo), new TimeSpan(0, 0, -1, 0));
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
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleDaysAgo), 10), new TimeSpan(-10, 0, 0, 0));
        }

        [Fact]
        public void OneMonthAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.SingleMonthAgo), new TimeSpan(-30, 0, 0, 0));
        }

        [Fact]
        public void AFewMonthsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.MultipleMonthsAgo), 2), new TimeSpan(-60, 0, 0, 0));
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