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
    static MethodInfo GetGenericHumanizeMethodInfo(params Type[] parameterTypes) =>
        typeof(EnumHumanizeExtensions)
            .GetMethods()
            .Single(method =>
                method.Name == nameof(Humanize) &&
                method.IsGenericMethodDefinition &&
                method.GetGenericArguments().Length == 1 &&
                method.GetParameters()
                    .Skip(1)
                    .Select(parameter => parameter.ParameterType)
                    .SequenceEqual(parameterTypes));

    static readonly Lazy<MethodInfo> GenericHumanizeMethod = new(() => GetGenericHumanizeMethodInfo());
    static readonly Lazy<MethodInfo> GenericHumanizeWithCasingMethod = new(() => GetGenericHumanizeMethodInfo(typeof(LetterCasing)));
    static readonly Lazy<MethodInfo> GenericHumanizeWithCasingAndSourceMethod = new(() => GetGenericHumanizeMethodInfo(typeof(LetterCasing), typeof(EnumHumanizeSource)));

#if NET6_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2060", Justification = "The method is only used by Humanize which preserves the generic methods with DynamicDependency")]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The method is only used by Humanize which already has RequiresUnreferencedCode")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The method is only used by Humanize which already has RequiresDynamicCode")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(EnumHumanizeExtensions))]
#endif
    static string InvokeGenericHumanize(Enum input, Lazy<MethodInfo> method, params object[] arguments)
    {
        try
        {
            return (string)method.Value
                .MakeGenericMethod(input.GetType())
                .Invoke(null, arguments)!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }

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
    public static string Humanize(this Enum input) =>
        InvokeGenericHumanize(input, GenericHumanizeMethod, input);

    /// <summary>
    /// Converts an enum value to a human-readable string with the specified letter casing applied to the enum member name
    /// when the concrete enum type is only known at runtime. Authored metadata on a defined enum value is returned unchanged.
    /// </summary>
    /// <param name="input">The enum value to be humanized.</param>
    /// <param name="casing">The desired letter casing to apply when humanizing the enum member name.</param>
    /// <returns>A human-readable string representation of the enum value.</returns>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
#endif
    public static string Humanize(this Enum input, LetterCasing casing) =>
        InvokeGenericHumanize(input, GenericHumanizeWithCasingMethod, input, casing);

    /// <summary>
    /// Converts an enum value to a human-readable string using the specified casing and source when the concrete enum type is only known at runtime.
    /// </summary>
    /// <param name="input">The enum value to be humanized.</param>
    /// <param name="casing">The desired letter casing to apply when humanizing the enum member name.</param>
    /// <param name="source">The source used to humanize the enum value.</param>
    /// <returns>A human-readable string representation of the enum value.</returns>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(EnumHumanizeExtensions))]
#endif
    public static string Humanize(this Enum input, LetterCasing casing, EnumHumanizeSource source) =>
        InvokeGenericHumanize(input, GenericHumanizeWithCasingAndSourceMethod, input, casing, source);

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
        where T : struct, Enum =>
        Humanize(input, null, EnumHumanizeSource.Default);

    static string Humanize<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(
        T input,
        LetterCasing? casing,
        EnumHumanizeSource source)
        where T : struct, Enum
    {
        if (!Enum.IsDefined(source))
        {
            throw new ArgumentOutOfRangeException(nameof(source));
        }

        var (zero, humanized, values) = EnumCache<T>.GetInfo();
        if (EnumCache<T>.TreatAsFlags(input))
        {
            if (casing is { } flagsCasing && !Enum.IsDefined(flagsCasing))
            {
                throw new ArgumentOutOfRangeException(nameof(casing));
            }

            // Avoid LINQ allocations by manually iterating and building the list
            List<string>? flagValues = null;
            foreach (var value in values)
            {
                if (value.CompareTo(zero) != 0 && input.HasFlag(value))
                {
                    flagValues ??= new List<string>();
                    var flag = source == EnumHumanizeSource.Default
                        ? humanized[value]
                        : EnumCache<T>.GetHumanized(value, source);
                    flagValues.Add(casing is { } flagCasing && !flag.IsMetadata ? flag.Text.ApplyCase(flagCasing) : flag.Text);
                }
            }

            return flagValues?.Humanize() ?? string.Empty;
        }

        var humanizedEnum = source == EnumHumanizeSource.Default
            ? humanized[input]
            : EnumCache<T>.GetHumanized(input, source);
        if (casing is not { } enumCasing)
        {
            return humanizedEnum.Text;
        }

        if (!Enum.IsDefined(enumCasing))
        {
            throw new ArgumentOutOfRangeException(nameof(casing));
        }

        return humanizedEnum.IsMetadata ? humanizedEnum.Text : humanizedEnum.Text.ApplyCase(enumCasing);
    }


    /// <summary>
    /// Converts an enum value to a human-readable string with the specified letter casing applied to the enum member name.
    /// Authored metadata on a defined enum value is returned unchanged.
    /// </summary>
    /// <typeparam name="T">The enum type. Must be a struct and implement <see cref="Enum"/>.</typeparam>
    /// <param name="input">The enum value to be humanized.</param>
    /// <param name="casing">The desired letter casing to apply when humanizing the enum member name.</param>
    /// <returns>
    /// A human-readable string representation of the enum value.
    /// If a defined enum value has authored metadata such as <see cref="System.ComponentModel.DescriptionAttribute"/>, its value is returned unchanged.
    /// </returns>
    /// <remarks>
    /// For a defined enum value, the specified casing is applied only when the output is derived from the enum member name.
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
        => Humanize(input, casing, EnumHumanizeSource.Default);

    /// <summary>
    /// Converts an enum value to a human-readable string using the specified casing and source.
    /// </summary>
    /// <typeparam name="T">The enum type. Must be a struct and implement <see cref="Enum"/>.</typeparam>
    /// <param name="input">The enum value to be humanized.</param>
    /// <param name="casing">The desired letter casing to apply when humanizing the enum member name.</param>
    /// <param name="source">The source used to humanize the enum value.</param>
    /// <returns>A human-readable string representation of the enum value.</returns>
    public static string Humanize<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(
        this T input,
        LetterCasing casing,
        EnumHumanizeSource source)
        where T : struct, Enum =>
        Humanize(input, (LetterCasing?)casing, source);
}