using Humanizer.Localisation.GrammaticalNumber;
using JetBrains.Annotations;

namespace Humanizer.Localisation.Formatters
{
    internal class UkrainianFormatter : DefaultFormatter
    {
        public UkrainianFormatter()
            : base("uk")
        {
        }

        [NotNull]
        protected override string GetResourceKey(string resourceKey, int number)
        {
            var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(number);
            var suffix = GetSuffix(grammaticalNumber);
            return resourceKey + suffix;
        }

        [NotNull]
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