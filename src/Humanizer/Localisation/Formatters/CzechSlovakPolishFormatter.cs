namespace Humanizer
{
    internal class CzechSlovakPolishFormatter(string localeCode) :
        DefaultFormatter(localeCode)
    {
        private const string PaucalPostfix = "_Paucal";

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number > 1 && number < 5)
            {
                return resourceKey + PaucalPostfix;
            }

            return resourceKey;
        }
    }
}
