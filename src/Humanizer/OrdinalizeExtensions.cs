namespace Humanizer;

/// <summary>
/// Ordinalize extensions
/// </summary>
public static class OrdinalizeExtensions
{
    /// <summary>
    /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
    /// </summary>
    /// <param name="numberString">The number, in string, to be ordinalized</param>
    public static string Ordinalize(this string numberString) =>
        Configurator.Ordinalizer.Convert(int.Parse(numberString), NormalizeOrdinalNumberString(numberString));

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations.
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// "1".Ordinalize(WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
    /// "1".Ordinalize(WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// </code>
    /// </example>
    /// <param name="numberString">The number, in string, to be ordinalized</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <returns>The number ordinalized</returns>
    public static string Ordinalize(this string numberString, WordForm wordForm) =>
        Configurator.Ordinalizer.Convert(int.Parse(numberString), NormalizeOrdinalNumberString(numberString), wordForm);

    /// <summary>
    /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
    /// </summary>
    /// <param name="numberString">The number, in string, to be ordinalized</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    public static string Ordinalize(this string numberString, CultureInfo culture)
    {
        var resolvedCulture = culture ?? CultureInfo.CurrentUICulture;
        return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(ParseOrdinalNumber(numberString, resolvedCulture), NormalizeOrdinalNumberString(numberString));
    }

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations.
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// "1".Ordinalize(new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
    /// "1".Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// </code>
    /// </example>
    /// <param name="numberString">The number to be ordinalized</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <returns>The number ordinalized</returns>
    public static string Ordinalize(this string numberString, CultureInfo culture, WordForm wordForm)
    {
        var resolvedCulture = culture ?? CultureInfo.CurrentUICulture;
        return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(ParseOrdinalNumber(numberString, resolvedCulture), NormalizeOrdinalNumberString(numberString), wordForm);
    }

    /// <summary>
    /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
    /// Gender for Brazilian Portuguese locale
    /// "1".Ordinalize(GrammaticalGender.Masculine) -> "1º"
    /// "1".Ordinalize(GrammaticalGender.Feminine) -> "1ª"
    /// </summary>
    /// <param name="numberString">The number, in string, to be ordinalized</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    public static string Ordinalize(this string numberString, GrammaticalGender gender) =>
        Configurator.Ordinalizer.Convert(int.Parse(numberString), NormalizeOrdinalNumberString(numberString), gender);

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
    /// locale's variations using the grammatical gender provided
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// "1".Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
    /// "1".Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// "1".Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) -> 1.ª // As in "Es 1ª vez que hago esto"
    /// </code>
    /// </example>
    /// <param name="numberString">The number to be ordinalized</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <returns>The number ordinalized</returns>
    public static string Ordinalize(this string numberString, GrammaticalGender gender, WordForm wordForm) =>
        Configurator.Ordinalizer.Convert(int.Parse(numberString), NormalizeOrdinalNumberString(numberString), gender, wordForm);

