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
var unsupportedCulture = "aon point dhà".TryToDecimalNumber(
    out var unsupportedDecimal,
    CultureInfo.GetCultureInfo("gd"),
    out var unsupportedPhrase);
string nullWords = null!;
var nullRejected = nullWords.TryToDecimalNumber(
    out var nullDecimal,
    culture,
    out var nullUnrecognized);

AssertEqual(205, Convert.ToInt64(parsed));
AssertEqual(true, succeeded);
AssertEqual(-42, Convert.ToInt64(negative));
AssertEqual<string?>(null, unrecognized);
AssertEqual(false, rejected);
AssertEqual(0, Convert.ToInt64(invalid));
AssertEqual("otters", invalidWord);
AssertEqual(true, decimalSucceeded);
AssertEqual("1.50", decimalValue.ToString(CultureInfo.InvariantCulture));
AssertEqual<string?>(null, decimalUnrecognized);
AssertEqual(false, decimalRejected);
AssertEqual(0m, invalidDecimal);
AssertEqual("ten", invalidDecimalWord);
AssertEqual(true, frenchSucceeded);
AssertEqual(1.2m, frenchDecimal);
AssertEqual<string?>(null, frenchUnrecognized);
AssertEqual(false, unsupportedCulture);
AssertEqual(0m, unsupportedDecimal);
AssertEqual("aon point dhà", unsupportedPhrase);
AssertEqual(false, nullRejected);
AssertEqual(0m, nullDecimal);
AssertEqual(string.Empty, nullUnrecognized);
AssertThrows<NotSupportedException>(
    () => "aon point dhà".ToDecimalNumber(CultureInfo.GetCultureInfo("gd")));
AssertThrows<ArgumentNullException>(() => nullWords.ToDecimalNumber(culture));

Console.WriteLine("205; -42; 1.50; 1,2; rejected at otters and ten");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}

static void AssertThrows<TException>(Action action)
    where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}
