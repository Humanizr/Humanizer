namespace Humanizer.Localisation.Formatters
{
    internal class SerbianFormatter(string localeCode) :
        DefaultFormatter(localeCode)
    {
        private const string PaucalPostfix = "_Paucal";

        protected override string GetResourceKey(string resourceKey, int number)
        {
            var mod10 = number % 10;

            if (mod10 > 1 && mod10 < 5)
            {
                return resourceKey + PaucalPostfix;
            }

            return resourceKey;
        }
    }
}
