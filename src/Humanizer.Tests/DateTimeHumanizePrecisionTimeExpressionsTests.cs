using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class DateTimeHumanizePrecisionTimeExpressionsTests
    {

        private const double DefaultPrecision = .75;
        private static readonly TimeExpressionProviderToTest _timeExpressionProvider = new TimeExpressionProviderToTest();

        [Theory]
        [InlineData(1, "now")]
        [InlineData(749, "now")]
        [InlineData(750, "for one second")]
        [InlineData(1000, "for one second")]
        [InlineData(1749, "for one second")]
        [InlineData(1750, "for 2 seconds")]
        public void MillisecondsAgo(int milliseconds, string expected)
        {
            DateHumanize.Verify(expected, milliseconds, TimeUnit.Millisecond, Tense.Past, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "now")]
        [InlineData(749, "now")]
        [InlineData(750, "in one second")]
        [InlineData(1000, "in one second")]
        [InlineData(1749, "in one second")]
        [InlineData(1750, "in 2 seconds")]
        public void MillisecondsFromNow(int milliseconds, string expected)
        {
            DateHumanize.Verify(expected, milliseconds, TimeUnit.Millisecond, Tense.Future, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for one second")]
        [InlineData(10, "for 10 seconds")]
        [InlineData(44, "for 44 seconds")]
        [InlineData(45, "for a minute")]
        [InlineData(60, "for a minute")]
        [InlineData(104, "for a minute")]
        [InlineData(105, "for 2 minutes")]
        [InlineData(120, "for 2 minutes")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in one second")]
        [InlineData(10, "in 10 seconds")]
        [InlineData(44, "in 44 seconds")]
        [InlineData(45, "in a minute")]
        [InlineData(60, "in a minute")]
        [InlineData(104, "in a minute")]
        [InlineData(105, "in 2 minutes")]
        [InlineData(120, "in 2 minutes")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for a minute")]
        [InlineData(10, "for 10 minutes")]
        [InlineData(44, "for 44 minutes")]
        [InlineData(45, "for an hour")]
        [InlineData(60, "for an hour")]
        [InlineData(104, "for an hour")]
        [InlineData(105, "for 2 hours")]
        [InlineData(120, "for 2 hours")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in a minute")]
        [InlineData(10, "in 10 minutes")]
        [InlineData(44, "in 44 minutes")]
        [InlineData(45, "in an hour")]
        [InlineData(60, "in an hour")]
        [InlineData(104, "in an hour")]
        [InlineData(105, "in 2 hours")]
        [InlineData(120, "in 2 hours")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for an hour")]
        [InlineData(10, "for 10 hours")]
        [InlineData(17, "for 17 hours")]
        [InlineData(18, "for one day")]
        [InlineData(24, "for one day")]
        [InlineData(41, "for one day")]
        [InlineData(42, "for 2 days")]
        [InlineData(48, "for 2 days")]
        [InlineData(60, "for 2 days")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in an hour")]
        [InlineData(10, "in 10 hours")]
        [InlineData(18, "tomorrow")]
        [InlineData(24, "tomorrow")]
        [InlineData(41, "tomorrow")]
        [InlineData(42, "in 2 days")]
        [InlineData(48, "in 2 days")]
        [InlineData(60, "in 2 days")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for one day")]
        [InlineData(10, "for 10 days")]
        [InlineData(20, "for 20 days")]
        [InlineData(22, "for 22 days")]
        [InlineData(23, "for one month")]
        [InlineData(31, "for one month")]
        [InlineData(43, "for one month")]
        [InlineData(53, "for 2 months")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "tomorrow")]
        [InlineData(10, "in 10 days")]
        [InlineData(20, "in 20 days")]
        [InlineData(22, "in 22 days")]
        [InlineData(23, "in one month")]
        [InlineData(31, "in one month")]
        [InlineData(43, "in one month")]
        [InlineData(53, "in 2 months")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for one month")]
        [InlineData(8, "for 8 months")]
        [InlineData(9, "for one year")]
        [InlineData(12, "for one year")]
        [InlineData(19, "for one year")]
        [InlineData(21, "for 2 years")]
        [InlineData(24, "for 2 years")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in one month")]
        [InlineData(8, "in 8 months")]
        [InlineData(9, "in one year")]
        [InlineData(12, "in one year")]
        [InlineData(19, "in one year")]
        [InlineData(21, "in 2 years")]
        [InlineData(24, "in 2 years")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "for one year")]
        [InlineData(2, "for 2 years")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }

        [Theory]
        [InlineData(1, "in one year")]
        [InlineData(2, "in 2 years")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future, DefaultPrecision, dateTimeExpressionProvider: _timeExpressionProvider);
        }
    }
}
