using System;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
{
    public class DateHumanizeTests
    {
        static void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow)
        {
            if (expectedString == null)
                throw new ArgumentNullException("expectedString");

            var utcNow = DateTime.UtcNow;
            var localNow = DateTime.Now;

            // feels like the only way to avoid breaking tests because CPU ticks over is to inject the base date
            Assert.Equal(expectedString, utcNow.Add(deltaFromNow).Humanize(utcDate: true, dateToCompareAgainst: utcNow));
            Assert.Equal(expectedString, localNow.Add(deltaFromNow).Humanize(utcDate: false, dateToCompareAgainst: localNow));
        }

        static void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow)
        {
            if (expectedString == null)
                throw new ArgumentNullException("expectedString");

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
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, 1, true)), new TimeSpan(0, 0, 0, 1));
        }

        [Fact]
        public void SecondsFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, 10, true)), 10), new TimeSpan(0, 0, 0, 10));
        }

        [Fact]
        public void OneMinuteFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, 1, true)), new TimeSpan(0, 0, 1, 1));
        }

        [Fact]
        public void AFewMinutesFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, 10, true)), 10), new TimeSpan(0, 0, 10, 0));
        }

        [Fact]
        public void AnHourFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, 1, true)), new TimeSpan(0, 1, 10, 0));
        }

        [Fact]
        public void HoursFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, 10, true)), 10), new TimeSpan(0, 10, 0, 0));
        }

        [Fact]
        public void Tomorrow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, 1, true)), new TimeSpan(1, 10, 0, 0));
        }

        [Fact]
        public void AFewDaysFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, 10, true)), 10), new TimeSpan(10, 1, 0, 0));
        }

        [Fact]
        public void OneMonthFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, 1, true)), new TimeSpan(31, 1, 0, 0));
        }

        [Fact]
        public void AFewMonthsFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, 2, true)), 2), new TimeSpan(62, 1, 0, 0));
        }

        [Fact]
        public void OneYearFromNowIsNotAccureate()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, 1, true)), new TimeSpan(360, 0, 0, 0));
        }

        [Fact]
        public void OneYearFromNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, 1, true)), new TimeSpan(400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsFromNow()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, 2, true)), 2), new TimeSpan(900, 0, 0, 0));
        }

        [Fact]
        public void JustNow()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.Now), new TimeSpan(0, 0, 0, 0));
        }

        [Fact]
        public void OneSecondAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second)), new TimeSpan(0, 0, 0, -1));
        }

        [Fact]
        public void SecondsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, 10)), 10), new TimeSpan(0, 0, 0, -10));
        }

        [Fact]
        public void OneMinuteAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute)), new TimeSpan(0, 0, -1, -10));
        }

        [Fact]
        public void AFewMinutesAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, 10)), 10), new TimeSpan(0, 0, -10, 0));
        }

        [Fact]
        public void AnHourAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour)), new TimeSpan(0, -1, -10, 0));
        }

        [Fact]
        public void HoursAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, 10)), 10), new TimeSpan(0, -10, 0, 0));
        }

        [Fact]
        public void Yesterday()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day)), new TimeSpan(-1, -10, 0, 0));
        }

        [Fact]
        public void AFewDaysAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, 10)), 10), new TimeSpan(-10, -1, 0, 0));
        }

        [Fact]
        public void OneMonthAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month)), new TimeSpan(-31, -1, 0, 0));
        }

        [Fact]
        public void AFewMonthsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, 2)), 2), new TimeSpan(-62, -1, 0, 0));
        }

        [Fact]
        public void OneYearAgoIsNotAccureate()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year)), new TimeSpan(-360, 0, 0, 0));
        }

        [Fact]
        public void OneYearAgo()
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year)), new TimeSpan(-400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsAgo()
        {
            Verify(string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, 2)), 2), new TimeSpan(-900, 0, 0, 0));
        }
    }
}
