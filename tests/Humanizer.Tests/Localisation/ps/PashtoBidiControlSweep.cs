namespace Humanizer.Tests.Localisation.ps;

static class PashtoBidiControlSweep
{
    const char LeftToRightMark = '\u200E';
    const char RightToLeftMark = '\u200F';
    const char ArabicLetterMark = '\u061C';

    public static void AssertNoBidiControls(string value)
    {
        Assert.DoesNotContain(LeftToRightMark, value);
        Assert.DoesNotContain(RightToLeftMark, value);
        Assert.DoesNotContain(ArabicLetterMark, value);
    }
}