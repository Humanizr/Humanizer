using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords.Romanian
{
    internal class RomanianOrdinalNumberConverter
    {
        /// <summary>
        /// Lookup table converting units number to text. Index 1 for 1, index 2 for 2, up to index 9.
        /// </summary>
        private readonly Dictionary<int, string> _ordinalsUnder10 = new Dictionary<int, string>
            {
                {1, "primul|prima"},
                {2, "doilea|doua"},
                {3, "treilea|treia"},
                {4, "patrulea|patra"},
                {5, "cincilea|cincea"},
                {6, "șaselea|șasea"},
                {7, "șaptelea|șaptea"},
                {8, "optulea|opta"},
                {9, "nouălea|noua"},
            };

        private readonly string _femininePrefix = "a";
        private readonly string _masculinePrefix = "al";
        private readonly string _feminineSuffix = "a";
        private readonly string _masculineSuffix = "lea";

        public string Convert(int number, GrammaticalGender gender)
        {
            // it's easier to treat zero as a completely distinct case
            if (number == 0)
            {
                return "zero";
            }
            else if (number == 1)
            {
                // no prefixes for primul/prima
                return this.getPartByGender(_ordinalsUnder10[number], gender);
            }
            else if (number <= 9)
            {
                // units ordinals, 2 to 9, are totally different than the rest: treat them as a distinct case
                return string.Format("{0} {1}",
                                        gender == GrammaticalGender.Feminine ? _femininePrefix : _masculinePrefix,
                                        this.getPartByGender(_ordinalsUnder10[number], gender)
                                     );
            }
            else
            {
                var coverter = new RomanianCardinalNumberConverter();
                var words = coverter.Convert(number, gender);

                // remove 'de' preposition
                words = words.Replace(" de ", " ");

                if ((gender == GrammaticalGender.Feminine) && words.EndsWith("zeci"))
                {
                    words = words.Substring(0, words.Length - 4) + "zece";
                }
                else if ((gender == GrammaticalGender.Feminine) && words.Contains("zeci") && (words.Contains("milioane") || words.Contains("miliarde")))
                {
                    words = words.Replace("zeci", "zecea");
                }

                if ((gender == GrammaticalGender.Feminine) && words.StartsWith("un "))
                {
                    words = words.Substring(2).TrimStart();
                }

                if (words.EndsWith("milioane"))
                {
                    if (gender == GrammaticalGender.Feminine)
                    {
                        words = words.Substring(0, words.Length - 8) + "milioana";
                    }
                }

                var customMasculineSuffix = _masculineSuffix;
                if (words.EndsWith("milion"))
                {
                    if (gender == GrammaticalGender.Feminine)
                    {
                        words = words.Substring(0, words.Length - 6) + "milioana";
                    }
                    else
                    {
                        customMasculineSuffix = "u" + _masculineSuffix;
                    }
                }
                else if (words.EndsWith("miliard"))
                {
                    if (gender == GrammaticalGender.Masculine)
                    {
                        customMasculineSuffix = "u" + _masculineSuffix;
                    }
                }

                // trim last letter
                if ((gender == GrammaticalGender.Feminine) && (!words.EndsWith("zece") &&
                                                               (words.EndsWith("a") ||
                                                               words.EndsWith("ă") ||
                                                               words.EndsWith("e") ||
                                                               words.EndsWith("i"))))
                {
                    words = words.Substring(0, words.Length - 1);
                }

                return string.Format("{0} {1}{2}",
                                        gender == GrammaticalGender.Feminine ? _femininePrefix : _masculinePrefix,
                                        words,
                                        gender == GrammaticalGender.Feminine ? _feminineSuffix : customMasculineSuffix
                                    );
            }
        }

        private string getPartByGender(string multiGenderPart, GrammaticalGender gender)
        {
            if (multiGenderPart.Contains("|"))
            {
                var parts = multiGenderPart.Split('|');
                if (gender == GrammaticalGender.Feminine)
                {
                    return parts[1];
                }
                else
                {
                    return parts[0];
                }
            }
            else
            {
                return multiGenderPart;
            }
        }
    }
}
