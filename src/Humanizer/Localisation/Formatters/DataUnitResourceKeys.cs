namespace Humanizer;

static class DataUnitResourceKeys
{
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
