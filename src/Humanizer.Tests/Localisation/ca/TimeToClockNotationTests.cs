#if NET6_0_OR_GREATER

namespace ca;

[UseCulture("ca")]
public class TimeToClockNotationTests
{
    [Theory]
    [InlineData(0, 0, "mitjanit")]
    [InlineData(0, 7, "les dotze i set de la nit")]
    [InlineData(1, 11, "la una i onze de la matinada")]
    [InlineData(4, 0, "les quatre de la matinada")]
    [InlineData(5, 1, "les cinc i un de la matinada")]
    [InlineData(6, 0, "les sis del matí")]
    [InlineData(6, 5, "les sis i cinc del matí")]
    [InlineData(7, 10, "les set i deu del matí")]
    [InlineData(8, 15, "les vuit i quart del matí")]
    [InlineData(9, 20, "les nou i vint del matí")]
    [InlineData(10, 25, "les deu i vint-i-cinc del matí")]
    [InlineData(11, 30, "les onze i mitja del matí")]
    [InlineData(12, 00, "migdia")]
    [InlineData(12, 38, "les dotze i trenta-vuit de la tarda")]
    [InlineData(12, 35, "la una menys vint-i-cinc de la tarda")]
    [InlineData(15, 40, "les quatre menys vint de la tarda")]
    [InlineData(17, 45, "les sis menys quart de la tarda")]
    [InlineData(19, 50, "les vuit menys deu de la tarda")]
    [InlineData(21, 0, "les nou de la nit")]
    [InlineData(21, 55, "les deu menys cinc de la nit")]
    [InlineData(22, 59, "les deu i cinquanta-nou de la nit")]
    [InlineData(23, 43, "les onze i quaranta-tres de la nit")]
    public void ConvertToClockNotationTimeOnlyString(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation();
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(0, 0, "mitjanit")]
    [InlineData(0, 7, "les dotze i cinc de la nit")]
    [InlineData(1, 11, "la una i deu de la matinada")]
    [InlineData(4, 0, "les quatre de la matinada")]
    [InlineData(5, 1, "les cinc de la matinada")]
    [InlineData(6, 0, "les sis del matí")]
    [InlineData(6, 5, "les sis i cinc del matí")]
    [InlineData(7, 10, "les set i deu del matí")]
    [InlineData(8, 15, "les vuit i quart del matí")]
    [InlineData(9, 20, "les nou i vint del matí")]
    [InlineData(10, 25, "les deu i vint-i-cinc del matí")]
    [InlineData(11, 30, "les onze i mitja del matí")]
    [InlineData(12, 00, "migdia")]
    [InlineData(12, 38, "la una menys vint de la tarda")]
    [InlineData(12, 35, "la una menys vint-i-cinc de la tarda")]
    [InlineData(15, 40, "les quatre menys vint de la tarda")]
    [InlineData(17, 45, "les sis menys quart de la tarda")]
    [InlineData(19, 50, "les vuit menys deu de la tarda")]
    [InlineData(21, 0, "les nou de la nit")]
    [InlineData(21, 55, "les deu menys cinc de la nit")]
    [InlineData(22, 59, "les onze de la nit")]
    [InlineData(23, 43, "les dotze menys quart de la nit")]
    public void ConvertToRoundedClockNotationTimeOnlyString(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
        Assert.Equal(expectedResult, actualResult);
    }
}

#endif