namespace Humanizer.Localisation.Formatters
{
    internal class FrenchFormatter : DefaultFormatter
    {
        private const string DualPostfix = "_Dual";

        public FrenchFormatter(string localeCode)
            : base(localeCode)
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number == 2 && (resourceKey == "DateHumanize_MultipleDaysAgo" || resourceKey == "DateHumanize_MultipleDaysFromNow"))
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
}
