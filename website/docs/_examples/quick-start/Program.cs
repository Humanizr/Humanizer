using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var result = TimeSpan.FromMinutes(2).Humanize();
if (result != "2 minutes")
    throw new InvalidOperationException($"Unexpected result: {result}");

Console.WriteLine(result);
