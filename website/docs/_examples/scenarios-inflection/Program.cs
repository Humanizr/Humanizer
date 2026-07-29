using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

AssertEqual("people", "person".Pluralize());
AssertEqual("person", "people".Singularize());
AssertEqual("two people", "person".ToQuantity(2, ShowQuantityAs.Words));

Vocabularies.Default.AddIrregular("cactoid", "cactoidae", matchEnding: false);
AssertEqual("cactoidae", "cactoid".Pluralize());
AssertEqual("cactoid", "cactoidae".Singularize());

var polishForms = new CardinalInflectionForms(
    lemma: "plik",
    other: "pliku",
    one: "plik",
    few: "pliki",
    many: "plików");
AssertTrue(polishForms.TryInflect(5m, CultureInfo.GetCultureInfo("pl"), out var polish));
AssertEqual("plików", polish);

AssertTrue("persona".TryInflect(2m, CultureInfo.GetCultureInfo("es"), out var spanish));
AssertEqual("personas", spanish);
AssertTrue("personas".TryLemmatize(CultureInfo.GetCultureInfo("es"), out var lemma));
AssertEqual("persona", lemma);

Console.WriteLine("people; two people; cactoidae; plików; personas");

static void AssertEqual(string expected, string? actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}

static void AssertTrue(bool value)
{
    if (!value)
        throw new InvalidOperationException("Expected success.");
}
