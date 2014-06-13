namespace Humanizer.Localisation.Formatters
{
    internal class ArabicFormatter : DefaultFormatter
    {
        private const string DualPostfix = "_Dual";
        private const string PluralPostfix = "_Plural";

        public ArabicFormatter()
            : base("ar")
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            //In Arabic pluralization 2 entities gets a different word.
            if (number == 2)
                return resourceKey + DualPostfix;

            //In Arabic pluralization entities where the count is between 3 and 10 gets a different word.
            if (number >= 3 && number <= 10 )
                return resourceKey + PluralPostfix;

            return resourceKey;
        }
    }
}
