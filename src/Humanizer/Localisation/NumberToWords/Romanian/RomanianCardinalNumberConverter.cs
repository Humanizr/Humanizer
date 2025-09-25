namespace Humanizer;

class RomanianCardinalNumberConverter
{
    /// <summary>
    /// Lookup table converting units number to text. Index 1 for 1, index 2 for 2, up to index 9 for 9.
    /// </summary>
    readonly string[] units =
    [
        string.Empty,
        "unu|una|unu",
        "doi|două|două",
        "trei",
        "patru",
        "cinci",
        "șase",
        "șapte",
        "opt",
        "nouă"
    ];

    /// <summary>
    /// Lookup table converting teens number to text. Index 0 for 10, index 1 for 11, up to index 9 for 19.
    /// </summary>
    readonly string[] teensUnder20NumberToText =
    [
        "zece",
        "unsprezece",
        "doisprezece|douăsprezece|douăsprezece",
        "treisprezece",
        "paisprezece",
        "cincisprezece",
        "șaisprezece",
        "șaptesprezece",
        "optsprezece",
        "nouăsprezece"
    ];

    /// <summary>
    /// Lookup table converting tens number to text. Index 2 for 20, index 3 for 30, up to index 9 for 90.
    /// </summary>
    readonly string[] tensOver20NumberToText =
    [
        string.Empty,
        string.Empty,
        "douăzeci",
        "treizeci",
        "patruzeci",
        "cincizeci",
        "șaizeci",
        "șaptezeci",
        "optzeci",
        "nouăzeci"
    ];

    readonly string feminineSingular = "o";
    readonly string masculineSingular = "un";

    readonly string joinGroups = "și";
    readonly string joinAbove20 = "de";
    readonly string minusSign = "minus";

    /// <summary>
    /// Enumerates sets of three-digits having distinct conversion to text.
    /// </summary>
    enum ThreeDigitSets
    {
        /// <summary>
        /// Lowest three-digits set, from 1 to 999.
        /// </summary>
        Units = 0,

        /// <summary>
        /// Three-digits set counting the thousands, from 1'000 to 999'000.
        /// </summary>
        Thousands = 1,

        /// <summary>
        /// Three-digits set counting millions, from 1'000'000 to 999'000'000.
        /// </summary>
        Millions = 2,

        /// <summary>
        /// Three-digits set counting billions, from 1'000'000'000 to 999'000'000'000.
        /// </summary>
        Billions = 3,

        /// <summary>
        /// Three-digits set beyond 999 billions, from 1'000'000'000'000 onward.
        /// </summary>
        More = 4
    }

    public string Convert(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return "zero";
        }

        var words = string.Empty;

        var prefixMinusSign = false;

        if (number < 0)
        {
            prefixMinusSign = true;
            number = -number;
        }

        var _threeDigitParts = SplitEveryThreeDigits(number);

        for (var i = 0; i < _threeDigitParts.Count; i++)
        {
            var currentSet = (ThreeDigitSets) Enum.ToObject(typeof(ThreeDigitSets), i);

            var partToString = GetNextPartConverter(currentSet);

            if (partToString != null)
            {
                words = partToString(_threeDigitParts[i], gender)
                    .Trim() + " " + words.Trim();
            }
        }

        if (prefixMinusSign)
        {
            words = minusSign + " " + words;
        }

