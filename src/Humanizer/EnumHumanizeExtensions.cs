namespace Humanizer;

/// <summary>
/// Contains extension methods for humanizing Enums
/// </summary>
public static class EnumHumanizeExtensions
{
    /// <summary>
    /// Converts an enum value to a human-readable string by intelligently formatting the enum member name
    /// and respecting any <see cref="System.ComponentModel.DescriptionAttribute"/> applied to the member.
    /// </summary>
    /// <typeparam name="T">The enum type. Must be a struct and implement <see cref="Enum"/>.</typeparam>
    /// <param name="input">The enum value to be humanized.</param>
    /// <returns>
    /// A human-readable string representation of the enum value.
    /// If the enum has the <see cref="FlagsAttribute"/> and multiple flags are set, returns a humanized,
    /// comma-separated list of the flag values.
    /// If a <see cref="System.ComponentModel.DescriptionAttribute"/> is present on the enum member, its value is returned.
    /// Otherwise, the enum member name is humanized (e.g., "AnonymousUser" becomes "Anonymous user").
    /// </returns>
    /// <remarks>
    /// For flags enums, only non-zero flags are included in the output, and each flag is humanized individually.
    /// The humanization process converts PascalCase to space-separated text with appropriate capitalization.
    /// </remarks>
    /// <example>
    /// <code>
    /// enum UserType { AnonymousUser, RegisteredUser }
    /// UserType.AnonymousUser.Humanize() => "Anonymous user"
    ///
    /// [Flags]
    /// enum Permission { None = 0, Read = 1, Write = 2, Delete = 4 }
    /// (Permission.Read | Permission.Write).Humanize() => "Read, Write"
    ///
    /// enum Status
    /// {
    ///     [Description("Currently active")]
    ///     Active
    /// }
    /// Status.Active.Humanize() => "Currently active"
    /// </code>
    /// </example>
    public static string Humanize<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(this T input)
        where T : struct, Enum
    {
        var (zero, humanized, values) = EnumCache<T>.GetInfo();
        if (EnumCache<T>.TreatAsFlags(input))
        {
            // Avoid LINQ allocations by manually iterating and building the list
            List<string>? flagValues = null;
            foreach (var value in values)
            {
                if (value.CompareTo(zero) != 0 && input.HasFlag(value))
                {
                    flagValues ??= new List<string>();
                    flagValues.Add(humanized[value]);
                }
            }

            return flagValues?.Humanize() ?? string.Empty;
        }

        return humanized[input];
    }

    public static string Humanize(this Enum input)
    {
        if (input == null)
            ArgumentNullException.ThrowIfNull(input);

        var enumType = input.GetType();

        // Get the generic Humanize<T> method
        var method = typeof(EnumHumanizeExtensions)
            .GetMethod(nameof(Humanize), new[] { enumType });

        if (method == null)
            throw new InvalidOperationException($"No Humanize method found for enum type {enumType}.");

        return (string)method.Invoke(null, new object[] { input })!;
    }


    /// <summary>
    /// Converts an enum value to a human-readable string with the specified letter casing applied.
    /// Respects any <see cref="System.ComponentModel.DescriptionAttribute"/> applied to the enum member.
    /// </summary>
    /// <typeparam name="T">The enum type. Must be a struct and implement <see cref="Enum"/>.</typeparam>
    /// <param name="input">The enum value to be humanized.</param>
    /// <param name="casing">The desired letter casing to apply to the humanized enum value.</param>
    /// <returns>
    /// A human-readable string representation of the enum value with the specified casing applied.
    /// If a <see cref="System.ComponentModel.DescriptionAttribute"/> is present, its value is used and then cased.
    /// </returns>
    /// <remarks>
    /// This is a convenience method that combines <see cref="Humanize{T}(T)"/> with <see cref="CasingExtensions.ApplyCase"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// enum UserType { AnonymousUser, RegisteredUser }
    /// UserType.AnonymousUser.Humanize(LetterCasing.AllCaps) => "ANONYMOUS USER"
    /// UserType.AnonymousUser.Humanize(LetterCasing.Title) => "Anonymous User"
    /// UserType.AnonymousUser.Humanize(LetterCasing.LowerCase) => "anonymous user"
    /// </code>
    /// </example>
    public static string Humanize<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(this T input, LetterCasing casing)
        where T : struct, Enum
    {
        var humanizedEnum = Humanize(input);

        return humanizedEnum.ApplyCase(casing);
    }
}