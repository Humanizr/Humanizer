namespace Humanizer;

/// <summary>
/// Detects the Russian grammatical number form for a numeric value.
/// </summary>
static class RussianGrammaticalNumberDetector
{
    /// <summary>
    /// Returns the grammatical number form required for the given <paramref name="number"/>.
    /// </summary>
    /// <returns>The Russian grammatical number form.</returns>
    public static RussianGrammaticalNumber Detect(long number)
    {
        var tens = number % 100 / 10;
        if (tens != 1)
        {
            var unity = number % 10;

            // Teens are always plural in Russian, so the last digit only matters when the tens
            // place is not 1.
            if (unity == 1)
            {
                return RussianGrammaticalNumber.Singular;
            }

            if (unity is > 1 and < 5)
            {
                return RussianGrammaticalNumber.Paucal;
            }
        }

        return RussianGrammaticalNumber.Plural;
    }
}
