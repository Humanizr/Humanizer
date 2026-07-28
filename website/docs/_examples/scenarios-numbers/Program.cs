using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

AssertEqual("forty-two", 42.ToWords(culture));
AssertEqual("twenty-first", 21.ToOrdinalWords(culture));
AssertEqual("21st", 21.Ordinalize(culture));
AssertEqual("XIV", 14.ToRoman());
AssertEqual("1.5 KB", 1536.Bytes().Humanize("0.0", culture));
AssertEqual("two people", "person".ToQuantity(2, ShowQuantityAs.Words));

Console.WriteLine("forty-two; twenty-first; XIV; 1.5 KB; two people");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(actual, expected))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
