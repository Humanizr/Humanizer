namespace Humanizer;

/// <summary>
/// Detects the Lithuanian grammatical number form for a numeric value.
/// </summary>
static class LithuanianNumberFormDetector
{
    /// <summary>
    /// Returns the grammatical number form required for the given <paramref name="number"/>.
    /// </summary>
    /// <returns>The Lithuanian grammatical number form.</returns>
    public static LithuanianNumberForm Detect(long number)
    {
        var tens = number % 100 / 10;
        var units = number % 10;

        // Teens and round tens use the genitive plural form, even though the final digit might
        // otherwise map to the singular or plural buckets.
        if (tens == 1 || units == 0)
        {
            return LithuanianNumberForm.GenitivePlural;
        }

        if (units == 1)
        {
            return LithuanianNumberForm.Singular;
        }

        return LithuanianNumberForm.Plural;
    }
}