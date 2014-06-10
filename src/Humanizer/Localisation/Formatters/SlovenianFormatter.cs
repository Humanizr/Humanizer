namespace Humanizer.Localisation.Formatters
{
    internal class SlovenianFormatter : DefaultFormatter
    {
        private const string DualPostfix = "_Dual";
        private const string TrialQuadralPostfix = "_TrialQuadral";

        public SlovenianFormatter()
            : base("sl")
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number == 2)
                return resourceKey + DualPostfix;
            
            // When the count is three or four some some words have a different form when counting in Slovenian language
            if (number == 3 || number == 4)
                return resourceKey + TrialQuadralPostfix;

            return resourceKey;
        }
    }
}
