#if NET6_0_OR_GREATER

namespace Humanizer.Tests.Localisation.Default;

/// <summary>
/// Tests for <see cref="DefaultDateOnlyToOrdinalWordConverter"/>, the fallback when no
/// locale-specific converter is registered for <see cref="DateOnly"/> values.
/// </summary>
public class DefaultDateOnlyToOrdinalWordConverterTests
{
    readonly DefaultDateOnlyToOrdinalWordConverter converter = new();

    [Fact]
    [UseCulture("en-US")]
    public void EnglishCulture_RendersOrdinalDay()
    {
        var date = new DateOnly(2024, 3, 15);
        var result = converter.Convert(date);

        Assert.Equal("15th March 2024", result);
    }

    [Fact]
    [UseCulture("en-US")]
    public void EnglishCulture_GrammaticalCaseOverload_DelegatesToConvert()
    {
        var date = new DateOnly(2024, 3, 15);
        var result = converter.Convert(date, GrammaticalCase.Genitive);

        // The default converter ignores grammatical case, delegates to Convert(DateOnly)
        Assert.Equal("15th March 2024", result);
    }

    [Fact]
    [UseCulture("en-US")]
    public void EnglishCulture_FirstOfMonth_RendersCorrectOrdinal()
    {
        var date = new DateOnly(2024, 1, 1);
        var result = converter.Convert(date);

        Assert.Equal("1st January 2024", result);
    }

    [Fact]
    public void NonEnglishCulture_UsesShortDatePattern()
    {
        // Use Esperanto (eo) -- a non-English culture that won't have marks
        var culture = new CultureInfo("eo");
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = culture;
            var date = new DateOnly(2024, 3, 15);
            var result = converter.Convert(date);

            var expected = date.ToString("d", culture);
            Assert.Equal(expected, result);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void NonEnglishCulture_GrammaticalCaseOverload_DelegatesToConvert()
    {
        var culture = new CultureInfo("eo");
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = culture;
            var date = new DateOnly(2024, 3, 15);
            var result = converter.Convert(date, GrammaticalCase.Dative);
            var expected = date.ToString("d", culture);
            Assert.Equal(expected, result);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void SanitizeNonEnglishDate_StripsLeftToRightMark()
    {
        var baseCulture = (CultureInfo)new CultureInfo("fr-FR").Clone();
        baseCulture.DateTimeFormat.ShortDatePattern = "'\u200E'dd/MM/yyyy";

        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = baseCulture;
            var date = new DateOnly(2024, 3, 15);
            var result = converter.Convert(date);

            AssertNoDirectionalMarks(result);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void SanitizeNonEnglishDate_StripsRightToLeftMark()
    {
        var baseCulture = (CultureInfo)new CultureInfo("fr-FR").Clone();
        baseCulture.DateTimeFormat.ShortDatePattern = "'\u200F'dd/MM/yyyy";

        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = baseCulture;
            var date = new DateOnly(2024, 3, 15);
            var result = converter.Convert(date);

            AssertNoDirectionalMarks(result);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void SanitizeNonEnglishDate_StripsArabicLetterMark()
    {
        var baseCulture = (CultureInfo)new CultureInfo("fr-FR").Clone();
        baseCulture.DateTimeFormat.ShortDatePattern = "'\u061C'dd/MM/yyyy";

        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = baseCulture;
            var date = new DateOnly(2024, 3, 15);
            var result = converter.Convert(date);

            AssertNoDirectionalMarks(result);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void SanitizeNonEnglishDate_StripsAllThreeMarksSimultaneously()
    {
        var baseCulture = (CultureInfo)new CultureInfo("fr-FR").Clone();
        baseCulture.DateTimeFormat.ShortDatePattern = "'\u200E\u200F\u061C'dd/MM/yyyy";

        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = baseCulture;
            var date = new DateOnly(2024, 3, 15);
            var result = converter.Convert(date);

            AssertNoDirectionalMarks(result);
            Assert.Contains("15", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    /// <summary>
    /// Asserts that none of the three directional/formatting Unicode marks appear anywhere
    /// in <paramref name="value"/>. Uses ordinal comparison to avoid the culture-sensitive
    /// issue where <c>StringComparison.CurrentCulture</c> treats these zero-width marks as
    /// ignorable and falsely reports them as present in any string.
    /// </summary>
    static void AssertNoDirectionalMarks(string value)
    {
        Assert.DoesNotContain("\u200E", value, StringComparison.Ordinal);
        Assert.DoesNotContain("\u200F", value, StringComparison.Ordinal);
        Assert.DoesNotContain("\u061C", value, StringComparison.Ordinal);
    }
}

#endif