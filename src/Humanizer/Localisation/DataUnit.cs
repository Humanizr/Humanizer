namespace Humanizer;

/// <summary>
/// Enumeration of supported units of digital information.
/// Used by <see cref="ByteSize"/> and <see cref="IFormatter.DataUnitHumanize"/> to express byte quantities.
/// </summary>
public enum DataUnit
{
    /// <summary>A single binary digit (0 or 1). There are 8 bits in one byte.</summary>
    Bit,
    /// <summary>The base unit of digital information. One byte consists of 8 bits.</summary>
    Byte,
    /// <summary>1,024 bytes (2^10 bytes).</summary>
    Kilobyte,
    /// <summary>1,048,576 bytes (2^20 bytes).</summary>
    Megabyte,
    /// <summary>1,073,741,824 bytes (2^30 bytes).</summary>
    Gigabyte,
    /// <summary>1,099,511,627,776 bytes (2^40 bytes).</summary>
    Terabyte
}
