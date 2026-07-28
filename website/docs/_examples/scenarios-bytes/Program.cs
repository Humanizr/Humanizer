using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var parsed = ByteSize.Parse("1.5 KB", culture);
var combined = parsed + 512.Bytes();
var rate = 3.Megabytes()
    .Per(TimeSpan.FromSeconds(2))
    .Humanize("0.0", TimeUnit.Second, culture);
var composite = ByteSize.FromBits(81_937);

AssertEqual(1536d, parsed.Bytes);
AssertEqual("2 KB", combined.Humanize("0", culture));
AssertEqual("1.5 MB/s", rate);
AssertEqual(true, ByteSize.TryParse("12 b", culture, out var bits));
AssertEqual(12L, bits.Bits);
AssertEqual(
    "10 KB 2 B",
    composite.HumanizeComposite(precision: 2, formatProvider: culture));
AssertEqual(
    "10 KB 2 B 1 b",
    composite.HumanizeComposite(precision: 3, formatProvider: culture));
AssertEqual(
    "10 kilobytes, 2 bytes",
    10_242.Bytes().HumanizeComposite(
        formatProvider: culture,
        separator: ", ",
        toWords: true));
AssertEqual("-10 KB 2 B", (-10_242).Bytes().HumanizeComposite(formatProvider: culture));
AssertEqual("0 b", 0.Bytes().HumanizeComposite(formatProvider: culture));
AssertEqual(
    "1 PB 1 TB 1 GB",
    ByteSize.FromBytes(
            ByteSize.BytesInPetabyte +
            ByteSize.BytesInTerabyte +
            ByteSize.BytesInGigabyte)
        .HumanizeComposite(precision: 3, formatProvider: culture));

Console.WriteLine("1.5 KB; 2 KB; 10 KB 2 B 1 b; 1.5 MB/s");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
