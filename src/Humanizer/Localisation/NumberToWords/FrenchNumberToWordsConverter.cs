using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class FrenchNumberToWordsConverter : FrenchNumberToWordsConverterBase
    {
        protected override void CollectPartsUnderAHundred(ICollection<string> parts, ref long number, GrammaticalGender gender, bool pluralize)
        {
            if (number == 71)
            {
                parts.Add("soixante et onze");
            }
            else if (number == 80)
            {
                parts.Add(pluralize ? "quatre-vingts" : "quatre-vingt");
            }
            else if (number >= 70)
            {
                var @base = number < 80 ? 60 : 80;
                var units = number - @base;
                var tens = @base / 10;
                parts.Add($"{GetTens(tens)}-{GetUnits(units, gender)}");
            }
            else
            {
                base.CollectPartsUnderAHundred(parts, ref number, gender, pluralize);
            }
        }

        protected override string GetTens(long tens)
        {
            if (tens == 8)
            {
                return "quatre-vingt";
            }

            return base.GetTens(tens);
        }
    }
}
