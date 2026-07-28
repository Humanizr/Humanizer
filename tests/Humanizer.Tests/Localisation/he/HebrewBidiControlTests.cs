namespace Humanizer.Tests.Localisation.he;

[UseCulture("he")]
public class HebrewBidiControlTests
{
    [Fact]
    public void RawLocalizedOutputs_DoNotContainBidiControls() =>
        RtlBidiControlSweep.AssertRawLocalizedOutputsDoNotContainBidiControls();
}