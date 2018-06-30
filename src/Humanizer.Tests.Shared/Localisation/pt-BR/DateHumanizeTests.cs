using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.ptBR
{
    [UseCulture("pt-BR")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(-2, "2 segundos atrás")]
        [InlineData(-1, "um segundo atrás")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "em um segundo")]
        [InlineData(2, "em 2 segundos")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 minutos atrás")]
        [InlineData(-1, "um minuto atrás")]
        [InlineData(60, "uma hora atrás")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "em um minuto")]
        [InlineData(2, "em 2 minutos")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 horas atrás")]
        [InlineData(-1, "uma hora atrás")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "em uma hora")]
        [InlineData(2, "em 2 horas")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 dias atrás")]
        [InlineData(-1, "ontem")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "amanhã")]
        [InlineData(2, "em 2 dias")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 meses atrás")]
        [InlineData(-1, "um mês atrás")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "em um mês")]
        [InlineData(2, "em 2 meses")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 anos atrás")]
        [InlineData(-1, "um ano atrás")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "em um ano")]
        [InlineData(2, "em 2 anos")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("agora", 0, TimeUnit.Day, Tense.Future);
        }
    }
}
