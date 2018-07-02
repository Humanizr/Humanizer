namespace Humanizer.Localisation.Formatters
{
    internal class CroatianFormatter : DefaultFormatter
    {
        private const string DualTrialQuadralPostfix = "_DualTrialQuadral";

        public CroatianFormatter()
            : base("hr")
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if ((number % 10 == 2 || number % 10 == 3 || number % 10 == 4) && number != 12 && number != 13 && number != 14)
            {
                return resourceKey + DualTrialQuadralPostfix;
            }

            return resourceKey;
        }
    }
}
