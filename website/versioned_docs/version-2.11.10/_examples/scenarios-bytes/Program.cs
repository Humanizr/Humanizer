using System.Globalization;
using Humanizer;
using Humanizer.Bytes;
using Humanizer.Localisation;

var culture = CultureInfo.GetCultureInfo("en-US");
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var parsed = ByteSize.Parse("1.5 KB");
var combined = parsed + 512.Bytes();
var rate = 3.Megabytes()
    .Per(TimeSpan.FromSeconds(2))
    .Humanize("0.0", TimeUnit.Second);

AssertEqual(1536d, parsed.Bytes);
AssertEqual("2 KB", combined.Humanize("0", culture));
AssertEqual("1.5 MB/s", rate);
AssertEqual(true, ByteSize.TryParse("12 b", out var bits));
AssertEqual(12L, bits.Bits);

Console.WriteLine("1.5 KB; 2 KB; 1.5 MB/s");

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new InvalidOperationException($"Expected '{expected}', got '{actual}'.");
}
