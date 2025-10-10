namespace Humanizer;

class HebrewNumberToWordsConverter(CultureInfo culture) :
    GenderedNumberToWordsConverter(GrammaticalGender.Feminine)
{
    static readonly string[] UnitsFeminine = ["אפס", "אחת", "שתיים", "שלוש", "ארבע", "חמש", "שש", "שבע", "שמונה", "תשע", "עשר"];
    static readonly string[] UnitsMasculine = ["אפס", "אחד", "שניים", "שלושה", "ארבעה", "חמישה", "שישה", "שבעה", "שמונה", "תשעה", "עשרה"];
    static readonly string[] TensUnit = ["עשר", "עשרים", "שלושים", "ארבעים", "חמישים", "שישים", "שבעים", "שמונים", "תשעים"];

    [AttributeUsage(AttributeTargets.Field)]
    class DescriptionAttribute(string description) :
        Attribute
    {
        public string Description { get; set; } = description;
    }

    enum Group
    {
        Hundreds = 100,
        Thousands = 1000,

        [Description("מיליון")]
        Millions = 1000000,

        [Description("מיליארד")]
        Billions = 1000000000
    }

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;

        if (number < 0)
        {
            return $"מינוס {Convert(-number, gender)}";
        }

        if (number == 0)
        {
            return UnitsFeminine[0];
        }

        var parts = new List<string>();
        if (number >= (int)Group.Billions)
        {
            ToBigNumber(number, Group.Billions, parts);
            number %= (int)Group.Billions;
        }

        if (number >= (int)Group.Millions)
        {
            ToBigNumber(number, Group.Millions, parts);
            number %= (int)Group.Millions;
        }

        if (number >= (int)Group.Thousands)
        {
            ToThousands(number, parts);
            number %= (int)Group.Thousands;
        }

        if (number >= (int)Group.Hundreds)
        {
            ToHundreds(number, parts);
            number %= (int)Group.Hundreds;
        }

        if (number > 0)
        {
            var appendAnd = parts.Count != 0;

            if (number <= 10)
            {
                var unit = gender == GrammaticalGender.Masculine ? UnitsMasculine[number] : UnitsFeminine[number];
                if (appendAnd)
                {
                    unit = "ו" + unit;
                }

                parts.Add(unit);
            }
            else if (number < 20)
            {
                var unit = Convert(number % 10, gender);
                unit = unit.Replace("יי", "י");
                unit = $"{unit} {(gender == GrammaticalGender.Masculine ? "עשר" : "עשרה")}";
                if (appendAnd)
                {
                    unit = "ו" + unit;
                }

                parts.Add(unit);
            }
            else
            {
                var tenUnit = TensUnit[number / 10 - 1];
                if (number % 10 == 0)
                {
                    parts.Add(tenUnit);
                }
                else
                {
                    var unit = Convert(number % 10, gender);
                    parts.Add($"{tenUnit} ו{unit}");
                }
            }
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        number.ToString(culture);

    void ToBigNumber(int number, Group group, List<string> parts)
    {
        // Big numbers (million and above) always use the masculine form
        // See https://www.safa-ivrit.org/dikduk/numbers.php

        var digits = number / (int)group;
        if (digits == 2)
        {
            parts.Add("שני");
        }
        else if (digits > 2)
        {
            parts.Add(Convert(digits, GrammaticalGender.Masculine));
        }

        parts.Add(group.Humanize());
    }

    void ToThousands(int number, List<string> parts)
    {
        var thousands = number / (int)Group.Thousands;

        if (thousands == 1)
        {
            parts.Add("אלף");
        }
        else if (thousands == 2)
        {
            parts.Add("אלפיים");
        }
        else if (thousands == 8)
        {
            parts.Add("שמונת אלפים");
        }
        else if (thousands <= 10)
        {
            parts.Add(UnitsFeminine[thousands] + "ת" + " אלפים");
        }
        else
        {
            parts.Add(Convert(thousands) + " אלף");
        }
    }

    static void ToHundreds(int number, List<string> parts)
    {
        // For hundreds, Hebrew is using the feminine form
        // See https://www.safa-ivrit.org/dikduk/numbers.php

        var hundreds = number / (int)Group.Hundreds;

        if (hundreds == 1)
        {
            parts.Add("מאה");
        }
        else if (hundreds == 2)
        {
            parts.Add("מאתיים");
        }
        else
        {
            parts.Add(UnitsFeminine[hundreds] + " מאות");
        }
    }
}