using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.sk
{
    public class DateTimeHumanizeTests : AmbientCulture
    {
        public DateTimeHumanizeTests()
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
        public void SecondsFromNow(int number, string expected)
        {            
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "o minútu")]
        [InlineData(2, "o 2 minúty")]
        [InlineData(3, "o 3 minúty")]
        [InlineData(4, "o 4 minúty")]
        [InlineData(5, "o 5 minút")]
        [InlineData(6, "o 6 minút")]
        [InlineData(10, "o 10 minút")]
        public void MinutesFromNow(int number, string expected)
        {            
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(number).Humanize());
        }

        [Theory]
        [InlineData(1, "o hodinu")]
        [InlineData(2, "o 2 hodiny")]
        [InlineData(3, "o 3 hodiny")]
        [InlineData(4, "o 4 hodiny")]
        [InlineData(5, "o 5 hodín")]
        [InlineData(6, "o 6 hodín")]
        [InlineData(10, "o 10 hodín")]
        public void HoursFromNow(int number, string expected)
        {            
            Assert.Equal(expected, DateTime.UtcNow.AddHours(number).Humanize());
        }

        [Theory]
        [InlineData(1, "zajtra")]
        [InlineData(2, "o 2 dni")]
        [InlineData(3, "o 3 dni")]
        [InlineData(4, "o 4 dni")]
        [InlineData(9, "o 9 dní")]        
        [InlineData(10, "o 10 dní")]
        public void DayFromNow(int number, string expected)
        {            
            Assert.Equal(expected, DateTime.UtcNow.AddDays(number).Humanize());
        }

        [Theory]
        [InlineData(1, "o mesiac")]
        [InlineData(2, "o 2 mesiace")]
        [InlineData(3, "o 3 mesiace")]
        [InlineData(4, "o 4 mesiace")]
        [InlineData(5, "o 5 mesiacov")]
        [InlineData(6, "o 6 mesiacov")]
        [InlineData(10, "o 10 mesiacov")]
        public void MonthsFromNow(int number, string expected)
        {            
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(number).Humanize());
        }

        [Theory]
        [InlineData(1, "o rok")]
        [InlineData(2, "o 2 roky")]
        [InlineData(3, "o 3 roky")]
        [InlineData(4, "o 4 roky")]
        [InlineData(5, "o 5 rokov")]
        [InlineData(6, "o 6 rokov")]
        [InlineData(10, "o 10 rokov")]
        public void YearsFromNow(int number, string expected)
        {            
            Assert.Equal(expected, DateTime.UtcNow.AddYears(number).Humanize());
        }              
    }
}
