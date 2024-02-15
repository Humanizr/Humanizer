namespace Humanizer.Localisation.Formatters
{
    internal class SlovenianFormatter() :
        DefaultFormatter("sl")
    {
        private const string DualPostfix = "_Dual";
        private const string TrialQuadralPostfix = "_TrialQuadral";

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number == 2)
            {
                return resourceKey + DualPostfix;
            }

            // When the count is three or four some some words have a different form when counting in Slovenian language
            if (number is 3 or 4)
            {
                return resourceKey + TrialQuadralPostfix;
            }

            return resourceKey;
        }
    }
}
