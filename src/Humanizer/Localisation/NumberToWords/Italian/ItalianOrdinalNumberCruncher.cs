namespace Humanizer;

class ItalianOrdinalNumberCruncher(int number, GrammaticalGender gender)
{
    public string Convert()
    {
        // it's easier to treat zero as a completely distinct case
        if (fullNumber == 0)
        {
            return "zero";
        }

        if (fullNumber <= 9)
        {
            // units ordinals, 1 to 9, are totally different than the rest: treat them as a distinct case
            return UnitsUnder10NumberToText[fullNumber] + genderSuffix;
        }

        var cardinalCruncher = new ItalianCardinalNumberCruncher(fullNumber, gender);

        var words = cardinalCruncher.Convert();

        var tensAndUnits = fullNumber % 100;

        if (tensAndUnits == 10)
        {
            // for numbers ending in 10, cardinal and ordinal endings are different, suffix doesn't work
            words = words[..^LengthOf10AsCardinal] + "decim" + genderSuffix;
        }
        else
        {
            // truncate last vowel
            words = words[..^1];

            var units = fullNumber % 10;

            // reintroduce *unaccented* last vowel in some corner cases
            if (units == 3)
            {
                words += 'e';
            }
            else if (units == 6)
            {
                words += 'i';
            }

            var lowestThreeDigits = fullNumber % 1000;
            var lowestSixDigits = fullNumber % 1000000;
            var lowestNineDigits = fullNumber % 1000000000;

            if (lowestNineDigits == 0)
            {
                // if exact billions, cardinal number words are joined
                words = words.Replace(" miliard", "miliard");

                // if 1 billion, numeral prefix is removed completely
                if (fullNumber == 1000000000)
                {
                    words = words.Replace("un", string.Empty);
                }
            }
            else if (lowestSixDigits == 0)
            {
                // if exact millions, cardinal number words are joined
                words = words.Replace(" milion", "milion");

                // if 1 million, numeral prefix is removed completely
                if (fullNumber == 1000000)
                {
                    words = words.Replace("un", string.Empty);
                }
            }
            else if (lowestThreeDigits == 0 && fullNumber > 1000)
            {
                // if exact thousands, double the final 'l', apart from 1000 already having that
                words += 'l';
            }

            // append common ordinal suffix
            words += "esim" + genderSuffix;
        }

        return words;
    }

    readonly int fullNumber = number;
    readonly GrammaticalGender gender = gender;
    readonly string genderSuffix = gender == GrammaticalGender.Feminine ? "a" : "o";

    /// <summary>
    /// Lookup table converting units number to text. Index 1 for 1, index 2 for 2, up to index 9.
    /// </summary>
    static readonly string[] UnitsUnder10NumberToText =
    [
        string.Empty,
        "prim",
        "second",
        "terz",
        "quart",
        "quint",
        "sest",
        "settim",
        "ottav",
        "non"
    ];

    static readonly int LengthOf10AsCardinal = "dieci".Length;
}