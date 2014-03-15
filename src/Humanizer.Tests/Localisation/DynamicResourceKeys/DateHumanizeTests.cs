using System;
using System.Collections.Generic;
using Humanizer.Localisation.DynamicResourceKeys;
using Xunit;
using Xunit.Extensions;
using Resources = Humanizer.Localisation.Resources;
using ResourceKeys = Humanizer.Localisation.DynamicResourceKeys.ResourceKeys;
using Dyna = Humanizer.DynamicResourceKeys.DateHumanizeExtensions;

namespace Humanizer.Tests.Localisation.DynamicResourceKeys
{
    public class DateHumanizeWithResourceKeysTests
    {
        static void VerifyWithCurrentDate(string expectedString, TimeSpan deltaFromNow)
        {
            Assert.Equal(expectedString, Dyna.Humanize(DateTime.UtcNow.Add(deltaFromNow)));
            Assert.Equal(expectedString, Dyna.Humanize(DateTime.Now.Add(deltaFromNow), false));
        }

        static void VerifyWithDateInjection(string expectedString, TimeSpan deltaFromNow)
        {
            var utcNow = new DateTime(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);
            var now = new DateTime(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);

            Assert.Equal(expectedString, Dyna.Humanize(utcNow.Add(deltaFromNow), true, utcNow));
            Assert.Equal(expectedString, Dyna.Humanize(now.Add(deltaFromNow), false, now));
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

        [Theory]
        [PropertyData("OneTimeUnitAgoTestsSource")]
        public void OneTimeUnitAgo(TimeUnit unit, TimeSpan timeSpan)
        {
            Verify(Resources.GetResource(ResourceKeys.DateHumanize.GetResourceKey(unit, 1)), timeSpan);
        }
    }
}