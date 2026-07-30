using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var titles = new[] { "The Theater", "An Apple", "Bear" };
var sorted = EnglishArticle.PrependArticleSuffix(
    EnglishArticle.AppendArticlePrefix(titles));

Console.WriteLine($"Heading: {225d.ToHeading(HeadingStyle.Full, culture)} {225d.ToHeadingArrow()}");
Console.WriteLine($"Roman: {14.ToRoman()}");
Console.WriteLine($"Multiplier: {2.Millions():N0}");
Console.WriteLine($"Unit: {TimeUnit.Minute.ToSymbol(culture)}");
Console.WriteLine($"Tuple: {4.Tupleize()}");
Console.WriteLine($"Titles: {string.Join(", ", sorted)}");
