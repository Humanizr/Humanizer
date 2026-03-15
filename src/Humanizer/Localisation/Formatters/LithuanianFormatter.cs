namespace Humanizer;

class LithuanianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    private static readonly HashSet<string> KeysWithoutNumberForms = ["TimeSpanHumanize_Zero", "DateHumanize_Now"];

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (KeysWithoutNumberForms.Contains(resourceKey))
        {
            return resourceKey;
        }

        var grammaticalNumber = LithuanianNumberFormDetector.Detect(number);
        var suffix = GetSuffix(grammaticalNumber);
        return resourceKey + suffix;
    }

    static string GetSuffix(LithuanianNumberForm form)
    {
        if (form == LithuanianNumberForm.Singular)
        {
            return "_Singular";
        }

        if (form == LithuanianNumberForm.GenitivePlural)
        {
            return "_Plural";
        }

        return "";
    }
}