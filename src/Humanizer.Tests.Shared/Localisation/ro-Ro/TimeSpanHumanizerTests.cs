﻿using System;
using Xunit;

namespace Humanizer.Tests.Localisation.roRO
{


    /// <summary>
    /// Test that for values bigger than 19 "de" is added between the numeral
    /// and the time unit: http://ebooks.unibuc.ro/filologie/NForascu-DGLR/numerale.htm.
    /// There is no test for months since there are only 12 of them in a year.
    /// </summary>
    [UseCulture("ro-RO")]
    public class TimeSpanHumanizerTests 
    {

        [Theory]
        [InlineData(1, "1 milisecundă")]
        [InlineData(14, "14 milisecunde")]
        [InlineData(21, "21 de milisecunde")]
        [InlineData(3000, "3 secunde")]
        public void Milliseconds(int millisSeconds, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(millisSeconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 secundă")]
        [InlineData(14, "14 secunde")]
        [InlineData(21, "21 de secunde")]
        [InlineData(156, "2 minute")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 minut")]
        [InlineData(14, "14 minute")]
        [InlineData(21, "21 de minute")]
        [InlineData(156, "2 ore")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 oră")]
        [InlineData(14, "14 ore")]
        [InlineData(21, "21 de ore")]
        [InlineData(48, "2 zile")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData(1, "1 zi")]
        [InlineData(6, "6 zile")]
        [InlineData(7, "1 săptămână")]
        [InlineData(14, "2 săptămâni")]
        [InlineData(21, "3 săptămâni")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 săptămână")]
        [InlineData(14, "14 săptămâni")]
        [InlineData(21, "21 de săptămâni")]
        public void Weeks(int weeks, string expected)
        {
            var actual = TimeSpan.FromDays(7 * weeks).Humanize();
            Assert.Equal(expected, actual);
        }
    }
}
