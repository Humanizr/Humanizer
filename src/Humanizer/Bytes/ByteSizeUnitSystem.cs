namespace Humanizer;

/// <summary>
/// Selects the unit system used by explicit <see cref="ByteSize"/> parsing and formatting APIs.
/// </summary>
/// <remarks>
/// For <see cref="DecimalSi"/> and <see cref="BinaryIec"/>, explicit parsers and format-token selectors
/// match unit symbols without regard to letter casing, while formatting emits canonical symbol casing.
/// <see cref="Legacy"/> preserves the established parsing and formatting behavior.
/// </remarks>
public enum ByteSizeUnitSystem
{
    /// <summary>
    /// Uses Humanizer's established mixed units: binary KB through TB and decimal PB and EB.
    /// </summary>
    Legacy = 0,

    /// <summary>
    /// Uses decimal SI units where each successive unit is 1000 times the previous unit.
    /// </summary>
    DecimalSi = 1,

    /// <summary>
    /// Uses binary IEC units where each successive unit is 1024 times the previous unit.
    /// </summary>
    BinaryIec = 2
}