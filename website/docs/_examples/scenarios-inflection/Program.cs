using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

Vocabularies.Default.AddIrregular("cactoid", "cactoidae", matchEnding: false);

Console.WriteLine($"Plural: {"person".Pluralize()}");
Console.WriteLine($"Quantity: {"person".ToQuantity(2, ShowQuantityAs.Words)}");
Console.WriteLine($"Custom plural: {"cactoid".Pluralize()}");
