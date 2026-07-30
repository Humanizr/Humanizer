using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

Vocabularies.Default.AddAcronym("iOS");

Console.WriteLine($"Label: {"CustomerOrderHistory".Humanize()}");
Console.WriteLine($"Acronym: {"IOSSettings".Humanize()}");
Console.WriteLine($"Ampersand: {"Research&Development".Humanize()}");
Console.WriteLine($"Snake case: {"CustomerOrderHistory".Underscore()}");
Console.WriteLine($"Preserved case: {"HTMLParser".Underscore(preserveCase: true)}");
Console.WriteLine($"Kebab case: {"CustomerOrderHistory".Kebaberize()}");
Console.WriteLine($"Identifier: {"customer order history".Dehumanize()}");
Console.WriteLine($"Truncated: {"Customer order history".Truncate(9)}");
