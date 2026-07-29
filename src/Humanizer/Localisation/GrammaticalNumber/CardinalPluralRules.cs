namespace Humanizer;

/// <summary>
/// Identifies one of the CLDR 48.2 cardinal rule sets used by Humanizer locales.
/// </summary>
enum CardinalPluralRuleKind
{
    Other,
    AmharicLike,
    Armenian,
    EnglishLike,
    Sinhala,
    Punjabi,
    One,
    Danish,
    Icelandic,
    Macedonian,
    Filipino,
    Latvian,
    Hebrew,
    Romanian,
    SouthSlavic,
    French,
    Portuguese,
    CatalanItalian,
    Spanish,
    Slovenian,
    CzechSlovak,
    Polish,
    Belarusian,
    Lithuanian,
    RussianUkrainian,
    Maltese,
    Irish,
    Arabic,
    Welsh
}

/// <summary>
/// Unicode CLDR 48.2 cardinal selectors for the rule sets referenced by Humanizer locales.
/// </summary>
static class CardinalPluralRules
{
    public static CardinalPluralCategory Select(CardinalPluralRuleKind kind, decimal quantity)
    {
        var value = CardinalPluralOperands.Create(quantity);
        var n = value.N;
        var i = value.I;
        var v = value.V;
        var f = value.F;
        var t = value.T;

        return kind switch
        {
            CardinalPluralRuleKind.Other => CardinalPluralCategory.Other,
            CardinalPluralRuleKind.AmharicLike => i == 0 || n == 1 ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Armenian => i is 0 or 1 ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.EnglishLike => i == 1 && v == 0 ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Sinhala => n is 0 or 1 || i == 0 && f == 1 ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Punjabi => IsIntegerInRange(n, 0, 1) ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.One => n == 1 ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Danish => n == 1 || t != 0 && i is 0 or 1 ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Icelandic => t == 0 && i % 10 == 1 && i % 100 != 11 ||
                                                t % 10 == 1 && t % 100 != 11
                ? CardinalPluralCategory.One
                : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Macedonian => v == 0 && i % 10 == 1 && i % 100 != 11 ||
                                                 f % 10 == 1 && f % 100 != 11
                ? CardinalPluralCategory.One
                : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Filipino => SelectFilipino(value),
            CardinalPluralRuleKind.Latvian => SelectLatvian(value),
            CardinalPluralRuleKind.Hebrew => SelectHebrew(value),
            CardinalPluralRuleKind.Romanian => i == 1 && v == 0
                ? CardinalPluralCategory.One
                : v != 0 || n == 0 || n != 1 && IsIntegerInRange(n % 100, 1, 19)
                    ? CardinalPluralCategory.Few
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.SouthSlavic => SelectSouthSlavic(value),
            CardinalPluralRuleKind.French => i is 0 or 1
                ? CardinalPluralCategory.One
                : IsExactNonZeroMillion(i, v)
                    ? CardinalPluralCategory.Many
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Portuguese => i is >= 0 and <= 1
                ? CardinalPluralCategory.One
                : IsExactNonZeroMillion(i, v)
                    ? CardinalPluralCategory.Many
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.CatalanItalian => i == 1 && v == 0
                ? CardinalPluralCategory.One
                : IsExactNonZeroMillion(i, v)
                    ? CardinalPluralCategory.Many
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Spanish => n == 1
                ? CardinalPluralCategory.One
                : IsExactNonZeroMillion(i, v)
                    ? CardinalPluralCategory.Many
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Slovenian => SelectSlovenian(value),
            CardinalPluralRuleKind.CzechSlovak => i == 1 && v == 0
                ? CardinalPluralCategory.One
                : i is >= 2 and <= 4 && v == 0
                    ? CardinalPluralCategory.Few
                    : v != 0
                        ? CardinalPluralCategory.Many
                        : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Polish => SelectPolish(value),
            CardinalPluralRuleKind.Belarusian => SelectBelarusian(n),
            CardinalPluralRuleKind.Lithuanian => SelectLithuanian(value),
            CardinalPluralRuleKind.RussianUkrainian => SelectRussianUkrainian(value),
            CardinalPluralRuleKind.Maltese => SelectMaltese(n),
            CardinalPluralRuleKind.Irish => SelectIrish(n),
            CardinalPluralRuleKind.Arabic => SelectArabic(n),
            CardinalPluralRuleKind.Welsh => SelectWelsh(n),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
    }

    static CardinalPluralCategory SelectFilipino(CardinalPluralOperands value)
    {
        var integerIsOneToThree = value.V == 0 && value.I is 1 or 2 or 3;
        var integerEndsInSupportedDigit = value.V == 0 && value.I % 10 is not (4 or 6 or 9);
        var fractionEndsInSupportedDigit = value.V != 0 && value.F % 10 is not (4 or 6 or 9);
        return integerIsOneToThree || integerEndsInSupportedDigit || fractionEndsInSupportedDigit
            ? CardinalPluralCategory.One
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectHebrew(CardinalPluralOperands value)
    {
        if (value.I == 1 && value.V == 0)
        {
            return CardinalPluralCategory.One;
        }

        if (value.I == 0 && value.V != 0)
        {
            return CardinalPluralCategory.One;
        }

        return value.I == 2 && value.V == 0
            ? CardinalPluralCategory.Two
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectLatvian(CardinalPluralOperands value)
    {
        var n = value.N;
        var v = value.V;
        var f = value.F;
        var wholeEndsInZero = n % 10 == 0;
        var wholeEndsInElevenToNineteen = IsIntegerInRange(n % 100, 11, 19);
        var twoDigitFractionEndsInElevenToNineteen = v == 2 && f % 100 is >= 11 and <= 19;
        if (wholeEndsInZero || wholeEndsInElevenToNineteen || twoDigitFractionEndsInElevenToNineteen)
        {
            return CardinalPluralCategory.Zero;
        }

        var wholeEndsInOneButNotEleven = n % 10 == 1 && n % 100 != 11;
        var twoDigitFractionEndsInOneButNotEleven = v == 2 && f % 10 == 1 && f % 100 != 11;
        var otherFractionEndsInOne = v != 2 && f % 10 == 1;
        return wholeEndsInOneButNotEleven ||
               twoDigitFractionEndsInOneButNotEleven ||
               otherFractionEndsInOne
            ? CardinalPluralCategory.One
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectSouthSlavic(CardinalPluralOperands value)
    {
        var i = value.I;
        var v = value.V;
        var f = value.F;
        if (v == 0 && i % 10 == 1 && i % 100 != 11 || f % 10 == 1 && f % 100 != 11)
        {
            return CardinalPluralCategory.One;
        }

        return v == 0 && i % 10 is >= 2 and <= 4 && i % 100 is not (>= 12 and <= 14) ||
               f % 10 is >= 2 and <= 4 && f % 100 is not (>= 12 and <= 14)
            ? CardinalPluralCategory.Few
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectSlovenian(CardinalPluralOperands value)
    {
        var i = value.I;
        var v = value.V;
        if (v == 0 && i % 100 == 1)
        {
            return CardinalPluralCategory.One;
        }

        if (v == 0 && i % 100 == 2)
        {
            return CardinalPluralCategory.Two;
        }

        return v == 0 && i % 100 is 3 or 4 || v != 0
            ? CardinalPluralCategory.Few
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectPolish(CardinalPluralOperands value)
    {
        var i = value.I;
        var v = value.V;
        if (i == 1 && v == 0)
        {
            return CardinalPluralCategory.One;
        }

        if (v == 0 && i % 10 is >= 2 and <= 4 && i % 100 is not (>= 12 and <= 14))
        {
            return CardinalPluralCategory.Few;
        }

        if (v != 0)
        {
            return CardinalPluralCategory.Other;
        }

        if (i != 1 && i % 10 is >= 0 and <= 1)
        {
            return CardinalPluralCategory.Many;
        }

        if (i % 10 is >= 5 and <= 9)
        {
            return CardinalPluralCategory.Many;
        }

        return i % 100 is >= 12 and <= 14
            ? CardinalPluralCategory.Many
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectBelarusian(decimal n)
    {
        if (n % 10 == 1 && n % 100 != 11)
        {
            return CardinalPluralCategory.One;
        }

        if (IsIntegerInRange(n % 10, 2, 4) && !IsIntegerInRange(n % 100, 12, 14))
        {
            return CardinalPluralCategory.Few;
        }

        return n % 10 == 0 || IsIntegerInRange(n % 10, 5, 9) || IsIntegerInRange(n % 100, 11, 14)
            ? CardinalPluralCategory.Many
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectLithuanian(CardinalPluralOperands value)
    {
        var n = value.N;
        if (n % 10 == 1 && !IsIntegerInRange(n % 100, 11, 19))
        {
            return CardinalPluralCategory.One;
        }

        if (IsIntegerInRange(n % 10, 2, 9) && !IsIntegerInRange(n % 100, 11, 19))
        {
            return CardinalPluralCategory.Few;
        }

        return value.F != 0 ? CardinalPluralCategory.Many : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectRussianUkrainian(CardinalPluralOperands value)
    {
        var i = value.I;
        var v = value.V;
        if (v == 0 && i % 10 == 1 && i % 100 != 11)
        {
            return CardinalPluralCategory.One;
        }

        if (v == 0 && i % 10 is >= 2 and <= 4 && i % 100 is not (>= 12 and <= 14))
        {
            return CardinalPluralCategory.Few;
        }

        if (v != 0)
        {
            return CardinalPluralCategory.Other;
        }

        if (i % 10 == 0)
        {
            return CardinalPluralCategory.Many;
        }

        if (i % 10 is >= 5 and <= 9)
        {
            return CardinalPluralCategory.Many;
        }

        return i % 100 is >= 11 and <= 14
            ? CardinalPluralCategory.Many
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectMaltese(decimal n) =>
        n == 1
            ? CardinalPluralCategory.One
            : n == 2
                ? CardinalPluralCategory.Two
                : n == 0 || IsIntegerInRange(n % 100, 3, 10)
                    ? CardinalPluralCategory.Few
                    : IsIntegerInRange(n % 100, 11, 19)
                        ? CardinalPluralCategory.Many
                        : CardinalPluralCategory.Other;

    static CardinalPluralCategory SelectIrish(decimal n) =>
        n == 1
            ? CardinalPluralCategory.One
            : n == 2
                ? CardinalPluralCategory.Two
                : IsIntegerInRange(n, 3, 6)
                    ? CardinalPluralCategory.Few
                    : IsIntegerInRange(n, 7, 10)
                        ? CardinalPluralCategory.Many
                        : CardinalPluralCategory.Other;

    static CardinalPluralCategory SelectArabic(decimal n) =>
        n == 0
            ? CardinalPluralCategory.Zero
            : n == 1
                ? CardinalPluralCategory.One
                : n == 2
                    ? CardinalPluralCategory.Two
                    : IsIntegerInRange(n % 100, 3, 10)
                        ? CardinalPluralCategory.Few
                        : IsIntegerInRange(n % 100, 11, 99)
                            ? CardinalPluralCategory.Many
                            : CardinalPluralCategory.Other;

    static CardinalPluralCategory SelectWelsh(decimal n) =>
        n == 0
            ? CardinalPluralCategory.Zero
            : n == 1
                ? CardinalPluralCategory.One
                : n == 2
                    ? CardinalPluralCategory.Two
                    : n == 3
                        ? CardinalPluralCategory.Few
                        : n == 6
                            ? CardinalPluralCategory.Many
                            : CardinalPluralCategory.Other;

    static bool IsExactNonZeroMillion(decimal i, int v) =>
        i != 0 && i % 1_000_000 == 0 && v == 0;

    static bool IsIntegerInRange(decimal value, int minimum, int maximum) =>
        decimal.Truncate(value) == value && value >= minimum && value <= maximum;
}