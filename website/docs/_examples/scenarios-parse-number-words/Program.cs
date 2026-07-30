using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");

var parsed = "two hundred five".ToNumber(culture);
var succeeded = "negative forty-two".TryToNumber(
    out var negative,
    culture,
    out var unrecognized);
var rejected = "two otters".TryToNumber(
    out var invalid,
    culture,
    out var invalidWord);
var decimalSucceeded = "one point five zero".TryToDecimalNumber(
    out var decimalValue,
    culture,
    out var decimalUnrecognized);
var decimalRejected = "one point ten".TryToDecimalNumber(
    out var invalidDecimal,
    culture,
    out var invalidDecimalWord);
var frenchCulture = CultureInfo.GetCultureInfo("fr-FR");
var frenchSucceeded = "un virgule deux".TryToDecimalNumber(
    out var frenchDecimal,
    frenchCulture,
    out var frenchUnrecognized);

Console.WriteLine($"Integer: {parsed}");
Console.WriteLine($"Negative: {(succeeded ? negative : unrecognized)}");
Console.WriteLine($"Rejected: {(rejected ? invalid : invalidWord)}");
Console.WriteLine(
    $"Decimal: {(decimalSucceeded ? decimalValue.ToString(CultureInfo.InvariantCulture) : decimalUnrecognized)}");
Console.WriteLine(
    $"Invalid decimal: {(decimalRejected ? invalidDecimal : invalidDecimalWord)}");
Console.WriteLine(
    $"French decimal: {(frenchSucceeded ? frenchDecimal.ToString(frenchCulture) : frenchUnrecognized)}");
