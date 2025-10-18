namespace Humanizer;

class LuxembourgishFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    const string DualPostfix = "_Dual";

    // https://lb.wikipedia.org/wiki/Eifeler_Reegel
    const char EifelerRuleSuffix = 'n';
#if NET8_0_OR_GREATER
    static readonly SearchValues<char> EifelerRuleCharacters = SearchValues.Create("unitedzohay");
#else
    const string EifelerRuleCharacters = "unitedzohay";
#endif

    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
        base
            .DataUnitHumanize(dataUnit, count, toSymbol)
            .TrimEnd('s');

    public static string ApplyEifelerRule(string word)
        => word.TrimEnd(EifelerRuleSuffix);

    public static string CheckForAndApplyEifelerRule(string word, string nextWord)
        => DoesEifelerRuleApply(nextWord.AsSpan())
            ? word.TrimEnd(EifelerRuleSuffix)
            : word;

    public static bool DoesEifelerRuleApply(CharSpan nextWord)
        => !nextWord.IsWhiteSpace()
           && !EifelerRuleCharacters.Contains(nextWord[0]);

    protected override string Format(TimeUnit unit, string resourceKey, int number, bool toWords = false)
    {
        var resourceString = Resources.GetResource(GetResourceKey(resourceKey, number), Culture);
        var numberAsWord = number.ToWords(GetUnitGender(unit), Culture);

        return string.Format(resourceString,
            toWords ? numberAsWord : number,
            DoesEifelerRuleApply(numberAsWord.AsSpan()) ? "" : EifelerRuleSuffix);
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

    static GrammaticalGender GetUnitGender(TimeUnit unit) =>
        unit switch
        {
            TimeUnit.Day or TimeUnit.Month or TimeUnit.Year => GrammaticalGender.Masculine,
            _ => GrammaticalGender.Feminine
        };
}
