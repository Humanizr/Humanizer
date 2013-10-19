using System.Globalization;

namespace Humanizer.Transformer
{
    public class ToTitleCase : IStringTransformer
    {
        public string Transform(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }
    }
}