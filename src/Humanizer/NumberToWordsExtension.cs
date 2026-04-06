namespace Humanizer;

/// <summary>
/// Converts numbers to localized words and ordinals.
/// The output is culture-aware, including locale-specific high-range scale names and
/// English-family differences such as <c>en</c>, <c>en-GB</c>, and <c>en-IN</c>.
/// </summary>
public static class NumberToWordsExtension
{
    /// <summary>
    /// 1.ToOrdinalWords() -> "first"
    /// </summary>
    /// <param name="number">Number to be turned to ordinal words</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    public static string ToOrdinalWords(this int number, CultureInfo? culture = null) =>
        number == 1 && TryGetNativeFirstOrdinalWord(culture, null) is { } firstOrdinalWord
            ? firstOrdinalWord
            : Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number);

    /// <summary>
    /// Converts a number to ordinal words supporting locale's specific variations.
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// 1.ToOrdinalWords(WordForm.Normal) -> "primero" // As in "He llegado el primero".
    /// 3.ToOrdinalWords(WordForm.Abbreviation) -> "tercer" // As in "Vivo en el tercer piso"
    /// </code>
    /// </example>
    /// <param name="number">Number to be turned to ordinal words</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The number converted into ordinal words</returns>
    public static string ToOrdinalWords(this int number, WordForm wordForm, CultureInfo? culture = null) =>
        Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number, wordForm);

    /// <summary>
    /// for Brazilian Portuguese locale
    /// 1.ToOrdinalWords(GrammaticalGender.Masculine) -> "primeiro"
    /// 1.ToOrdinalWords(GrammaticalGender.Feminine) -> "primeira"
    /// </summary>
    /// <param name="number">Number to be turned to words</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    public static string ToOrdinalWords(this int number, GrammaticalGender gender, CultureInfo? culture = null) =>
        number == 1 && TryGetNativeFirstOrdinalWord(culture, gender) is { } firstOrdinalWord
            ? firstOrdinalWord
            : Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number, gender);

    /// <summary>
    /// Converts a number to ordinal words supporting locale's specific variations.
    /// </summary>
    /// <example>
    /// In Spanish:
    /// <code>
    /// 3.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Normal) -> "tercero"
    /// 3.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Abbreviation) -> "tercer"
    /// 3.ToOrdinalWords(GrammaticalGender.Feminine, WordForm.Normal) -> "tercera"
    /// 3.ToOrdinalWords(GrammaticalGender.Feminine, WordForm.Abbreviation) -> "tercera"
    /// </code>
    /// </example>
    /// <param name="number">Number to be turned to ordinal words</param>
    /// <param name="gender">The grammatical gender to use for output words</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The number converted into ordinal words</returns>
    public static string ToOrdinalWords(this int number, GrammaticalGender gender, WordForm wordForm, CultureInfo? culture = null) =>
        number == 1 && TryGetNativeFirstOrdinalWord(culture, gender) is { } firstOrdinalWord
            ? firstOrdinalWord
            : Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number, gender, wordForm);

    /// <summary>
    /// 1.ToTuple() -> "single"
    /// </summary>
    /// <param name="number">Number to be turned to tuple</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    public static string ToTuple(this int number, CultureInfo? culture = null) =>
        Configurator.GetNumberToWordsConverter(culture).ConvertToTuple(number);

    /// <summary>
    /// Converts the given value to localized cardinal words.
    /// </summary>
    /// <param name="number">The value to convert.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this int number, CultureInfo? culture = null) =>
        ((long)number).ToWords(culture);

    /// <summary>
    /// Converts the given value to localized cardinal words using both word form and grammatical gender.
    /// </summary>
    /// <example>
    /// In Spanish, numbers ended in 1 change its form depending on their position in the sentence.
    /// <code>
    /// 21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
    /// 21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
    /// </code>
    /// </example>
    /// <param name="number">Number to be turned to words</param>
    /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The number converted to words</returns>
    public static string ToWords(this int number, WordForm wordForm, CultureInfo? culture = null) =>
        ((long)number).ToWords(wordForm, culture);

    static string? TryGetNativeFirstOrdinalWord(CultureInfo? culture, GrammaticalGender? gender)
    {
        if (culture is null)
        {
            return null;
        }

        return culture.TwoLetterISOLanguageName switch
        {
            "da" => "første",
            "fil" => "una",
            "id" => "pertama",
            "ms" => "pertama",
            "sk" => gender switch
            {
                GrammaticalGender.Masculine => "prvý",
                GrammaticalGender.Feminine => "prvá",
                GrammaticalGender.Neuter => "prvé",
                _ => "prvý"
            },
            "zu" => "okokuqala",
            _ => null
        };
    }

    /// <summary>
    /// Converts the given value to localized cardinal words with an explicit conjunction choice.
    /// </summary>
    /// <param name="number">The value to convert.</param>
    /// <param name="addAnd">Whether to include the culture's conjunction before the terminal group.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this int number, bool addAnd, CultureInfo? culture = null) =>
        ((long)number).ToWords(culture, addAnd);

    /// <summary>
    /// Converts the given value to localized cardinal words with an explicit conjunction choice
    /// and requested word form.
    /// </summary>
    /// <example>
    /// In Spanish, numbers ended in 1 changes its form depending on their position in the sentence.
    /// <code>
    /// 21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
    /// 21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
    /// </code>
    /// </example>
    /// <param name="number">The value to convert.</param>
    /// <param name="addAnd">Whether to include the culture's conjunction before the terminal group.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this int number, bool addAnd, WordForm wordForm, CultureInfo? culture = null) =>
        ((long)number).ToWords(wordForm, culture, addAnd);

    /// <summary>
    /// Converts the given value to localized cardinal words using grammatical gender where supported.
    /// </summary>
    /// <example>
    /// Russian:
    /// <code>
    ///   1.ToWords(GrammaticalGender.Masculine) -> "один"
    ///   1.ToWords(GrammaticalGender.Feminine) -> "одна"
    /// </code>
    /// Hebrew:
    /// <code>
    ///   1.ToWords(GrammaticalGender.Masculine) -> "אחד"
    ///   1.ToWords(GrammaticalGender.Feminine) -> "אחת"
    /// </code>
    /// </example>
    /// <param name="number">The value to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale supports gendered forms.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this int number, GrammaticalGender gender, CultureInfo? culture = null) =>
        ((long)number).ToWords(gender, culture);

    /// <summary>
    /// Converts the given value to localized cardinal words using both word form and grammatical gender.
    /// </summary>
    /// <example>
    /// In Spanish, numbers ended in 1 change its form depending on their position in the sentence.
    /// <code>
    /// 21.ToWords(WordForm.Normal, GrammaticalGender.Masculine) -> veintiuno // as in "Mi número favorito es el veintiuno".
    /// 21.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) -> veintiún // as in "En total, conté veintiún coches"
    /// 21.ToWords(WordForm.Normal, GrammaticalGender.Feminine) -> veintiuna // as in "veintiuna personas"
    /// </code>
    /// </example>
    /// <param name="number">The value to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <param name="gender">The grammatical gender to use when the locale supports gendered forms.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this int number, WordForm wordForm, GrammaticalGender gender, CultureInfo? culture = null) =>
        ((long)number).ToWords(wordForm, gender, culture);

    /// <summary>
    /// Converts the given value to localized cardinal words using the culture's default conjunction policy.
    /// </summary>
    /// <param name="number">The value to convert.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <param name="addAnd">Whether to include the culture's conjunction before the terminal group.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this long number, CultureInfo? culture = null, bool addAnd = true) =>
        Configurator.GetNumberToWordsConverter(culture).Convert(number, addAnd);

    /// <summary>
    /// Converts the given value to localized cardinal words using the requested word form.
    /// </summary>
    /// <example>
    /// In Spanish, numbers ended in 1 changes its form depending on their position in the sentence.
    /// <code>
    /// 21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
    /// 21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
    /// </code>
    /// </example>
    /// <param name="number">The value to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <param name="addAnd">Whether to include the culture's conjunction before the terminal group.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this long number, WordForm wordForm, CultureInfo? culture = null, bool addAnd = false) =>
        Configurator.GetNumberToWordsConverter(culture).Convert(number, addAnd, wordForm);

    /// <summary>
    /// Converts the given value to localized cardinal words using grammatical gender where supported.
    /// </summary>
    /// <example>
    /// Russian:
    /// <code>
    ///   1.ToWords(GrammaticalGender.Masculine) -> "один"
    ///   1.ToWords(GrammaticalGender.Feminine) -> "одна"
    /// </code>
    /// Hebrew:
    /// <code>
    ///   1.ToWords(GrammaticalGender.Masculine) -> "אחד"
    ///   1.ToWords(GrammaticalGender.Feminine) -> "אחת"
    /// </code>
    /// </example>
    ///
    /// <param name="number">The value to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale supports gendered forms.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this long number, GrammaticalGender gender, CultureInfo? culture = null) =>
        Configurator.GetNumberToWordsConverter(culture).Convert(number, gender);

    /// <summary>
    /// Converts the given value to localized cardinal words using both word form and grammatical gender.
    /// </summary>
    /// <example>
    /// In Spanish, numbers ended in 1 changes its form depending on their position in the sentence.
    /// <code>
    /// 21.ToWords(WordForm.Normal, GrammaticalGender.Masculine) -> veintiuno // as in "Mi número favorito es el veintiuno".
    /// 21.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) -> veintiún // as in "En total, conté veintiún coches"
    /// 21.ToWords(WordForm.Normal, GrammaticalGender.Feminine) -> veintiuna // as in "veintiuna personas"
    /// </code>
    /// </example>
    /// <param name="number">The value to convert.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <param name="gender">The grammatical gender to use when the locale supports gendered forms.</param>
    /// <param name="culture">The culture to use. If <c>null</c>, the current UI culture is used.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public static string ToWords(this long number, WordForm wordForm, GrammaticalGender gender, CultureInfo? culture = null) =>
        Configurator.GetNumberToWordsConverter(culture).Convert(number, wordForm, gender);
}
