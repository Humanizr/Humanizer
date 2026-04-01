namespace Humanizer;

/// <summary>
/// Builds resource keys for localized data-unit names and symbols.
/// </summary>
static class DataUnitResourceKeys
{
    /// <summary>
    /// Returns the resource key for a data unit name or symbol.
    /// </summary>
    /// <param name="dataUnit">The data unit whose resource key should be built.</param>
    /// <param name="toSymbol">Whether to return the symbol key instead of the full-word key.</param>
    /// <returns>The resource key for the requested data unit representation.</returns>
    /// <remarks>
    /// Unknown enum values fall back to the naming convention so future units can still resolve
    /// to resources without requiring code changes for every new member.
    /// </remarks>
    public static string GetResourceKey(DataUnit dataUnit, bool toSymbol) =>
        (toSymbol, dataUnit) switch
        {
            (true, DataUnit.Bit) => "DataUnit_BitSymbol",
            (true, DataUnit.Byte) => "DataUnit_ByteSymbol",
            (true, DataUnit.Kilobyte) => "DataUnit_KilobyteSymbol",
            (true, DataUnit.Megabyte) => "DataUnit_MegabyteSymbol",
            (true, DataUnit.Gigabyte) => "DataUnit_GigabyteSymbol",
            (true, DataUnit.Terabyte) => "DataUnit_TerabyteSymbol",
            (true, _) => $"DataUnit_{dataUnit}Symbol",

            (false, DataUnit.Bit) => "DataUnit_Bit",
            (false, DataUnit.Byte) => "DataUnit_Byte",
            (false, DataUnit.Kilobyte) => "DataUnit_Kilobyte",
            (false, DataUnit.Megabyte) => "DataUnit_Megabyte",
            (false, DataUnit.Gigabyte) => "DataUnit_Gigabyte",
            (false, DataUnit.Terabyte) => "DataUnit_Terabyte",
            (false, _) => $"DataUnit_{dataUnit}",
        };
}
