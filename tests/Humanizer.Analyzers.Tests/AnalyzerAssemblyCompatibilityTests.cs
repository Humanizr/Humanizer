using System.Reflection;
using System.Linq;

using Humanizer.Analyzers;
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

        var isCompatible = immutableReference.Version is not null && immutableReference.Version.Major <= 7;
        if (!isCompatible)
            throw new InvalidOperationException($"Expected System.Collections.Immutable major version <= 7 but found {immutableReference.Version}.");
    }

    private static AssemblyName[] GetAssemblyReferences() =>
        typeof(NamespaceMigrationAnalyzer).Assembly.GetReferencedAssemblies();
}
