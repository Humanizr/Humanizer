namespace Humanizer;

/// <summary>
/// Converts numbers to Macedonian words.
/// </summary>
class MacedonianNumberToWordsConverter(CultureInfo culture) : GenderedNumberToWordsConverter(GrammaticalGender.Masculine)
{
    static readonly string[] MasculineUnits =
    [
        "нула", "еден", "два", "три", "четири", "пет", "шест", "седум", "осум", "девет",
        "десет", "единаесет", "дванаесет", "тринаесет", "четиринаесет", "петнаесет", "шеснаесет", "седумнаесет", "осумнаесет", "деветнаесет"
    ];

    static readonly string[] FeminineUnits =
    [
        "нула", "една", "две", "три", "четири", "пет", "шест", "седум", "осум", "девет",
        "десет", "единаесет", "дванаесет", "тринаесет", "четиринаесет", "петнаесет", "шеснаесет", "седумнаесет", "осумнаесет", "деветнаесет"
    ];

    static readonly string[] NeuterUnits =
    [
        "нула", "едно", "две", "три", "четири", "пет", "шест", "седум", "осум", "девет",
        "десет", "единаесет", "дванаесет", "тринаесет", "четиринаесет", "петнаесет", "шеснаесет", "седумнаесет", "осумнаесет", "деветнаесет"
    ];

    static readonly string[] Tens =
    [
        "", "", "дваесет", "триесет", "четириесет", "педесет", "шеесет", "седумдесет", "осумдесет", "деведесет"
    ];

    static readonly string[] Hundreds =
    [
        "", "сто", "двесте", "триста", "четиристотини", "петстотини", "шестотини", "седумстотини", "осумстотини", "деветстотини"
    ];

    static readonly Scale[] Scales =
    [
        new(1_000_000_000_000_000_000, GrammaticalGender.Masculine, "квинтилион", "квинтилиони", "квинтилионит"),
        new(1_000_000_000_000_000, GrammaticalGender.Masculine, "квадрилион", "квадрилиони", "квадрилионит"),
        new(1_000_000_000_000, GrammaticalGender.Masculine, "трилион", "трилиони", "трилионит"),
        new(1_000_000_000, GrammaticalGender.Feminine, "милијарда", "милијарди", "милијардит"),
        new(1_000_000, GrammaticalGender.Masculine, "милион", "милиони", "милионит"),
        new(1_000, GrammaticalGender.Feminine, "илјада", "илјади", "илјадит", OmitOne: true)
    ];

    static readonly string[] OrdinalMasculine =
    [
        "нулти", "прв", "втор", "трет", "четврти", "петти", "шести", "седми", "осми", "деветти",
        "десетти", "единаесетти", "дванаесетти", "тринаесетти", "четиринаесетти", "петнаесетти", "шеснаесетти", "седумнаесетти", "осумнаесетти", "деветнаесетти"
    ];

    static readonly string[] OrdinalFeminine =
    [
        "нулта", "прва", "втора", "трета", "четврта", "петта", "шеста", "седма", "осма", "деветта",
        "десетта", "единаесетта", "дванаесетта", "тринаесетта", "четиринаесетта", "петнаесетта", "шеснаесетта", "седумнаесетта", "осумнаесетта", "деветнаесетта"
    ];

    static readonly string[] OrdinalNeuter =
    [
        "нулто", "прво", "второ", "трето", "четврто", "петто", "шесто", "седмо", "осмо", "деветто",
        "десетто", "единаесетто", "дванаесетто", "тринаесетто", "четиринаесетто", "петнаесетто", "шеснаесетто", "седумнаесетто", "осумнаесетто", "деветнаесетто"
    ];

    static readonly string[] OrdinalTensStems =
    [
        "", "", "дваесетт", "триесетт", "четириесетт", "педесетт", "шеесетт", "седумдесетт", "осумдесетт", "деведесетт"
    ];

    static readonly string[] OrdinalHundredStems =
    [
        "", "стот", "двестот", "тристот", "четиристот", "петстот", "шестот", "седумстот", "осумстот", "деветстот"
    ];

