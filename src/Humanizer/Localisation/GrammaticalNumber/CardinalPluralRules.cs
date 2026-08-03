using System.Numerics;

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
    public static CardinalPluralCategory Select(CardinalPluralRuleKind kind, decimal quantity) =>
        Select(kind, CardinalPluralOperands.Create(quantity));

    public static CardinalPluralCategory Select(CardinalPluralRuleKind kind, long quantity) =>
        Select(kind, CardinalPluralOperands.Create(quantity));

    public static bool TrySelect(
        CardinalPluralRuleKind kind,
        double quantity,
        out CardinalPluralCategory category)
    {
        var value = CardinalPluralOperands.Create(quantity);
        if (!value.IsSupported)
        {
            category = CardinalPluralCategory.Other;
            return false;
        }

        category = Select(kind, value);
        return true;
    }

    static CardinalPluralCategory Select(CardinalPluralRuleKind kind, CardinalPluralOperands value)
    {
        var i = value.IntegerDigits;
        var v = value.V;
        var f = value.FractionDigits;
        var t = value.FractionDigitsWithoutTrailingZeros;

        return kind switch
        {
            CardinalPluralRuleKind.Other => CardinalPluralCategory.Other,
            CardinalPluralRuleKind.AmharicLike => i == 0 || NEquals(value, 1) ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Armenian => i == 0 || i == 1 ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.EnglishLike => i == 1 && v == 0 ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Sinhala => NEquals(value, 0) || NEquals(value, 1) || i == 0 && f == 1
                ? CardinalPluralCategory.One
                : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Punjabi => NIsInRange(value, 0, 1) ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.One => NEquals(value, 1) ? CardinalPluralCategory.One : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Danish => NEquals(value, 1) || !t.IsZero && (i == 0 || i == 1)
                ? CardinalPluralCategory.One
                : CardinalPluralCategory.Other,
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
                : v != 0 || NEquals(value, 0) || !NEquals(value, 1) && NModuloIsInRange(value, 100, 1, 19)
                    ? CardinalPluralCategory.Few
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.SouthSlavic => SelectSouthSlavic(value),
            CardinalPluralRuleKind.French => i == 0 || i == 1
                ? CardinalPluralCategory.One
                : IsExactNonZeroMillion(i, v)
                    ? CardinalPluralCategory.Many
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Portuguese => i == 0 || i == 1
                ? CardinalPluralCategory.One
                : IsExactNonZeroMillion(i, v)
                    ? CardinalPluralCategory.Many
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.CatalanItalian => i == 1 && v == 0
                ? CardinalPluralCategory.One
                : IsExactNonZeroMillion(i, v)
                    ? CardinalPluralCategory.Many
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Spanish => NEquals(value, 1)
                ? CardinalPluralCategory.One
                : IsExactNonZeroMillion(i, v)
                    ? CardinalPluralCategory.Many
                    : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Slovenian => SelectSlovenian(value),
            CardinalPluralRuleKind.CzechSlovak => i == 1 && v == 0
                ? CardinalPluralCategory.One
                : IsIntegerInRange(i, 2, 4) && v == 0
                    ? CardinalPluralCategory.Few
                    : v != 0
                        ? CardinalPluralCategory.Many
                        : CardinalPluralCategory.Other,
            CardinalPluralRuleKind.Polish => SelectPolish(value),
            CardinalPluralRuleKind.Belarusian => SelectBelarusian(value),
            CardinalPluralRuleKind.Lithuanian => SelectLithuanian(value),
            CardinalPluralRuleKind.RussianUkrainian => SelectRussianUkrainian(value),
            CardinalPluralRuleKind.Maltese => SelectMaltese(value),
            CardinalPluralRuleKind.Irish => SelectIrish(value),
            CardinalPluralRuleKind.Arabic => SelectArabic(value),
            CardinalPluralRuleKind.Welsh => SelectWelsh(value),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
    }

    static CardinalPluralCategory SelectFilipino(CardinalPluralOperands value)
    {
        var integer = value.IntegerDigits;
        var fraction = value.FractionDigits;
        var integerEnding = integer % 10;
        var fractionEnding = fraction % 10;
        var integerIsOneToThree = value.V == 0 && IsIntegerInRange(integer, 1, 3);
        var integerEndsInSupportedDigit = value.V == 0 && integerEnding != 4 && integerEnding != 6 && integerEnding != 9;
        var fractionEndsInSupportedDigit = value.V != 0 && fractionEnding != 4 && fractionEnding != 6 && fractionEnding != 9;
        return integerIsOneToThree || integerEndsInSupportedDigit || fractionEndsInSupportedDigit
            ? CardinalPluralCategory.One
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectHebrew(CardinalPluralOperands value)
    {
        if (value.IntegerDigits == 1 && value.V == 0)
        {
            return CardinalPluralCategory.One;
        }

        if (value.IntegerDigits == 0 && value.V != 0)
        {
            return CardinalPluralCategory.One;
        }

        return value.IntegerDigits == 2 && value.V == 0
            ? CardinalPluralCategory.Two
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectLatvian(CardinalPluralOperands value)
    {
        var v = value.V;
        var f = value.FractionDigits;
        var wholeEndsInZero = NModuloEquals(value, 10, 0);
        var wholeEndsInElevenToNineteen = NModuloIsInRange(value, 100, 11, 19);
        var twoDigitFractionEndsInElevenToNineteen = v == 2 && IsIntegerInRange(f % 100, 11, 19);
        if (wholeEndsInZero || wholeEndsInElevenToNineteen || twoDigitFractionEndsInElevenToNineteen)
        {
            return CardinalPluralCategory.Zero;
        }

        var wholeEndsInOneButNotEleven = NModuloEquals(value, 10, 1) && !NModuloEquals(value, 100, 11);
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
        var i = value.IntegerDigits;
        var v = value.V;
        var f = value.FractionDigits;
        if (v == 0 && i % 10 == 1 && i % 100 != 11 || f % 10 == 1 && f % 100 != 11)
        {
            return CardinalPluralCategory.One;
        }

        return v == 0 && IsIntegerInRange(i % 10, 2, 4) && !IsIntegerInRange(i % 100, 12, 14) ||
               IsIntegerInRange(f % 10, 2, 4) && !IsIntegerInRange(f % 100, 12, 14)
            ? CardinalPluralCategory.Few
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectSlovenian(CardinalPluralOperands value)
    {
        var i = value.IntegerDigits;
        var v = value.V;
        if (v == 0 && i % 100 == 1)
        {
            return CardinalPluralCategory.One;
        }

        if (v == 0 && i % 100 == 2)
        {
            return CardinalPluralCategory.Two;
        }

        return v == 0 && (i % 100 == 3 || i % 100 == 4) || v != 0
            ? CardinalPluralCategory.Few
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectPolish(CardinalPluralOperands value)
    {
        var i = value.IntegerDigits;
        var v = value.V;
        if (i == 1 && v == 0)
        {
            return CardinalPluralCategory.One;
        }

        if (v == 0 && IsIntegerInRange(i % 10, 2, 4) && !IsIntegerInRange(i % 100, 12, 14))
        {
            return CardinalPluralCategory.Few;
        }

        if (v != 0)
        {
            return CardinalPluralCategory.Other;
        }

        if (i != 1 && IsIntegerInRange(i % 10, 0, 1))
        {
            return CardinalPluralCategory.Many;
        }

        if (IsIntegerInRange(i % 10, 5, 9))
        {
            return CardinalPluralCategory.Many;
        }

        return IsIntegerInRange(i % 100, 12, 14)
            ? CardinalPluralCategory.Many
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectBelarusian(CardinalPluralOperands value)
    {
        if (NModuloEquals(value, 10, 1) && !NModuloEquals(value, 100, 11))
        {
            return CardinalPluralCategory.One;
        }

        if (NModuloIsInRange(value, 10, 2, 4) && !NModuloIsInRange(value, 100, 12, 14))
        {
            return CardinalPluralCategory.Few;
        }

        return NModuloEquals(value, 10, 0) ||
               NModuloIsInRange(value, 10, 5, 9) ||
               NModuloIsInRange(value, 100, 11, 14)
            ? CardinalPluralCategory.Many
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectLithuanian(CardinalPluralOperands value)
    {
        if (NModuloEquals(value, 10, 1) && !NModuloIsInRange(value, 100, 11, 19))
        {
            return CardinalPluralCategory.One;
        }

        if (NModuloIsInRange(value, 10, 2, 9) && !NModuloIsInRange(value, 100, 11, 19))
        {
            return CardinalPluralCategory.Few;
        }

        return !value.FractionDigits.IsZero ? CardinalPluralCategory.Many : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectRussianUkrainian(CardinalPluralOperands value)
    {
        var i = value.IntegerDigits;
        var v = value.V;
        if (v == 0 && i % 10 == 1 && i % 100 != 11)
        {
            return CardinalPluralCategory.One;
        }

        if (v == 0 && IsIntegerInRange(i % 10, 2, 4) && !IsIntegerInRange(i % 100, 12, 14))
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

        if (IsIntegerInRange(i % 10, 5, 9))
        {
            return CardinalPluralCategory.Many;
        }

        return IsIntegerInRange(i % 100, 11, 14)
            ? CardinalPluralCategory.Many
            : CardinalPluralCategory.Other;
    }

    static CardinalPluralCategory SelectMaltese(CardinalPluralOperands value) =>
        NEquals(value, 1)
            ? CardinalPluralCategory.One
            : NEquals(value, 2)
                ? CardinalPluralCategory.Two
                : NEquals(value, 0) || NModuloIsInRange(value, 100, 3, 10)
                    ? CardinalPluralCategory.Few
                    : NModuloIsInRange(value, 100, 11, 19)
                        ? CardinalPluralCategory.Many
                        : CardinalPluralCategory.Other;

    static CardinalPluralCategory SelectIrish(CardinalPluralOperands value) =>
        NEquals(value, 1)
            ? CardinalPluralCategory.One
            : NEquals(value, 2)
                ? CardinalPluralCategory.Two
                : NIsInRange(value, 3, 6)
                    ? CardinalPluralCategory.Few
                    : NIsInRange(value, 7, 10)
                        ? CardinalPluralCategory.Many
                        : CardinalPluralCategory.Other;

    static CardinalPluralCategory SelectArabic(CardinalPluralOperands value) =>
        NEquals(value, 0)
            ? CardinalPluralCategory.Zero
            : NEquals(value, 1)
                ? CardinalPluralCategory.One
                : NEquals(value, 2)
                    ? CardinalPluralCategory.Two
                    : NModuloIsInRange(value, 100, 3, 10)
                        ? CardinalPluralCategory.Few
                        : NModuloIsInRange(value, 100, 11, 99)
                            ? CardinalPluralCategory.Many
                            : CardinalPluralCategory.Other;

    static CardinalPluralCategory SelectWelsh(CardinalPluralOperands value) =>
        NEquals(value, 0)
            ? CardinalPluralCategory.Zero
            : NEquals(value, 1)
                ? CardinalPluralCategory.One
                : NEquals(value, 2)
                    ? CardinalPluralCategory.Two
                    : NEquals(value, 3)
                        ? CardinalPluralCategory.Few
                        : NEquals(value, 6)
                            ? CardinalPluralCategory.Many
                            : CardinalPluralCategory.Other;

    static bool IsExactNonZeroMillion(BigInteger i, int v) =>
        i != 0 && i % 1_000_000 == 0 && v == 0;

    static bool NEquals(CardinalPluralOperands value, int expected) =>
        value.FractionDigits.IsZero && value.IntegerDigits == expected;

    static bool NIsInRange(CardinalPluralOperands value, int minimum, int maximum) =>
        value.FractionDigits.IsZero &&
        IsIntegerInRange(value.IntegerDigits, minimum, maximum);

    static bool NModuloEquals(CardinalPluralOperands value, int divisor, int expected) =>
        value.FractionDigits.IsZero && value.IntegerDigits % divisor == expected;

    static bool NModuloIsInRange(
        CardinalPluralOperands value,
        int divisor,
        int minimum,
        int maximum) =>
        value.FractionDigits.IsZero &&
        IsIntegerInRange(value.IntegerDigits % divisor, minimum, maximum);

    static bool IsIntegerInRange(BigInteger value, int minimum, int maximum) =>
        value >= minimum && value <= maximum;
}