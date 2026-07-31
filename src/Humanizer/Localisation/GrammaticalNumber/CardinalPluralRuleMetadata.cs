namespace Humanizer;

static class CardinalPluralRuleMetadata
{
    public static string[] GetReachableCategories(string cardinalRule) =>
        cardinalRule switch
        {
            "Other" => ["other"],
            "AmharicLike" or "Armenian" or "EnglishLike" or "Sinhala" or
                "Punjabi" or "One" or "Danish" or "Icelandic" or
                "Macedonian" or "Filipino" => ["one", "other"],
            "Latvian" => ["zero", "one", "other"],
            "Hebrew" => ["one", "two", "other"],
            "Romanian" or "SouthSlavic" => ["one", "few", "other"],
            "French" or "Portuguese" or "CatalanItalian" or "Spanish" =>
                ["one", "many", "other"],
            "Slovenian" => ["one", "two", "few", "other"],
            "CzechSlovak" or "Polish" or "Belarusian" or "Lithuanian" or
                "RussianUkrainian" => ["one", "few", "many", "other"],
            "Maltese" or "Irish" => ["one", "two", "few", "many", "other"],
            "Arabic" or "Welsh" => ["zero", "one", "two", "few", "many", "other"],
            _ => throw new ArgumentOutOfRangeException(
                nameof(cardinalRule),
                cardinalRule,
                "Unknown cardinal plural rule.")
        };
}