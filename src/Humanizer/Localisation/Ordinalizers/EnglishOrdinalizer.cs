namespace Humanizer.Localisation.Ordinalizers
{
    internal class EnglishOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            var nMod100 = number % 100;

            if (nMod100 is >= 11 and <= 13)
            {
                return numberString + "th";
            }

            return (number % 10) switch
            {
                1 => numberString + "st",
                2 => numberString + "nd",
                3 => numberString + "rd",
                _ => numberString + "th"
            };
        }
    }
}
