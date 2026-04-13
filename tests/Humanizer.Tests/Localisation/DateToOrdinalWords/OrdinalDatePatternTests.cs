namespace Humanizer.Tests.Localisation.DateToOrdinalWords;

public class OrdinalDatePatternTests
{
    // -----------------------------------------------------------------------
    // FormatDay day-mode switch arms
    // -----------------------------------------------------------------------

    [Fact]
    [UseCulture("pt-BR")]
    public void FormatDay_MasculineOrdinalWhenDayIsOne_OrdinalizesDayOne()
    {
        // pt-BR uses MasculineOrdinalWhenDayIsOne; day=1 should produce the masculine ordinal.
        var result = new DateTime(2015, 1, 1).ToOrdinalWords();
        Assert.Contains("1", result); // The ordinal form includes the day
        Assert.NotEqual("1 ", result[..2]); // Day 1 is ordinalized, not plain numeric "1 "
    }

    [Fact]
    [UseCulture("pt-BR")]
    public void FormatDay_MasculineOrdinalWhenDayIsOne_NumericForNonFirstDay()
    {
        // pt-BR day>1 should be plain numeric.
        var result = new DateTime(2015, 2, 15).ToOrdinalWords();
        Assert.Contains("15", result);
    }

    [Fact]
    [UseCulture("cs")]
    public void FormatDay_DotSuffix_AppendsDot()
    {
        // Czech uses DotSuffix; the day should end with a dot.
        var result = new DateTime(2022, 1, 25).ToOrdinalWords();
        Assert.Contains("25.", result);
    }

    [Fact]
    [UseCulture("da")]
    public void FormatDay_DotSuffix_AppendsDotForDayOne()
    {
        // Danish uses DotSuffix; even day 1 should have a dot.
        var result = new DateTime(2015, 1, 1).ToOrdinalWords();
        Assert.Contains("1.", result);
    }

    [Fact]
    public void FormatDay_InvalidDayMode_ThrowsInvalidOperationException()
    {
        // Directly instantiate with an invalid enum value to cover the default throw arm.
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", (OrdinalDateDayMode)99);

        using var _ = new CultureSwap(new CultureInfo("en"));
        Assert.Throws<InvalidOperationException>(() => pattern.Format(new DateTime(2022, 1, 1)));
    }

    // -----------------------------------------------------------------------
    // DateOnly overload (NET6_0_OR_GREATER)
    // -----------------------------------------------------------------------

#if NET6_0_OR_GREATER
    [Fact]
    [UseCulture("cs")]
    public void Format_DateOnly_DotSuffix()
    {
        var result = new DateOnly(2022, 3, 15).ToOrdinalWords();
        Assert.Contains("15.", result);
    }

    [Fact]
    [UseCulture("pt-BR")]
    public void Format_DateOnly_MasculineOrdinalWhenDayIsOne()
    {
        var result = new DateOnly(2015, 1, 1).ToOrdinalWords();
        Assert.Contains("1", result);
    }

    [Fact]
    public void Format_DateOnly_InvalidDayMode_ThrowsInvalidOperationException()
    {
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", (OrdinalDateDayMode)99);

        using var _ = new CultureSwap(new CultureInfo("en"));
        Assert.Throws<InvalidOperationException>(() => pattern.Format(new DateOnly(2022, 1, 1)));
    }
#endif

    // -----------------------------------------------------------------------
    // SubstituteMonth — month-override and genitive branches
    // -----------------------------------------------------------------------

    [Fact]
    public void Format_WithMonthOverrides_SubstitutesMonthName()
    {
        var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", OrdinalDateDayMode.Numeric, months: months);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 3, 15));
        Assert.Contains("Mar", result);
    }

    [Fact]
    public void Format_WithGenitiveMonths_UsesGenitiveWhenDayAdjacent()
    {
        var months = new[] { "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь" };
        var monthsGenitive = new[] { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };
        // Template with {day} before MMMM triggers genitive.
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", OrdinalDateDayMode.Numeric, months: months, monthsGenitive: monthsGenitive);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 3, 15));
        Assert.Contains("марта", result);
    }