    readonly CultureInfo culture = culture;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return MasculineUnits[0];
        }

        if (number < 0)
        {
            if (number == long.MinValue)
            {
                return "минус " + ConvertPositive((ulong)long.MaxValue + 1UL, gender);
            }

            return "минус " + ConvertPositive((ulong)-number, gender);
        }

        return ConvertPositive((ulong)number, gender);
    }

    public override string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, gender, addAnd);

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number < 0)
        {
            return "минус " + ConvertOrdinalPositive(-(long)number, gender);
        }

        return ConvertOrdinalPositive(number, gender);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm) =>
        ConvertToOrdinal(number, gender);

    static string ConvertPositive(ulong number, GrammaticalGender gender)
    {
        var parts = new List<string>();
        foreach (var scale in Scales)
        {
            var count = number / scale.Value;
            if (count == 0)
            {
                continue;
            }

            if (!(scale.OmitOne && count == 1))
            {
                parts.Add(ConvertPositive(count, scale.Gender));
            }

            parts.Add(count == 1 ? scale.Singular : scale.Plural);
            number %= scale.Value;
        }

        if (number > 0)
        {
            if (parts.Count > 0 && number < 100)
            {
                parts.Add("и");
            }

            AppendUnderThousand(parts, (int)number, gender);
        }

        return string.Join(" ", parts);
    }

    static void AppendUnderThousand(List<string> parts, int number, GrammaticalGender gender)
    {
        if (number >= 100)
        {
            parts.Add(Hundreds[number / 100]);
            number %= 100;
            if (number == 0)
            {
                return;
            }

            if (number < 20 || number % 10 == 0)
            {
                parts.Add("и");
            }
        }

        if (number >= 20)
        {
            parts.Add(Tens[number / 10]);
            number %= 10;
            if (number == 0)
            {
                return;
            }

            parts.Add("и");
        }

        parts.Add(GetUnit(number, gender));
    }

    static string GetUnit(int number, GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Feminine => FeminineUnits[number],
            GrammaticalGender.Neuter => NeuterUnits[number],
            _ => MasculineUnits[number]
        };

    string ConvertOrdinalPositive(long number, GrammaticalGender gender)
    {
        if (number < 20)
        {
            return GetOrdinalUnit((int)number, gender);
        }

        foreach (var scale in Scales)
        {
            if ((ulong)number < scale.Value)
            {
                continue;
            }

            var scaleValue = (long)scale.Value;
            var count = number / scaleValue;
            var remainder = number % scaleValue;
            var prefix = scale.OmitOne && count == 1
                ? scale.Singular
                : Convert(count, scale.Gender) + " " + (count == 1 ? scale.Singular : scale.Plural);

            if (remainder == 0)
            {
                return scale.OmitOne && count == 1
                    ? ApplyOrdinalEnding(scale.OrdinalStem, gender)
                    : Convert(count, scale.Gender) + " " + ApplyOrdinalEnding(scale.OrdinalStem, gender);
            }

            return prefix + (remainder < 100 ? " и " : " ") + ConvertOrdinalPositive(remainder, gender);
        }

        if (number >= 100)
        {
            var hundreds = (int)(number / 100);
            var remainder = number % 100;
            if (remainder == 0)
            {
                return ApplyOrdinalEnding(OrdinalHundredStems[hundreds], gender);
            }

            return Hundreds[hundreds] + (remainder < 20 || remainder % 10 == 0 ? " и " : " ") + ConvertOrdinalPositive(remainder, gender);
        }

        if (number >= 20)
        {
            var tens = (int)(number / 10);
            var remainder = number % 10;
            return remainder == 0
                ? ApplyOrdinalEnding(OrdinalTensStems[tens], gender)
                : Tens[tens] + " и " + ConvertOrdinalPositive(remainder, gender);
        }

        return number.ToString(culture);
    }

    static string GetOrdinalUnit(int number, GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Feminine => OrdinalFeminine[number],
            GrammaticalGender.Neuter => OrdinalNeuter[number],
            _ => OrdinalMasculine[number]
        };

    static string ApplyOrdinalEnding(string stem, GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Feminine => stem + "а",
            GrammaticalGender.Neuter => stem + "о",
            _ => stem + "и"
        };

    readonly record struct Scale(ulong Value, GrammaticalGender Gender, string Singular, string Plural, string OrdinalStem, bool OmitOne = false);
}