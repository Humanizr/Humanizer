namespace Humanizer;

class LuxembourgishFormatter() :
    DefaultFormatter(LocaleCode)
{
    const string LocaleCode = "lb";
    readonly CultureInfo localCulture = new(LocaleCode);
    const string DualPostfix = "_Dual";
    // https://lb.wikipedia.org/wiki/Eifeler_Reegel
    const char EifelerRuleSuffix = 'n';
    const string EifelerRuleCharacters = "unitedzohay";

    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
        base.DataUnitHumanize(dataUnit, count, toSymbol)?.TrimEnd('s');

    public static string ApplyEifelerRule(string word)
        => word.TrimEnd(EifelerRuleSuffix);

    public static string CheckForAndApplyEifelerRule(string word, string nextWord)
        => DoesEifelerRuleApply(nextWord)
            ? word.TrimEnd(EifelerRuleSuffix)
            : word;

    public static bool DoesEifelerRuleApply(string nextWord)
        => !string.IsNullOrWhiteSpace(nextWord)
           && !EifelerRuleCharacters.Contains(nextWord[0]);

    protected override string Format(string resourceKey, int number, bool toWords = false)
    {
        var resourceString = Resources.GetResource(GetResourceKey(resourceKey, number), localCulture);

        var unitGender = GetUnitGender(resourceString);

        var numberAsWord = number.ToWords(unitGender, localCulture);

        if (DoesEifelerRuleApply(numberAsWord))
        {
            if (toWords)
            {
                return string.Format(resourceString, numberAsWord, string.Empty);
            }

            return string.Format(resourceString, number, string.Empty);
        }

        if (toWords)
        {
            return string.Format(resourceString, numberAsWord, EifelerRuleSuffix);
        }

        return string.Format(resourceString, number, EifelerRuleSuffix);
    }

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (number == 2 &&
            resourceKey is "DateHumanize_MultipleDaysAgo" or "DateHumanize_MultipleDaysFromNow")
        {
            return resourceKey + DualPostfix;
        }

        return resourceKey;
    }

    static GrammaticalGender GetUnitGender(string resourceString)
    {
        if (resourceString.EndsWith(" Millisekonnen") ||
            resourceString.EndsWith(" Sekonnen") ||
            resourceString.EndsWith(" Minutten") ||
            resourceString.EndsWith(" Stonnen") ||
            resourceString.EndsWith(" Wochen"))
        {
            return GrammaticalGender.Feminine;
        }

        return GrammaticalGender.Masculine;
    }
}
