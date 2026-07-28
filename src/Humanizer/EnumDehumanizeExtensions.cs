namespace Humanizer;

/// <summary>
/// Contains extension methods for dehumanizing Enum string values.
/// </summary>
public static class EnumDehumanizeExtensions
{
    /// <summary>
    /// Converts a humanized string back to its original enum value by matching it against enum member names,
    /// their humanized representations, and configured metadata aliases.
    /// </summary>
    /// <typeparam name="TTargetEnum">The enum type to convert to. Must be a struct and implement <see cref="Enum"/>.</typeparam>
    /// <param name="input">The humanized string to be converted back to an enum value. Must not be null.</param>
    /// <returns>
    /// The enum value that matches the input string.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <typeparamref name="TTargetEnum"/> is not an enum type.</exception>
    /// <exception cref="NoMatchFoundException">
    /// Thrown when no enum member matches the input string.
    /// </exception>
    /// <remarks>
    /// The method attempts to match the input string against:
    /// <list type="number">
    /// <item><description>The exact enum member name.</description></item>
    /// <item><description>The humanized version of the enum member name.</description></item>
    /// <item><description><see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.Name"/>,
    /// <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.Description"/>, and
    /// <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.ShortName"/> values.</description></item>
    /// <item><description>The configured description attribute value when selected as the member's authored description.</description></item>
    /// </list>
    /// Matching is case-insensitive and does not trim whitespace. If aliases collide, later values in the enum's
    /// unsigned numeric order take precedence, while the current humanized representation takes precedence over
    /// supplemental aliases.
    /// </remarks>
    /// <example>
    /// <code>
    /// enum UserType { AnonymousUser, RegisteredUser }
    /// "Anonymous user".DehumanizeTo&lt;UserType&gt;() => UserType.AnonymousUser
    /// "Registered user".DehumanizeTo&lt;UserType&gt;() => UserType.RegisteredUser
    /// "AnonymousUser".DehumanizeTo&lt;UserType&gt;() => UserType.AnonymousUser
    /// </code>
    /// </example>
    public static TTargetEnum DehumanizeTo<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TTargetEnum>(this string input)
        where TTargetEnum : struct, Enum =>
        DehumanizeToPrivate<TTargetEnum>(input, OnNoMatch.ThrowsException)!.Value;

    /// <summary>
    /// Converts a humanized string back to its original enum value with configurable behavior when no match is found.
    /// </summary>
    /// <typeparam name="TTargetEnum">The enum type to convert to. Must be a struct and implement <see cref="Enum"/>.</typeparam>
    /// <param name="input">The humanized string to be converted back to an enum value. Must not be null.</param>
    /// <param name="onNoMatch">
    /// Specifies what to do when no matching enum member is found.
    /// Default is <see cref="OnNoMatch.ThrowsException"/>.
    /// </param>
    /// <returns>
    /// The enum value that matches the input string, or null if no match is found and 
    /// <paramref name="onNoMatch"/> is set to <see cref="OnNoMatch.ReturnsNull"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <typeparamref name="TTargetEnum"/> is not an enum type.</exception>
    /// <exception cref="NoMatchFoundException">
    /// Thrown when no enum member matches the input string and <paramref name="onNoMatch"/> is set to 
    /// <see cref="OnNoMatch.ThrowsException"/>.
    /// </exception>
    /// <remarks>
    /// This overload provides more control over error handling compared to the parameterless version.
    /// </remarks>
    /// <example>
    /// <code>
    /// enum UserType { AnonymousUser, RegisteredUser }
    /// "Anonymous user".DehumanizeTo&lt;UserType&gt;() => UserType.AnonymousUser
    /// "Invalid".DehumanizeTo&lt;UserType&gt;(OnNoMatch.ReturnsNull) => null
    /// "Invalid".DehumanizeTo&lt;UserType&gt;(OnNoMatch.ThrowsException) => throws NoMatchFoundException
    /// </code>
    /// </example>
    public static TTargetEnum? DehumanizeTo<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TTargetEnum>(this string input, OnNoMatch onNoMatch = OnNoMatch.ThrowsException)
        where TTargetEnum : struct, Enum =>
        DehumanizeToPrivate<TTargetEnum>(input, onNoMatch);

#if NET6_0_OR_GREATER
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The method is only used by DehumanizeTo which already has RequiresUnreferencedCode")]
    [UnconditionalSuppressMessage("Trimming", "IL2111", Justification = "The method is only used by DehumanizeTo which already has RequiresUnreferencedCode")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The method is only used by DehumanizeTo which already has RequiresDynamicCode")]
#endif
    static MethodInfo GetDehumanizeToMethodInfo() =>
        typeof(EnumDehumanizeExtensions).GetMethod("DehumanizeTo", [typeof(string), typeof(OnNoMatch)])!;

    static readonly Lazy<MethodInfo> DehumanizeToMethod = new(GetDehumanizeToMethodInfo);

    /// <summary>
    /// Converts a humanized string back to its original enum value using runtime type information.
    /// This is a non-generic overload that accepts the target enum type as a <see cref="Type"/> parameter.
    /// </summary>
    /// <param name="input">The humanized string to be converted back to an enum value. Must not be null.</param>
    /// <param name="targetEnum">
    /// The <see cref="Type"/> of the target enum. Must be an enum type.
    /// </param>
    /// <param name="onNoMatch">
    /// Specifies what to do when no matching enum member is found.
    /// Default is <see cref="OnNoMatch.ThrowsException"/>.
    /// </param>
    /// <returns>
    /// The enum value (as <see cref="Enum"/>) that matches the input string.
    /// </returns>
    /// <exception cref="NoMatchFoundException">
    /// Thrown when no enum member matches the input string and <paramref name="onNoMatch"/> is set to 
    /// <see cref="OnNoMatch.ThrowsException"/>.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="targetEnum"/> is not an enum type.</exception>
    /// <remarks>
    /// This method uses reflection and is less type-safe than the generic overload. Use the generic 
    /// <see cref="DehumanizeTo{TTargetEnum}(string, OnNoMatch)"/> method when the target enum type is known at compile time.
    /// </remarks>
    /// <example>
    /// <code>
    /// enum UserType { AnonymousUser, RegisteredUser }
    /// "Anonymous user".DehumanizeTo(typeof(UserType)) => UserType.AnonymousUser (as Enum)
    /// </code>
    /// </example>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(EnumDehumanizeExtensions))]
#endif
    public static Enum DehumanizeTo(this string input, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type targetEnum, OnNoMatch onNoMatch = OnNoMatch.ThrowsException)
    {
        var genericMethod = DehumanizeToMethod.Value.MakeGenericMethod(targetEnum);
        try
        {
            return (Enum)genericMethod.Invoke(null, [input, onNoMatch])!;
        }
        catch (TargetInvocationException exception)
        {
            throw exception.InnerException!;
        }
    }

    static T? DehumanizeToPrivate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(string input, OnNoMatch onNoMatch)
        where T : struct, Enum
    {
        var dehumanized = EnumCache<T>.GetDehumanized();
        if (dehumanized.TryGetValue(input, out var value))
        {
            return value;
        }

        if (onNoMatch != OnNoMatch.ThrowsException)
        {
            return null;
        }

        throw new NoMatchFoundException($"Couldn't find any enum member that matches the string '{input}'");
    }
}