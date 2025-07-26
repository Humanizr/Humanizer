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
            // If the input contains word separators and all letters are uppercase, transform it to sentence case
            // This handles the issue #1557 case: "HYPEN-SEPARATOR" -> "Hypen-separator"
            if ((input.Contains('_') || input.Contains('-')) && 
                input.Where(char.IsLetter).Any() && 
                input.Where(char.IsLetter).All(char.IsUpper))
            {
                return StringHumanizeExtensions.Concat(
                    culture.TextInfo.ToUpper(input[0]), 
                    culture.TextInfo.ToLower(input[1..]).AsSpan());
            }
            
            // If first character is already uppercase, leave as-is (handles normal case and short acronyms)
            if (char.IsUpper(input[0]))
            {
                return input;
            }

            return StringHumanizeExtensions.Concat(culture.TextInfo.ToUpper(input[0]), input.AsSpan(1));
        }

        return culture.TextInfo.ToUpper(input);
    }
}