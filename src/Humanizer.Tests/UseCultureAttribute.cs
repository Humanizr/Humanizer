using System.Reflection;




/// <summary>
/// Apply this attribute to your test method to replace the
/// <see cref="Thread.CurrentThread" /> <see cref="CultureInfo.CurrentCulture" /> and
/// <see cref="CultureInfo.CurrentUICulture" /> with another culture.
/// </summary>
/// <remarks>
/// Replaces the culture and UI culture of the current thread with
/// <paramref name="culture" />
/// </remarks>
/// <remarks>
/// <para>
/// This constructor overload uses <paramref name="culture" /> for both
/// <see cref="CultureInfo.CurrentCulture" /> and <see cref="CultureInfo.CurrentUICulture" />.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UseCultureAttribute(string culture) : BeforeAfterTestAttribute
{
    readonly Lazy<CultureInfo> culture = new(() => new CultureInfo(culture));
    CultureInfo? originalCulture;
    CultureInfo? originalUICulture;

    public CultureInfo Culture => culture.Value;

    /// <summary>
    /// Stores the current <see cref="CultureInfo.CurrentCulture" />
    /// <see cref="CultureInfo.CurrentCulture" /> and <see cref="CultureInfo.CurrentUICulture" />
    /// and replaces them with the new cultures defined in the constructor.
    /// </summary>
    /// <param name="methodUnderTest">The method under test</param>
    public override void Before(MethodInfo methodUnderTest, IXunitTest test)
    {
        originalCulture = CultureInfo.CurrentCulture;
        originalUICulture = CultureInfo.CurrentUICulture;

        CultureInfo.CurrentCulture = Culture;
        CultureInfo.CurrentUICulture = Culture;
    }

    /// <summary>
    /// Restores the original <see cref="CultureInfo.CurrentCulture" /> and
    /// <see cref="CultureInfo.CurrentUICulture" /> to <see cref="CultureInfo.CurrentCulture" />
    /// </summary>
    /// <param name="methodUnderTest">The method under test</param>
    public override void After(MethodInfo methodUnderTest, IXunitTest test)
    {
        CultureInfo.CurrentCulture = originalCulture!;
        CultureInfo.CurrentUICulture = originalUICulture!;
    }
}