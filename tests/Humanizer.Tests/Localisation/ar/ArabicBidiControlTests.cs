namespace Humanizer.Tests.Localisation.ar;

[UseCulture("ar")]
public class ArabicBidiControlTests
{
    [Fact]
    public void RawLocalizedOutputs_DoNotContainBidiControls() =>
        RtlBidiControlSweep.AssertRawLocalizedOutputsDoNotContainBidiControls();
}