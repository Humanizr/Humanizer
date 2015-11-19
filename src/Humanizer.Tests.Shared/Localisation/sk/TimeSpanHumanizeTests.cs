using System;
using Xunit;

namespace Humanizer.Tests.Localisation.sk
{
    [UseCulture("sk-SK")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [InlineData(1, "1 milisekunda")]
        [InlineData(2, "2 milisekundy")]
        [InlineData(3, "3 milisekundy")]
        [InlineData(4, "4 milisekundy")]
        [InlineData(5, "5 milisekúnd")]
        [InlineData(6, "6 milisekúnd")]
        [InlineData(10, "10 milisekúnd")]
        public void Milliseconds(int number, string expected)
        {            
            Assert.Equal(expected, TimeSpan.FromMilliseconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 sekunda")]
        [InlineData(2, "2 sekundy")]
        [InlineData(3, "3 sekundy")]
        [InlineData(4, "4 sekundy")]
        [InlineData(5, "5 sekúnd")]
        [InlineData(6, "6 sekúnd")]
        [InlineData(10, "10 sekúnd")]
        public void Seconds(int number, string expected)
        {            
            Assert.Equal(expected, TimeSpan.FromSeconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minúta")]
        [InlineData(2, "2 minúty")]
        [InlineData(3, "3 minúty")]
        [InlineData(4, "4 minúty")]
        [InlineData(5, "5 minút")]
        [InlineData(6, "6 minút")]
        [InlineData(10, "10 minút")]
        public void Minutes(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 hodina")]
        [InlineData(2, "2 hodiny")]
        [InlineData(3, "3 hodiny")]
        [InlineData(4, "4 hodiny")]
        [InlineData(5, "5 hodín")]
        [InlineData(6, "6 hodín")]
        [InlineData(10, "10 hodín")]
        public void Hours(int number, string expected)
        {            
            Assert.Equal(expected, TimeSpan.FromHours(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 deň")]
        [InlineData(2, "2 dni")]
        [InlineData(3, "3 dni")]
        [InlineData(4, "4 dni")]
        [InlineData(5, "5 dní")]
        [InlineData(6, "6 dní")]
        public void Days(int number, string expected)
        {            
            Assert.Equal(expected, TimeSpan.FromDays(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 týždeň")]
        [InlineData(2, "2 týždne")]
        [InlineData(3, "3 týždne")]
        [InlineData(4, "4 týždne")]
        [InlineData(5, "5 týždňov")]
        [InlineData(6, "6 týždňov")]
        public void Weeks(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number * 7).Humanize());
        }
    }
}
