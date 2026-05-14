namespace Humanizer.Tests.Localisation.ig;

#if NET6_0_OR_GREATER
[UseCulture("ig")]
public class IgboClockNotationTests
{
    // Igbo day-period vocabulary sources: https://www.teachyourselfigbo.com/igbo-times-of-day.php and
    // https://ezinaulo.com/igbo-lessons/vocabulary/time-date-seasons/.
    [Theory]
    [InlineData(1, 0, "elekere otu n'ụtụtụ")]
    [InlineData(1, 5, "elekere otu na nkeji ise n'ụtụtụ")]
    [InlineData(13, 23, "elekere otu na nkeji iri abụọ na atọ n'ehihie")]
    [InlineData(20, 0, "elekere asatọ n'anyasị")]
    [InlineData(23, 40, "elekere iri na otu na nkeji iri anọ n'abalị")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("elekere otu na nkeji iri abụọ na ise n'ehihie", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}
#endif