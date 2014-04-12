using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class HebrewNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private static readonly string[] UnitsFeminine  = { "אחת", "שתיים", "שלוש", "ארבע", "חמש", "שש", "שבע", "שמונה", "תשע", "עשר" };
        private static readonly string[] UnitsMasculine = { "אחד", "שניים", "שלושה", "ארבעה", "חמישה", "שישה", "שבעה", "שמונה", "תשעה", "עשרה" };
        private static readonly string[] TensUnit       = { "עשר", "עשרים", "שלושים", "ארבעים", "חמישים", "שישים", "שבעים", "שמונים", "תשעים" };

        public override string Convert(int number)
        {
            // in Hebrew, the default number gender form is feminine.
            return Convert(number, GrammaticalGender.Feminine);
        }

        public override string Convert(int number, GrammaticalGender gender)
        {
            if (number < 0)
                return string.Format("מינוס {0}", Convert(-number, gender));
            
            if (number == 0)
                return "אפס";

            if (number <= 10)
                return gender == GrammaticalGender.Masculine ? UnitsMasculine[number - 1] : UnitsFeminine[number - 1];

            if (number < 20)
            {
                string unit = Convert(number % 10, gender);
                unit = unit.Replace("יי", "י");
                return string.Format("{0} {1}", unit, gender == GrammaticalGender.Masculine ? "עשר" : "עשרה");
            }
            if (number < 100)
            {
                if (number % 10 == 0)
                    return TensUnit[number / 10 - 1];

                string unit = Convert(number % 10, gender);
                return string.Format("{0} ו{1}", TensUnit[number / 10 - 1], unit);
            }
            if (number < 1000)
            {
                if (number == 100) return "מאה";
                if (number == 200) return "מאתיים";

                int hundredsDigit = number / 100;
                if ((hundredsDigit * 100) == number)
                    return UnitsFeminine[hundredsDigit - 1] + " מאות";

                if (hundredsDigit == 1)
                    return ToHundredsString("מאה", number, gender);
                if (hundredsDigit == 2)
                    return ToHundredsString("מאתיים", number, gender);

                return ToHundredsString(UnitsFeminine[hundredsDigit - 1] + " מאות", number, gender);

            }

            return number.ToString();
        }

        private string ToHundredsString(string prefix, int number, GrammaticalGender gender)
        {
            int tens = number % 100;
            return string.Format("{0} {1}", 
                prefix, 
                tens < 20
                    ? "ו" + Convert(tens, gender)
                    : Convert(tens, gender));
        }
    }
}