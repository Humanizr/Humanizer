namespace Humanizer;

/// <summary>
/// Contains extension methods for humanizing Enums
/// </summary>
public static class EnumHumanizeExtensions
{
#if NET6_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The method is only used by Humanize which already has RequiresUnreferencedCode")]
    [UnconditionalSuppressMessage("Trimming", "IL2111", Justification = "The method is only used by Humanize which already has RequiresUnreferencedCode")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The method is only used by Humanize which already has RequiresDynamicCode")]
#endif
    static MethodInfo GetGenericHumanizeMethodInfo() =>
        typeof(EnumHumanizeExtensions)
            .GetMethods()
            .Single(method =>
                method.Name == nameof(Humanize) &&
                method.IsGenericMethodDefinition &&
                method.GetParameters().Length == 1 &&
                method.GetGenericArguments().Length == 1);

    static readonly Lazy<MethodInfo> GenericHumanizeMethod = new(GetGenericHumanizeMethodInfo);

    /// <summary>
    /// Converts an enum value to a human-readable string when the concrete enum type is only known at runtime.
    /// </summary>
    /// <param name="input">The enum value to be humanized.</param>
    /// <returns>A human-readable string representation of the enum value.</returns>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(EnumHumanizeExtensions))]
#endif
    public static string Humanize(this Enum input)
    {
        try
        {
            return (string)GenericHumanizeMethod.Value
                .MakeGenericMethod(input.GetType())
                .Invoke(null, [input])!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }

    /// <summary>
    /// Converts an enum value to a human-readable string with the specified letter casing when the concrete enum type is only known at runtime.
    /// </summary>
    /// <param name="input">The enum value to be humanized.</param>
    /// <param name="casing">The desired letter casing to apply to the humanized enum value.</param>
    /// <returns>A human-readable string representation of the enum value with the specified casing applied.</returns>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
#endif
    public static string Humanize(this Enum input, LetterCasing casing) =>
        input.Humanize().ApplyCase(casing);

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
