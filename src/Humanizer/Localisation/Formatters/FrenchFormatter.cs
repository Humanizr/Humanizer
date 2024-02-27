namespace Humanizer;

class FrenchFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string DualPostfix = "_Dual";

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (number == 2 && resourceKey is "DateHumanize_MultipleDaysAgo" or "DateHumanize_MultipleDaysFromNow")
        {
            return resourceKey + DualPostfix;
        }

        if (number == 0 && resourceKey.StartsWith("TimeSpanHumanize_Multiple"))
        {
            return resourceKey + "_Singular";
        }

        return resourceKey;
    }
}