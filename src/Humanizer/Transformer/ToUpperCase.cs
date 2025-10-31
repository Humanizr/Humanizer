namespace Humanizer;

class ToUpperCase : ICulturedStringTransformer
{
    public string Transform(string input) =>
        input.ToUpper(CultureInfo.CurrentCulture);

    public string Transform(string input, CultureInfo? culture) =>
        input.ToUpper(culture ?? CultureInfo.CurrentCulture);
}