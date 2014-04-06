using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ruRU
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("ru-RU")
        {
        }

        [Theory]
        [InlineData(1, "секунду назад")]
        [InlineData(2, "2 секунды назад")]
        [InlineData(3, "3 секунды назад")]
        [InlineData(4, "4 секунды назад")]
        [InlineData(5, "5 секунд назад")]
        [InlineData(6, "6 секунд назад")]
        [InlineData(10, "10 секунд назад")]
        [InlineData(11, "11 секунд назад")]
        [InlineData(19, "19 секунд назад")]
        [InlineData(20, "20 секунд назад")]
        [InlineData(21, "21 секунду назад")]
        [InlineData(22, "22 секунды назад")]
        [InlineData(23, "23 секунды назад")]
        [InlineData(24, "24 секунды назад")]
        [InlineData(25, "25 секунд назад")]
        [InlineData(40, "40 секунд назад")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "минуту назад")]
        [InlineData(2, "2 минуты назад")]
        [InlineData(3, "3 минуты назад")]
        [InlineData(4, "4 минуты назад")]
        [InlineData(5, "5 минут назад")]
        [InlineData(6, "6 минут назад")]
        [InlineData(10, "10 минут назад")]
        [InlineData(11, "11 минут назад")]
        [InlineData(19, "19 минут назад")]
        [InlineData(20, "20 минут назад")]
        [InlineData(21, "21 минуту назад")]
        [InlineData(22, "22 минуты назад")]
        [InlineData(23, "23 минуты назад")]
        [InlineData(24, "24 минуты назад")]
        [InlineData(25, "25 минут назад")]
        [InlineData(40, "40 минут назад")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "час назад")]
        [InlineData(2, "2 часа назад")]
        [InlineData(3, "3 часа назад")]
        [InlineData(4, "4 часа назад")]
        [InlineData(5, "5 часов назад")]
        [InlineData(6, "6 часов назад")]
        [InlineData(10, "10 часов назад")]
        [InlineData(11, "11 часов назад")]
        [InlineData(19, "19 часов назад")]
        [InlineData(20, "20 часов назад")]
        [InlineData(21, "21 час назад")]
        [InlineData(22, "22 часа назад")]
        [InlineData(23, "23 часа назад")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "вчера")]
        [InlineData(2, "2 дня назад")]
        [InlineData(3, "3 дня назад")]
        [InlineData(4, "4 дня назад")]
        [InlineData(5, "5 дней назад")]
        [InlineData(6, "6 дней назад")]
        [InlineData(10, "10 дней назад")]
        [InlineData(11, "11 дней назад")]
        [InlineData(19, "19 дней назад")]
        [InlineData(20, "20 дней назад")]
        [InlineData(21, "21 день назад")]
        [InlineData(22, "22 дня назад")]
        [InlineData(23, "23 дня назад")]
        [InlineData(24, "24 дня назад")]
        [InlineData(25, "25 дней назад")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "месяц назад")]
        [InlineData(2, "2 месяца назад")]
        [InlineData(3, "3 месяца назад")]
        [InlineData(4, "4 месяца назад")]
        [InlineData(5, "5 месяцев назад")]
        [InlineData(6, "6 месяцев назад")]
        [InlineData(10, "10 месяцев назад")]
        [InlineData(11, "11 месяцев назад")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }
        
        [Theory]
        [InlineData(1, "год назад")]
        [InlineData(2, "2 года назад")]
        [InlineData(3, "3 года назад")]
        [InlineData(4, "4 года назад")]
        [InlineData(5, "5 лет назад")]
        [InlineData(6, "6 лет назад")]
        [InlineData(10, "10 лет назад")]
        [InlineData(11, "11 лет назад")]
        [InlineData(19, "19 лет назад")]
        [InlineData(21, "21 год назад")]
        [InlineData(111, "111 лет назад")]
        [InlineData(121, "121 год назад")]
        [InlineData(222, "222 года назад")]
        [InlineData(325, "325 лет назад")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
