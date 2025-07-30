namespace Humanizer;

class LithuanianFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    private static HashSet<string> keysWithoutNumberForms = ["TimeSpanHumanize_Zero", "DateHumanize_Now"];

    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (keysWithoutNumberForms.Contains(resourceKey))
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