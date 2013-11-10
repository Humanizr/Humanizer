using System.Globalization;

namespace Humanizer
{
    class ToTitleCase : IStringTransformer
    {
        public string Transform(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }
    }
}