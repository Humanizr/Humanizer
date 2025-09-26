namespace Humanizer;

class CatalanFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    protected override string Format(TimeUnit unit, string resourceKey, int number, bool toWords = false)
    {
        var resolvedKey = GetResourceKey(resourceKey, number);
        var resourceString = Resources.GetResource(resolvedKey, Culture);

        var gender = unit switch
        {
            TimeUnit.Hour or TimeUnit.Week => GrammaticalGender.Feminine,
            _ => GrammaticalGender.Masculine
        };

        return string.Format(resourceString, toWords ? number.ToWords(gender, Culture) : number);
    }
}