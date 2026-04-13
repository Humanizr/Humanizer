namespace Humanizer.Tests.Localisation.ur;

/// <summary>
/// Shared helper that asserts absence of Unicode directionality control characters
/// (U+200F RLM, U+200E LRM, U+061C ALM) in authored Urdu output strings.
/// </summary>
static class UrduBidiControlSweep
{
    const char LeftToRightMark = '\u200E';
    const char RightToLeftMark = '\u200F';
    const char ArabicLetterMark = '\u061C';

    /// <summary>
    /// Asserts that the given string contains no Unicode directionality control characters.
    /// </summary>
    public static void AssertNoBidiControls(string value)
    {
        Assert.DoesNotContain(LeftToRightMark, value);
        Assert.DoesNotContain(RightToLeftMark, value);
        Assert.DoesNotContain(ArabicLetterMark, value);
    }
}