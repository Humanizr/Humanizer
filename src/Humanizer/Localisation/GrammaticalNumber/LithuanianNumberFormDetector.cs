namespace Humanizer.Localisation.GrammaticalNumber
{
    internal static class LithuanianNumberFormDetector
    {
        public static LithuanianNumberForm Detect(long number)
        {
            var tens = number % 100 / 10;
            if (tens != 1)
            {
                var units = number % 10;

                if (units == 1) // 1, 21, 31, 41 ... 91, 101, 121 ...
                {
                    return LithuanianNumberForm.Singular;
                }

                if (units != 0 && units < 10) // 2, 3, 4, 5, 6, 7, 8, 9
                {
                    return LithuanianNumberForm.Plural;
                }
            }

            return LithuanianNumberForm.GenitivePlural;
        }
    }
}

