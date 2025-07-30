namespace Humanizer;

class SlovenianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string DualPostfix = "_Dual";
    const string TrialQuadralPostfix = "_Paucal";

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (number == 2)
        {
            return resourceKey + DualPostfix;
        }

        // When the count is three or four some words have a different form when counting in Slovenian language
        if (number is 3 or 4)
        {
            return resourceKey + TrialQuadralPostfix;
        }

        return resourceKey;
    }
}