namespace Humanizer;

internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
{
    private static readonly string[] LocalizedRoundTripCultures =
    [
        "af", "ar", "az", "bg", "bn", "ca", "cs", "da",
        "de", "de-CH", "de-LI",
        "el", "es", "fa", "fi", "fil",
        "fr", "fr-BE", "fr-CH",
        "he", "hr", "hu", "hy", "is", "it", "ja", "ko", "ku", "lb", "lt", "lv",
        "mt", "nb", "nl", "pl", "pt", "pt-BR", "ro", "ru",
        "sk", "sl", "sr", "sr-Latn", "sv", "ta", "th", "tr", "uk",
        "uz-Latn-UZ", "uz-Cyrl-UZ", "vi",
        "zh-CN", "zh-Hans", "zh-Hant", "id", "ms"
    ];

    public WordsToNumberConverterRegistry()
        : base(culture => culture.TwoLetterISOLanguageName == "en"
            ? new EnglishWordsToNumberConverter()
            : new DefaultWordsToNumberConverter(culture))
    {
        Register("en", _ => new EnglishWordsToNumberConverter());

        foreach (var cultureName in LocalizedRoundTripCultures)
        {
            Register(cultureName, culture => new LocalizedWordsToNumberConverter(culture));
        }
    }
}
