namespace Humanizer.Localisation
{
    internal class ArabicFormatter : DefaultFormatter
    {
        private const string DualPostfix = "_Dual";

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number == 2)
            {
                return resourceKey + DualPostfix;
            }
            return resourceKey;
        }
    }
}
