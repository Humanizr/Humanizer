using System;

namespace Humanizer;

public partial class In
{
    /// <summary>
    /// Returns 1st of January of the current year
    /// </summary>
    public static DateTime January
        => new(DateTime.UtcNow.Year, 1, 1);

    /// <summary>
    /// Returns 1st of January of the year passed in
    /// </summary>
    public static DateTime JanuaryOf(int year)
        => new(year, 1, 1);

    /// <summary>
    /// Returns 1st of February of the current year
    /// </summary>
    public static DateTime February
        => new(DateTime.UtcNow.Year, 2, 1);

    /// <summary>
    /// Returns 1st of February of the year passed in
    /// </summary>
    public static DateTime FebruaryOf(int year)
        => new(year, 2, 1);

    /// <summary>
    /// Returns 1st of March of the current year
    /// </summary>
    public static DateTime March
        => new(DateTime.UtcNow.Year, 3, 1);

    /// <summary>
    /// Returns 1st of March of the year passed in
    /// </summary>
    public static DateTime MarchOf(int year)
        => new(year, 3, 1);

    /// <summary>
    /// Returns 1st of April of the current year
    /// </summary>
    public static DateTime April
        => new(DateTime.UtcNow.Year, 4, 1);

    /// <summary>
    /// Returns 1st of April of the year passed in
    /// </summary>
    public static DateTime AprilOf(int year)
        => new(year, 4, 1);

    /// <summary>
    /// Returns 1st of May of the current year
    /// </summary>
    public static DateTime May
        => new(DateTime.UtcNow.Year, 5, 1);

    /// <summary>
    /// Returns 1st of May of the year passed in
    /// </summary>
    public static DateTime MayOf(int year)
        => new(year, 5, 1);

    /// <summary>
    /// Returns 1st of June of the current year
    /// </summary>
    public static DateTime June
        => new(DateTime.UtcNow.Year, 6, 1);

    /// <summary>
    /// Returns 1st of June of the year passed in
    /// </summary>
    public static DateTime JuneOf(int year)
        => new(year, 6, 1);

    /// <summary>
    /// Returns 1st of July of the current year
    /// </summary>
    public static DateTime July
        => new(DateTime.UtcNow.Year, 7, 1);

    /// <summary>
    /// Returns 1st of July of the year passed in
    /// </summary>
    public static DateTime JulyOf(int year)
        => new(year, 7, 1);

    /// <summary>
    /// Returns 1st of August of the current year
    /// </summary>
    public static DateTime August
        => new(DateTime.UtcNow.Year, 8, 1);

    /// <summary>
    /// Returns 1st of August of the year passed in
    /// </summary>
    public static DateTime AugustOf(int year)
        => new(year, 8, 1);

    /// <summary>
    /// Returns 1st of September of the current year
    /// </summary>
    public static DateTime September
        => new(DateTime.UtcNow.Year, 9, 1);

    /// <summary>
    /// Returns 1st of September of the year passed in
    /// </summary>
    public static DateTime SeptemberOf(int year)
        => new(year, 9, 1);

    /// <summary>
    /// Returns 1st of October of the current year
    /// </summary>
    public static DateTime October
        => new(DateTime.UtcNow.Year, 10, 1);

    /// <summary>
    /// Returns 1st of October of the year passed in
    /// </summary>
    public static DateTime OctoberOf(int year)
        => new(year, 10, 1);

    /// <summary>
    /// Returns 1st of November of the current year
    /// </summary>
    public static DateTime November
        => new(DateTime.UtcNow.Year, 11, 1);

    /// <summary>
    /// Returns 1st of November of the year passed in
    /// </summary>
    public static DateTime NovemberOf(int year)
        => new(year, 11, 1);

    /// <summary>
    /// Returns 1st of December of the current year
    /// </summary>
    public static DateTime December
        => new(DateTime.UtcNow.Year, 12, 1);

    /// <summary>
    /// Returns 1st of December of the year passed in
    /// </summary>
    public static DateTime DecemberOf(int year)
        => new(year, 12, 1);

}
