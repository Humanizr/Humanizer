namespace Humanizer;

class RomanianOrdinalNumberConverter
{
    /// <summary>
    /// Lookup table converting units number to text. Index 1 for 1, index 2 for 2, up to index 9.
    /// </summary>
    readonly Dictionary<int, string> ordinalsUnder10 = new()
    {
        {
            1, "primul|prima"
        },
        {
            2, "doilea|doua"
        },
        {
            3, "treilea|treia"
        },
        {
            4, "patrulea|patra"
        },
        {
            5, "cincilea|cincea"
        },
        {
            6, "șaselea|șasea"
        },
        {
            7, "șaptelea|șaptea"
        },
        {
            8, "optulea|opta"
        },
        {
            9, "nouălea|noua"
        },
    };

    readonly string femininePrefix = "a";
    readonly string masculinePrefix = "al";
    readonly string feminineSuffix = "a";
    readonly string masculineSuffix = "lea";

    public string Convert(int number, GrammaticalGender gender)
    {
        // it's easier to treat zero as a completely distinct case
        if (number == 0)
        {
            return "zero";
        }

        if (number == 1)
        {
            // no prefixes for primul/prima
            return GetPartByGender(ordinalsUnder10[number], gender);
        }

        if (number <= 9)
        {
            // units ordinals, 2 to 9, are totally different than the rest: treat them as a distinct case
            return string.Format("{0} {1}",
                gender == GrammaticalGender.Feminine ? femininePrefix : masculinePrefix,
                GetPartByGender(ordinalsUnder10[number], gender)
            );
        }

        var coverter = new RomanianCardinalNumberConverter();
        var words = coverter.Convert(number, gender);

        // remove 'de' preposition
        words = words.Replace(" de ", " ");

        if (gender == GrammaticalGender.Feminine && words.EndsWith("zeci"))
        {
            words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 4), "zece".AsSpan());
        }
        else if (gender == GrammaticalGender.Feminine && words.Contains("zeci") && (words.Contains("milioane") || words.Contains("miliarde")))
        {
            words = words.Replace("zeci", "zecea");
        }

        if (gender == GrammaticalGender.Feminine && words.StartsWith("un "))
        {
            words = words
                .AsSpan(2)
                .TrimStart()
                .ToString();
        }

        if (words.EndsWith("milioane"))
        {
            if (gender == GrammaticalGender.Feminine)
            {
                words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 8), "milioana".AsSpan());
            }
        }

        var customMasculineSuffix = masculineSuffix;
        if (words.EndsWith("milion"))
        {
            if (gender == GrammaticalGender.Feminine)
            {
                words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 6), "milioana".AsSpan());
            }
            else
            {
                customMasculineSuffix = "u" + masculineSuffix;
            }
        }
        else if (words.EndsWith("miliard"))
        {
            if (gender == GrammaticalGender.Masculine)
            {
                customMasculineSuffix = "u" + masculineSuffix;
            }
        }

        // trim last letter
        if (gender == GrammaticalGender.Feminine && !words.EndsWith("zece") &&
            (words.EndsWith("a") ||
             words.EndsWith("ă") ||
             words.EndsWith("e") ||
             words.EndsWith("i")))
        {
            words = words.Substring(0, words.Length - 1);
        }

        return string.Format("{0} {1}{2}",
            gender == GrammaticalGender.Feminine ? femininePrefix : masculinePrefix,
            words,
            gender == GrammaticalGender.Feminine ? feminineSuffix : customMasculineSuffix
        );
    }

    static string GetPartByGender(string multiGenderPart, GrammaticalGender gender)
    {
        if (multiGenderPart.Contains("|"))
        {
            var parts = multiGenderPart.Split('|');
            if (gender == GrammaticalGender.Feminine)
            {
                return parts[1];
            }

            return parts[0];
        }

        return multiGenderPart;
    }
}