using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var duration = 1.5.Days();
var startingPoint = new DateTime(2025, 1, 20, 9, 0, 0);
var twoMonthsLater = In.Two.MonthsFrom(startingPoint);
var appointment = In.AprilOf(2025).AddDays(2).At(14, 30);

AssertEqual(TimeSpan.FromHours(36), duration);
AssertEqual(new DateTime(2025, 3, 20, 9, 0, 0), twoMonthsLater);
AssertEqual(new DateTime(2025, 4, 3, 14, 30, 0), appointment);

Console.WriteLine("36 hours; 2025-03-20; 2025-04-03 14:30");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
