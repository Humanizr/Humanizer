using System.Globalization;

namespace Humanizer;

sealed class HeadingTable(string[] full, string[] shortForms)
{
    readonly string[] full = full;
    readonly string[] shortForms = shortForms;

    public string GetHeading(HeadingStyle style, int index) =>
        style == HeadingStyle.Abbreviated
            ? shortForms[index]
            : full[index];

    public bool TryParseAbbreviated(string value, CultureInfo culture, out double heading)
    {
        for (var index = 0; index < shortForms.Length; index++)
        {
            if (culture.CompareInfo.Compare(value, shortForms[index], CompareOptions.IgnoreCase) == 0)
            {
                heading = index * 22.5;
                return true;
            }
        }

        heading = -1;
        return false;
    }
}

static partial class HeadingTableCatalog
{
    public static HeadingTable Resolve(CultureInfo culture)
    {
        for (var current = culture; current != CultureInfo.InvariantCulture; current = current.Parent)
        {
            if (ResolveCore(current.Name) is { } table)
            {
                return table;
            }

            if (string.IsNullOrEmpty(current.Name))
            {
                break;
            }
        }

        return ResolveCore("en") ?? Invariant;
    }

    internal static partial HeadingTable? ResolveCore(string localeCode);

    static HeadingTable Invariant => invariantCache.Value;

    static class invariantCache
    {
        internal static readonly HeadingTable Value = new(
            ["N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"],
            ["N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"]);
    }
}
