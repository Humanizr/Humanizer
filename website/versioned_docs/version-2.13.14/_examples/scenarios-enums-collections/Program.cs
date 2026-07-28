using System.ComponentModel;
using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
string[] drinks = ["tea", "coffee", "water"];

AssertEqual("Waiting for payment", OrderState.AwaitingPayment.Humanize());
AssertEqual("Ready to ship", OrderState.ReadyToShip.Humanize());
AssertEqual(OrderState.AwaitingPayment, "WAITING FOR PAYMENT".DehumanizeTo<OrderState>());
AssertEqual("tea, coffee, and water", drinks.Humanize());
AssertEqual(
    "Ada and Grace",
    new[] { new Person("Ada"), new Person("Grace") }
        .Humanize(person => person.DisplayName));

Console.WriteLine("Waiting for payment; tea, coffee, and water");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(actual, expected))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}

enum OrderState
{
    [Description("Waiting for payment")]
    AwaitingPayment,
    ReadyToShip
}

record Person(string DisplayName);
