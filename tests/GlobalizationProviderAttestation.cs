namespace Humanizer.Tests.Infrastructure;

internal static class GlobalizationProviderAttestation
{
    static bool initialized;

    [System.Runtime.CompilerServices.ModuleInitializer]
    internal static void Initialize()
    {
        if (initialized)
            return;

        initialized = true;
        var expected = Environment.GetEnvironmentVariable("HUMANIZER_EXPECTED_MODERN_GLOBALIZATION_PROVIDER");
        if (string.IsNullOrEmpty(expected))
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TF_BUILD")))
                throw new InvalidOperationException("Azure test processes must declare the expected globalization provider.");

            return;
        }

        if (expected is not ("ICU" or "NLS"))
            throw new InvalidOperationException("Unknown expected globalization provider: " + expected);

        string actual;
        string proof;
#if NET48
        if (Environment.OSVersion.Platform != PlatformID.Win32NT ||
            typeof(object).Assembly.GetName().Name != "mscorlib" ||
            Environment.Version.Major != 4 ||
            Type.GetType("Mono.Runtime") is not null)
            throw new InvalidOperationException("The net48 test process must use Windows .NET Framework native NLS.");

        actual = "NLS";
        proof = "Windows Microsoft CLR4 .NETFramework native NLS";
        var effectiveExpected = "NLS";
#else
        var mode = Type.GetType("System.Globalization.GlobalizationMode, System.Private.CoreLib")
            ?? throw new InvalidOperationException("Native globalization mode is unavailable.");
        var flags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;
        var invariant = (bool)(mode.GetProperty("Invariant", flags)?.GetValue(null)
            ?? throw new InvalidOperationException("Native invariant mode is unavailable."));
        if (invariant)
            throw new InvalidOperationException("Invariant globalization is not valid for the locale test matrix.");

        var windows = Environment.OSVersion.Platform == PlatformID.Win32NT;
        var nls = windows && (bool)(mode.GetProperty("UseNls", flags)?.GetValue(null)
            ?? throw new InvalidOperationException("Native Windows NLS mode is unavailable."));
        if (!nls)
        {
            var version = System.Globalization.CultureInfo.InvariantCulture.CompareInfo.Version;
            var icuVersion = BitConverter.ToInt32(version.SortId.ToByteArray(), 0);
            if (icuVersion == 0 || icuVersion != version.FullVersion)
                throw new InvalidOperationException("The test process must attest an actual ICU collation provider.");
        }

        actual = nls ? "NLS" : "ICU";
        proof = windows
            ? "reflected GlobalizationMode.UseNls/Invariant plus ICU SortVersion when selected"
            : "reflected non-invariant mode and ICU SortVersion identity";
        var effectiveExpected = expected;
#endif
        if (actual != effectiveExpected)
            throw new InvalidOperationException($"Globalization provider mismatch: expected {effectiveExpected}, actual {actual}.");

        Console.WriteLine($"GLOBALIZATION_PROVIDER_ATTESTATION assembly={System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} requestedModern={expected} expected={effectiveExpected} actual={actual} runtime={Environment.Version} proof={proof}");
    }
}