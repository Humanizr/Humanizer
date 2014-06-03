namespace Humanizer.Localisation.Formatters
{
    internal class RomanianFormatter : DefaultFormatter
    {
        private const string Above20PostFix = "_Above20";

        public RomanianFormatter()
            : base("ro")
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            var mod100 = number%100;

            if (0 < mod100 && mod100 < 20)
            {
                return resourceKey;
            }

            return resourceKey + Above20PostFix;
        }
    }
}
