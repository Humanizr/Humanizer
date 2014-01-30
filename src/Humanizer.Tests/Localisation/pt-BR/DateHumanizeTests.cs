using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ptBR
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("pt-BR") { }

        [Theory]
        [InlineData(-10, "10 dias atrás")]
        [InlineData(-3, "3 dias atrás")]
        [InlineData(-2, "2 dias atrás")]
		[InlineData(-1, "ontem")]
        public void DaysAgo(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 horas atrás")]
        [InlineData(-3, "3 horas atrás")]
        [InlineData(-2, "2 horas atrás")]
        [InlineData(-1, "uma hora atrás")]
        public void HoursAgo(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 minutos atrás")]
        [InlineData(-3, "3 minutos atrás")]
        [InlineData(-2, "2 minutos atrás")]
        [InlineData(-1, "um minuto atrás")]
        public void MinutesAgo(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 meses atrás")]
        [InlineData(-3, "3 meses atrás")]
        [InlineData(-2, "2 meses atrás")]
        [InlineData(-1, "um mês atrás")]
        public void MonthsAgo(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 segundos atrás")]
        [InlineData(-3, "3 segundos atrás")]
        [InlineData(-2, "2 segundos atrás")]
        [InlineData(-1, "um segundo atrás")]
        public void SecondsAgo(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 anos atrás")]
        [InlineData(-3, "3 anos atrás")]
        [InlineData(-2, "2 anos atrás")]
        [InlineData(-1, "um ano atrás")]
        public void YearsAgo(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }
    }
}