    /// <summary>
    /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
    /// Gender for Brazilian Portuguese locale
    /// "1".Ordinalize(GrammaticalGender.Masculine) -> "1º"
    /// "1".Ordinalize(GrammaticalGender.Feminine) -> "1ª"
    /// </summary>
    /// <param name="numberString">The number, in string, to be ordinalized</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    public static string Ordinalize(this string numberString, GrammaticalGender gender, CultureInfo culture)
    {
        var resolvedCulture = culture ?? CultureInfo.CurrentUICulture;
        return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(ParseOrdinalNumber(numberString, resolvedCulture), NormalizeOrdinalNumberString(numberString), gender);
    }

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
    /// locale's variations using the grammatical gender provided
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// "1".Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
    /// "1".Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// "1".Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// </code>
    /// </example>
    /// <param name="numberString">The number to be ordinalized</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <returns>The number ordinalized</returns>
    public static string Ordinalize(this string numberString, GrammaticalGender gender, CultureInfo culture, WordForm wordForm)
    {
        var resolvedCulture = culture ?? CultureInfo.CurrentUICulture;
        return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(ParseOrdinalNumber(numberString, resolvedCulture), NormalizeOrdinalNumberString(numberString), gender, wordForm);
    }

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
    /// </summary>
    /// <param name="number">The number to be ordinalized</param>
    public static string Ordinalize(this int number) =>
        Configurator.Ordinalizer.Convert(number, NormalizeOrdinalNumberString(number.ToString(CultureInfo.InvariantCulture)));

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations.
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// 1.Ordinalize(WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
    /// 1.Ordinalize(WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// </code>
    /// </example>
    /// <param name="number">The number to be ordinalized</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <returns>The number ordinalized</returns>
    public static string Ordinalize(this int number, WordForm wordForm) =>
        Configurator.Ordinalizer.Convert(number, NormalizeOrdinalNumberString(number.ToString(CultureInfo.InvariantCulture)), wordForm);

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
    /// </summary>
    /// <param name="number">The number to be ordinalized</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    public static string Ordinalize(this int number, CultureInfo culture)
    {
        var resolvedCulture = culture ?? CultureInfo.CurrentUICulture;
        return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(number, NormalizeOrdinalNumberString(FormatOrdinalNumberString(number, resolvedCulture)));
    }

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations.
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// 1.Ordinalize(new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
    /// 1.Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// </code>
    /// </example>
    /// <param name="number">The number to be ordinalized</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <returns>The number ordinalized</returns>
    public static string Ordinalize(this int number, CultureInfo culture, WordForm wordForm)
    {
        var resolvedCulture = culture ?? CultureInfo.CurrentUICulture;
        return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(number, NormalizeOrdinalNumberString(FormatOrdinalNumberString(number, resolvedCulture)), wordForm);
    }

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
    /// Gender for Brazilian Portuguese locale
    /// 1.Ordinalize(GrammaticalGender.Masculine) -> "1º"
    /// 1.Ordinalize(GrammaticalGender.Feminine) -> "1ª"
    /// </summary>
    /// <param name="number">The number to be ordinalized</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    public static string Ordinalize(this int number, GrammaticalGender gender) =>
        Configurator.Ordinalizer.Convert(number, NormalizeOrdinalNumberString(number.ToString(CultureInfo.InvariantCulture)), gender);

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
    /// locale's variations using the grammatical gender provided
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// 1.Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
    /// 1.Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// 1.Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) -> 1.ª // As in "Es 1ª vez que hago esto"
    /// </code>
    /// </example>
    /// <param name="number">The number to be ordinalized</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <returns>The number ordinalized</returns>
    public static string Ordinalize(this int number, GrammaticalGender gender, WordForm wordForm) =>
        Configurator.Ordinalizer.Convert(number, NormalizeOrdinalNumberString(number.ToString(CultureInfo.InvariantCulture)), gender, wordForm);

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
    /// Gender for Brazilian Portuguese locale
    /// 1.Ordinalize(GrammaticalGender.Masculine) -> "1º"
    /// 1.Ordinalize(GrammaticalGender.Feminine) -> "1ª"
    /// </summary>
    /// <param name="number">The number to be ordinalized</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    public static string Ordinalize(this int number, GrammaticalGender gender, CultureInfo culture)
    {
        var resolvedCulture = culture ?? CultureInfo.CurrentUICulture;
        return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(number, NormalizeOrdinalNumberString(FormatOrdinalNumberString(number, resolvedCulture)), gender);
    }

    /// <summary>
    /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
    /// locale's variations using the grammatical gender provided
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// 1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
    /// 1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// 1.Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
    /// </code>
    /// </example>
    /// <param name="number">The number to be ordinalized</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <returns>The number ordinalized</returns>
    public static string Ordinalize(this int number, GrammaticalGender gender, CultureInfo culture, WordForm wordForm)
    {
        var resolvedCulture = culture ?? CultureInfo.CurrentUICulture;
        return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(number, NormalizeOrdinalNumberString(FormatOrdinalNumberString(number, resolvedCulture)), gender, wordForm);
    }

    static string FormatOrdinalNumberString(int number, CultureInfo culture)
    {
        if (number >= 0)
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }

        if (LocaleNumberFormattingOverrides.TryGetNegativeSign(culture, out var negativeSign))
        {
            return FormatNegativeOrdinalNumberString(number, negativeSign!);
        }

        var cultureNegativeSign = culture.NumberFormat.NegativeSign;
        return cultureNegativeSign == NumberFormatInfo.InvariantInfo.NegativeSign
            ? number.ToString(CultureInfo.InvariantCulture)
            : FormatNegativeOrdinalNumberString(number, cultureNegativeSign);
    }

    static string FormatNegativeOrdinalNumberString(int number, string negativeSign)
    {
        if (negativeSign == NumberFormatInfo.InvariantInfo.NegativeSign)
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }

        var magnitude = number == int.MinValue ? (long)int.MaxValue + 1 : Math.Abs(number);
        return negativeSign + magnitude.ToString(CultureInfo.InvariantCulture);
    }

    static int ParseOrdinalNumber(string numberString, CultureInfo culture) =>
        int.Parse(numberString, culture);

    static string NormalizeOrdinalNumberString(string numberString)
    {
        var builder = default(System.Text.StringBuilder);

        for (var i = 0; i < numberString.Length; i++)
        {
            var character = numberString[i];
            if (char.GetUnicodeCategory(character) != UnicodeCategory.Format)
            {
                builder?.Append(character);
                continue;
            }

            builder ??= new System.Text.StringBuilder(numberString.Length);
            builder.Append(numberString, 0, i);
        }

        return builder?.ToString() ?? numberString;
    }
}