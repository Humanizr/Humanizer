namespace Humanizer
{
    class CroatianFormatter() :
        DefaultFormatter("hr")
    {
        private const string DualTrialQuadralPostfix = "_DualTrialQuadral";

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
