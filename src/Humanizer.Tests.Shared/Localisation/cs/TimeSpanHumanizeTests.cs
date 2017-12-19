using System;
using Xunit;

namespace Humanizer.Tests.Localisation.cs
{
    [UseCulture("cs-CZ")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 rok")]
        [InlineData(731, "2 roky")]
        [InlineData(1096, "3 roky")]
        [InlineData(4018, "11 let")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 měsíc")]
        [InlineData(61, "2 měsíce")]
        [InlineData(92, "3 měsíce")]
        [InlineData(335, "11 měsíců")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(1, "1 milisekunda")]
        [InlineData(2, "2 milisekundy")]
        [InlineData(3, "3 milisekundy")]
        [InlineData(4, "4 milisekundy")]
        [InlineData(5, "5 milisekund")]
        [InlineData(6, "6 milisekund")]
        [InlineData(10, "10 milisekund")]
        public void Milliseconds(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 sekunda")]
        [InlineData(2, "2 sekundy")]
        [InlineData(3, "3 sekundy")]
        [InlineData(4, "4 sekundy")]
        [InlineData(5, "5 sekund")]
        [InlineData(6, "6 sekund")]
        [InlineData(10, "10 sekund")]
        public void Seconds(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minuta")]
        [InlineData(2, "2 minuty")]
        [InlineData(3, "3 minuty")]
        [InlineData(4, "4 minuty")]
        [InlineData(5, "5 minut")]
        [InlineData(6, "6 minut")]
        [InlineData(10, "10 minut")]
        public void Minutes(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 hodina")]
        [InlineData(2, "2 hodiny")]
        [InlineData(3, "3 hodiny")]
        [InlineData(4, "4 hodiny")]
        [InlineData(5, "5 hodin")]
        [InlineData(6, "6 hodin")]
        [InlineData(10, "10 hodin")]
        public void Hours(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 den")]
        [InlineData(2, "2 dny")]
        [InlineData(3, "3 dny")]
        [InlineData(4, "4 dny")]
        [InlineData(5, "5 dnů")]
        [InlineData(6, "6 dnů")]
        public void Days(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 týden")]
        [InlineData(2, "2 týdny")]
        [InlineData(3, "3 týdny")]
        [InlineData(4, "4 týdny")]
        [InlineData(5, "5 týdnů")]
        [InlineData(6, "6 týdnů")]
        public void Weeks(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number * 7).Humanize());
        }
    }
}
