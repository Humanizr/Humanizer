using System.Globalization;
using Humanizer;

var french = CultureInfo.GetCultureInfo("fr-FR");
CultureInfo.CurrentCulture = french;
CultureInfo.CurrentUICulture = french;

AssertEqual("quarante-deux", 42.ToWords(french));
AssertEqual("BONJOUR", "bonjour".Transform(french, new LoudTransformer()));

Console.WriteLine("quarante-deux; BONJOUR");

static void AssertEqual(string expected, string actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}

sealed class LoudTransformer : ICulturedStringTransformer
{
    public string Transform(string input) =>
        Transform(input, CultureInfo.CurrentCulture);

    public string Transform(string input, CultureInfo culture) =>
        culture.TextInfo.ToUpper(input);
}
