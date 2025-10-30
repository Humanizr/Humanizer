namespace Humanizer;

class ToLowerCase : ICulturedStringTransformer
{
    public string Transform(string input) =>
        input.ToLower();

    public string Transform(string input, CultureInfo? culture)
    {
        if (culture is null)
        {
            return input.ToLower();
        }

        return culture.TextInfo.ToLower(input);
    }
}