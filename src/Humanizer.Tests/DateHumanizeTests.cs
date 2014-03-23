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
            var oneSecondFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, 1, true));
            Verify(oneSecondFromNow, new TimeSpan(0, 0, 0, 1));
        }

        [Fact]
        public void SecondsFromNow()
        {
            var secsFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, 10, true)), 10);
            Verify(secsFromNow, new TimeSpan(0, 0, 0, 10));
        }

        [Fact]
        public void OneMinuteFromNow()
        {
            var oneMinFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, 1, true));
            Verify(oneMinFromNow, new TimeSpan(0, 0, 1, 1));
        }

        [Fact]
        public void AFewMinutesFromNow()
        {
            var minsFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, 10, true)), 10);
            Verify(minsFromNow, new TimeSpan(0, 0, 10, 0));
        }

        [Fact]
        public void AnHourFromNow()
        {
            var anHourFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, 1, true));
            Verify(anHourFromNow, new TimeSpan(0, 1, 10, 0));
        }

        [Fact]
        public void HoursFromNow()
        {
            var hoursFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, 10, true)), 10);
            Verify(hoursFromNow, new TimeSpan(0, 10, 0, 0));
        }

        [Fact]
        public void Tomorrow()
        {
            var tomorrow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, 1, true));
            Verify(tomorrow, new TimeSpan(1, 10, 0, 0));
        }

        [Fact]
        public void AFewDaysFromNow()
        {
            var daysFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, 10, true)), 10);
            Verify(daysFromNow, new TimeSpan(10, 1, 0, 0));
        }

        [Fact]
        public void OneMonthFromNow()
        {
            var oneMonthFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, 1, true));
            Verify(oneMonthFromNow, new TimeSpan(31, 1, 0, 0));
        }

        [Fact]
        public void AFewMonthsFromNow()
        {
            var monthsFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, 2, true)), 2);
            Verify(monthsFromNow, new TimeSpan(62, 1, 0, 0));
        }

        [Fact]
        public void OneYearFromNowIsNotAccureate()
        {
            var aYearFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, 1, true));
            Verify(aYearFromNow, new TimeSpan(360, 0, 0, 0));
        }

        [Fact]
        public void OneYearFromNow()
        {
            var aYearFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, 1, true));
            Verify(aYearFromNow, new TimeSpan(400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsFromNow()
        {
            var fewYearsFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, 2, true)), 2);
            Verify(fewYearsFromNow, new TimeSpan(900, 0, 0, 0));
        }

        [Fact]
        public void JustNow()
        {
            var now = Resources.GetResource(ResourceKeys.DateHumanize.Now);
            Verify(now, new TimeSpan(0, 0, 0, 0));
        }

        [Fact]
        public void OneSecondAgo()
        {
            var aSecAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second));
            Verify(aSecAgo, new TimeSpan(0, 0, 0, -1));
        }

        [Fact]
        public void SecondsAgo()
        {
            var secondsAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, 10)), 10);
            Verify(secondsAgo, new TimeSpan(0, 0, 0, -10));
        }

        [Fact]
        public void OneMinuteAgo()
        {
            var aMinuteAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute));
            Verify(aMinuteAgo, new TimeSpan(0, 0, -1, -10));
        }

        [Fact]
        public void AFewMinutesAgo()
        {
            var minsAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, 10)), 10);
            Verify(minsAgo, new TimeSpan(0, 0, -10, 0));
        }

        [Fact]
        public void AnHourAgo()
        {
            var anHourAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour));
            Verify(anHourAgo, new TimeSpan(0, -1, -10, 0));
        }

        [Fact]
        public void HoursAgo()
        {
            var hoursAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, 10)), 10);
            Verify(hoursAgo, new TimeSpan(0, -10, 0, 0));
        }

        [Fact]
        public void Yesterday()
        {
            var yesterday = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day));
            Verify(yesterday, new TimeSpan(-1, -10, 0, 0));
        }

        [Fact]
        public void AFewDaysAgo()
        {
            var fewDaysAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, 10)), 10);
            Verify(fewDaysAgo, new TimeSpan(-10, -1, 0, 0));
        }

        [Fact]
        public void OneMonthAgo()
        {
            var aMonthAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month));
            Verify(aMonthAgo, new TimeSpan(-31, -1, 0, 0));
        }

        [Fact]
        public void AFewMonthsAgo()
        {
            var monthsAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, 2)), 2);
            Verify(monthsAgo, new TimeSpan(-62, -1, 0, 0));
        }

        [Fact]
        public void OneYearAgoIsNotAccureate()
        {
            var aYearAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year));
            Verify(aYearAgo, new TimeSpan(-360, 0, 0, 0));
        }

        [Fact]
        public void OneYearAgo()
        {
            var aYearAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year));
            Verify(aYearAgo, new TimeSpan(-400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsAgo()
        {
            var yearsAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, 2)), 2);
            Verify(yearsAgo, new TimeSpan(-900, 0, 0, 0));
        }
    }
}