        // remove extra spaces
        return words
            .TrimEnd()
            .Replace("  ", " ");
    }

    /// <summary>
    /// Splits a number into a sequence of three-digits numbers,
    /// starting from units, then thousands, millions, and so on.
    /// </summary>
    /// <param name="number">The number to split.</param>
    /// <returns>The sequence of three-digit numbers.</returns>
    static List<int> SplitEveryThreeDigits(int number)
    {
        var parts = new List<int>();
        var rest = number;

        while (rest > 0)
        {
            var threeDigit = rest % 1000;

            parts.Add(threeDigit);

            rest /= 1000;
        }

        return parts;
    }

    /// <summary>
    /// During number conversion to text, finds out the converter
    /// to use for the next three-digit set.
    /// </summary>
    /// <returns>The next conversion function to use.</returns>
    Func<int, GrammaticalGender, string>? GetNextPartConverter(ThreeDigitSets currentSet) =>
        currentSet switch
        {
            ThreeDigitSets.Units => UnitsConverter,
            ThreeDigitSets.Thousands => ThousandsConverter,
            ThreeDigitSets.Millions => MillionsConverter,
            ThreeDigitSets.Billions => BillionsConverter,
            ThreeDigitSets.More => null,
            _ => throw new ArgumentOutOfRangeException("Unknow ThreeDigitSet: " + currentSet)
        };

    /// <summary>
    /// Converts a three-digit set to text.
    /// </summary>
    /// <param name="number">The three-digit set to convert.</param>
    /// <param name="gender">The grammatical gender to convert to.</param>
    /// <returns>The same three-digit set expressed as text.</returns>
    string ThreeDigitSetConverter(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        // grab lowest two digits
        var tensAndUnits = number % 100;
        // grab third digit
        var hundreds = number / 100;

        // grab also first and second digits separately
        var units = tensAndUnits % 10;
        var tens = tensAndUnits / 10;

        var words = string.Empty;

        // append text for hundreds
        words += HundredsToText(hundreds);

        // append text for tens, only those from twenty upward
        words += (tens >= 2 ? " " : string.Empty) + tensOver20NumberToText[tens];

        if (tensAndUnits <= 9)
        {
            // simple case for units, under 10
            words += " " + GetPartByGender(this.units[tensAndUnits], gender);
        }
        else if (tensAndUnits <= 19)
        {
            // special case for 'teens', from 10 to 19
            words += " " + GetPartByGender(teensUnder20NumberToText[tensAndUnits - 10], gender);
        }
        else
        {
            // exception for zero
            var unitsText = units == 0 ? string.Empty : " " + joinGroups + " " + GetPartByGender(this.units[units], gender);

            words += unitsText;
        }

        return words;
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

            if (gender == GrammaticalGender.Neuter)
            {
                return parts[2];
            }

            return parts[0];
        }

        return multiGenderPart;
    }

    static bool IsAbove20(int number) =>
        number >= 20;

    string HundredsToText(int hundreds)
    {
        if (hundreds == 0)
        {
            return string.Empty;
        }

        if (hundreds == 1)
        {
            return feminineSingular + " sută";
        }

        return GetPartByGender(units[hundreds], GrammaticalGender.Feminine) + " sute";
    }

    /// <summary>
    /// Converts a three-digit number, as units, to text.
    /// </summary>
    /// <param name="number">The three-digit number, as units, to convert.</param>
    /// <param name="gender">The grammatical gender to convert to.</param>
    /// <returns>The same three-digit number, as units, expressed as text.</returns>
    string UnitsConverter(int number, GrammaticalGender gender) =>
        ThreeDigitSetConverter(number, gender);

    /// <summary>
    /// Converts a thousands three-digit number to text.
    /// </summary>
    /// <param name="number">The three-digit number, as thousands, to convert.</param>
    /// <param name="gender">The grammatical gender to convert to.</param>
    /// <returns>The same three-digit number of thousands expressed as text.</returns>
    string ThousandsConverter(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        if (number == 1)
        {
            return feminineSingular + " mie";
        }

        return ThreeDigitSetConverter(number, GrammaticalGender.Feminine) + (IsAbove20(number) ? " " + joinAbove20 : string.Empty) + " mii";
    }

    // Large numbers (above 10^6) use a combined form of the long and short scales.
    /*
            Singular    Plural            Order     Scale
            -----------------------------------------------
            zece        zeci              10^1      -
            sută        sute              10^2      -
            mie         mii               10^3      -
            milion      milioane          10^6      short/long
            miliard     miliarde          10^9      long
            trilion     trilioane         10^12     short
    */

    /// <summary>
    /// Converts a millions three-digit number to text.
    /// </summary>
    /// <param name="number">The three-digit number, as millions, to convert.</param>
    /// <param name="gender">The grammatical gender to convert to.</param>
    /// <returns>The same three-digit number of millions expressed as text.</returns>
    string MillionsConverter(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        if (number == 1)
        {
            return masculineSingular + " milion";
        }

        return ThreeDigitSetConverter(number, GrammaticalGender.Feminine) + (IsAbove20(number) ? " " + joinAbove20 : string.Empty) + " milioane";
    }

    /// <summary>
    /// Converts a billions three-digit number to text.
    /// </summary>
    /// <param name="number">The three-digit number, as billions, to convert.</param>
    /// <param name="gender">The grammatical gender to convert to.</param>
    /// <returns>The same three-digit number of billions expressed as text.</returns>
    string BillionsConverter(int number, GrammaticalGender gender)
    {
        if (number == 1)
        {
            return masculineSingular + " miliard";
        }

        return ThreeDigitSetConverter(number, GrammaticalGender.Feminine) + (IsAbove20(number) ? " " + joinAbove20 : string.Empty) + " miliarde";
    }
}