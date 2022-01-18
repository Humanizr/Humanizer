using System.Collections.Generic;
using System.Globalization;

namespace Humanizer.Localisation.Ordinalizers
{
    internal class SpanishOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return Convert(number, numberString, GrammaticalGender.Masculine);
        }

        public override string Convert(int number, string numberString, GrammaticalGender gender)
        {
            var genderMap = new Dictionary<GrammaticalGender, string>()
            {
                { GrammaticalGender.Masculine, ".º" },
                { GrammaticalGender.Feminine, ".ª" },
                { GrammaticalGender.Neuter, GetNeuterMap(number) }
            };

            // N/A in Spanish
            if (number == 0 || number == int.MinValue)
            {
                return "0";
            }

            if (number < 0)
            {
                return Convert(-number, GetNumberString(-number), gender);
            }

            return $"{numberString}{genderMap[gender]}";
        }

        private static string GetNeuterMap(int number)
        {
            return (number % 10 == 1 || number % 10 == 3) ? ".er" : ".º";
        }

        private static string GetNumberString(int number)
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }
    }
}
