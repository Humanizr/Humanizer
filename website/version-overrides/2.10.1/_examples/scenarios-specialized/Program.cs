using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

AssertEqual("southwest", 225d.ToHeading(HeadingStyle.Full, culture));
AssertEqual('↙', 225d.ToHeadingArrow());
AssertEqual("XIV", 14.ToRoman());
AssertEqual(2_000_000, 2.Millions());
AssertEqual("quadruple", 4.Tupleize());

var titles = new[] { "The Theater", "An Apple", "Bear" };
var sorted = EnglishArticle.PrependArticleSuffix(
    EnglishArticle.AppendArticlePrefix(titles));
AssertEqual("An Apple, Bear, The Theater", string.Join(", ", sorted));

Console.WriteLine("southwest; ↙; XIV; 2,000,000; quadruple");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
