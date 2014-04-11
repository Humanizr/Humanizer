using Humanizer.Localisation.GrammaticalNumber;

namespace Humanizer.Localisation.Formatters
{
    internal class RussianFormatter : DefaultFormatter
    {
        private readonly string[] _map = new string[3] { "_Singular", "_Paucal", "" };

        protected override string GetResourceKey(string resourceKey, int number)
        {
            var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(number);
            var suffix = _map[(int) grammaticalNumber];
            return resourceKey + suffix;
        }
    }
}