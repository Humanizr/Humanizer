namespace Humanizer;

/// <summary>
/// Contains extension methods for humanizing Enums
/// </summary>
public static class EnumHumanizeExtensions
{
    /// <summary>
    /// Turns an enum member into a human readable string; e.g. AnonymousUser -> Anonymous user. It also honors DescriptionAttribute data annotation
    /// </summary>
    /// <param name="input">The enum member to be humanized</param>
    public static string Humanize<T>(this T input)
        where T : struct, Enum
    {
        var type = typeof(T);

        if (EnumCache<T>.IsBitFieldEnum && !Enum.IsDefined(type, input))
        {
            return EnumCache<T>
                .Values
                .Where(_ => input.HasFlag(_) &&
                            _.CompareTo(Convert.ChangeType(Enum.ToObject(type, 0), type)) != 0)
                .Select(_ => _.Humanize())
                .Humanize();
        }

        return EnumCache<T>.Humanized[input];
    }

    /// <summary>
    /// Turns an enum member into a human readable string with the provided casing; e.g. AnonymousUser with Title casing -> Anonymous User. It also honors DescriptionAttribute data annotation
    /// </summary>
    /// <param name="input">The enum member to be humanized</param>
    /// <param name="casing">The casing to use for humanizing the enum member</param>
    public static string Humanize<T>(this T input, LetterCasing casing)
        where T : struct, Enum
    {
        var humanizedEnum = Humanize(input);

        return humanizedEnum.ApplyCase(casing);
    }
}