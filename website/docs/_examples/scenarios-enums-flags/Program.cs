using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var access = Access.Read | Access.Write;

AssertEqual("Can view and Can edit", access.Humanize());
AssertEqual(Access.Read, "CAN VIEW".DehumanizeTo<Access>());
AssertEqual(null, "missing".DehumanizeTo<Access>(OnNoMatch.ReturnsNull));
AssertEqual("", ((Access)8).Humanize());
AssertEqual("Awaiting reviewer", DeliveryState.NeedsReview.Humanize());
AssertEqual(DeliveryState.NeedsReview, "NeedsReview".DehumanizeTo<DeliveryState>());
AssertEqual(DeliveryState.NeedsReview, "Needs review".DehumanizeTo<DeliveryState>());
AssertEqual(DeliveryState.NeedsReview, "Review required".DehumanizeTo<DeliveryState>());
AssertEqual(DeliveryState.NeedsReview, "Awaiting reviewer".DehumanizeTo<DeliveryState>());
AssertEqual(DeliveryState.NeedsReview, "REVIEW".DehumanizeTo<DeliveryState>());

Console.WriteLine("Can view and Can edit; all delivery-state aliases -> NeedsReview");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}

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
    NeedsReview
}
