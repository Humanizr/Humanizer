namespace Humanizer.Tests.Localisation.mt;

#if NET6_0_OR_GREATER
[UseCulture("mt")]
public class MalteseClockNotationTests
{
    static readonly CultureInfo MtMt = new("mt-MT");

    [Theory]
    [InlineData(1, "is-siegħa ħamsa u minuta")]
    [InlineData(2, "is-siegħa ħamsa u tnejn")]
    [InlineData(21, "is-siegħa ħamsa u wieħed u għoxrin")]
    public void ToClockNotation_UsesTheOneMinuteConstructionWithoutChangingOtherMinuteValues(int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(5, minutes).ToClockNotation(ClockNotationRounding.None));
    }

    [Fact]
    public void ToClockNotation_RegionalCultureUsesMalteseProfile()
    {
        Assert.Equal(
            "is-siegħa ħamsa u minuta",
            new TimeOnly(5, 1).ToClockNotation(ClockNotationRounding.None, MtMt));
    }
}
#endif