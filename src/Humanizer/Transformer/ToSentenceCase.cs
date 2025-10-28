namespace Humanizer;

class ToSentenceCase : ICulturedStringTransformer
{
    public string Transform(string input) =>
        Transform(input, null);

    public string Transform(string input, CultureInfo? culture)
    {
        culture ??= CultureInfo.CurrentCulture;

        if (input.Length >= 1)
        {
            var firstChar = input[0];
            if (char.IsUpper(firstChar))
            {
                return input;
            }

            var upperChar = char.ToUpper(firstChar, culture);
            return StringHumanizeExtensions.Concat(upperChar, input.AsSpan(1));
        }

        return culture.TextInfo.ToUpper(input);
    }
}