namespace Humanizer;

class ArabicFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string DualPostfix = "_Dual";
    const string PluralPostfix = "_Plural";

    protected override string GetResourceKey(string resourceKey, int number)
    {
        //In Arabic pluralization 2 entities gets a different word.
        if (number == 2)
        {
            return resourceKey + DualPostfix;
        }

        //In Arabic pluralization entities where the count is between 3 and 10 gets a different word.
        if (number is >= 3 and <= 10)
        {
            return resourceKey + PluralPostfix;
        }

        return resourceKey;
    }
}