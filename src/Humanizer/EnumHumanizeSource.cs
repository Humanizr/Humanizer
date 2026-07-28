namespace Humanizer;

/// <summary>
/// Specifies the source used to humanize an enum value.
/// </summary>
public enum EnumHumanizeSource
{
    /// <summary>
    /// Uses the default metadata precedence, falling back to the enum member name.
    /// </summary>
    Default,

    /// <summary>
    /// Uses the enum member name.
    /// </summary>
    EnumName,

    /// <summary>
    /// Uses <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.Name"/>,
    /// falling back to the enum member name.
    /// </summary>
    DisplayName,

    /// <summary>
    /// Uses <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.Description"/>,
    /// falling back to the enum member name.
    /// </summary>
    DisplayDescription,

    /// <summary>
    /// Uses <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.ShortName"/>,
    /// then <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.Name"/>,
    /// falling back to the enum member name when neither is available.
    /// </summary>
    DisplayShortName
}