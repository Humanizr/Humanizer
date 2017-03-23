using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class FrenchNumberToWordsConverter : FrenchNumberToWordsConverterBase
    {
        protected override void CollectPartsUnderAHundred(ICollection<string> parts, ref int number, GrammaticalGender gender, bool pluralize)
        {
            if (number == 71)
                parts.Add("soixante et onze");
            else if (number == 80)
                parts.Add("quatre-vingt" + (pluralize ? "s" : string.Empty));
            else if (number >= 70)
            {
                var @base = number < 80 ? 60 : 80;
                int units = number - @base;
                var tens = @base / 10;
                parts.Add($"{GetTens(tens)}-{GetUnits(units, gender)}");
            }
            else
                base.CollectPartsUnderAHundred(parts, ref number, gender, pluralize);
        }

        protected override string GetTens(int tens) => tens == 8 ? "quatre-vingt" : base.GetTens(tens);
    }
}
