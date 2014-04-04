using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.cs
{
    public class DateTimeHumanizeTests : AmbientCulture
    {
        public DateTimeHumanizeTests()
            : base("cs-CZ")
        {
        }

        [Theory]
        [InlineData(1, "o sekundu")]
        [InlineData(2, "o 2 sekundy")]
        [InlineData(3, "o 3 sekundy")]
        [InlineData(4, "o 4 sekundy")]
        [InlineData(5, "o 5 sekund")]
        [InlineData(6, "o 6 sekund")]
        [InlineData(10, "o 10 sekund")]
        public void SecondsFromNow(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "o minutu")]
        [InlineData(2, "o 2 minuty")]
        [InlineData(3, "o 3 minuty")]
        [InlineData(4, "o 4 minuty")]
        [InlineData(5, "o 5 minut")]
        [InlineData(6, "o 6 minut")]
        [InlineData(10, "o 10 minut")]
        public void MinutesFromNow(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(number).Humanize());
        }

        [Theory]
        [InlineData(1, "o hodinu")]
        [InlineData(2, "o 2 hodiny")]
        [InlineData(3, "o 3 hodiny")]
        [InlineData(4, "o 4 hodiny")]
        [InlineData(5, "o 5 hodin")]
        [InlineData(6, "o 6 hodin")]
        [InlineData(10, "o 10 hodin")]
        public void HoursFromNow(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(number).Humanize());
        }

        [Theory]
        [InlineData(1, "zítra")]
        [InlineData(2, "o 2 dny")]
        [InlineData(3, "o 3 dny")]
        [InlineData(4, "o 4 dny")]
        [InlineData(9, "o 9 dnů")]
        [InlineData(10, "o 10 dnů")]
        public void DayFromNow(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(number).Humanize());
        }

        [Theory]
        [InlineData(1, "o měsíc")]
        [InlineData(2, "o 2 měsíce")]
        [InlineData(3, "o 3 měsíce")]
        [InlineData(4, "o 4 měsíce")]
        [InlineData(5, "o 5 měsíců")]
        [InlineData(6, "o 6 měsíců")]
        [InlineData(10, "o 10 měsíců")]
        public void MonthsFromNow(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(number).Humanize());
        }

        [Theory]
        [InlineData(1, "o rok")]
        [InlineData(2, "o 2 roky")]
        [InlineData(3, "o 3 roky")]
        [InlineData(4, "o 4 roky")]
        [InlineData(5, "o 5 roků")]
        [InlineData(6, "o 6 roků")]
        [InlineData(10, "o 10 roků")]
        public void YearsFromNow(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(number).Humanize());
        }

        [Theory]
        [InlineData(1, "před sekundou")]
        [InlineData(2, "před 2 sekundami")]
        [InlineData(3, "před 3 sekundami")]
        [InlineData(4, "před 4 sekundami")]
        [InlineData(5, "před 5 sekundami")]
        [InlineData(6, "před 6 sekundami")]
        [InlineData(10, "před 10 sekundami")]
        public void SecondsAgo(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(-1 * number).Humanize());
        }

        [Theory]
        [InlineData(1, "před minutou")]
        [InlineData(2, "před 2 minutami")]
        [InlineData(3, "před 3 minutami")]
        [InlineData(4, "před 4 minutami")]
        [InlineData(5, "před 5 minutami")]
        [InlineData(6, "před 6 minutami")]
        [InlineData(10, "před 10 minutami")]
        public void MinutesAgo(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(-1 * number).Humanize());
        }

        [Theory]
        [InlineData(1, "před hodinou")]
        [InlineData(2, "před 2 hodinami")]
        [InlineData(3, "před 3 hodinami")]
        [InlineData(4, "před 4 hodinami")]
        [InlineData(5, "před 5 hodinami")]
        [InlineData(6, "před 6 hodinami")]
        [InlineData(10, "před 10 hodinami")]
        public void HoursAgo(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(-1 * number).Humanize());
        }

        [Theory]
        [InlineData(1, "včera")]
        [InlineData(2, "před 2 dny")]
        [InlineData(3, "před 3 dny")]
        [InlineData(4, "před 4 dny")]
        [InlineData(9, "před 9 dny")]
        [InlineData(10, "před 10 dny")]
        public void DayAgo(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(-1 * number).Humanize());
        }

        [Theory]
        [InlineData(1, "před měsícem")]
        [InlineData(2, "před 2 měsíci")]
        [InlineData(3, "před 3 měsíci")]
        [InlineData(4, "před 4 měsíci")]
        [InlineData(5, "před 5 měsíci")]
        [InlineData(6, "před 6 měsíci")]
        [InlineData(10, "před 10 měsíci")]
        public void MonthsAgo(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(-1 * number).Humanize());
        }

        [Theory]
        [InlineData(1, "před rokem")]
        [InlineData(2, "před 2 roky")]
        [InlineData(3, "před 3 roky")]
        [InlineData(4, "před 4 roky")]
        [InlineData(5, "před 5 roky")]
        [InlineData(6, "před 6 roky")]
        [InlineData(10, "před 10 roky")]
        public void YearsAgo(int number, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(-1 * number).Humanize());
        }
    }
}
