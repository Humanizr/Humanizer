using System;
using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.es
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("es-ES") { }

        [Theory]
        [InlineData(1, "hace un segundo")]
        [InlineData(10, "hace 10 segundos")]
        [InlineData(59, "hace 59 segundos")]
        [InlineData(60, "hace un minuto")]
        public void SecondsAgo(int seconds, string expected) 
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "un segundo desde ahora")]
        [InlineData(10, "10 segundos desde ahora")]
        [InlineData(59, "59 segundos desde ahora")]
        [InlineData(60, "un minuto desde ahora")]
        public void SecondsFromNow(int seconds, string expected) 
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "hace un minuto")]
        [InlineData(10, "hace 10 minutos")]
        [InlineData(44, "hace 44 minutos")]
        [InlineData(45, "hace una hora")]
        [InlineData(119, "hace una hora")]
        [InlineData(120, "hace 2 horas")]
        public void MinutesAgo(int minutes, string expected) 
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "un minuto desde ahora")]
        [InlineData(10, "10 minutos desde ahora")]
        [InlineData(44, "44 minutos desde ahora")]
        [InlineData(45, "una hora desde ahora")]
        [InlineData(119, "una hora desde ahora")]
        [InlineData(120, "2 horas desde ahora")]
        public void MinutesFromNow(int minutes, string expected) 
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "hace una hora")]
        [InlineData(10, "hace 10 horas")]
        [InlineData(23, "hace 23 horas")]
        [InlineData(24, "ayer")]
        public void HoursAgo(int hours, string expected) 
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "una hora desde ahora")]
        [InlineData(10, "10 horas desde ahora")]
        [InlineData(23, "23 horas desde ahora")]
        [InlineData(24, "mañana")]
        public void HoursFromNow(int hours, string expected) 
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "ayer")]
        [InlineData(10, "hace 10 días")]
        [InlineData(28, "hace 28 días")]
        [InlineData(32, "hace un mes")]
        public void DaysAgo(int days, string expected) 
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "mañana")]
        [InlineData(10, "10 días desde ahora")]
        [InlineData(28, "28 días desde ahora")]
        [InlineData(32, "un mes desde ahora")]
        public void DaysFromNow(int days, string expected) 
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "hace un mes")]
        [InlineData(10, "hace 10 meses")]
        [InlineData(11, "hace 11 meses")]
        [InlineData(12, "hace un año")]
        public void MonthsAgo(int months, string expected) 
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "un mes desde ahora")]
        [InlineData(10, "10 meses desde ahora")]
        [InlineData(11, "11 meses desde ahora")]
        [InlineData(12, "un año desde ahora")]
        public void MonthsFromNow(int months, string expected) 
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "hace un año")]
        [InlineData(2, "hace 2 años")]
        public void YearsAgo(int years, string expected) 
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "un año desde ahora")]
        [InlineData(2, "2 años desde ahora")]
        public void YearsFromNow(int years, string expected) 
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
