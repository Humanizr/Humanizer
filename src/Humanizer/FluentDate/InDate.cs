#if NET6_0_OR_GREATER
namespace Humanizer;

public partial class InDate
{
    /// <summary>
    /// Returns the first of January of the provided year
    /// </summary>
    public static DateOnly TheYear(int year) =>
        new(year, 1, 1);
}
#endif
