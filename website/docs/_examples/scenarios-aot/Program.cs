using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var label = JobState.ReadyToShip.Humanize();
var parsed = "READY TO SHIP".DehumanizeTo<JobState>();

Console.WriteLine($"Label: {label}");
Console.WriteLine($"Parsed: {parsed}");

enum JobState
{
    ReadyToShip
}
