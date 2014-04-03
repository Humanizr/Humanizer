using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation
{
    public class SlovakDateHumanizeTests : AmbientCulture
    {
        public SlovakDateHumanizeTests()
            : base("sk-SK")
        {
        }

        [Theory]
        [InlineData(1, "o sekundu")]
        [InlineData(2, "o 2 sekundy")]
        [InlineData(3, "o 3 sekundy")]
        [InlineData(4, "o 4 sekundy")]
        [InlineData(5, "o 5 sekúnd")]
        [InlineData(6, "o 6 sekúnd")]
        [InlineData(10, "o 10 sekúnd")]
        public void NSecondsFromNow(int number, string expected)
        {
            var humanize = DateTime.UtcNow.AddSeconds(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "o minútu")]
        [InlineData(2, "o 2 minúty")]
        [InlineData(3, "o 3 minúty")]
        [InlineData(4, "o 4 minúty")]
        [InlineData(5, "o 5 minút")]
        [InlineData(6, "o 6 minút")]
        [InlineData(10, "o 10 minút")]
        public void NMinutesFromNow(int number, string expected)
        {
            var humanize = DateTime.UtcNow.AddMinutes(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "o hodinu")]
        [InlineData(2, "o 2 hodiny")]
        [InlineData(3, "o 3 hodiny")]
        [InlineData(4, "o 4 hodiny")]
        [InlineData(5, "o 5 hodín")]
        [InlineData(6, "o 6 hodín")]
        [InlineData(10, "o 10 hodín")]
        public void NHoursFromNow(int number, string expected)
        {
            var humanize = DateTime.UtcNow.AddHours(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "zajtra")]
        [InlineData(2, "o 2 dni")]
        [InlineData(3, "o 3 dni")]
        [InlineData(4, "o 4 dni")]
        [InlineData(9, "o 9 dní")]        
        [InlineData(10, "o 10 dní")]
        public void NDayFromNow(int number, string expected)
        {
            var humanize = DateTime.UtcNow.AddDays(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "o mesiac")]
        [InlineData(2, "o 2 mesiace")]
        [InlineData(3, "o 3 mesiace")]
        [InlineData(4, "o 4 mesiace")]
        [InlineData(5, "o 5 mesiacov")]
        [InlineData(6, "o 6 mesiacov")]
        [InlineData(10, "o 10 mesiacov")]
        public void NMonthsFromNow(int number, string expected)
        {
            var humanize = DateTime.UtcNow.AddMonths(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "o rok")]
        [InlineData(2, "o 2 roky")]
        [InlineData(3, "o 3 roky")]
        [InlineData(4, "o 4 roky")]
        [InlineData(5, "o 5 rokov")]
        [InlineData(6, "o 6 rokov")]
        [InlineData(10, "o 10 rokov")]
        public void NYearsFromNow(int number, string expected)
        {
            var humanize = DateTime.UtcNow.AddYears(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "1 milisekunda")]
        [InlineData(2, "2 milisekundy")]
        [InlineData(3, "3 milisekundy")]
        [InlineData(4, "4 milisekundy")]
        [InlineData(5, "5 milisekúnd")]
        [InlineData(6, "6 milisekúnd")]
        [InlineData(10, "10 milisekúnd")]
        public void NMiliseconds(int number, string expected)
        {
            var humanize = TimeSpan.FromMilliseconds(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "1 sekunda")]
        [InlineData(2, "2 sekundy")]
        [InlineData(3, "3 sekundy")]
        [InlineData(4, "4 sekundy")]
        [InlineData(5, "5 sekúnd")]
        [InlineData(6, "6 sekúnd")]
        [InlineData(10, "10 sekúnd")]
        public void NSeconds(int number, string expected)
        {
            var humanize = TimeSpan.FromSeconds(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "1 minúta")]
        [InlineData(2, "2 minúty")]
        [InlineData(3, "3 minúty")]
        [InlineData(4, "4 minúty")]
        [InlineData(5, "5 minút")]
        [InlineData(6, "6 minút")]
        [InlineData(10, "10 minút")]
        public void NMinutes(int number, string expected)
        {
            var humanize = TimeSpan.FromMinutes(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "1 hodina")]
        [InlineData(2, "2 hodiny")]
        [InlineData(3, "3 hodiny")]
        [InlineData(4, "4 hodiny")]
        [InlineData(5, "5 hodín")]
        [InlineData(6, "6 hodín")]
        [InlineData(10, "10 hodín")]
        public void NHours(int number, string expected)
        {
            var humanize = TimeSpan.FromHours(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "1 deň")]
        [InlineData(2, "2 dni")]
        [InlineData(3, "3 dni")]
        [InlineData(4, "4 dni")]
        [InlineData(5, "5 dní")]
        [InlineData(6, "6 dní")]        
        public void NDays(int number, string expected)
        {
            var humanize = TimeSpan.FromDays(number).Humanize();
            Assert.Equal(expected, humanize);
        }

        [Theory]
        [InlineData(1, "1 týždeň")]
        [InlineData(2, "2 týždne")]
        [InlineData(3, "3 týždne")]
        [InlineData(4, "4 týždne")]
        [InlineData(5, "5 týždňov")]
        [InlineData(6, "6 týždňov")]
        public void NWeeks(int number, string expected)
        {
            var humanize = TimeSpan.FromDays(number*7).Humanize();
            Assert.Equal(expected, humanize);
        }

        
    }
}
