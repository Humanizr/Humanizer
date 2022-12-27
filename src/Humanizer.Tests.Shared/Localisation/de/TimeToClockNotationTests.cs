#if NET6_0_OR_GREATER

using System;
using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace Humanizer.Tests.Localisation.de;

[UseCulture("de-DE")]
[SuppressMessage("ReSharper", "StringLiteralTypo")]
public static class TimeToClockNotationTests
{
    #region [InlineData(0, 0, "")]

    [Theory]
    [InlineData(0, 0, "Mitternacht")]
    [InlineData(0, 7, "12 Uhr 7 nachts")]
    [InlineData(1, 11, "1 Uhr 11 nachts")]
    [InlineData(4, 0, "4 Uhr nachts")]
    [InlineData(5, 1, "5 Uhr 1 nachts")]
    [InlineData(6, 0, "6 Uhr morgens")]
    [InlineData(6, 5, "fünf nach 6 morgens")]
    [InlineData(7, 10, "zehn nach 7 morgens")]
    [InlineData(8, 15, "Viertel nach 8 morgens")]
    [InlineData(9, 20, "zwanzig nach 9 morgens")]
    [InlineData(10, 25, "fünf vor halb 11 morgens")]
    [InlineData(11, 30, "halb 12 morgens")]
    [InlineData(12, 00, "Mittag")]
    [InlineData(12, 38, "12 Uhr 38 nachmittags")]
    [InlineData(12, 35, "fünf nach halb 1 nachmittags")]
    [InlineData(15, 40, "zwanzig vor 4 nachmittags")]
    [InlineData(17, 45, "Viertel vor 6 nachmittags")]
    [InlineData(19, 50, "zehn vor 8 abends")]
    [InlineData(21, 0, "9 Uhr abends")]
    [InlineData(21, 55, "fünf vor 10 abends")]
    [InlineData(22, 59, "10 Uhr 59 abends")]
    [InlineData(23, 43, "11 Uhr 43 abends")]

    #endregion [InlineData(0, 0, "")]

    public static void ConvertToClockNotationTimeOnlyStringDe(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation();
        Assert.Equal(expectedResult, actualResult);
    }


    #region [InlineData(0, 0, "")]

    [Theory]
    [InlineData(0, 0, "Mitternacht")]
    [InlineData(0, 7, "fünf nach 12 nachts")]
    [InlineData(1, 11, "zehn nach 1 nachts")]
    [InlineData(4, 0, "4 Uhr nachts")]
    [InlineData(5, 1, "5 Uhr nachts")]
    [InlineData(6, 0, "6 Uhr morgens")]
    [InlineData(6, 5, "fünf nach 6 morgens")]
    [InlineData(7, 10, "zehn nach 7 morgens")]
    [InlineData(8, 15, "Viertel nach 8 morgens")]
    [InlineData(9, 20, "zwanzig nach 9 morgens")]
    [InlineData(10, 25, "fünf vor halb 11 morgens")]
    [InlineData(11, 30, "halb 12 morgens")]
    [InlineData(12, 00, "Mittag")]
    [InlineData(12, 38, "zwanzig vor 1 nachmittags")]
    [InlineData(12, 35, "fünf nach halb 1 nachmittags")]
    [InlineData(15, 40, "zwanzig vor 4 nachmittags")]
    [InlineData(17, 45, "Viertel vor 6 nachmittags")]
    [InlineData(19, 50, "zehn vor 8 abends")]
    [InlineData(21, 0, "9 Uhr abends")]
    [InlineData(21, 55, "fünf vor 10 abends")]
    [InlineData(22, 59, "11 Uhr abends")]
    [InlineData(23, 43, "Viertel vor 12 abends")]
    [InlineData(23, 58, "Mitternacht")]

    #endregion [InlineData(0, 0, "")]

    public static void ConvertToRoundedClockNotationTimeOnlyStringPtBr(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
        Assert.Equal(expectedResult, actualResult);
    }
}

#endif
