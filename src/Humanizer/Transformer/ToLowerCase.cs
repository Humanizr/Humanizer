namespace Humanizer
{
    class ToLowerCase : ICulturedStringTransformer
    {
        public string Transform(string input) =>
            Transform(input, null);

        public string Transform(string input, CultureInfo? culture)
        {
            culture ??= CultureInfo.CurrentCulture;

            return culture.TextInfo.ToLower(input);
        }
    }
}