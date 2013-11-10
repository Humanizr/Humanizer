using System.Globalization;

namespace Humanizer
{
    class ToLowerCase : IStringTransformer
    {
        public string Transform(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToLower(input);
        }
    }
}