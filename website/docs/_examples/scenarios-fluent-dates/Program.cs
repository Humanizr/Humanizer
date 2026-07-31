using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var duration = 1.5.Days();
var startingPoint = new DateTime(2025, 1, 20, 9, 0, 0);
var twoMonthsLater = In.Two.MonthsFrom(startingPoint);
var appointment = In.AprilOf(2025).AddDays(2).At(14, 30);

Console.WriteLine($"Duration: {duration.TotalHours} hours");
Console.WriteLine($"Two months later: {twoMonthsLater:yyyy-MM-dd HH:mm}");
Console.WriteLine($"Appointment: {appointment:yyyy-MM-dd HH:mm}");
