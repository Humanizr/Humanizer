namespace Humanizer.Localisation.Formatters;

internal class LuxembourgishFormatter() :
    DefaultFormatter(LocaleCode)
{
    private const string LocaleCode = "lb";
    private readonly CultureInfo _localCulture = new(LocaleCode);
    private const string DualPostfix = "_Dual";
    // https://lb.wikipedia.org/wiki/Eifeler_Reegel
    private const char EifelerRuleSuffix = 'n';
    private const string EifelerRuleCharacters = "unitedzohay";

    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        return base.DataUnitHumanize(dataUnit, count, toSymbol)?.TrimEnd('s');
    }

    public static string ApplyEifelerRule(string word)
        => word.TrimEnd(EifelerRuleSuffix);

    public static string CheckForAndApplyEifelerRule(string word, string nextWord)
        => DoesEifelerRuleApply(nextWord)
            ? word.TrimEnd(EifelerRuleSuffix)
            : word;

    public static bool DoesEifelerRuleApply(string nextWord)
        => !string.IsNullOrWhiteSpace(nextWord)
           && !EifelerRuleCharacters.Contains(nextWord.Substring(0, 1));

    protected override string Format(string resourceKey, int number, bool toWords = false)
    {
        var resourceString = Resources.GetResource(GetResourceKey(resourceKey, number), _localCulture);

        if (string.IsNullOrEmpty(resourceString))
        {
            throw new ArgumentException($@"The resource object with key '{resourceKey}' was not found", nameof(resourceKey));
        }

        var unitGender = GetUnitGender(resourceString);

        var numberAsWord = number.ToWords(unitGender, _localCulture);
        var doesEifelerRuleApply = DoesEifelerRuleApply(numberAsWord);

        return toWords
            ? resourceString.FormatWith(numberAsWord, doesEifelerRuleApply ? string.Empty : EifelerRuleSuffix)
            : resourceString.FormatWith(number, doesEifelerRuleApply ? string.Empty : EifelerRuleSuffix);
    }

    protected override string GetResourceKey(string resourceKey, int number)
    {
        return number switch
        {
            2 when resourceKey is "DateHumanize_MultipleDaysAgo" or "DateHumanize_MultipleDaysFromNow" => resourceKey + DualPostfix,
            _ => resourceKey
        };
    }

    private static GrammaticalGender GetUnitGender(string resourceString)
    {
        var words = resourceString.Split(' ');
        return words.Last() switch
        {
            var x when 
                x.StartsWith("Millisekonnen")
                || x.StartsWith("Sekonnen")
                || x.StartsWith("Minutten")
                || x.StartsWith("Stonnen")
                || x.StartsWith("Wochen") => GrammaticalGender.Feminine,
            _ => GrammaticalGender.Masculine
        };
    }
}
