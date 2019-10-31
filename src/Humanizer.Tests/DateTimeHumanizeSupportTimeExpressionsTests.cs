using System;
using System.Globalization;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class DateTimeHumanizeSupportTimeExpressionsTests
    {

        private static readonly TimeExpressionProviderToTest TimeExpressionProvider = new TimeExpressionProviderToTest();

        [Theory]
        [InlineData(1, "for one second")]
        [InlineData(10, "for 10 seconds")]
        [InlineData(59, "for 59 seconds")]
        [InlineData(60, "for a minute")]
        public void SecondsFor(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in one second")]
        [InlineData(10, "in 10 seconds")]
        [InlineData(59, "in 59 seconds")]
        [InlineData(60, "in a minute")]
        public void SecondsIn(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for a minute")]
        [InlineData(10, "for 10 minutes")]
        [InlineData(44, "for 44 minutes")]
        [InlineData(45, "for 45 minutes")]
        [InlineData(59, "for 59 minutes")]
        [InlineData(60, "for an hour")]
        [InlineData(119, "for an hour")]
        [InlineData(120, "for 2 hours")]
        public void MinutesFor(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in a minute")]
        [InlineData(10, "in 10 minutes")]
        [InlineData(44, "in 44 minutes")]
        [InlineData(45, "in 45 minutes")]
        [InlineData(59, "in 59 minutes")]
        [InlineData(60, "in an hour")]
        [InlineData(119, "in an hour")]
        [InlineData(120, "in 2 hours")]
        public void MinutesIn(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for an hour")]
        [InlineData(10, "for 10 hours")]
        [InlineData(23, "for 23 hours")]
        [InlineData(24, "for one day")]
        public void HoursFor(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in an hour")]
        [InlineData(10, "in 10 hours")]
        [InlineData(23, "in 23 hours")]
        [InlineData(24, "tomorrow")]
        public void HoursIn(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for one day")]
        [InlineData(10, "for 10 days")]
        [InlineData(27, "for 27 days")]
        [InlineData(32, "for one month")]
        public void DaysFor(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "tomorrow")]
        [InlineData(10, "in 10 days")]
        [InlineData(27, "in 27 days")]
        [InlineData(32, "in one month")]
        public void DaysIn(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for one month")]
        [InlineData(10, "for 10 months")]
        [InlineData(11, "for 11 months")]
        [InlineData(12, "for one year")]
        public void MonthsFor(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in one month")]
        [InlineData(10, "in 10 months")]
        [InlineData(11, "in 11 months")]
        [InlineData(12, "in one year")]
        public void MonthsIn(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for one year")]
        [InlineData(2, "for 2 years")]
        public void YearsFor(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past, dateTimeExpressionProvider: TimeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in one year")]
        [InlineData(2, "in 2 years")]
        public void YearsIn(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future, dateTimeExpressionProvider: TimeExpressionProvider);
        }
    }
}
