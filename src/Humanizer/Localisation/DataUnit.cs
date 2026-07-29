namespace Humanizer;

/// <summary>
/// Represents the data units supported by Humanizer's data-size formatting.
/// </summary>
public enum DataUnit
{
    /// <summary>
    /// A bit.
    /// </summary>
    Bit,

    /// <summary>
    /// A byte.
    /// </summary>
    Byte,

    /// <summary>
    /// A kilobyte.
    /// </summary>
    Kilobyte,

    /// <summary>
    /// A megabyte.
    /// </summary>
    Megabyte,

    /// <summary>
    /// A gigabyte.
    /// </summary>
    Gigabyte,

    /// <summary>
    /// A terabyte.
    /// </summary>
    Terabyte,

    /// <summary>
    /// A petabyte.
    /// </summary>
    Petabyte,

    /// <summary>
    /// An exabyte.
    /// </summary>
    Exabyte,

    /// <summary>
    /// A pebibyte.
    /// </summary>
    Pebibyte,

    /// <summary>
    /// A kibibyte.
    /// </summary>
    Kibibyte,

    /// <summary>
    /// A mebibyte.
    /// </summary>
    Mebibyte,

    /// <summary>
    /// A gibibyte.
    /// </summary>
    Gibibyte,

    /// <summary>
    /// A tebibyte.
    /// </summary>
    Tebibyte,

    /// <summary>A decimal SI kilobyte.</summary>
    DecimalKilobyte = 13,

    /// <summary>A decimal SI megabyte.</summary>
    DecimalMegabyte = 14,

    /// <summary>A decimal SI gigabyte.</summary>
    DecimalGigabyte = 15,

    /// <summary>A decimal SI terabyte.</summary>
    DecimalTerabyte = 16,

    /// <summary>A decimal SI petabyte.</summary>
    DecimalPetabyte = 17,

    /// <summary>A decimal SI exabyte.</summary>
    DecimalExabyte = 18,

    /// <summary>A binary IEC kibibyte used by explicit unit-system formatting.</summary>
    BinaryKibibyte = 19,

    /// <summary>A binary IEC mebibyte used by explicit unit-system formatting.</summary>
    BinaryMebibyte = 20,

    /// <summary>A binary IEC gibibyte used by explicit unit-system formatting.</summary>
    BinaryGibibyte = 21,

    /// <summary>A binary IEC tebibyte used by explicit unit-system formatting.</summary>
    BinaryTebibyte = 22,

    /// <summary>A binary IEC pebibyte used by explicit unit-system formatting.</summary>
    BinaryPebibyte = 23
}