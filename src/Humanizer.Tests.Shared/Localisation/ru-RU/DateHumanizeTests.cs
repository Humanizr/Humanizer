using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.ruRU
{
    [UseCulture("ru-RU")]
    public class DateHumanizeTests
    {

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
        [InlineData(1, "через секунду")]
        [InlineData(2, "через 2 секунды")]
        [InlineData(3, "через 3 секунды")]
        [InlineData(4, "через 4 секунды")]
        [InlineData(5, "через 5 секунд")]
        [InlineData(6, "через 6 секунд")]
        [InlineData(10, "через 10 секунд")]
        [InlineData(11, "через 11 секунд")]
        [InlineData(19, "через 19 секунд")]
        [InlineData(20, "через 20 секунд")]
        [InlineData(21, "через 21 секунду")]
        [InlineData(22, "через 22 секунды")]
        [InlineData(23, "через 23 секунды")]
        [InlineData(24, "через 24 секунды")]
        [InlineData(25, "через 25 секунд")]
        [InlineData(40, "через 40 секунд")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
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
        [InlineData(60, "час назад")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "через минуту")]
        [InlineData(2, "через 2 минуты")]
        [InlineData(3, "через 3 минуты")]
        [InlineData(4, "через 4 минуты")]
        [InlineData(5, "через 5 минут")]
        [InlineData(6, "через 6 минут")]
        [InlineData(10, "через 10 минут")]
        [InlineData(11, "через 11 минут")]
        [InlineData(19, "через 19 минут")]
        [InlineData(20, "через 20 минут")]
        [InlineData(21, "через 21 минуту")]
        [InlineData(22, "через 22 минуты")]
        [InlineData(23, "через 23 минуты")]
        [InlineData(24, "через 24 минуты")]
        [InlineData(25, "через 25 минут")]
        [InlineData(40, "через 40 минут")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
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
        [InlineData(1, "через час")]
        [InlineData(2, "через 2 часа")]
        [InlineData(3, "через 3 часа")]
        [InlineData(4, "через 4 часа")]
        [InlineData(5, "через 5 часов")]
        [InlineData(6, "через 6 часов")]
        [InlineData(10, "через 10 часов")]
        [InlineData(11, "через 11 часов")]
        [InlineData(19, "через 19 часов")]
        [InlineData(20, "через 20 часов")]
        [InlineData(21, "через 21 час")]
        [InlineData(22, "через 22 часа")]
        [InlineData(23, "через 23 часа")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
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
        [InlineData(1, "завтра")]
        [InlineData(2, "через 2 дня")]
        [InlineData(3, "через 3 дня")]
        [InlineData(4, "через 4 дня")]
        [InlineData(5, "через 5 дней")]
        [InlineData(6, "через 6 дней")]
        [InlineData(10, "через 10 дней")]
        [InlineData(11, "через 11 дней")]
        [InlineData(19, "через 19 дней")]
        [InlineData(20, "через 20 дней")]
        [InlineData(21, "через 21 день")]
        [InlineData(22, "через 22 дня")]
        [InlineData(23, "через 23 дня")]
        [InlineData(24, "через 24 дня")]
        [InlineData(25, "через 25 дней")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
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
        [InlineData(1, "через месяц")]
        [InlineData(2, "через 2 месяца")]
        [InlineData(3, "через 3 месяца")]
        [InlineData(4, "через 4 месяца")]
        [InlineData(5, "через 5 месяцев")]
        [InlineData(6, "через 6 месяцев")]
        [InlineData(10, "через 10 месяцев")]
        [InlineData(11, "через 11 месяцев")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
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

        [Theory]
        [InlineData(1, "через год")]
        [InlineData(2, "через 2 года")]
        [InlineData(3, "через 3 года")]
        [InlineData(4, "через 4 года")]
        [InlineData(5, "через 5 лет")]
        [InlineData(6, "через 6 лет")]
        [InlineData(10, "через 10 лет")]
        [InlineData(11, "через 11 лет")]
        [InlineData(19, "через 19 лет")]
        [InlineData(20, "через 20 лет")]
        [InlineData(21, "через 21 год")]
        [InlineData(111, "через 111 лет")]
        [InlineData(121, "через 121 год")]
        [InlineData(222, "через 222 года")]
        [InlineData(325, "через 325 лет")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("сейчас", 0, TimeUnit.Day, Tense.Past);
        }
    }
}
