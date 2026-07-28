using System.Globalization;
using Humanizer;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

Vocabularies.Default.AddAcronym("iOS");

AssertEqual("Customer order history", "CustomerOrderHistory".Humanize());
AssertEqual("Customer Order History", "CustomerOrderHistory".Humanize(LetterCasing.Title));
AssertEqual("iOS settings", "IOSSettings".Humanize());
AssertEqual("Research & development", "Research&Development".Humanize());
AssertEqual("CustomerOrderHistory", "customer order history".Dehumanize());
AssertEqual("customer_order_history", "CustomerOrderHistory".Underscore());
AssertEqual("HTML_Parser", "HTMLParser".Underscore(preserveCase: true));
AssertEqual("customer-order-history", "CustomerOrderHistory".Kebaberize());
AssertEqual("CustomerOrderHistory", "customer_order_history".Pascalize());
AssertEqual("Customer…", "Customer order history".Truncate(9));
AssertEqual(
    "Text with more words…",
    "Text with more words than the limit".Truncate(4, Truncator.FixedNumberOfWords));
AssertEqual("Order History", "order history".Transform(To.TitleCase));

Console.WriteLine("Customer order history; iOS settings; Research & development");

static void AssertEqual(string expected, string? actual)
{
    if (actual != expected)
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
