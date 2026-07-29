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
var decimalSize = ByteSize.FromBytes(1_000_000);
var binarySize = ByteSize.FromMebibytes(1);
var decimalRate = ByteSize.FromDecimalMegabytes(1)
    .Per(TimeSpan.FromSeconds(1))
    .HumanizeWithUnitSystem(ByteSizeUnitSystem.DecimalSi, culture: culture);
var binaryRate = binarySize
    .Per(TimeSpan.FromSeconds(1))
    .HumanizeWithUnitSystem(ByteSizeUnitSystem.BinaryIec, culture: culture);

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
AssertEqual("1 MB", decimalSize.Format(ByteSizeUnitSystem.DecimalSi, formatProvider: culture));
AssertEqual("976.56 KiB", decimalSize.Format(ByteSizeUnitSystem.BinaryIec, formatProvider: culture));
AssertEqual(
    "1 MB 1 kB 1 B",
    ByteSize.FromBytes(1_001_001)
        .HumanizeCompositeWithUnitSystem(
            ByteSizeUnitSystem.DecimalSi,
            precision: 3,
            formatProvider: culture));
AssertEqual(
    "1 MiB 1 KiB 1 B",
    ByteSize.FromBytes(
            ByteSize.BytesInMebibyte +
            ByteSize.BytesInKibibyte +
            1)
        .HumanizeCompositeWithUnitSystem(
            ByteSizeUnitSystem.BinaryIec,
            precision: 3,
            formatProvider: culture));
AssertEqual(decimalSize, ByteSize.ParseWithUnitSystem("1 MB", ByteSizeUnitSystem.DecimalSi, culture));
AssertEqual(binarySize, ByteSize.ParseWithUnitSystem("1 MiB", ByteSizeUnitSystem.BinaryIec, culture));
AssertEqual(false, ByteSize.TryParseWithUnitSystem("1 MB", ByteSizeUnitSystem.BinaryIec, culture, out _));
AssertEqual("1 MB/s", decimalRate);
AssertEqual("1 MiB/s", binaryRate);

Console.WriteLine("1.5 KB; 2 KB; 10 KB 2 B 1 b; 1.5 MB/s; 1 MB; 1 MB/s; 1 MiB/s");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
