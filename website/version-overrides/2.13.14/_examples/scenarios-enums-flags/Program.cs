using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var access = Access.Read | Access.Write;

AssertEqual("Can view and Can edit", access.Humanize());
AssertEqual(Access.Read, "CAN VIEW".DehumanizeTo<Access>());
AssertEqual("", ((Access)8).Humanize());

Console.WriteLine("Can view and Can edit; CAN VIEW -> Read");

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
