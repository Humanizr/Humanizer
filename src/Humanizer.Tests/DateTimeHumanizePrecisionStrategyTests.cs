using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class DateTimeHumanizePrecisionStrategyTests 
    {
        [Theory]
        [InlineData(1, "now")]
        [InlineData(749, "now")]
        [InlineData(750, "one second ago")]
        [InlineData(1000, "one second ago")]
        [InlineData(1499, "one second ago")]
        [InlineData(1500, "2 seconds ago")]
        public void MillisecondsAgo(int milliseconds, string expected)
        {
            DateHumanize.Verify(expected, milliseconds, TimeUnit.Millisecond, Tense.Past, .75);
        }

        [Theory]
        [InlineData(1, "now")]
        [InlineData(749, "now")]
        [InlineData(750, "one second from now")]
        [InlineData(1000, "one second from now")]
        [InlineData(1499, "one second from now")]
        [InlineData(1500, "2 seconds from now")]
        public void MillisecondsFromNow(int milliseconds, string expected)
        {
            DateHumanize.Verify(expected, milliseconds, TimeUnit.Millisecond, Tense.Future, .75);
        }

        [Theory]
        [InlineData(1, "one second ago")]
        [InlineData(10, "10 seconds ago")]
        [InlineData(44, "44 seconds ago")]
        [InlineData(45, "a minute ago")]
        [InlineData(60, "a minute ago")]
        [InlineData(89, "a minute ago")]
        [InlineData(90, "2 minutes ago")]
        [InlineData(120, "2 minutes ago")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past, .75);
        }

        [Theory]
        [InlineData(1, "one second from now")]
        [InlineData(10, "10 seconds from now")]
        [InlineData(44, "44 seconds from now")]
        [InlineData(45, "a minute from now")]
        [InlineData(60, "a minute from now")]
        [InlineData(89, "a minute from now")]
        [InlineData(90, "2 minutes from now")]
        [InlineData(120, "2 minutes from now")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future, .75);
        }

        [Theory]
        [InlineData(1, "a minute ago")]
        [InlineData(10, "10 minutes ago")]
        [InlineData(44, "44 minutes ago")]
        [InlineData(45, "an hour ago")]
        [InlineData(60, "an hour ago")]
        [InlineData(89, "an hour ago")]
        [InlineData(90, "2 hours ago")]
        [InlineData(120, "2 hours ago")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past, .75);
        }

        [Theory]
        [InlineData(1, "a minute from now")]
        [InlineData(10, "10 minutes from now")]
        [InlineData(44, "44 minutes from now")]
        [InlineData(45, "an hour from now")]
        [InlineData(60, "an hour from now")]
        [InlineData(89, "an hour from now")]
        [InlineData(90, "2 hours from now")]
        [InlineData(120, "2 hours from now")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future, .75);
        }

        [Theory]
        [InlineData(1, "an hour ago")]
        [InlineData(10, "10 hours ago")]
        [InlineData(17, "17 hours ago")]
        [InlineData(18, "yesterday")]
        [InlineData(24, "yesterday")]
        [InlineData(35, "yesterday")]
        [InlineData(36, "2 days ago")]
        [InlineData(48, "2 days ago")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past, .75);
        }

        [Theory]
        [InlineData(1, "an hour from now")]
        [InlineData(10, "10 hours from now")]
        [InlineData(18, "tomorrow")]
        [InlineData(24, "tomorrow")]
        [InlineData(41, "tomorrow")]
        [InlineData(42, "2 days from now")]
        [InlineData(60, "2 days from now")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future, .75);
        }

        [Theory]
        [InlineData(1, "yesterday")]
        [InlineData(10, "10 days ago")]
        [InlineData(20, "20 days ago")]
        [InlineData(25, "one month ago")]
        [InlineData(32, "one month ago")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past, .75);
        }

        [Theory]
        [InlineData(1, "tomorrow")]
        [InlineData(10, "10 days from now")]
        [InlineData(20, "20 days from now")]
        [InlineData(25, "one month from now")]
        [InlineData(32, "one month from now")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future, .75);
        }

        [Theory]
        [InlineData(1, "one month ago")]
        [InlineData(8, "8 months ago")]
        [InlineData(9, "one year ago")]
        [InlineData(12, "one year ago")]
        [InlineData(18, "2 years ago")]
        [InlineData(24, "2 years ago")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past, .75);
        }

        [Theory]
        [InlineData(1, "one month from now")]
        [InlineData(8, "8 months from now")]
        [InlineData(9, "one year from now")]
        [InlineData(12, "one year from now")]
        [InlineData(18, "2 years from now")]
        [InlineData(24, "2 years from now")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future, .75);
        }

        [Theory]
        [InlineData(1, "one year ago")]
        [InlineData(2, "2 years ago")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past, .75);
        }

        [Theory]
        [InlineData(1, "one year from now")]
        [InlineData(2, "2 years from now")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future, .75);
        }
    }
}
