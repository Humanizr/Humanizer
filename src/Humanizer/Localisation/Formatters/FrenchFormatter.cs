namespace Humanizer.Localisation.Formatters
{
    internal class FrenchFormatter : DefaultFormatter
    {
        private const string DualPostfix = "_Dual";
        
        public FrenchFormatter()
            : base("fr")
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number == 2 && (resourceKey == "DateHumanize_MultipleDaysAgo" || resourceKey == "DateHumanize_MultipleDaysFromNow"))
                return resourceKey + DualPostfix;

            return resourceKey;
        }
    }
}
