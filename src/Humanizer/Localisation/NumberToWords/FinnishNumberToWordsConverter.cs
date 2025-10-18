namespace Humanizer;

class FinnishNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["nolla", "yksi", "kaksi", "kolme", "neljä", "viisi", "kuusi", "seitsemän", "kahdeksan", "yhdeksän", "kymmenen"];
    static readonly string[] OrdinalUnitsMap = ["nollas", "ensimmäinen", "toinen", "kolmas", "neljäs", "viides", "kuudes", "seitsemäs", "kahdeksas", "yhdeksäs", "kymmenes"];

    static readonly FrozenDictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
    {
        {1, "yhdes" },
        {2, "kahdes" }
    }.ToFrozenDictionary();

    public override string Convert(long input)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }
        var number = (int)input;

        if (number < 0)
        {
            return $"miinus {Convert(-number)}";
        }

        if (number == 0)
        {
            return UnitsMap[0];
        }

        var parts = new List<string>();

        if (number / 1000000000 > 0)
        {
            parts.Add(number / 1000000000 == 1
                ? "miljardi "
                : $"{Convert(number / 1000000000)}miljardia ");

            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            parts.Add(number / 1000000 == 1
                ? "miljoona "
                : $"{Convert(number / 1000000)}miljoonaa ");

            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? "tuhat "
                : $"{Convert(number / 1000)}tuhatta ");

            number %= 1000;
        }

        if (number / 100 > 0)
        {
            parts.Add(number / 100 == 1
                ? "sata"
                : $"{Convert(number / 100)}sataa");

            number %= 100;
        }

        if (number >= 20)
        {
            parts.Add($"{Convert(number / 10)}kymmentä");
            number %= 10;
        }
        else if (number is > 10 and < 20)
        {
            parts.Add($"{UnitsMap[number % 10]}toista");
        }

        if (number is > 0 and <= 10)
        {
            parts.Add(UnitsMap[number]);
        }

        return string.Concat(parts).Trim();
    }

    static string GetOrdinalUnit(int number, bool useExceptions)
    {
        if (useExceptions && OrdinalExceptions.TryGetValue(number, out var unit))
        {
            return unit;
        }

        return OrdinalUnitsMap[number];
    }

    static string ToOrdinal(int number, bool useExceptions)
    {
        if (number == 0)
        {
            return OrdinalUnitsMap[0];
        }

        var parts = new List<string>();

        if (number / 1000000000 > 0)
        {
            parts.Add($"{(number / 1000000000 == 1 ? "" : ToOrdinal(number / 1000000000, true))}miljardis");
            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            parts.Add($"{(number / 1000000 == 1 ? "" : ToOrdinal(number / 1000000, true))}miljoonas");
            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            parts.Add($"{(number / 1000 == 1 ? "" : ToOrdinal(number / 1000, true))}tuhannes");
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            parts.Add($"{(number / 100 == 1 ? "" : ToOrdinal(number / 100, true))}sadas");
            number %= 100;
        }

        if (number >= 20)
        {
            parts.Add($"{ToOrdinal(number / 10, true)}kymmenes");
            number %= 10;
        }
        else if (number is > 10 and < 20)
        {
            parts.Add($"{GetOrdinalUnit(number % 10, true)}toista");
        }

        if (number is > 0 and <= 10)
        {
            parts.Add(GetOrdinalUnit(number, useExceptions));
        }

        return string.Concat(parts);
    }

    public override string ConvertToOrdinal(int number) =>
        ToOrdinal(number, false);
}
