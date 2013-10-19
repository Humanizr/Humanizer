using System.Globalization;

namespace Humanizer.Transformer
{
    public class ToLowerCase : IStringTransformer
    {
        public string Transform(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToLower(input);
        }
    }
}