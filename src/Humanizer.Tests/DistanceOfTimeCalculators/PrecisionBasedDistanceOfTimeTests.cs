using System;
using Humanizer.Configuration;
using Humanizer.DistanceOfTimeCalculators;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.DistanceOfTimeCalculators
{
    public class PrecisionBasedDistanceOfTimeTests : DateHumanizeTestsBase
    {
        public PrecisionBasedDistanceOfTimeTests()
        {
            Configurator.DistanceOfTimeInWords = new PrecisionBasedDistanceOfTime();
        }

        [Fact]
        public void OneSecondFromNow()
        {
            var oneSecondFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, TimeUnitTense.Future));
            Verify(oneSecondFromNow, new TimeSpan(0, 0, 0, 1));
        }

        [Fact]
        public void SecondsFromNow()
        {
            var secsFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, TimeUnitTense.Future, 10)), 10);
            Verify(secsFromNow, new TimeSpan(0, 0, 0, 10));
        }

        [Fact]
        public void OneMinuteFromNow()
        {
            var oneMinFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, TimeUnitTense.Future));
            Verify(oneMinFromNow, new TimeSpan(0, 0, 1, 1));
        }

        [Fact]
        public void AFewMinutesFromNow()
        {
            var minsFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, TimeUnitTense.Future, 10)), 10);
            Verify(minsFromNow, new TimeSpan(0, 0, 10, 0));
        }

        [Fact]
        public void AnHourFromNow()
        {
            var anHourFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, TimeUnitTense.Future));
            Verify(anHourFromNow, new TimeSpan(0, 1, 10, 0));
        }

        [Fact]
        public void HoursFromNow()
        {
            var hoursFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, TimeUnitTense.Future, 10)), 10);
            Verify(hoursFromNow, new TimeSpan(0, 10, 0, 0));
        }

        [Fact]
        public void Tomorrow()
        {
            var tomorrow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, TimeUnitTense.Future));
            Verify(tomorrow, new TimeSpan(1, 10, 0, 0));
        }

        [Fact]
        public void AFewDaysFromNow()
        {
            var daysFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, TimeUnitTense.Future, 10)), 10);
            Verify(daysFromNow, new TimeSpan(10, 1, 0, 0));
        }

        [Fact]
        public void OneMonthFromNow()
        {
            var oneMonthFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, TimeUnitTense.Future));
            Verify(oneMonthFromNow, new TimeSpan(31, 1, 0, 0));
        }

        [Fact]
        public void AFewMonthsFromNow()
        {
            var monthsFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, TimeUnitTense.Future, 2)), 2);
            Verify(monthsFromNow, new TimeSpan(62, 1, 0, 0));
        }

        [Fact]
        public void OneYearFromNowIsNotAccureate()
        {
            var aYearFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, TimeUnitTense.Future));
            Verify(aYearFromNow, new TimeSpan(360, 0, 0, 0));
        }

        [Fact]
        public void OneYearFromNow()
        {
            var aYearFromNow = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, TimeUnitTense.Future));
            Verify(aYearFromNow, new TimeSpan(400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsFromNow()
        {
            var fewYearsFromNow = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, TimeUnitTense.Future, 2)), 2);
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
            var aSecAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, TimeUnitTense.Past));
            Verify(aSecAgo, new TimeSpan(0, 0, 0, -1));
        }

        [Fact]
        public void SecondsAgo()
        {
            var secondsAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, TimeUnitTense.Past, 10)), 10);
            Verify(secondsAgo, new TimeSpan(0, 0, 0, -10));
        }

        [Fact]
        public void OneMinuteAgo()
        {
            var aMinuteAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, TimeUnitTense.Past));
            Verify(aMinuteAgo, new TimeSpan(0, 0, -1, -10));
        }

        [Fact]
        public void AFewMinutesAgo()
        {
            var minsAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, TimeUnitTense.Past, 10)), 10);
            Verify(minsAgo, new TimeSpan(0, 0, -10, 0));
        }

        [Fact]
        public void AnHourAgo()
        {
            var anHourAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, TimeUnitTense.Past));
            Verify(anHourAgo, new TimeSpan(0, -1, -10, 0));
        }

        [Fact]
        public void HoursAgo()
        {
            var hoursAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, TimeUnitTense.Past, 10)), 10);
            Verify(hoursAgo, new TimeSpan(0, -10, 0, 0));
        }

        [Fact]
        public void Yesterday()
        {
            var yesterday = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, TimeUnitTense.Past));
            Verify(yesterday, new TimeSpan(-1, -10, 0, 0));
        }

        [Fact]
        public void AFewDaysAgo()
        {
            var fewDaysAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, TimeUnitTense.Past, 10)), 10);
            Verify(fewDaysAgo, new TimeSpan(-10, -1, 0, 0));
        }

        [Fact]
        public void OneMonthAgo()
        {
            var aMonthAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, TimeUnitTense.Past));
            Verify(aMonthAgo, new TimeSpan(-31, -1, 0, 0));
        }

        [Fact]
        public void AFewMonthsAgo()
        {
            var monthsAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, TimeUnitTense.Past, 2)), 2);
            Verify(monthsAgo, new TimeSpan(-62, -1, 0, 0));
        }

        [Fact]
        public void OneYearAgoIsNotAccureate()
        {
            var aYearAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, TimeUnitTense.Past));
            Verify(aYearAgo, new TimeSpan(-360, 0, 0, 0));
        }

        [Fact]
        public void OneYearAgo()
        {
            var aYearAgo = Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, TimeUnitTense.Past));
            Verify(aYearAgo, new TimeSpan(-400, 0, 0, 0));
        }

        [Fact]
        public void FewYearsAgo()
        {
            var yearsAgo = string.Format(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, TimeUnitTense.Past, 2)), 2);
            Verify(yearsAgo, new TimeSpan(-900, 0, 0, 0));
        }
    }
}
