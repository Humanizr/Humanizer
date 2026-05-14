namespace Humanizer.Tests.Localisation.ps;

static class PashtoBidiControlSweep
{
    const char LeftToRightMark = '\u200E';
    const char RightToLeftMark = '\u200F';
    const char ArabicLetterMark = '\u061C';
    static readonly char[] OtherBidiControls =
    [
        '\u202A',
        '\u202B',
        '\u202C',
        '\u202D',
        '\u202E',
        '\u2066',
        '\u2067',
        '\u2068',
        '\u2069'
    ];

    public static void AssertNoBidiControls(string value)
    {
        Assert.DoesNotContain(LeftToRightMark, value);
        Assert.DoesNotContain(RightToLeftMark, value);
        Assert.DoesNotContain(ArabicLetterMark, value);
        foreach (var bidiControl in OtherBidiControls)
            Assert.DoesNotContain(bidiControl, value);
    }
}