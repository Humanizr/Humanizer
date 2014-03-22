using System;
using System.Collections.Generic;
using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class DateHumanizeOneTimeUnitTests
    {
        static void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow)
        {
            if (expectedString == null)
                throw new ArgumentNullException("expectedString");

            Assert.Equal(expectedString, DateTime.UtcNow.Add(deltaFromNow).Humanize());
            Assert.Equal(expectedString, DateTime.Now.Add(deltaFromNow).Humanize(false));
        }

        static void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow)
        {
            if (expectedString == null)
                throw new ArgumentNullException("expectedString");

            var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
            var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

            Assert.Equal(expectedString, utcNow.Add(deltaFromNow).Humanize(true, utcNow));
            Assert.Equal(expectedString, now.Add(deltaFromNow).Humanize(false, now));
        }

        static void Verify(string expectedString, TimeSpan deltaFromNow)
        {
            VerifyWithCurrentDate(expectedString, deltaFromNow);
            VerifyWithDateInjection(expectedString, deltaFromNow);
        }

        public static IEnumerable<object[]> OneTimeUnitAgoTestsSource
        {
            get
            {
                return new[] {
                    new object[]{ TimeUnit.Second, TimeSpan.FromSeconds(-1) },
                    new object[]{ TimeUnit.Minute, TimeSpan.FromMinutes(-1) },
                    new object[]{ TimeUnit.Hour, TimeSpan.FromHours(-1) },
                    new object[]{ TimeUnit.Day, TimeSpan.FromDays(-1) },
                    new object[]{ TimeUnit.Month, TimeSpan.FromDays(-30) },
                    new object[]{ TimeUnit.Year, TimeSpan.FromDays(-365) },
                };
            }
        }

        public static IEnumerable<object[]> OneTimeUnitFromNowTestsSource
        {
            get
            {
                return new[] {
                    new object[]{ TimeUnit.Second, TimeSpan.FromSeconds(1) },
                    new object[]{ TimeUnit.Minute, TimeSpan.FromMinutes(1) },
                    new object[]{ TimeUnit.Hour, TimeSpan.FromHours(1) },
                    new object[]{ TimeUnit.Day, TimeSpan.FromDays(1) },
                    new object[]{ TimeUnit.Month, TimeSpan.FromDays(30) },
                    new object[]{ TimeUnit.Year, TimeSpan.FromDays(365) },
                };
            }
        }

        [Theory]
        [PropertyData("OneTimeUnitAgoTestsSource")]
        public void OneTimeUnitAgo(TimeUnit timeUnit, TimeSpan timeSpan)
        {
            string resourceKey = ResourceKeys.DateHumanize.GetResourceKey(timeUnit, 1);
            Verify(Resources.GetResource(resourceKey), timeSpan);
        }

        [Theory]
        [PropertyData("OneTimeUnitFromNowTestsSource")]
        public void OneTimeUnitFromNow(TimeUnit timeUnit, TimeSpan timeSpan)
        {
            string resourceKey = ResourceKeys.DateHumanize.GetResourceKey(timeUnit, 1, true);
            Verify(Resources.GetResource(resourceKey), timeSpan);
        }
    }
}