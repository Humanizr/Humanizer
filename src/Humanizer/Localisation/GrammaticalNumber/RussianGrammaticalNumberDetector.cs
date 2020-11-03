namespace Humanizer.Localisation.GrammaticalNumber
{
    internal static class RussianGrammaticalNumberDetector
    {
        public static RussianGrammaticalNumber Detect(long number)
        {
            var tens = number % 100 / 10;
            if (tens != 1)
            {
                var unity = number % 10;

                if (unity == 1) // 1, 21, 31, 41 ... 91, 101, 121 ...
                {
                    return RussianGrammaticalNumber.Singular;
                }

                if (unity > 1 && unity < 5) // 2, 3, 4, 22, 23, 24 ...
                {
                    return RussianGrammaticalNumber.Paucal;
                }
            }

            return RussianGrammaticalNumber.Plural;
        }
    }
}
