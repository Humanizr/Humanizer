namespace Humanizer;

/// <summary>
/// Contains extension methods for dehumanizing Enum string values.
/// </summary>
public static class EnumDehumanizeExtensions
{
    /// <summary>
    /// Dehumanizes a string into the Enum it was originally Humanized from!
    /// </summary>
    /// <typeparam name="TTargetEnum">The target enum</typeparam>
    /// <param name="input">The string to be converted</param>
    /// <exception cref="ArgumentException">If TTargetEnum is not an enum</exception>
    /// <exception cref="NoMatchFoundException">Couldn't find any enum member that matches the string</exception>
    public static TTargetEnum DehumanizeTo<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TTargetEnum>(this string input)
        where TTargetEnum : struct, Enum =>
        DehumanizeToPrivate<TTargetEnum>(input, OnNoMatch.ThrowsException)!.Value;

    /// <summary>
    /// Dehumanizes a string into the Enum it was originally Humanized from!
    /// </summary>
    /// <typeparam name="TTargetEnum">The target enum</typeparam>
    /// <param name="input">The string to be converted</param>
    /// <exception cref="ArgumentException">If TTargetEnum is not an enum</exception>
    /// <exception cref="NoMatchFoundException">Couldn't find any enum member that matches the string</exception>
    public static TTargetEnum? DehumanizeTo<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TTargetEnum>(this string input, OnNoMatch onNoMatch = OnNoMatch.ThrowsException)
        where TTargetEnum : struct, Enum =>
        DehumanizeToPrivate<TTargetEnum>(input, onNoMatch);

#if NET6_0_OR_GREATER
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(EnumDehumanizeExtensions))]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Access to DehumanizeTo via reflection is intentional and documented.")]
#endif
    static readonly MethodInfo dehumanizeToMethod = typeof(EnumDehumanizeExtensions)
        .GetMethod("DehumanizeTo", [typeof(string), typeof(OnNoMatch)])!;

    /// <summary>
    /// Dehumanizes a string into the Enum it was originally Humanized from!
    /// </summary>
    /// <param name="input">The string to be converted</param>
    /// <param name="targetEnum">The target enum</param>
    /// <param name="onNoMatch">What to do when input is not matched to the enum.</param>
    /// <exception cref="NoMatchFoundException">Couldn't find any enum member that matches the string</exception>
    /// <exception cref="ArgumentException">If targetEnum is not an enum</exception>
#if NET6_0_OR_GREATER
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
#endif
    public static Enum DehumanizeTo(this string input, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type targetEnum, OnNoMatch onNoMatch = OnNoMatch.ThrowsException)
    {
        var genericMethod = dehumanizeToMethod.MakeGenericMethod(targetEnum);
        try
        {
            return (Enum) genericMethod.Invoke(null, [input, onNoMatch])!;
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