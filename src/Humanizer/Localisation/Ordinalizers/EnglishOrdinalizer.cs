namespace Humanizer;

class EnglishOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString)
    {
            var nMod100 = number % 100;

            if (nMod100 is >= 11 and <= 20)
            {
                return numberString + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return numberString + "st";

                case 2:
                    return numberString + "nd";

                case 3:
                    return numberString + "rd";

                default:
                    return numberString + "th";
            }
        }
}