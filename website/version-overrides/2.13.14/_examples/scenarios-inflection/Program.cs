using System.Globalization;
using Humanizer;
using Humanizer.Inflections;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

AssertEqual("people", "person".Pluralize());
AssertEqual("person", "people".Singularize());
AssertEqual("two people", "person".ToQuantity(2, ShowQuantityAs.Words));
AssertEqual("minus one people", "person".ToQuantity(-1, ShowQuantityAs.Words));

Vocabularies.Default.AddIrregular("cactoid", "cactoidae", matchEnding: false);
AssertEqual("cactoidae", "cactoid".Pluralize());
AssertEqual("cactoid", "cactoidae".Singularize());

Console.WriteLine("people; two people; cactoidae");

static void AssertEqual(string expected, string? actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
