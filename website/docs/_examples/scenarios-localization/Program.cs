using System.Globalization;
using Humanizer;

var french = CultureInfo.GetCultureInfo("fr-FR");
CultureInfo.CurrentCulture = french;
CultureInfo.CurrentUICulture = french;

Console.WriteLine($"Number: {42.ToWords(french)}");
Console.WriteLine($"Transformed: {"bonjour".Transform(french, new LoudTransformer())}");

sealed class LoudTransformer : ICulturedStringTransformer
{
    public string Transform(string input) =>
        Transform(input, CultureInfo.CurrentCulture);

    public string Transform(string input, CultureInfo culture) =>
        culture.TextInfo.ToUpper(input);
}
