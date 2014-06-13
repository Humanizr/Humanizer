using Humanizer.Localisation.GrammaticalNumber;

namespace Humanizer.Localisation.Formatters
{
    internal class RussianFormatter : DefaultFormatter
    {
        public RussianFormatter()
            : base("ru")
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(number);
            var suffix = GetSuffix(grammaticalNumber);
            return resourceKey + suffix;
        }

        private string GetSuffix(RussianGrammaticalNumber grammaticalNumber)
        {
            if (grammaticalNumber == RussianGrammaticalNumber.Singular)
                return "_Singular";
            if (grammaticalNumber == RussianGrammaticalNumber.Paucal)
                return "_Paucal";
            return "";
        }
    }
}