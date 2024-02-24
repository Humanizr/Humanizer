namespace Humanizer;

class SerbianFormatter(string localeCode) :
    DefaultFormatter(localeCode)
{
    const string PaucalPostfix = "_Paucal";

    protected override string GetResourceKey(string resourceKey, int number)
    {
            var mod10 = number % 10;
            if (mod10 is > 1 and < 5)
            {
                return resourceKey + PaucalPostfix;
            }

            return resourceKey;
        }
}