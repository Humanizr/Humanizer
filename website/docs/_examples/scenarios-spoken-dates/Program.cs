using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var date = new DateTime(2022, 1, 25).ToOrdinalWords();
var dateInGenitive = new DateOnly(2022, 1, 25).ToOrdinalWords(GrammaticalCase.Genitive);
var time = new TimeOnly(13, 23)
    .ToClockNotation(ClockNotationRounding.NearestFiveMinutes, culture);

Console.WriteLine($"Date: {date}");
Console.WriteLine($"Genitive date: {dateInGenitive}");
Console.WriteLine($"Time: {time}");
