using System;
using System.Globalization;
using System.Linq;
using Humanizer.Localisation;
using Xunit;

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
                           select new { g.Key, Count = g.Count() };
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
            var actual = TimeSpan.FromDays(days).Humanize(precision: 7, maxUnit: TimeUnit.Year);
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
            var actual = TimeSpan.FromDays(days).Humanize(precision: 7, maxUnit: TimeUnit.Year);
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
        [InlineData(10, "no time", TimeUnit.Second, true)]
        [InlineData(10, "no time", TimeUnit.Minute, true)]
        [InlineData(10, "no time", TimeUnit.Hour, true)]
        [InlineData(10, "no time", TimeUnit.Day, true)]
        [InlineData(10, "no time", TimeUnit.Week, true)]
        [InlineData(10, "0 seconds", TimeUnit.Second)]
        [InlineData(10, "0 minutes", TimeUnit.Minute)]
        [InlineData(10, "0 hours", TimeUnit.Hour)]
        [InlineData(10, "0 days", TimeUnit.Day)]
        [InlineData(10, "0 weeks", TimeUnit.Week)]
        [InlineData(2500, "2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(2500, "2 seconds", TimeUnit.Second)]
        [InlineData(2500, "no time", TimeUnit.Minute, true)]
        [InlineData(2500, "no time", TimeUnit.Hour, true)]
        [InlineData(2500, "no time", TimeUnit.Day, true)]
        [InlineData(2500, "no time", TimeUnit.Week, true)]
        [InlineData(2500, "0 minutes", TimeUnit.Minute)]
        [InlineData(2500, "0 hours", TimeUnit.Hour)]
        [InlineData(2500, "0 days", TimeUnit.Day)]
        [InlineData(2500, "0 weeks", TimeUnit.Week)]
        [InlineData(122500, "2 minutes, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(122500, "2 minutes, 2 seconds", TimeUnit.Second)]
        [InlineData(122500, "2 minutes", TimeUnit.Minute)]
        [InlineData(122500, "no time", TimeUnit.Hour, true)]
        [InlineData(122500, "no time", TimeUnit.Day, true)]
        [InlineData(122500, "no time", TimeUnit.Week, true)]
        [InlineData(122500, "0 hours", TimeUnit.Hour)]
        [InlineData(122500, "0 days", TimeUnit.Day)]
        [InlineData(122500, "0 weeks", TimeUnit.Week)]
        [InlineData(3722500, "1 hour, 2 minutes, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(3722500, "1 hour, 2 minutes, 2 seconds", TimeUnit.Second)]
        [InlineData(3722500, "1 hour, 2 minutes", TimeUnit.Minute)]
        [InlineData(3722500, "1 hour", TimeUnit.Hour)]
        [InlineData(3722500, "no time", TimeUnit.Day, true)]
        [InlineData(3722500, "no time", TimeUnit.Week, true)]
        [InlineData(3722500, "0 days", TimeUnit.Day)]
        [InlineData(3722500, "0 weeks", TimeUnit.Week)]
        [InlineData(90122500, "1 day, 1 hour, 2 minutes, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(90122500, "1 day, 1 hour, 2 minutes, 2 seconds", TimeUnit.Second)]
        [InlineData(90122500, "1 day, 1 hour, 2 minutes", TimeUnit.Minute)]
        [InlineData(90122500, "1 day, 1 hour", TimeUnit.Hour)]
        [InlineData(90122500, "1 day", TimeUnit.Day)]
        [InlineData(90122500, "no time", TimeUnit.Week, true)]
        [InlineData(90122500, "0 weeks", TimeUnit.Week)]
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
        [InlineData(2768462500, "no time", TimeUnit.Year, true)]
        [InlineData(2768462500, "0 years", TimeUnit.Year)]
        [InlineData(34390862500, "1 year, 1 month, 2 days, 1 hour, 1 minute, 2 seconds, 500 milliseconds", TimeUnit.Millisecond)]
        [InlineData(34390862500, "1 year, 1 month, 2 days, 1 hour, 1 minute, 2 seconds", TimeUnit.Second)]
        [InlineData(34390862500, "1 year, 1 month, 2 days, 1 hour, 1 minute", TimeUnit.Minute)]
        [InlineData(34390862500, "1 year, 1 month, 2 days, 1 hour", TimeUnit.Hour)]
        [InlineData(34390862500, "1 year, 1 month, 2 days", TimeUnit.Day)]
        [InlineData(34390862500, "1 year, 1 month", TimeUnit.Week)]
        [InlineData(34390862500, "1 year, 1 month", TimeUnit.Month)]
        [InlineData(34390862500, "1 year", TimeUnit.Year)]
        public void TimeSpanWithMinTimeUnit(long ms, string expected, TimeUnit minUnit, bool toWords = false)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize(minUnit: minUnit, precision: 7, maxUnit: TimeUnit.Year, toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "no time", true)]
        [InlineData(0, 2, "no time", true)]
        [InlineData(0, 3, "0 milliseconds")]
        [InlineData(0, 2, "0 milliseconds")]
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
        public void TimeSpanWithPrecision(long milliseconds, int precision, string expected, bool toWords = false)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, maxUnit: TimeUnit.Year, toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3 * 7 + 4, 2, "3 weeks, 4 days")]
        [InlineData(6 * 7 + 3, 2, "6 weeks, 3 days")]
        [InlineData(72 * 7 + 6, 2, "72 weeks, 6 days")]
        public void DaysWithPrecision(int days, int precision, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize(precision: precision);
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
        [InlineData(0, 3, "no time", true)]
        [InlineData(0, 2, "no time", true)]
        [InlineData(0, 3, "0 milliseconds")]
        [InlineData(0, 2, "0 milliseconds")]
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
        public void TimeSpanWithPrecisionAndCountingEmptyUnits(int milliseconds, int precision, string expected, bool toWords = false)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision: precision, countEmptyUnits: true, toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "no time", true)]
        [InlineData(0, 2, "no time", true)]
        [InlineData(0, 3, "0 milliseconds")]
        [InlineData(0, 2, "0 milliseconds")]
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
        public void TimeSpanWithPrecisionAndAlternativeCollectionFormatter(int milliseconds, int precision,
            string expected, bool toWords = false)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, collectionSeparator: null, toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "no time")]
        [InlineData(0, 2, "no time")]
        [InlineData(10, 2, "ten milliseconds")]
        [InlineData(1400, 2, "one second, four hundred milliseconds")]
        [InlineData(2500, 2, "two seconds, five hundred milliseconds")]
        [InlineData(120000, 2, "two minutes")]
        [InlineData(62000, 2, "one minute, two seconds")]
        [InlineData(62020, 2, "one minute, two seconds")]
        [InlineData(62020, 3, "one minute, two seconds, twenty milliseconds")]
        [InlineData(3600020, 4, "one hour, twenty milliseconds")]
        [InlineData(3600020, 3, "one hour, twenty milliseconds")]
        [InlineData(3600020, 2, "one hour, twenty milliseconds")]
        [InlineData(3600020, 1, "one hour")]
        [InlineData(3603001, 2, "one hour, three seconds")]
        [InlineData(3603001, 3, "one hour, three seconds, one millisecond")]
        [InlineData(86400000, 3, "one day")]
        [InlineData(86400000, 2, "one day")]
        [InlineData(86400000, 1, "one day")]
        [InlineData(86401000, 1, "one day")]
        [InlineData(86401000, 2, "one day, one second")]
        [InlineData(86401200, 2, "one day, one second")]
        [InlineData(86401200, 3, "one day, one second, two hundred milliseconds")]
        [InlineData(1296000000, 1, "two weeks")]
        [InlineData(1296000000, 2, "two weeks, one day")]
        [InlineData(1299600000, 2, "two weeks, one day")]
        [InlineData(1299600000, 3, "two weeks, one day, one hour")]
        [InlineData(1299630020, 3, "two weeks, one day, one hour")]
        [InlineData(1299630020, 4, "two weeks, one day, one hour, thirty seconds")]
        [InlineData(1299630020, 5, "two weeks, one day, one hour, thirty seconds, twenty milliseconds")]
        public void TimeSpanWithNumbersConvertedToWords(int milliseconds, int precision, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, toWords: true);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("0 milliseconds", actual);
        }

        [Fact]
        public void NoTimeToWords()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize(toWords: true);
            Assert.Equal("no time", actual);
        }

        [Theory]
        [InlineData(1, 1, "en-US", "1 millisecond", ", ")]
        [InlineData(6 * 24 * 60 * 60 * 1000, 1, "ru-RU", "6 дней", ", ")]
        [InlineData(11 * 60 * 60 * 1000, 1, "ar", "11 ساعة", ", ")]
        [InlineData(3603001, 2, "it-IT", "1 ora e 3 secondi", null)]
        public void CanSpecifyCultureExplicitly(int ms, int precision, string culture, string expected, string collectionSeparator)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize(precision: precision, culture: new CultureInfo(culture), collectionSeparator: collectionSeparator);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(31 * 4, 1, "en-US", "four months")]
        [InlineData(236,2,"ar", "سبعة أشهر, اثنان و عشرون يوم")]
        [InlineData(321, 2,"es", "diez meses, dieciséis días")]
        public void CanSpecifyCultureExplicitlyToWords(int days, int precision,string culture, string expected)
        {
            var timeSpan = new TimeSpan(days, 0, 0, 0);
            var actual = timeSpan.Humanize(precision: precision, culture: new CultureInfo(culture), maxUnit: TimeUnit.Year, toWords: true);
            Assert.Equal(expected: expected, actual);
        }
    }
}
