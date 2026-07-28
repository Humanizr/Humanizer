namespace Humanizer.Tests.Localisation.fa;

[UseCulture("fa")]
public class PersianBidiControlTests
{
    [Fact]
    public void RawLocalizedOutputs_DoNotContainBidiControls() =>
        RtlBidiControlSweep.AssertRawLocalizedOutputsDoNotContainBidiControls();

    [Fact]
    public void Output_PreservesZeroWidthNonJoiner()
    {
        var culture = CultureInfo.GetCultureInfo("fa");
        var value = TimeSpan.FromMilliseconds(2).Humanize(culture: culture);

        Assert.Equal("2 میلی\u200Cثانیه", value);
        Assert.Contains('\u200C', value);
        RtlBidiControlSweep.AssertNoBidiControls("millisecond duration", value);
    }
}