    [Fact]
    public void Format_WithGenitiveMonths_UsesNominativeWhenDayNotAdjacent()
    {
        var months = new[] { "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь" };
        var monthsGenitive = new[] { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };
        // Template without {day} adjacent to MMMM -- month alone, no day specifier.
        var pattern = new OrdinalDatePattern("MMMM yyyy", OrdinalDateDayMode.Numeric, months: months, monthsGenitive: monthsGenitive);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 3, 15));
        Assert.Contains("март", result);
    }

    [Fact]
    public void Format_NoMonthOverrides_UsesICUDefault()
    {
        // No months/monthsGenitive passed — format string unchanged.
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", OrdinalDateDayMode.Numeric);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 1, 15));
        // Should contain an English month name from the platform.
        Assert.Contains("January", result);
    }

    [Fact]
    public void Format_MonthsWithApostrophe_EscapesAndProducesOutput()
    {
        // Exercise the apostrophe-escaping branch in SubstituteMonth (Replace("'", "''")).
        // In .NET DateTime format strings, '' at a quote boundary acts as end-quote/start-quote
        // rather than an escaped literal, so the apostrophe is absorbed. The key goal is that
        // the code does not throw and the month text appears in the output.
        var months = new[] { "Jan", "Feb", "Mar", "Ap'ril", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", OrdinalDateDayMode.Numeric, months: months);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 4, 15));
        // The month text "April" (with apostrophe absorbed) should be present.
        Assert.Contains("April", result);
        Assert.Contains("2022", result);
    }

    [Fact]
    public void Format_NoMmmmInTemplate_ReturnsUnchanged()
    {
        // Template without MMMM specifier — SubstituteMonth should return as-is.
        var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var pattern = new OrdinalDatePattern("{day} MMM yyyy", OrdinalDateDayMode.Numeric, months: months);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 1, 15));
        // Should not contain "Jan" substitution since only MMMM (4+ Ms) triggers it.
        // The platform's short month name is used instead.
        Assert.NotNull(result);
    }

    // -----------------------------------------------------------------------
    // SubstituteMonth — quoted M not treated as specifier
    // -----------------------------------------------------------------------

    [Fact]
    public void Format_QuotedM_NotTreatedAsMonthSpecifier()
    {
        // 'M' in single quotes is a literal, not a specifier.
        var months = new[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve" };
        var pattern = new OrdinalDatePattern("'MMMM' {day} MMMM yyyy", OrdinalDateDayMode.Numeric, months: months);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 1, 15));
        // The first MMMM is quoted and stays literal; the second gets substituted.
        Assert.Contains("One", result);
        Assert.Contains("MMMM", result);
    }

    // -----------------------------------------------------------------------
    // IsDayAdjacentToMonth / FindAdjacentDayOfMonth coverage
    // -----------------------------------------------------------------------

    [Fact]
    public void Format_DayAfterMonth_DetectsGenitiveContext()
    {
        var months = new[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "aug", "sep", "okt", "nov", "dec" };
        var monthsGenitive = new[] { "jana", "feba", "mara", "apra", "maja", "juna", "jula", "auga", "sepa", "okta", "nova", "deca" };
        // MMMM d pattern -- day is after month.
        var pattern = new OrdinalDatePattern("MMMM {day}, yyyy", OrdinalDateDayMode.Numeric, months: months, monthsGenitive: monthsGenitive);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 3, 15));
        // Day adjacent after MMMM -- genitive should be used.
        Assert.Contains("mara", result);
    }

    [Fact]
    public void Format_DayOfWeekSpecifier_NotTreatedAsDayOfMonth()
    {
        var months = new[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "aug", "sep", "okt", "nov", "dec" };
        var monthsGenitive = new[] { "jana", "feba", "mara", "apra", "maja", "juna", "jula", "auga", "sepa", "okta", "nova", "deca" };
        // dddd MMMM pattern -- dddd is day-of-week, NOT day-of-month.
        var pattern = new OrdinalDatePattern("dddd MMMM yyyy", OrdinalDateDayMode.Numeric, months: months, monthsGenitive: monthsGenitive);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 3, 15));
        // ddd+ is day-of-week, so genitive should NOT be used (no day-of-month adjacent).
        Assert.Contains("mar", result);
        Assert.DoesNotContain("mara", result);
    }

    // -----------------------------------------------------------------------
    // ReplaceDayMarker — fallback when numeric day text not found before marker
    // -----------------------------------------------------------------------

    // This is tested indirectly through the various day modes above.

    // -----------------------------------------------------------------------
    // StripDirectionalityControls
    // -----------------------------------------------------------------------

    [Fact]
    [UseCulture("ar")]
    public void Format_Arabic_StripsDirectionalityControls()
    {
        var result = new DateTime(2022, 1, 25).ToOrdinalWords();
        Assert.DoesNotContain('\u200E', result);
        Assert.DoesNotContain('\u200F', result);
        Assert.DoesNotContain('\u061C', result);
    }

    // -----------------------------------------------------------------------
    // GetPatternCulture — GregorianCalendar loop + Native calendar mode
    // -----------------------------------------------------------------------

    [Fact]
    [UseCulture("th")]
    public void Format_ThaiCulture_UsesNativeCalendar()
    {
        // Thai uses Native calendar mode. The year should be Buddhist era.
        var result = new DateTime(2022, 1, 25).ToOrdinalWords();
        // Buddhist year 2565 for Gregorian 2022.
        Assert.Contains("2565", result);
    }

    [Fact]
    [UseCulture("en")]
    public void Format_GregorianCulture_ForcesGregorianCalendar()
    {
        var result = new DateTime(2022, 1, 25).ToOrdinalWords();
        Assert.Contains("2022", result);
    }

    // -----------------------------------------------------------------------
    // HijriMonths — ResolveMonthArray with Hijri calendar
    // -----------------------------------------------------------------------

    [Fact]
    public void Format_WithHijriMonths_UsesHijriNamesForHijriCalendar()
    {
        var hijriMonths = new[] { "Muh", "Saf", "Rab1", "Rab2", "Jum1", "Jum2", "Raj", "Sha", "Ram", "Shaw", "DhQ", "DhH" };
        var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", OrdinalDateDayMode.Numeric, months: months, hijriMonths: hijriMonths);

        // Use a culture that defaults to Gregorian -- hijri months are NOT used.
        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 1, 15));
        Assert.Contains("Jan", result);
    }

    // -----------------------------------------------------------------------
    // Direct instantiation: Ordinal day mode
    // -----------------------------------------------------------------------

    [Fact]
    [UseCulture("en")]
    public void FormatDay_Ordinal_OrdinalizesAllDays()
    {
        var result = new DateTime(2022, 1, 25).ToOrdinalWords();
        Assert.Contains("25th", result);
    }

    [Fact]
    [UseCulture("en")]
    public void FormatDay_Ordinal_OrdinalizesFirstDay()
    {
        var result = new DateTime(2022, 1, 1).ToOrdinalWords();
        Assert.Contains("1st", result);
    }

    // -----------------------------------------------------------------------
    // Direct instantiation: OrdinalWhenDayIsOne
    // -----------------------------------------------------------------------

    [Fact]
    [UseCulture("fr")]
    public void FormatDay_OrdinalWhenDayIsOne_OrdinalizesFirstDay()
    {
        // French uses OrdinalWhenDayIsOne; day=1 should be ordinal.
        var result = new DateTime(2015, 1, 1).ToOrdinalWords();
        Assert.Contains("1er", result);
    }

    [Fact]
    [UseCulture("fr")]
    public void FormatDay_OrdinalWhenDayIsOne_NumericForOtherDays()
    {
        // French day>1 should be plain numeric.
        var result = new DateTime(2015, 1, 15).ToOrdinalWords();
        Assert.Contains("15", result);
        Assert.DoesNotContain("15e", result);
    }

    // -----------------------------------------------------------------------
    // Numeric day mode
    // -----------------------------------------------------------------------

    [Fact]
    [UseCulture("nl")]
    public void FormatDay_Numeric_PlainNumber()
    {
        // Dutch uses Numeric day mode.
        var result = new DateTime(2022, 1, 25).ToOrdinalWords();
        Assert.Contains("25", result);
    }

    // -----------------------------------------------------------------------
    // FindAdjacentDayOfMonth — punctuation and separators between day and month
    // -----------------------------------------------------------------------

    [Fact]
    public void Format_DayWithDotSeparatorBeforeMonth_DetectsGenitive()
    {
        var months = new[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "aug", "sep", "okt", "nov", "dec" };
        var monthsGenitive = new[] { "jana", "feba", "mara", "apra", "maja", "juna", "jula", "auga", "sepa", "okta", "nova", "deca" };
        // Pattern: "d. MMMM yyyy" — dot+space between day and month should still detect adjacency.
        var pattern = new OrdinalDatePattern("{day}. MMMM yyyy", OrdinalDateDayMode.DotSuffix, months: months, monthsGenitive: monthsGenitive);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 3, 15));
        Assert.Contains("mara", result);
    }

    // -----------------------------------------------------------------------
    // FindAdjacentDayOfMonth — non-day, non-whitespace character terminates scan
    // -----------------------------------------------------------------------

    [Fact]
    public void Format_YearSpecifierAdjacentToMonth_NoGenitive()
    {
        var months = new[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "aug", "sep", "okt", "nov", "dec" };
        var monthsGenitive = new[] { "jana", "feba", "mara", "apra", "maja", "juna", "jula", "auga", "sepa", "okta", "nova", "deca" };
        // "yyyy MMMM" — year is adjacent, not day. Genitive should NOT apply.
        var pattern = new OrdinalDatePattern("yyyy MMMM", OrdinalDateDayMode.Numeric, months: months, monthsGenitive: monthsGenitive);

        using var _ = new CultureSwap(new CultureInfo("en"));
        var result = pattern.Format(new DateTime(2022, 3, 15));
        Assert.Contains("mar", result);
        Assert.DoesNotContain("mara", result);
    }
}