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
        var (zero, humanized, values) = EnumCache<T>.GetInfo();
        if (EnumCache<T>.TreatAsFlags(input))
        {
            return values
                .Where(_ => _.CompareTo(zero) != 0 &&
                            input.HasFlag(_))
                .Select(_ => _.Humanize())
                .Humanize();
        }

        return humanized[input];
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