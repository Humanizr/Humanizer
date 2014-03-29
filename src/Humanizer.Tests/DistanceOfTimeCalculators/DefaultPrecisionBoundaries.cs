using System;
using Humanizer.DistanceOfTimeCalculators;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.DistanceOfTimeCalculators
{
    public class DefaultPrecisionBoundaries
    {
        readonly PrecisionBasedDistanceOfTime _calculator = new PrecisionBasedDistanceOfTime();

        private readonly string justNow = GetJustNow();
        private readonly string secondFromNow = GetResource(TimeUnit.Second, 1);
        private readonly string minuteFromNow = GetResource(TimeUnit.Minute, 1);
        private readonly string hourFromNow = GetResource(TimeUnit.Hour, 1);
        private readonly string dayFromNow = GetResource(TimeUnit.Day, 1);
        private readonly string monthFromNow = GetResource(TimeUnit.Month, 1);
        private readonly string yearFromNow = GetResource(TimeUnit.Year, 1);

        [Fact]
        public void MillisecondsBorder()
        {
            const int millisecondsBoder = 750;
            VerifyBorder(TimeSpan.FromMilliseconds, millisecondsBoder, justNow, secondFromNow);
        }

        [Fact]
        public void FourtyFiveSecondsBorder()
        {
            const int secondsBorder = 45;
            VerifyBorder(TimeSpan.FromSeconds, secondsBorder,
                GetResource(TimeUnit.Second, secondsBorder - 1),
                minuteFromNow);
        }

        [Fact]
        public void FourtyFiveMinutesBorder()
        {
            const int minutesBorder = 45;
            VerifyBorder(TimeSpan.FromMinutes, minutesBorder,
                GetResource(TimeUnit.Minute, minutesBorder - 1),
                hourFromNow);
        }

        [Fact]
        public void EighteenHoursBorder()
        {
            const int hoursBorder = 18;
            VerifyBorder(TimeSpan.FromHours, 18,
                GetResource(TimeUnit.Hour, hoursBorder - 1),
                dayFromNow);
        }

        [Fact]
        public void TwentyThreeDaysBorder()
        {
            const int monthBorderInDays = 23;
            VerifyBorder(TimeSpan.FromDays, monthBorderInDays,
                GetResource(TimeUnit.Day, monthBorderInDays - 1),
                monthFromNow);
        }

        [Fact]
        public void TwoHundredAndFourDaysBorder()
        {
            const int yearBorderInDays = 274;
            VerifyBorder(TimeSpan.FromDays, yearBorderInDays,
                GetResource(TimeUnit.Month, 9),
                yearFromNow);
        }

        private void VerifyBorder(Func<double, TimeSpan> timeSpanFunc, int border, string beforeBorder, string afterBorder)
        {
            var date = DateTime.UtcNow;
            var date1 = date.Add(timeSpanFunc(border - 1));
            var date2 = date.Add(timeSpanFunc(border));
            var date3 = date.Add(timeSpanFunc(border + 1));

            var actual1 = _calculator.Calculate(date1, date);
            var actual2 = _calculator.Calculate(date2, date);
            var actual3 = _calculator.Calculate(date3, date);

            Assert.Equal(beforeBorder, actual1);
            Assert.Equal(afterBorder, actual2);
            Assert.Equal(afterBorder, actual3);
        }

        private static string GetResource(TimeUnit timeUnit, int count)
        {
            string resourceKey = ResourceKeys.DateHumanize.GetResourceKey(timeUnit, TimeUnitTense.Future, count);
            return Resources.GetResource(resourceKey).FormatWith(count);
        }

        private static string GetJustNow()
        {
            return Resources.GetResource(ResourceKeys.DateHumanize.Now);
        }
    }
}