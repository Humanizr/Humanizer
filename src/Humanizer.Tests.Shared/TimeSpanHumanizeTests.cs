using System;
using System.Globalization;
using Humanizer.Localisation;
using Xunit;
using System.Linq;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class TimeSpanHumanizeTests
    {
        [Fact]
        public void AllTimeSpansMustBeUniqueForASequenceOfDays()
        {
            var culture = new CultureInfo("en-US");
            var qry = from i in Enumerable.Range(0, 100000)
                      let ts = TimeSpan.FromDays(i)
                      let text = ts.Humanize(precision: 3, culture: culture, maxUnit: TimeUnit.Year)
                      select text;
            var grouping = from t in qry
                           group t by t into g
                           select new { Key = g.Key, Count = g.Count() };
            var allUnique = grouping.All(g => g.Count == 1);
            Assert.True(allUnique);
        }

        [Theory]
        [InlineData(365, "11 months, 30 days")]
        [InlineData(365 + 1, "1 year")]
        [InlineData(365 + 365, "1 year, 11 months, 29 days")]
        [InlineData(365 + 365 + 1, "2 years")]
        [InlineData(365 + 365 + 365, "2 years, 11 months, 29 days")]
        [InlineData(365 + 365 + 365 + 1, "3 years")]
        [InlineData(365 + 365 + 365 + 365, "3 years, 11 months, 29 days")]
        [InlineData(365 + 365 + 365 + 365 + 1, "4 years")]
        [InlineData(365 + 365 + 365 + 365 + 366, "4 years, 11 months, 30 days")]
        [InlineData(365 + 365 + 365 + 365 + 366 + 1, "5 years")]
        public void Year(int days, string expected)
        {
            string actual = TimeSpan.FromDays(days).Humanize(precision: 7, maxUnit: TimeUnit.Year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(30, "4 weeks, 2 days")]
        [InlineData(30 + 1, "1 month")]
        [InlineData(30 + 30, "1 month, 29 days")]
        [InlineData(30 + 30 + 1, "2 months")]
        [InlineData(30 + 30 + 31, "2 months, 30 days")]
        [InlineData(30 + 30 + 31 + 1, "3 months")]
        [InlineData(30 + 30 + 31 + 30, "3 months, 29 days")]
        [InlineData(30 + 30 + 31 + 30 + 1, "4 months")]
        [InlineData(30 + 30 + 31 + 30 + 31, "4 months, 30 days")]
        [InlineData(30 + 30 + 31 + 30 + 31 + 1, "5 months")]
        [InlineData(365, "11 months, 30 days")]
        [InlineData(366, "1 year")]
        public void Month(int days, string expected)
        {
            string actual = TimeSpan.FromDays(days).Humanize(precision: 7, maxUnit: TimeUnit.Year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(14, "2 weeks")]
        [InlineData(7, "1 week")]
        [InlineData(-14, "2 weeks")]
        [InlineData(-7, "1 week")]
        [InlineData(730, "104 weeks")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 days")]
        [InlineData(2, "2 days")]
        [InlineData(1, "1 day")]
        [InlineData(-6, "6 days")]
        [InlineData(-2, "2 days")]
        [InlineData(-1, "1 day")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 hours")]
        [InlineData(1, "1 hour")]
        [InlineData(-2, "2 hours")]
        [InlineData(-1, "1 hour")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 minutes")]
        [InlineData(1, "1 minute")]
        [InlineData(-2, "2 minutes")]
        [InlineData(-1, "1 minute")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(135, "2 minutes")]
        [InlineData(60, "1 minute")]
        [InlineData(2, "2 seconds")]
        [InlineData(1, "1 second")]
        [InlineData(-135, "2 minutes")]
        [InlineData(-60, "1 minute")]
        [InlineData(-2, "2 seconds")]
        [InlineData(-1, "1 second")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 seconds")]
        [InlineData(1400, "1 second")]
        [InlineData(2, "2 milliseconds")]
        [InlineData(1, "1 millisecond")]
        [InlineData(-2500, "2 seconds")]
        [InlineData(-1400, "1 second")]
        [InlineData(-2, "2 milliseconds")]
        [InlineData(-1, "1 millisecond")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData((long)366 * 24 * 60 * 60 * 1000, "12 months", TimeUnit.Month)]
        [InlineData((long)6 * 7 * 24 * 60 * 60 * 1000, "6 weeks", TimeUnit.Week)]
        [InlineData(7 * 24 * 60 * 60 * 1000, "7 days", TimeUnit.Day)]
        [InlineData(24 * 60 * 60 * 1000, "24 hours", TimeUnit.Hour)]
        [InlineData(60 * 60 * 1000, "60 minutes", TimeUnit.Minute)]
        [InlineData(60 * 1000, "60 seconds", TimeUnit.Second)]
        [InlineData(1000, "1000 milliseconds", TimeUnit.Millisecond)]
        public void TimeSpanWithMaxTimeUnit(long ms, string expected, TimeUnit maxUnit)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize(maxUnit: maxUnit);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "10 milliseconds", TimeUnit.Millisecond)]
        [InlineData(10, "no time", TimeUnit.Second)]
        [InlineData(10, "no time", TimeUnit.Minute)]
        [InlineData(10, "no time", TimeUnit.Hour)]
        [InlineData(10, "no time", TimeUnit.Day)]
        [InlineData(10, "no time", TimeUnit.Week)]
        [InlineData(2500, "2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(2500, "2 seconds", TimeUnit.Second)]
        [InlineData(2500, "no time", TimeUnit.Minute)]
        [InlineData(2500, "no time", TimeUnit.Hour)]
        [InlineData(2500, "no time", TimeUnit.Day)]
        [InlineData(2500, "no time", TimeUnit.Week)]
        [InlineData(122500, "2 minutes, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(122500, "2 minutes, 2 seconds", TimeUnit.Second)]
        [InlineData(122500, "2 minutes", TimeUnit.Minute)]
        [InlineData(122500, "no time", TimeUnit.Hour)]
        [InlineData(122500, "no time", TimeUnit.Day)]
        [InlineData(122500, "no time", TimeUnit.Week)]
        [InlineData(3722500, "1 hour, 2 minutes, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(3722500, "1 hour, 2 minutes, 2 seconds", TimeUnit.Second)]
        [InlineData(3722500, "1 hour, 2 minutes", TimeUnit.Minute)]
        [InlineData(3722500, "1 hour", TimeUnit.Hour)]
        [InlineData(3722500, "no time", TimeUnit.Day)]
        [InlineData(3722500, "no time", TimeUnit.Week)]
        [InlineData(90122500, "1 day, 1 hour, 2 minutes, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(90122500, "1 day, 1 hour, 2 minutes, 2 seconds", TimeUnit.Second)]
        [InlineData(90122500, "1 day, 1 hour, 2 minutes", TimeUnit.Minute)]
        [InlineData(90122500, "1 day, 1 hour", TimeUnit.Hour)]
        [InlineData(90122500, "1 day", TimeUnit.Day)]
        [InlineData(90122500, "no time", TimeUnit.Week)]
        [InlineData(694922500, "1 week, 1 day, 1 hour, 2 minutes, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(694922500, "1 week, 1 day, 1 hour, 2 minutes, 2 seconds", TimeUnit.Second)]
        [InlineData(694922500, "1 week, 1 day, 1 hour, 2 minutes", TimeUnit.Minute)]
        [InlineData(694922500, "1 week, 1 day, 1 hour", TimeUnit.Hour)]
        [InlineData(694922500, "1 week, 1 day", TimeUnit.Day)]
        [InlineData(694922500, "1 week", TimeUnit.Week)]
        [InlineData(2768462500, "1 month, 1 day, 1 hour, 1 minute, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(2768462500, "1 month, 1 day, 1 hour, 1 minute, 2 seconds", TimeUnit.Second)]
        [InlineData(2768462500, "1 month, 1 day, 1 hour, 1 minute", TimeUnit.Minute)]
        [InlineData(2768462500, "1 month, 1 day, 1 hour", TimeUnit.Hour)]
        [InlineData(2768462500, "1 month, 1 day", TimeUnit.Day)]
        [InlineData(2768462500, "1 month", TimeUnit.Week)]
        [InlineData(2768462500, "1 month", TimeUnit.Month)]
        [InlineData(2768462500, "no time", TimeUnit.Year)]
        [InlineData(34390862500, "1 year, 1 month, 2 days, 1 hour, 1 minute, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(34390862500, "1 year, 1 month, 2 days, 1 hour, 1 minute, 2 seconds", TimeUnit.Second)]
        [InlineData(34390862500, "1 year, 1 month, 2 days, 1 hour, 1 minute", TimeUnit.Minute)]
        [InlineData(34390862500, "1 year, 1 month, 2 days, 1 hour", TimeUnit.Hour)]
        [InlineData(34390862500, "1 year, 1 month, 2 days", TimeUnit.Day)]
        [InlineData(34390862500, "1 year, 1 month", TimeUnit.Week)]
        [InlineData(34390862500, "1 year, 1 month", TimeUnit.Month)]
        [InlineData(34390862500, "1 year", TimeUnit.Year)]
        public void TimeSpanWithMinTimeUnit(long ms, string expected, TimeUnit minUnit)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize(minUnit: minUnit, precision: 7, maxUnit: TimeUnit.Year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "no time")]
        [InlineData(0, 2, "no time")]
        [InlineData(10, 2, "10 milliseconds")]
        [InlineData(1400, 2, "1 second, 400 milliseconds")]
        [InlineData(2500, 2, "2 seconds, 500 milliseconds")]
        [InlineData(120000, 2, "2 minutes")]
        [InlineData(62000, 2, "1 minute, 2 seconds")]
        [InlineData(62020, 2, "1 minute, 2 seconds")]
        [InlineData(62020, 3, "1 minute, 2 seconds, 20 milliseconds")]
        [InlineData(3600020, 4, "1 hour, 20 milliseconds")]
        [InlineData(3600020, 3, "1 hour, 20 milliseconds")]
        [InlineData(3600020, 2, "1 hour, 20 milliseconds")]
        [InlineData(3600020, 1, "1 hour")]
        [InlineData(3603001, 2, "1 hour, 3 seconds")]
        [InlineData(3603001, 3, "1 hour, 3 seconds, 1 millisecond")]
        [InlineData(86400000, 3, "1 day")]
        [InlineData(86400000, 2, "1 day")]
        [InlineData(86400000, 1, "1 day")]
        [InlineData(86401000, 1, "1 day")]
        [InlineData(86401000, 2, "1 day, 1 second")]
        [InlineData(86401200, 2, "1 day, 1 second")]
        [InlineData(86401200, 3, "1 day, 1 second, 200 milliseconds")]
        [InlineData(1296000000, 1, "2 weeks")]
        [InlineData(1296000000, 2, "2 weeks, 1 day")]
        [InlineData(1299600000, 2, "2 weeks, 1 day")]
        [InlineData(1299600000, 3, "2 weeks, 1 day, 1 hour")]
        [InlineData(1299630020, 3, "2 weeks, 1 day, 1 hour")]
        [InlineData(1299630020, 4, "2 weeks, 1 day, 1 hour, 30 seconds")]
        [InlineData(1299630020, 5, "2 weeks, 1 day, 1 hour, 30 seconds, 20 milliseconds")]
        [InlineData(2768462500, 6, "1 month, 1 day, 1 hour, 1 minute, 2 seconds, 500 milliseconds")]
        [InlineData(2768462500, 5, "1 month, 1 day, 1 hour, 1 minute, 2 seconds")]
        [InlineData(2768462500, 4, "1 month, 1 day, 1 hour, 1 minute")]
        [InlineData(2768462500, 3, "1 month, 1 day, 1 hour")]
        [InlineData(2768462500, 2, "1 month, 1 day")]
        [InlineData(2768462500, 1, "1 month")]
        [InlineData(34390862500, 7, "1 year, 1 month, 2 days, 1 hour, 1 minute, 2 seconds, 500 milliseconds")]
        [InlineData(34390862500, 6, "1 year, 1 month, 2 days, 1 hour, 1 minute, 2 seconds")]
        [InlineData(34390862500, 5, "1 year, 1 month, 2 days, 1 hour, 1 minute")]
        [InlineData(34390862500, 4, "1 year, 1 month, 2 days, 1 hour")]
        [InlineData(34390862500, 3, "1 year, 1 month, 2 days")]
        [InlineData(34390862500, 2, "1 year, 1 month")]
        [InlineData(34390862500, 1, "1 year")]
        public void TimeSpanWithPrecision(long milliseconds, int precision, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, maxUnit: TimeUnit.Year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(52)]
        public void TimeSpanWithMinAndMaxUnits_DoesNotReportExcessiveTime(int minutes)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize(2, null, TimeUnit.Hour, TimeUnit.Minute);
            var expected = TimeSpan.FromMinutes(minutes).Humanize(2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "no time")]
        [InlineData(0, 2, "no time")]
        [InlineData(10, 2, "10 milliseconds")]
        [InlineData(1400, 2, "1 second, 400 milliseconds")]
        [InlineData(2500, 2, "2 seconds, 500 milliseconds")]
        [InlineData(60001, 1, "1 minute")]
        [InlineData(60001, 2, "1 minute")]
        [InlineData(60001, 3, "1 minute, 1 millisecond")]
        [InlineData(120000, 2, "2 minutes")]
        [InlineData(62000, 2, "1 minute, 2 seconds")]
        [InlineData(62020, 2, "1 minute, 2 seconds")]
        [InlineData(62020, 3, "1 minute, 2 seconds, 20 milliseconds")]
        [InlineData(3600020, 4, "1 hour, 20 milliseconds")]
        [InlineData(3600020, 3, "1 hour")]
        [InlineData(3600020, 2, "1 hour")]
        [InlineData(3600020, 1, "1 hour")]
        [InlineData(3603001, 2, "1 hour")]
        [InlineData(3603001, 3, "1 hour, 3 seconds")]
        [InlineData(86400000, 3, "1 day")]
        [InlineData(86400000, 2, "1 day")]
        [InlineData(86400000, 1, "1 day")]
        [InlineData(86401000, 1, "1 day")]
        [InlineData(86401000, 2, "1 day")]
        [InlineData(86401000, 3, "1 day")]
        [InlineData(86401000, 4, "1 day, 1 second")]
        [InlineData(86401200, 4, "1 day, 1 second")]
        [InlineData(86401200, 5, "1 day, 1 second, 200 milliseconds")]
        [InlineData(1296000000, 1, "2 weeks")]
        [InlineData(1296000000, 2, "2 weeks, 1 day")]
        [InlineData(1299600000, 2, "2 weeks, 1 day")]
        [InlineData(1299600000, 3, "2 weeks, 1 day, 1 hour")]
        [InlineData(1299630020, 3, "2 weeks, 1 day, 1 hour")]
        [InlineData(1299630020, 4, "2 weeks, 1 day, 1 hour")]
        [InlineData(1299630020, 5, "2 weeks, 1 day, 1 hour, 30 seconds")]
        [InlineData(1299630020, 6, "2 weeks, 1 day, 1 hour, 30 seconds, 20 milliseconds")]
        public void TimeSpanWithPrecisionAndCountingEmptyUnits(int milliseconds, int precision, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision: precision, countEmptyUnits: true);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "no time")]
        [InlineData(0, 2, "no time")]
        [InlineData(10, 2, "10 milliseconds")]
        [InlineData(1400, 2, "1 second and 400 milliseconds")]
        [InlineData(2500, 2, "2 seconds and 500 milliseconds")]
        [InlineData(120000, 2, "2 minutes")]
        [InlineData(62000, 2, "1 minute and 2 seconds")]
        [InlineData(62020, 2, "1 minute and 2 seconds")]
        [InlineData(62020, 3, "1 minute, 2 seconds, and 20 milliseconds")]
        [InlineData(3600020, 4, "1 hour and 20 milliseconds")]
        [InlineData(3600020, 3, "1 hour and 20 milliseconds")]
        [InlineData(3600020, 2, "1 hour and 20 milliseconds")]
        [InlineData(3600020, 1, "1 hour")]
        [InlineData(3603001, 2, "1 hour and 3 seconds")]
        [InlineData(3603001, 3, "1 hour, 3 seconds, and 1 millisecond")]
        [InlineData(86400000, 3, "1 day")]
        [InlineData(86400000, 2, "1 day")]
        [InlineData(86400000, 1, "1 day")]
        [InlineData(86401000, 1, "1 day")]
        [InlineData(86401000, 2, "1 day and 1 second")]
        [InlineData(86401200, 2, "1 day and 1 second")]
        [InlineData(86401200, 3, "1 day, 1 second, and 200 milliseconds")]
        [InlineData(1296000000, 1, "2 weeks")]
        [InlineData(1296000000, 2, "2 weeks and 1 day")]
        [InlineData(1299600000, 2, "2 weeks and 1 day")]
        [InlineData(1299600000, 3, "2 weeks, 1 day, and 1 hour")]
        [InlineData(1299630020, 3, "2 weeks, 1 day, and 1 hour")]
        [InlineData(1299630020, 4, "2 weeks, 1 day, 1 hour, and 30 seconds")]
        [InlineData(1299630020, 5, "2 weeks, 1 day, 1 hour, 30 seconds, and 20 milliseconds")]
        public void TimeSpanWithPrecisionAndAlternativeCollectionFormatter(int milliseconds, int precision, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, collectionSeparator: null);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("no time", actual);
        }

        [Theory]
        [InlineData(1, "en-US", "1 millisecond")]
        [InlineData(6 * 24 * 60 * 60 * 1000, "ru-RU", "6 дней")]
        [InlineData(11 * 60 * 60 * 1000, "ar", "11 ساعة")]
        public void CanSpecifyCultureExplicitly(int ms, string culture, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize(culture: new CultureInfo(culture));
            Assert.Equal(expected, actual);
        }
    }
}
