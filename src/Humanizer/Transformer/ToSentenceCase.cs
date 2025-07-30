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
            if (char.IsUpper(input[0]))
            {
                return input;
            }

            return StringHumanizeExtensions.Concat(culture.TextInfo.ToUpper(input[0]), input.AsSpan(1));
        }

        return culture.TextInfo.ToUpper(input);
    }
}