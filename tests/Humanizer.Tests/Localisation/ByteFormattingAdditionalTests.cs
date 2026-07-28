namespace Humanizer.Tests.Localisation;

public class ByteFormattingAdditionalTests
{
    [Theory]
    [MemberData(nameof(LocaleAdditionalByteTheoryData.AdditionalByteFullWordCases), MemberType = typeof(LocaleAdditionalByteTheoryData))]
    public void UsesExpectedAdditionalByteFullWords(string localeName, string unit, double value, string? format, string expected)
    {
        var culture = GetCulture(localeName);

        WithCulture(
            culture,
            () =>
            {
                var byteSize = CreateByteSize(unit, value);
                var actual = format is null ? byteSize.ToFullWords() : byteSize.ToFullWords(format);

                Assert.Equal(NormalizeFormatting(expected), NormalizeFormatting(actual));
            });
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalByteTheoryData.AdditionalByteHumanizeCases), MemberType = typeof(LocaleAdditionalByteTheoryData))]
    public void UsesExpectedAdditionalByteHumanizeOutputs(string localeName, string unit, double value, string? format, string expected)
    {
        var culture = GetCulture(localeName);

        WithCulture(
            culture,
            () =>
            {
                var actual = CreateByteSize(unit, value).Humanize(format);

                Assert.Equal(NormalizeFormatting(expected), NormalizeFormatting(actual));
            });
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalByteTheoryData.AdditionalByteToStringCases), MemberType = typeof(LocaleAdditionalByteTheoryData))]
    public void UsesExpectedAdditionalByteToStringOutputs(string localeName, string unit, double value, string? format, string mode, string expected)
    {
        var culture = GetCulture(localeName);

        WithCulture(
            culture,
            () =>
            {
                var byteSize = CreateByteSize(unit, value);
                var actual = mode switch
                {
                    "Default" => byteSize.ToString(),
                    "Formatted" => byteSize.ToString(format),
                    "Interpolated" => $"{byteSize}",
                    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
                };

                Assert.Equal(NormalizeFormatting(expected), NormalizeFormatting(actual));
            });
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalByteTheoryData.AdditionalByteRateHumanizeCases), MemberType = typeof(LocaleAdditionalByteTheoryData))]
    public void UsesExpectedAdditionalByteRateHumanizeOutputs(string localeName, long inputBytes, double perSeconds, string expected)
    {
        var culture = GetCulture(localeName);

        WithCulture(
            culture,
            () =>
            {
                var rate = new ByteSize(inputBytes).Per(TimeSpan.FromSeconds(perSeconds)).Humanize();

                Assert.Equal(NormalizeFormatting(expected), NormalizeFormatting(rate));
            });
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalByteTheoryData.AdditionalByteRateDisplayIntervalCases), MemberType = typeof(LocaleAdditionalByteTheoryData))]
    public void UsesExpectedAdditionalByteRateDisplayIntervalOutputs(string localeName, long megabytes, double measurementIntervalSeconds, TimeUnit displayInterval, string expected)
    {
        var culture = GetCulture(localeName);

        WithCulture(
            culture,
            () =>
            {
                var rate = ByteSize.FromMegabytes(megabytes).Per(TimeSpan.FromSeconds(measurementIntervalSeconds)).Humanize(displayInterval);

                Assert.Equal(NormalizeFormatting(expected), NormalizeFormatting(rate));
            });
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalByteTheoryData.AdditionalByteRateFormattedDisplayIntervalCases), MemberType = typeof(LocaleAdditionalByteTheoryData))]
    public void UsesExpectedAdditionalByteRateFormattedDisplayIntervalOutputs(string localeName, long bytes, int measurementIntervalSeconds, TimeUnit displayInterval, string? format, string expected)
    {
        var culture = GetCulture(localeName);

        WithCulture(
            culture,
            () =>
            {
                var rate = ByteSize.FromBytes(bytes).Per(TimeSpan.FromSeconds(measurementIntervalSeconds)).Humanize(format, displayInterval);

                Assert.Equal(NormalizeFormatting(expected), NormalizeFormatting(rate));
            });
    }

    [Theory]
    [MemberData(nameof(LocaleAdditionalByteTheoryData.AdditionalUnsupportedByteRateUnits), MemberType = typeof(LocaleAdditionalByteTheoryData))]
    public void ThrowsForUnsupportedAdditionalByteRateUnits(string localeName, TimeUnit units)
    {
        var culture = GetCulture(localeName);

        WithCulture(
            culture,
            () =>
            {
                var dummyRate = ByteSize.FromBits(1).Per(TimeSpan.FromSeconds(1));

                Assert.Throws<NotSupportedException>(() => dummyRate.Humanize(units));
            });
    }

    static ByteSize CreateByteSize(string unit, double value) =>
        unit switch
        {
            "Bit" => ByteSize.FromBits((long)value),
            "Bits" => ByteSize.FromBits((long)value),
            "Byte" => ByteSize.FromBytes(value),
            "Bytes" => ByteSize.FromBytes(value),
            "Kilobyte" => ByteSize.FromKilobytes(value),
            "Kilobytes" => ByteSize.FromKilobytes(value),
            "Megabyte" => ByteSize.FromMegabytes(value),
            "Megabytes" => ByteSize.FromMegabytes(value),
            "Gigabyte" => ByteSize.FromGigabytes(value),
            "Gigabytes" => ByteSize.FromGigabytes(value),
            "Terabyte" => ByteSize.FromTerabytes(value),
            "Terabytes" => ByteSize.FromTerabytes(value),
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };

    static CultureInfo GetCulture(string localeName) =>
        localeName == "invariant"
            ? CultureInfo.InvariantCulture
            : CultureInfo.GetCultureInfo(localeName);

    static string NormalizeFormatting(string value) =>
        value
            .Replace('\u00A0', ' ')
            .Replace('\u202F', ' ');

    static void WithCulture(CultureInfo culture, Action action)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            action();
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }
}