namespace Humanizer;

class ToSentenceCase : ICulturedStringTransformer
{
    public string Transform(string input) =>
        Transform(input, CultureInfo.CurrentCulture);

    public string Transform(string input, CultureInfo culture)
    {

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
            
            // For multi-word strings (result of humanization), handle each word
            if (input.Contains(' '))
            {
                var words = input.Split(' ');
                var result = new StringBuilder();
                
                // Check if this looks like it came from separator-based input (all words are ALL-CAPS)
                bool allWordsAreCaps = words.Where(w => w.Any(char.IsLetter)).All(w => w.All(char.IsUpper));
                
                for (int i = 0; i < words.Length; i++)
                {
                    var word = words[i];
                    if (i > 0) result.Append(' ');
                    
                    if (i == 0)
                    {
                        // First word: capitalize first letter, lowercase the rest (unless it's an acronym)
                        if (word.Length > 0)
                        {
                            // For the first word in sentence case
                            if (word.All(char.IsUpper) && word.Any(char.IsLetter) && !allWordsAreCaps)
                            {
                                // Preserve ALL-CAPS words as likely acronyms only if not all words are caps
                                result.Append(word);
                            }
                            else
                            {
                                result.Append(culture.TextInfo.ToUpper(word[0]));
                                if (word.Length > 1)
                                {
                                    result.Append(culture.TextInfo.ToLower(word[1..]));
                                }
                            }
                        }
                    }
                    else
                    {
                        // Subsequent words: lowercase (unless it's an acronym)
                        if (word.All(char.IsUpper) && word.Any(char.IsLetter) && !allWordsAreCaps)
                        {
                            // Preserve ALL-CAPS words as likely acronyms only if not all words are caps
                            result.Append(word);
                        }
                        else
                        {
                            result.Append(culture.TextInfo.ToLower(word));
                        }
                    }
                }
                
                return result.ToString();
            }
            
            // Single word case
            // If first character is already uppercase, leave as-is (handles normal case and acronyms)
            if (char.IsUpper(input[0]))
            {
                return input;
            }

            return StringHumanizeExtensions.Concat(culture.TextInfo.ToUpper(input[0]), input.AsSpan(1));
        }

        return culture.TextInfo.ToUpper(input);
    }
}