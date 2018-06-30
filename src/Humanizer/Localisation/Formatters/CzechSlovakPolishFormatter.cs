namespace Humanizer.Localisation.Formatters
{
    internal class CzechSlovakPolishFormatter : DefaultFormatter
    {
        private const string PaucalPostfix = "_Paucal";

        public CzechSlovakPolishFormatter(string localeCode)
            : base(localeCode)
        {
        }

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
