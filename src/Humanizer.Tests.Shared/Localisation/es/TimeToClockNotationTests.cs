#if NET6_0_OR_GREATER

using System;

using Xunit;

namespace Humanizer.Tests.Localisation.es
{
    [UseCulture("es-ES")]
    public class TimeToClockNotationTests
    {
        [Theory]
        [InlineData(0, 0, "medianoche")]
        [InlineData(0, 7, "las doce y siete de la noche")]
        [InlineData(1, 11, "la una y once de la madrugada")]
        [InlineData(4, 0, "las cuatro de la madrugada")]
        [InlineData(5, 1, "las cinco y uno de la madrugada")]
        [InlineData(6, 0, "las seis de la mañana")]
        [InlineData(6, 5, "las seis y cinco de la mañana")]
        [InlineData(7, 10, "las siete y diez de la mañana")]
        [InlineData(8, 15, "las ocho y cuarto de la mañana")]
        [InlineData(9, 20, "las nueve y veinte de la mañana")]
        [InlineData(10, 25, "las diez y veinticinco de la mañana")]
        [InlineData(11, 30, "las once y media de la mañana")]
        [InlineData(12, 00, "mediodía")]
        [InlineData(12, 38, "las doce y treinta y ocho de la tarde")]
        [InlineData(12, 35, "la una menos veinticinco de la tarde")]
        [InlineData(15, 40, "las cuatro menos veinte de la tarde")]
        [InlineData(17, 45, "las seis menos cuarto de la tarde")]
        [InlineData(19, 50, "las ocho menos diez de la tarde")]
        [InlineData(21, 0, "las nueve de la noche")]
        [InlineData(21, 55, "las diez menos cinco de la noche")]
        [InlineData(22, 59, "las diez y cincuenta y nueve de la noche")]
        [InlineData(23, 43, "las once y cuarenta y tres de la noche")]
        public void ConvertToClockNotationTimeOnlyString(int hours, int minutes, string expectedResult)
        {
            var actualResult = new TimeOnly(hours, minutes).ToClockNotation();
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0, 0, "medianoche")]
        [InlineData(0, 7, "las doce y cinco de la noche")]
        [InlineData(1, 11, "la una y diez de la madrugada")]
        [InlineData(4, 0, "las cuatro de la madrugada")]
        [InlineData(5, 1, "las cinco de la madrugada")]
        [InlineData(6, 0, "las seis de la mañana")]
        [InlineData(6, 5, "las seis y cinco de la mañana")]
        [InlineData(7, 10, "las siete y diez de la mañana")]
        [InlineData(8, 15, "las ocho y cuarto de la mañana")]
        [InlineData(9, 20, "las nueve y veinte de la mañana")]
        [InlineData(10, 25, "las diez y veinticinco de la mañana")]
        [InlineData(11, 30, "las once y media de la mañana")]
        [InlineData(12, 00, "mediodía")]
        [InlineData(12, 38, "la una menos veinte de la tarde")]
        [InlineData(12, 35, "la una menos veinticinco de la tarde")]
        [InlineData(15, 40, "las cuatro menos veinte de la tarde")]
        [InlineData(17, 45, "las seis menos cuarto de la tarde")]
        [InlineData(19, 50, "las ocho menos diez de la tarde")]
        [InlineData(21, 0, "las nueve de la noche")]
        [InlineData(21, 55, "las diez menos cinco de la noche")]
        [InlineData(22, 59, "las once de la noche")]
        [InlineData(23, 43, "las doce menos cuarto de la noche")]
        public void ConvertToRoundedClockNotationTimeOnlyString(int hours, int minutes, string expectedResult)
        {
            var actualResult = new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}

#endif
