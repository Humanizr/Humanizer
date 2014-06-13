namespace Humanizer.Localisation.Formatters
{
    internal class HebrewFormatter : DefaultFormatter
    {
        private const string DualPostfix = "_Dual";
        private const string PluralPostfix = "_Plural";

        public HebrewFormatter()
            : base("he")
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            //In Hebrew pluralization 2 entities gets a different word.
            if (number == 2)
                return resourceKey + DualPostfix;

            //In Hebrew pluralization entities where the count is between 3 and 10 gets a different word.
            //See http://lib.cet.ac.il/pages/item.asp?item=21585 for explanation
            if (number >= 3 && number <= 10)
                return resourceKey + PluralPostfix;

            return resourceKey;
        }
    }
}