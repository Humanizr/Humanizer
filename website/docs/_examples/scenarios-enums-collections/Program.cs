using System.ComponentModel;
using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
string[] drinks = ["tea", "coffee", "water"];

var people = new[] { new Person("Ada"), new Person("Grace") };

Console.WriteLine($"State: {OrderState.AwaitingPayment.Humanize()}");
Console.WriteLine($"Parsed: {"WAITING FOR PAYMENT".DehumanizeTo<OrderState>()}");
Console.WriteLine($"Drinks: {drinks.Humanize()}");
Console.WriteLine($"People: {people.Humanize(person => person.DisplayName)}");

enum OrderState
{
    [Description("Waiting for payment")]
    AwaitingPayment,
    ReadyToShip
}

record Person(string DisplayName);
