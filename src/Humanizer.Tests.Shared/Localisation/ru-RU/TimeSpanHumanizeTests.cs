using System;
using Xunit;

namespace Humanizer.Tests.Localisation.ruRU
{
    [UseCulture("ru-RU")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "один год")]
        [InlineData(731, "2 года")]
        [InlineData(1096, "3 года")]
        [InlineData(4018, "11 лет")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "один месяц")]
        [InlineData(61, "2 месяца")]
        [InlineData(92, "3 месяца")]
        [InlineData(335, "11 месяцев")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "одна неделя")]
        [InlineData(14, "2 недели")]
        [InlineData(21, "3 недели")]
        [InlineData(28, "4 недели")]
        [InlineData(35, "5 недель")]
        [InlineData(77, "11 недель")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "один день")]
        [InlineData(2, "2 дня")]
        [InlineData(3, "3 дня")]
        [InlineData(4, "4 дня")]
        [InlineData(5, "5 дней")]
        [InlineData(6, "6 дней")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "один час")]
        [InlineData(2, "2 часа")]
        [InlineData(3, "3 часа")]
        [InlineData(4, "4 часа")]
        [InlineData(5, "5 часов")]
        [InlineData(6, "6 часов")]
        [InlineData(10, "10 часов")]
        [InlineData(11, "11 часов")]
        [InlineData(19, "19 часов")]
        [InlineData(20, "20 часов")]
        [InlineData(21, "21 час")]
        [InlineData(22, "22 часа")]
        [InlineData(23, "23 часа")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "одна минута")]
        [InlineData(2, "2 минуты")]
        [InlineData(3, "3 минуты")]
        [InlineData(4, "4 минуты")]
        [InlineData(5, "5 минут")]
        [InlineData(6, "6 минут")]
        [InlineData(10, "10 минут")]
        [InlineData(11, "11 минут")]
        [InlineData(19, "19 минут")]
        [InlineData(20, "20 минут")]
        [InlineData(21, "21 минута")]
        [InlineData(22, "22 минуты")]
        [InlineData(23, "23 минуты")]
        [InlineData(24, "24 минуты")]
        [InlineData(25, "25 минут")]
        [InlineData(40, "40 минут")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "одна секунда")]
        [InlineData(2, "2 секунды")]
        [InlineData(3, "3 секунды")]
        [InlineData(4, "4 секунды")]
        [InlineData(5, "5 секунд")]
        [InlineData(6, "6 секунд")]
        [InlineData(10, "10 секунд")]
        [InlineData(11, "11 секунд")]
        [InlineData(19, "19 секунд")]
        [InlineData(20, "20 секунд")]
        [InlineData(21, "21 секунда")]
        [InlineData(22, "22 секунды")]
        [InlineData(23, "23 секунды")]
        [InlineData(24, "24 секунды")]
        [InlineData(25, "25 секунд")]
        [InlineData(40, "40 секунд")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "одна миллисекунда")]
        [InlineData(2, "2 миллисекунды")]
        [InlineData(3, "3 миллисекунды")]
        [InlineData(4, "4 миллисекунды")]
        [InlineData(5, "5 миллисекунд")]
        [InlineData(6, "6 миллисекунд")]
        [InlineData(10, "10 миллисекунд")]
        [InlineData(11, "11 миллисекунд")]
        [InlineData(19, "19 миллисекунд")]
        [InlineData(20, "20 миллисекунд")]
        [InlineData(21, "21 миллисекунда")]
        [InlineData(22, "22 миллисекунды")]
        [InlineData(23, "23 миллисекунды")]
        [InlineData(24, "24 миллисекунды")]
        [InlineData(25, "25 миллисекунд")]
        [InlineData(40, "40 миллисекунд")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 миллисекунд", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("нет времени", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
