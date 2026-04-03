namespace Humanizer.Tests.Localisation;

static class LocaleNumberPhraseAssertions
{
    public static void VerifyCardinal(string localeName, int number, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToWords(culture));
    }

    public static void VerifyCardinalWithAddAnd(string localeName, int number, bool addAnd, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToWords(addAnd, culture));
    }

    public static void VerifyCardinalWithWordForm(string localeName, int number, WordForm wordForm, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToWords(wordForm, culture));
    }

    public static void VerifyCardinalWithGender(string localeName, int number, GrammaticalGender gender, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToWords(gender, culture));
    }

    public static void VerifyCardinalWithWordFormAndGender(string localeName, int number, WordForm wordForm, GrammaticalGender gender, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToWords(wordForm, gender, culture));
    }

    public static void VerifyOrdinal(string localeName, int number, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToOrdinalWords(culture));
    }

    public static void VerifyOrdinalWithWordForm(string localeName, int number, WordForm wordForm, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToOrdinalWords(wordForm, culture));
    }

    public static void VerifyOrdinalWithGender(string localeName, int number, GrammaticalGender gender, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToOrdinalWords(gender, culture));
    }

    public static void VerifyOrdinalWithWordFormAndGender(string localeName, int number, GrammaticalGender gender, WordForm wordForm, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToOrdinalWords(gender, wordForm, culture));
    }

    public static void VerifyTuple(string localeName, int number, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, number.ToTuple(culture));
    }
}
