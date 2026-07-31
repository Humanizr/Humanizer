using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var access = Access.Read | Access.Write;

Console.WriteLine($"Flags: {access.Humanize()}");
Console.WriteLine(
    $"Enum names: {access.Humanize(LetterCasing.Title, EnumHumanizeSource.EnumName)}");
Console.WriteLine($"Description: {DeliveryState.NeedsReview.Humanize()}");
Console.WriteLine(
    $"Display name: {DeliveryState.NeedsReview.Humanize(LetterCasing.Sentence, EnumHumanizeSource.DisplayName)}");
Console.WriteLine($"Parsed alias: {"REVIEW".DehumanizeTo<DeliveryState>()}");
Console.WriteLine(
    $"Missing: {"missing".DehumanizeTo<Access>(OnNoMatch.ReturnsNull)?.ToString() ?? "(none)"}");

[Flags]
enum Access
{
    [Display(Description = "None")]
    None = 0,
    [Display(Description = "Can view")]
    Read = 1,
    [Display(Description = "Can edit")]
    Write = 2
}

enum DeliveryState
{
    [Display(
        Name = "Review required",
        Description = "Awaiting reviewer",
        ShortName = "Review")]
    NeedsReview,
    [Display(Name = "Waiting in queue")]
    QueuedItem
}
