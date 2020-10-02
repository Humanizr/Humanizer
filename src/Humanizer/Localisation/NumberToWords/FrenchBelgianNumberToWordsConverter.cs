using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class FrenchBelgianNumberToWordsConverter : FrenchNumberToWordsConverterBase
    {
        protected override void CollectPartsUnderAHundred(ICollection<string> parts, ref long number, GrammaticalGender gender, bool pluralize)
        {
            if (number == 80)
            {
                parts.Add(pluralize ? "quatre-vingts" : "quatre-vingt");
            }
            else if (number == 81)
            {
                parts.Add(gender == GrammaticalGender.Feminine ? "quatre-vingt-une" : "quatre-vingt-un");
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
