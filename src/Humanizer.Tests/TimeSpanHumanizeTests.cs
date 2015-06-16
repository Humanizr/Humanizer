﻿using System;
using System.Globalization;
using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("en-US") { }

        [Theory]
        [InlineData(14, "2 weeks")]
        [InlineData(7, "1 week")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 days")]
        [InlineData(2, "2 days")]
        [InlineData(1, "1 day")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 hours")]
        [InlineData(1, "1 hour")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 minutes")]
        [InlineData(1, "1 minute")]
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
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(7 * 24 * 60 * 60 * 1000, "7 days", TimeUnit.Day)]
        [InlineData(24 * 60 * 60 * 1000, "24 hours", TimeUnit.Hour)]
        [InlineData(60 * 60 * 1000, "60 minutes", TimeUnit.Minute)]
        [InlineData(60 * 1000, "60 seconds", TimeUnit.Second)]
        [InlineData(1000, "1000 milliseconds", TimeUnit.Millisecond)]
        public void TimeSpanWithMaxTimeUnit(int ms, string expected, TimeUnit maxUnit)
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
        public void TimeSpanWithMinTimeUnit(int ms, string expected, TimeUnit minUnit)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize(minUnit: minUnit, precision: 6);
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
        public void TimeSpanWithPrecesion(int milliseconds, int precesion, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precesion);
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
