using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
var estonian = CultureInfo.GetCultureInfo("et");
var albanian = CultureInfo.GetCultureInfo("sq");
var simplifiedChinese = CultureInfo.GetCultureInfo("zh-CN");
var traditionalChinese = CultureInfo.GetCultureInfo("zh-TW");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

AssertEqual("forty-two", 42.ToWords(culture));
AssertEqual("twenty-first", 21.ToOrdinalWords(culture));
AssertEqual("21st", 21.Ordinalize(culture));
AssertEqual("2147483651st", 2_147_483_651L.Ordinalize(culture));
AssertEqual("-9223372036854775808th", long.MinValue.Ordinalize(culture));
AssertEqual("biljard", 1_000_000_000_000_000L.ToWords(estonian));
AssertEqual("dy biliarë", 2_000_000_000_000_000L.ToWords(albanian));
AssertEqual("triljon", 1_000_000_000_000_000_000L.ToWords(estonian));
AssertEqual("one arab", 1_000_000_000L.ToIndianWords());
AssertEqual(
    "one hundred crore",
    1_000_000_000L.ToIndianWords(IndianScaleStyle.CroreBased));
AssertEqual("1 1/4", 1.25m.Fractionalize(5, 0m));
AssertEqual("1/3", 0.34m.Fractionalize(5, 0.01m));
AssertEqual("¾", 0.75m.Fractionalize(4, 0m, useUnicode: true));
AssertEqual("0.11", 0.11m.Fractionalize(10, 0m));
AssertEqual("壹拾", 10L.ToChineseFinancialCharacters(simplifiedChinese));
AssertEqual("壹拾", 10L.ToChineseFinancialCharacters(traditionalChinese));
AssertEqual(
    "负玖佰贰拾贰京叁仟叁佰柒拾贰兆零叁佰陆拾捌亿伍仟肆佰柒拾柒万伍仟捌佰零捌",
    long.MinValue.ToChineseFinancialCharacters(simplifiedChinese));
AssertThrows<NotSupportedException>(
    () => 10L.ToChineseFinancialCharacters(CultureInfo.GetCultureInfo("zh")));
AssertEqual("XIV", 14.ToRoman());
AssertEqual("1.5 KB", 1536.Bytes().Humanize("0.0", culture));
AssertEqual("two people", "person".ToQuantity(2, ShowQuantityAs.Words));

Console.WriteLine("forty-two; 2147483651st; 1 1/4; 壹拾; XIV; 1.5 KB");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(actual, expected))
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
