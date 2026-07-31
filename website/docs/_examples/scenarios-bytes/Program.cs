using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var parsed = ByteSize.Parse("1.5 KB", culture);
var combined = parsed + 512.Bytes();
var composite = ByteSize.FromBits(81_937);
var decimalSize = ByteSize.FromBytes(1_000_000);
var binarySize = ByteSize.FromMebibytes(1);
var rate = 3.Megabytes()
    .Per(TimeSpan.FromSeconds(2))
    .Humanize("0.0", TimeUnit.Second, culture);
var decimalRate = ByteSize.FromDecimalMegabytes(1)
    .Per(TimeSpan.FromSeconds(1))
    .HumanizeWithUnitSystem(ByteSizeUnitSystem.DecimalSi, culture: culture);
var binaryRate = binarySize
    .Per(TimeSpan.FromSeconds(1))
    .HumanizeWithUnitSystem(ByteSizeUnitSystem.BinaryIec, culture: culture);

Console.WriteLine($"Parsed: {parsed.Humanize("0.0", culture)}");
Console.WriteLine($"Combined: {combined.Humanize("0", culture)}");
Console.WriteLine($"Composite: {composite.HumanizeComposite(precision: 3, formatProvider: culture)}");
Console.WriteLine($"Rate: {rate}");
Console.WriteLine($"Decimal SI: {decimalSize.Format(ByteSizeUnitSystem.DecimalSi, formatProvider: culture)}");
Console.WriteLine($"Binary IEC: {binarySize.Format(ByteSizeUnitSystem.BinaryIec, formatProvider: culture)}");
Console.WriteLine($"Decimal rate: {decimalRate}");
Console.WriteLine($"Binary rate: {binaryRate}");
