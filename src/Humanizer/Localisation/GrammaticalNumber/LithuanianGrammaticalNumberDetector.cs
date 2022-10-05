namespace Humanizer.Localisation.GrammaticalNumber
{
    internal static class LithuanianGrammaticalNumberDetector
    {
        public static LithuanianGrammaticalNumber Detect(long number)
        {
            var tens = number % 100 / 10;
            var units = number % 10;

            if (tens == 1 || units == 0) // 10-19, 20, 30, 40 ... 100, 110 ..
            {
                return LithuanianGrammaticalNumber.SpecialCase;
            }
            
            if (units == 1) // 1, 21, 31, 41 ... 91, 101, 121 ...
            {
                return LithuanianGrammaticalNumber.Singular;
            }

            // 2-9, 22-29, 32 ...
            return LithuanianGrammaticalNumber.Plural;
        }
    }
}
