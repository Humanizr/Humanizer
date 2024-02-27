namespace Humanizer;

class CzechSlovakPolishFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string PaucalPostfix = "_Paucal";

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (number is > 1 and < 5)
        {
            return resourceKey + PaucalPostfix;
        }

        return resourceKey;
    }
}