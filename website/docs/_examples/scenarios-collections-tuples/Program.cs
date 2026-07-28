using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var list = new[] { "tea", "coffee", "water" }.Humanize();
var britishList = new[] { "tea", "coffee", "water" }
    .Humanize(CultureInfo.GetCultureInfo("en-GB"));
var people = new[] { new Person("Ada"), new Person("Grace") }
    .Humanize(person => person.Name);
var cleaned = new string?[] { " tea ", null, " ", "coffee" }.Humanize();
var tuple = 4.Tupleize();

AssertEqual("tea, coffee, and water", list);
AssertEqual("tea, coffee and water", britishList);
AssertEqual("Ada and Grace", people);
AssertEqual("tea and coffee", cleaned);
AssertEqual("quadruple", tuple);

Console.WriteLine($"{list}; {britishList}; {tuple}");

static void AssertEqual(string expected, string actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}

record Person(string Name);
