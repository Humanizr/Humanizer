using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.ukUA
{
    [UseCulture("uk-UA")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "секунду тому")]
        [InlineData(2, "2 секунди тому")]
        [InlineData(3, "3 секунди тому")]
        [InlineData(4, "4 секунди тому")]
        [InlineData(5, "5 секунд тому")]
        [InlineData(6, "6 секунд тому")]
        [InlineData(10, "10 секунд тому")]
        [InlineData(11, "11 секунд тому")]
        [InlineData(19, "19 секунд тому")]
        [InlineData(20, "20 секунд тому")]
        [InlineData(21, "21 секунду тому")]
        [InlineData(22, "22 секунди тому")]
        [InlineData(23, "23 секунди тому")]
        [InlineData(24, "24 секунди тому")]
        [InlineData(25, "25 секунд тому")]
        [InlineData(40, "40 секунд тому")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "через секунду")]
        [InlineData(2, "через 2 секунди")]
        [InlineData(3, "через 3 секунди")]
        [InlineData(4, "через 4 секунди")]
        [InlineData(5, "через 5 секунд")]
        [InlineData(6, "через 6 секунд")]
        [InlineData(10, "через 10 секунд")]
        [InlineData(11, "через 11 секунд")]
        [InlineData(19, "через 19 секунд")]
        [InlineData(20, "через 20 секунд")]
        [InlineData(21, "через 21 секунду")]
        [InlineData(22, "через 22 секунди")]
        [InlineData(23, "через 23 секунди")]
        [InlineData(24, "через 24 секунди")]
        [InlineData(25, "через 25 секунд")]
        [InlineData(40, "через 40 секунд")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "хвилину тому")]
        [InlineData(2, "2 хвилини тому")]
        [InlineData(3, "3 хвилини тому")]
        [InlineData(4, "4 хвилини тому")]
        [InlineData(5, "5 хвилин тому")]
        [InlineData(6, "6 хвилин тому")]
        [InlineData(10, "10 хвилин тому")]
        [InlineData(11, "11 хвилин тому")]
        [InlineData(19, "19 хвилин тому")]
        [InlineData(20, "20 хвилин тому")]
        [InlineData(21, "21 хвилину тому")]
        [InlineData(22, "22 хвилини тому")]
        [InlineData(23, "23 хвилини тому")]
        [InlineData(24, "24 хвилини тому")]
        [InlineData(25, "25 хвилин тому")]
        [InlineData(40, "40 хвилин тому")]
        [InlineData(60, "годину тому")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "через хвилину")]
        [InlineData(2, "через 2 хвилини")]
        [InlineData(3, "через 3 хвилини")]
        [InlineData(4, "через 4 хвилини")]
        [InlineData(5, "через 5 хвилин")]
        [InlineData(6, "через 6 хвилин")]
        [InlineData(10, "через 10 хвилин")]
        [InlineData(11, "через 11 хвилин")]
        [InlineData(19, "через 19 хвилин")]
        [InlineData(20, "через 20 хвилин")]
        [InlineData(21, "через 21 хвилину")]
        [InlineData(22, "через 22 хвилини")]
        [InlineData(23, "через 23 хвилини")]
        [InlineData(24, "через 24 хвилини")]
        [InlineData(25, "через 25 хвилин")]
        [InlineData(40, "через 40 хвилин")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "годину тому")]
        [InlineData(2, "2 години тому")]
        [InlineData(3, "3 години тому")]
        [InlineData(4, "4 години тому")]
        [InlineData(5, "5 годин тому")]
        [InlineData(6, "6 годин тому")]
        [InlineData(10, "10 годин тому")]
        [InlineData(11, "11 годин тому")]
        [InlineData(19, "19 годин тому")]
        [InlineData(20, "20 годин тому")]
        [InlineData(21, "21 годину тому")]
        [InlineData(22, "22 години тому")]
        [InlineData(23, "23 години тому")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "через годину")]
        [InlineData(2, "через 2 години")]
        [InlineData(3, "через 3 години")]
        [InlineData(4, "через 4 години")]
        [InlineData(5, "через 5 годин")]
        [InlineData(6, "через 6 годин")]
        [InlineData(10, "через 10 годин")]
        [InlineData(11, "через 11 годин")]
        [InlineData(19, "через 19 годин")]
        [InlineData(20, "через 20 годин")]
        [InlineData(21, "через 21 годину")]
        [InlineData(22, "через 22 години")]
        [InlineData(23, "через 23 години")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "вчора")]
        [InlineData(2, "2 дні тому")]
        [InlineData(3, "3 дні тому")]
        [InlineData(4, "4 дні тому")]
        [InlineData(5, "5 днів тому")]
        [InlineData(6, "6 днів тому")]
        [InlineData(10, "10 днів тому")]
        [InlineData(11, "11 днів тому")]
        [InlineData(19, "19 днів тому")]
        [InlineData(20, "20 днів тому")]
        [InlineData(21, "21 день тому")]
        [InlineData(22, "22 дні тому")]
        [InlineData(23, "23 дні тому")]
        [InlineData(24, "24 дні тому")]
        [InlineData(25, "25 днів тому")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "завтра")]
        [InlineData(2, "через 2 дні")]
        [InlineData(3, "через 3 дні")]
        [InlineData(4, "через 4 дні")]
        [InlineData(5, "через 5 днів")]
        [InlineData(6, "через 6 днів")]
        [InlineData(10, "через 10 днів")]
        [InlineData(11, "через 11 днів")]
        [InlineData(19, "через 19 днів")]
        [InlineData(20, "через 20 днів")]
        [InlineData(21, "через 21 день")]
        [InlineData(22, "через 22 дні")]
        [InlineData(23, "через 23 дні")]
        [InlineData(24, "через 24 дні")]
        [InlineData(25, "через 25 днів")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "місяць тому")]
        [InlineData(2, "2 місяці тому")]
        [InlineData(3, "3 місяці тому")]
        [InlineData(4, "4 місяці тому")]
        [InlineData(5, "5 місяців тому")]
        [InlineData(6, "6 місяців тому")]
        [InlineData(10, "10 місяців тому")]
        [InlineData(11, "11 місяців тому")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "через місяць")]
        [InlineData(2, "через 2 місяці")]
        [InlineData(3, "через 3 місяці")]
        [InlineData(4, "через 4 місяці")]
        [InlineData(5, "через 5 місяців")]
        [InlineData(6, "через 6 місяців")]
        [InlineData(10, "через 10 місяців")]
        [InlineData(11, "через 11 місяців")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "рік тому")]
        [InlineData(2, "2 роки тому")]
        [InlineData(3, "3 роки тому")]
        [InlineData(4, "4 роки тому")]
        [InlineData(5, "5 років тому")]
        [InlineData(6, "6 років тому")]
        [InlineData(10, "10 років тому")]
        [InlineData(11, "11 років тому")]
        [InlineData(19, "19 років тому")]
        [InlineData(21, "21 рік тому")]
        [InlineData(111, "111 років тому")]
        [InlineData(121, "121 рік тому")]
        [InlineData(222, "222 роки тому")]
        [InlineData(325, "325 років тому")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "через рік")]
        [InlineData(2, "через 2 роки")]
        [InlineData(3, "через 3 роки")]
        [InlineData(4, "через 4 роки")]
        [InlineData(5, "через 5 років")]
        [InlineData(6, "через 6 років")]
        [InlineData(10, "через 10 років")]
        [InlineData(11, "через 11 років")]
        [InlineData(19, "через 19 років")]
        [InlineData(20, "через 20 років")]
        [InlineData(21, "через 21 рік")]
        [InlineData(111, "через 111 років")]
        [InlineData(121, "через 121 рік")]
        [InlineData(222, "через 222 роки")]
        [InlineData(325, "через 325 років")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("зараз", 0, TimeUnit.Day, Tense.Past);
        }
    }
}
