namespace Humanizer;

class ToLowerCase : ICulturedStringTransformer
{
    public string Transform(string input) =>
        Transform(input, CultureInfo.CurrentCulture);

    public string Transform(string input, CultureInfo culture)
    {
        return culture.TextInfo.ToLower(input);
    }
}