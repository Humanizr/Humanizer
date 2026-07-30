using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
var estonian = CultureInfo.GetCultureInfo("et");
var simplifiedChinese = CultureInfo.GetCultureInfo("zh-CN");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

Console.WriteLine($"Words: {42.ToWords(culture)}");
Console.WriteLine($"Ordinal words: {21.ToOrdinalWords(culture)}");
Console.WriteLine($"Long ordinal: {2_147_483_651L.Ordinalize(culture)}");
Console.WriteLine($"Indian scale: {1_000_000_000L.ToIndianWords(IndianScaleStyle.CroreBased)}");
Console.WriteLine($"Fraction: {1.25m.Fractionalize(5, 0m)}");
Console.WriteLine($"Financial: {10L.ToChineseFinancialCharacters(simplifiedChinese)}");
Console.WriteLine($"Estonian scale: {1_000_000_000_000_000_000L.ToWords(estonian)}");
Console.WriteLine($"Roman: {14.ToRoman()}");
