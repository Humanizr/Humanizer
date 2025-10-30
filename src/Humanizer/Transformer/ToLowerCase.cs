namespace Humanizer;

class ToLowerCase : ICulturedStringTransformer
{
    public string Transform(string input) =>
        input.ToLower(CultureInfo.CurrentCulture);

    public string Transform(string input, CultureInfo? culture) =>
        input.ToLower(culture ?? CultureInfo.CurrentCulture);
}