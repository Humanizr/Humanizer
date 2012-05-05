using System;
using Humanize.Extensions;
using Xunit;

namespace Humanizer.Tests.Extensions
{
    public class DateExtensionsTests
    {
        public void Verify(string expectedString, TimeSpan deltaFromNow)
        {
            Assert.Contains(expectedString, DateTime.UtcNow.Add(deltaFromNow).Humanize());
            Assert.Contains(expectedString, DateTime.Now.Add(deltaFromNow).Humanize(false));
        }

        [Fact]
        public void FutureDates()
        {
            Verify(DateExtensions.FutureDate, new TimeSpan(0, 0, 1, 0));
        }

        [Fact]
        public void JustNow()
        {
            Verify(DateExtensions.OneSecondAgo, new TimeSpan(0, 0, 0, -1));
        }

        [Fact]
        public void SecondsAgo()
        {
            Verify(DateExtensions.SecondsAgo, new TimeSpan(0, 0, 0, -10));
        }

        [Fact]
        public void OneMinuteAgo()
        {
            Verify(DateExtensions.OneMinuteAgo, new TimeSpan(0, 0, -1, 0));
        }

        [Fact]
        public void AFewMinutesAgo()
        {
            Verify(DateExtensions.MinutesAgo, new TimeSpan(0, 0, -10, 0));
        }

        [Fact]
        public void AnHourAgo()
        {
            Verify(DateExtensions.OneHourAgo, new TimeSpan(0, -1, -10, 0));
        }

        [Fact]
        public void HoursAgo()
        {
            Verify(DateExtensions.HoursAgo, new TimeSpan(0, -10, 0, 0));
        }

        [Fact]
        public void Yesterday()
        {
            Verify(DateExtensions.Yesterday, new TimeSpan(-1, -10, 0, 0));
        }

        [Fact]
        public void AFewDaysAgo()
        {
            Verify(DateExtensions.DaysAgo, new TimeSpan(-10, 0, 0, 0));
        }

        [Fact]
        public void OneMonthAgo()
        {
            Verify(DateExtensions.OneMonthAgo, new TimeSpan(-30, 0, 0, 0));
        }

        [Fact]
        public void AFewMonthsAgo()
        {
            Verify(DateExtensions.MonthsAgo, new TimeSpan(-60, 0, 0, 0));
        }

        [Fact]
        public void OneYearAgoIsNotAccureate()
        {
            Verify(DateExtensions.OneYearAgo, new TimeSpan(-360, 0, 0, 0));
        }

        [Fact]
        public void OneYearAgo()
        {
            Verify(DateExtensions.OneYearAgo, new TimeSpan(-400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsAgo()
        {
            Verify(DateExtensions.YearsAgo, new TimeSpan(-900, 0, 0, 0));
        }
    }
}