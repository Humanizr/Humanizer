using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var label = JobState.ReadyToShip.Humanize();
var parsed = "READY TO SHIP".DehumanizeTo<JobState>();

if (label != "Ready to ship" || parsed != JobState.ReadyToShip)
    throw new InvalidOperationException($"Unexpected enum result: {label}; {parsed}");

Console.WriteLine("Ready to ship");

enum JobState
{
    ReadyToShip
}
