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

Console.WriteLine($"US: {list}");
Console.WriteLine($"UK: {britishList}");
Console.WriteLine($"People: {people}");
Console.WriteLine($"Cleaned: {cleaned}");
Console.WriteLine($"Tuple: {tuple}");

record Person(string Name);
