namespace Humanizer;

public partial class In
{
    /// <summary>
    /// Returns the first of January of the provided year
    /// </summary>
    public static DateTime TheYear(int year) =>
        new(year, 1, 1);
}