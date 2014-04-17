namespace Humanizer.Localisation.Formatters
{
    internal class SerbianFormatter : DefaultFormatter
    {
        private const string PaucalPostfix = "_Paucal";

        protected override string GetResourceKey(string resourceKey, int number)
        {
            int mod10 = number % 10;

            if (mod10 > 1 && mod10 < 5)            
                return resourceKey + PaucalPostfix;
            
            return resourceKey;
        }
    }
}
