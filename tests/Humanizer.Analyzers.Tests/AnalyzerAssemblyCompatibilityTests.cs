using System.Reflection;
using Xunit;

namespace Humanizer.Analyzers.Tests;

public class AnalyzerAssemblyCompatibilityTests
{
    [Fact]
    public void ShouldReferenceCompatibleSystemMemoryVersion()
    {
        var systemMemoryReference = GetAssemblyReferences().SingleOrDefault(reference => reference.Name == "System.Memory");
        if (systemMemoryReference is null)
            return;

        var isCompatible = systemMemoryReference.Version is not null && systemMemoryReference.Version <= new Version(4, 0, 1, 2);
        if (!isCompatible)
            throw new InvalidOperationException($"Expected System.Memory version <= 4.0.1.2 but found {systemMemoryReference.Version}.");
    }

    [Fact]
    public void ShouldReferenceCompatibleSystemCollectionsImmutableVersion()
    {
        var immutableReference = GetAssemblyReferences().SingleOrDefault(reference => reference.Name == "System.Collections.Immutable");
        if (immutableReference is null)
            throw new InvalidOperationException("Expected System.Collections.Immutable reference but none was found.");

        var isCompatible = immutableReference.Version is not null && immutableReference.Version.Major <= MaxSupportedSystemCollectionsImmutableMajorVersion;
        if (!isCompatible)
            throw new InvalidOperationException($"Expected System.Collections.Immutable major version <= {MaxSupportedSystemCollectionsImmutableMajorVersion} but found {immutableReference.Version}.");
    }

    private static AssemblyName[] GetAssemblyReferences() =>
        typeof(NamespaceMigrationAnalyzer).Assembly.GetReferencedAssemblies();

#if SYSTEM_COLLECTIONS_IMMUTABLE_MAJOR_VERSION_LE_9
    private const int MaxSupportedSystemCollectionsImmutableMajorVersion = 9;
#elif SYSTEM_COLLECTIONS_IMMUTABLE_MAJOR_VERSION_LE_7
    private const int MaxSupportedSystemCollectionsImmutableMajorVersion = 7;
#else
#error Expected a System.Collections.Immutable compatibility cap for this analyzer test project.
#endif
}
